using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;


#endif

namespace RedOwl.Core
{
    [Serializable, InlineProperty]
    public class SceneReference : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        [SerializeField, SceneSelector, HideLabel] private Object asset = null;
        private bool IsValidSceneAsset => asset != null && asset is SceneAsset;
#endif

        [SerializeField, HideInInspector] private string path = string.Empty;
        
        public string ScenePath
        {
            get
            {
#if UNITY_EDITOR
                // In editor we always use the asset's path
                return GetScenePathFromAsset();
#else
                return path;
#endif
            }
            set
            {
                path = value;
#if UNITY_EDITOR
                asset = GetSceneAssetFromPath();
#endif
            }
        }
        
        public static implicit operator string(SceneReference sceneReference)
        {
            return sceneReference.ScenePath;
        }
        
        // TODO: implict operator for int to use the BuildIndex
        
#if UNITY_EDITOR
        private SceneAsset GetSceneAssetFromPath() => 
            string.IsNullOrEmpty(path) ? null : AssetDatabase.LoadAssetAtPath<SceneAsset>(path);

        private string GetScenePathFromAsset() => 
            asset == null ? string.Empty : AssetDatabase.GetAssetPath(asset);

        private void HandleBeforeSerialize()
        {
            // Asset is invalid but have Path to try and recover from
            if (IsValidSceneAsset == false && string.IsNullOrEmpty(path) == false)
            {
                asset = GetSceneAssetFromPath();
                if (asset == null) path = string.Empty;
                EditorSceneManager.MarkAllScenesDirty();
            }
            // Asset takes precedence and overwrites Path
            else
            {
                path = GetScenePathFromAsset();
            }
        }

        private void HandleAfterDeserialize()
        {
            // Asset is valid, don't do anything - Path will always be set based on it when it matters
            if (IsValidSceneAsset) return;
            // Asset is invalid but have path to try and recover from
            if (string.IsNullOrEmpty(path)) return;
            asset = GetSceneAssetFromPath();
            // No asset found, path was invalid. Make sure we don't carry over the old invalid path
            if (!asset) path = string.Empty;
            if (!Application.isPlaying) EditorSceneManager.MarkAllScenesDirty();
        }
#endif
        
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            HandleBeforeSerialize();
#endif
        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            // We sadly cannot touch asset database during serialization, so defer by a bit.
            EditorApplication.delayCall += HandleAfterDeserialize;
#endif
        }
    }
}