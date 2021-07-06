using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

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

    public static class AssetTools
    {
        private static ISerializer _serializer;
        public static ISerializer Serializer => _serializer ?? (_serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build());


        private static IDeserializer _deserializer;
        public static IDeserializer Deserializer => _deserializer ?? (_deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).IgnoreUnmatchedProperties().Build());

        #region Public
        
        public static void Load<T>(string key, bool autoRelease = true, Action<T> callback = null) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            if (Game.IsRunning)
            {
                LoadInternal(key, autoRelease, callback);
            }
            else
            {
                LoadEditor(key, callback);
            }
#else
            LoadInternal(key, autoRelease, callback);
#endif
        }



        public static void LoadAll<T>(string key, bool autoRelease = true, Action<IList<T>> callback = null, Action<T> forEach = null) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            if (Game.IsRunning)
            {
                LoadAllInternal(key, autoRelease, callback, forEach);
            }
            else
            {
                LoadAllEditor(key, callback, forEach);
            }
#else
            LoadAllInternal(key, autoRelease, callback, forEach);
#endif
        }

        #endregion

        #region Private
        
        private static async void LoadInternal<T>(string key, bool autoRelease = true, Action<T> callback = null) where T : UnityEngine.Object
        {
            var op = CreateLoadOperation<T>(key);
            op.Completed += handle =>
            {
                switch (handle.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        callback?.Invoke(handle.Result);
                        break;
                    case AsyncOperationStatus.Failed:
                        Addressables.Release(op);
                        break;
                }
            };
            await op.Task;
            if (autoRelease) Addressables.Release(op);
        }

        private static AsyncOperationHandle<T> CreateLoadOperation<T>(string key) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            return  Game.IsRunning ? Addressables.LoadAssetAsync<T>(key) : Addressables.ResourceManager.CreateCompletedOperation(LoadEditor<T>(key), "");
#else
            return Addressables.LoadAssetAsync<T>(key);
#endif
        }

        private static async void LoadAllInternal<T>(string key, bool autoRelease = true, Action<IList<T>> callback = null, Action<T> forEach = null) where T : UnityEngine.Object
        {
            var op = CreateLoadAllOperation<T>(key, forEach);
            op.Completed += handle =>
            {
                switch (handle.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        callback?.Invoke(handle.Result);
                        break;
                    case AsyncOperationStatus.Failed:
                        Addressables.Release(op);
                        break;
                }
            };
            await op.Task;
            if (autoRelease) Addressables.Release(op);
        }

        private static AsyncOperationHandle<IList<T>> CreateLoadAllOperation<T>(string key, Action<T> forEach = null) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            return Game.IsRunning ? Addressables.LoadAssetsAsync(key, forEach) : Addressables.ResourceManager.CreateCompletedOperation(LoadAllEditor(key, null, forEach), "");
#else
            return Addressables.LoadAssetsAsync(key, forEach);
#endif
        }

#if UNITY_EDITOR
        private static T LoadEditor<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
        {
            // TODO: if key has [] then we need to pull out a subasset
            if (key.Contains("[") || key.Contains("]"))
            {
                throw new Exception($"Unable to properly load {key} - we need to write code for this!!!");
            }
            foreach (var entry in AddressableDatabase.Get<T>())
            {
                if (key != entry.Key && !entry.Value.Contains(key)) continue;
                
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(entry.Key));
                if (asset != null)
                {
                    callback?.Invoke(asset);
                    return asset;
                }
            }
            return null;
        }

        private static IList<T> LoadAllEditor<T>(string key, Action<IList<T>> callback = null, Action<T> forEach = null) where T : UnityEngine.Object
        {
            // TODO: if key has [] then we need to pull out a subasset
            if (key.Contains("[") || key.Contains("]"))
            {
                throw new Exception($"Unable to properly load {key} - we need to write code for this!!!");
            }
            var assets = new List<T>();
            foreach (var entry in AddressableDatabase.Get<T>())
            {
                if (key == entry.Key || entry.Value.Contains(key))
                {
                    // TODO: if key has [] then we need to pull out a subasset
                    var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(entry.Key));
                    if (asset == null) continue;
                    forEach?.Invoke(asset);
                    assets.Add(asset);
                }
            }
            callback?.Invoke(assets);
            return assets;
        }
#endif

        #endregion


        [Conditional("UNITY_EDITOR")]
        public static void Rename<T>(T asset, string name) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            if (UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(asset, out string guid, out long _))
            {
                var currentPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                UnityEditor.AssetDatabase.RenameAsset(currentPath, name);
            };
#endif
        }
    }
}