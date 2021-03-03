using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace RedOwl.Engine
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Singleton : Attribute {}

    [HideMonoScript]
    public abstract class Asset : ScriptableObject
    {
        [SerializeField, Title("Description"), TextArea(1, 6), HideLabel, HideInInlineEditors] 
        private string developerDescription;
    }

    [HideMonoScript]
    public abstract class Asset<T> : Asset where T : Asset<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = GetInstance();
                _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                return _instance;
            }
        }

        private static T GetInstance()
        {
#if UNITY_EDITOR
            return Game.IsRunning ? GetInstanceRuntime() : GetInstanceEditor();
#else
            return GetInstanceRuntime();
#endif
        }

        private static T GetInstanceRuntime()
        {
            var results = Resources.FindObjectsOfTypeAll<T>();
            if (results.Length == 0)
            {
                Debug.LogWarning($"No instances of '{typeof(T).FullName}' found - creating a runtime instance");
                _instance = CreateInstance<T>();
                return _instance;
            }

            if (results.Length > 1)
            {
                Debug.LogWarning($"Multiple instances of '{typeof(T).FullName}' found - using '{results[0].name}'");
            }

            return results[0];
        }
        
#if UNITY_EDITOR
        private static T GetInstanceEditor()
        {
            var results = Resources.FindObjectsOfTypeAll<T>();
            if (results.Length == 0)
            {
                Debug.LogWarning($"No instances of '{typeof(T).FullName}' found - creating a asset instance");
                var output = (T)CreateInstance(typeof(T));
                UnityEditor.AssetDatabase.CreateAsset(output, $"Assets/Game/Resources/{typeof(T).Name}.asset");
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
                return output;
            }

            if (results.Length > 1)
            {
                Debug.LogWarning($"Multiple instances of '{typeof(T).FullName}' found - using '{results[0].name}'");
            }

            return results[0];
        }
#endif
    }
    
#if UNITY_EDITOR
    public static class AssetInitialization
    {
        [UnityEditor.InitializeOnLoadMethod]
        public static void Initialize()
        {
            bool needsRefresh = false;
            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Game")) UnityEditor.AssetDatabase.CreateFolder("Assets", "Game");
            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Game/Resources"))UnityEditor.AssetDatabase.CreateFolder("Assets/Game", "Resources");
            foreach (var type in TypeExtensions.GetAllTypesWithAttribute<Singleton>())
            {
                if (!typeof(ScriptableObject).IsAssignableFrom(type)) continue;
                if (UnityEditor.AssetDatabase.FindAssets($"t:{type.FullName}").Any()) continue;
                UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(type), $"Assets/Game/Resources/{type.Name}.asset");
                Debug.Log($"Initialized Singleton '{type.FullName}'");
                needsRefresh = true;
            }

            if (!needsRefresh) return;
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }
    }
#endif
}