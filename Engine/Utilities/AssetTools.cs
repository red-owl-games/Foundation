using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RedOwl.Engine
{
    
    #region Settings
    
    [Serializable, InlineProperty, HideLabel]
    public class CodeGenSettings
    {
        public string @namespace = "Project";
        [FolderPath(RequireExistingPath = true)]
        public string folder = "Assets/Game/Code/Generated";
    }

    public partial class GameSettings
    {
        [FoldoutGroup("Databases"), SerializeField]
        private CodeGenSettings codeGenSettings = new CodeGenSettings();
        public static CodeGenSettings CodeGenSettings => Instance.codeGenSettings;
    }
    
    #endregion
    
    public class BetterAssetReference<T> : AssetReference where T : UnityEngine.Object
    {
        public BetterAssetReference(string guid) : base(guid) {}
        
        public void Load(Action<T> callback) => this.Load<T>(callback);

        public static implicit operator BetterAssetReference<T>(string guid) => new BetterAssetReference<T>(guid);

        public static implicit operator T(BetterAssetReference<T> reference) => reference.Asset as T;
    }
    
    public static class AssetReferenceExtensions
    {
        public static void Load<T>(this AssetReference reference, Action<T> callback) where T : UnityEngine.Object
        {
            if (reference.IsValid())
            {
                if (reference.OperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    callback((T)reference.OperationHandle.Result);
                    return;
                }
                reference.ReleaseAsset();
            }
            AssetTools.Load<T>(reference).Completed += (result) =>
            {
                if (result.Status == AsyncOperationStatus.Succeeded)
                {
                    callback(result.Result);
                }
            };
        }
    }
    
    public static class AssetTools
    {
        public const string ASSET_REFERENCE_NULL = "[AssetTools] asset reference was null";
        public const string RUNTIME_KEY_INVALID = "[AssetTools] asset reference runtime key was invalid";

        // Commenting out until we have a usecase
        // I think this should probably use an IEnumerator
        // public static async void Preload(IEnumerable<string> labels, Addressables.MergeMode mergeMode = Addressables.MergeMode.Intersection)
        // {
        //     await Addressables.LoadResourceLocationsAsync(labels, mergeMode).Task;
        // }

        public static AsyncOperationHandle<T> Load<T>(AssetReference reference) where T : UnityEngine.Object
        {
            if (reference == null)
            {
                Log.Warn(ASSET_REFERENCE_NULL);
                return Addressables.ResourceManager.CreateCompletedOperation<T>(null, ASSET_REFERENCE_NULL);
            }

            if (!reference.RuntimeKeyIsValid())
            {
                Log.Warn(RUNTIME_KEY_INVALID);
                return Addressables.ResourceManager.CreateCompletedOperation<T>(null, RUNTIME_KEY_INVALID);
            }

#if UNITY_EDITOR
            return Application.isPlaying ? LoadWithAddressables<T>(reference) : LoadWithAssetDatabase<T>(reference);
#else
            return LoadWithAddressables<T>(reference);
#endif
        }

        private static AsyncOperationHandle<T> LoadWithAssetDatabase<T>(AssetReference reference) where T : UnityEngine.Object
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(reference.RuntimeKey.ToString());
            var foundAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
            var foundType = foundAsset == null
                ? UnityEditor.AssetDatabase.GetMainAssetTypeAtPath(path)
                : foundAsset.GetType();
            var expectedType = typeof(T);
            if (foundType != expectedType)
            {
                var unexpectedTypeMsg = $"[AssetTools] found '{foundType}' @ '{path}' but is not expected type '{expectedType}'.  Unable to load it.";
                Log.Warn(unexpectedTypeMsg);
                return Addressables.ResourceManager.CreateCompletedOperation<T>(null, unexpectedTypeMsg);
            }

            return Addressables.ResourceManager.CreateCompletedOperation(foundAsset, "");
        }

        private static AsyncOperationHandle<T> LoadWithAddressables<T>(AssetReference reference) where T : UnityEngine.Object
        {
            return reference.LoadAssetAsync<T>();
        }
    }
}