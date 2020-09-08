using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    // TODO: Initialization and Ensuring Game.Bind of ISettings is all over the place need to cleanup and find a unifying path
    
    public interface ISettings {}

    [Serializable, InlineProperty, HideReferenceObjectPicker]
    public abstract class Settings<T> : ISettings where T : Settings<T>, new()
    {
        [ShowInInspector, PropertyOrder(-10000000), DisplayAsString, HideLabel]
        internal string ConfigTitle => typeof(T).Name;
        
        public static T Instance { get; } = new SettingsReference<T>();
    }

    public class SettingsCache : TypeCache<ISettings>
    {
        protected override bool ShouldCache(Type type) => true;
    }

    public class SettingsReference<T> where T : Settings<T>, new()
    {
        private T _value;

        public static implicit operator T(SettingsReference<T> self)
        {
            if (self._value == null) self._value = Game.Find<T>();
            if (self._value == null)
            {
                GameSettingsInitialization.Initialize();
                self._value = Game.Find<T>();
            }

            return self._value;
        }
    }
    
    [HideMonoScript]
    internal class GameSettings : ScriptableObject
    {
        internal static GameSettings Instance;
        
        [SerializeReference, ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, Expanded = true), LabelText("Configs")]
        internal List<ISettings> all;

#if UNITY_EDITOR

        private static Sirenix.OdinInspector.Editor.PropertyTree _tree;
        
        [UnityEditor.SettingsProvider]
        public static UnityEditor.SettingsProvider Create()
        {
            GameSettingsEditorInitialization.EnsureAsset();
            GameSettingsInitialization.Initialize();
            _tree = Sirenix.OdinInspector.Editor.PropertyTree.Create(Instance);
            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Project Settings window.
            var provider = new UnityEditor.SettingsProvider($"Project/RedOwl", UnityEditor.SettingsScope.Project)
            {
                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = searchContext =>
                {
                    Sirenix.OdinInspector.Editor.InspectorUtilities.BeginDrawPropertyTree(_tree, false);
                    foreach (Sirenix.OdinInspector.Editor.InspectorProperty property in _tree.EnumerateTree(false))
                    {
                        property.Draw(property.Label);
                    }
                    Sirenix.OdinInspector.Editor.InspectorUtilities.EndDrawPropertyTree(_tree);
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = UnityEditor.SettingsProvider.GetSearchKeywordsFromSerializedObject(new UnityEditor.SerializedObject(Instance))
            };

            return provider;
        }

        internal void Initialize()
        {
            if (all == null) all = new List<ISettings>();
            var cache = new SettingsCache();
            foreach (var type in cache.All)
            {
                bool found = false;
                foreach (var config in all)
                {
                    if (config != null && config.GetType() == type) found = true;
                }
                if (found) continue;
                Log.Debug($"Ensuring Config Instance '{type}'");
                all.Add((ISettings)Activator.CreateInstance(type));
            }
            all.Sort((x, y) => string.Compare(x.GetType().Name, y.GetType().Name, StringComparison.Ordinal));
        }
#endif
    }

    internal static class GameSettingsInitialization
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        internal static void Initialize()
        {
            GameSettings settingses = Resources.Load<GameSettings>(nameof(GameSettings));
            if (settingses == null) return;
            foreach (var config in settingses.all)
            {
                Game.Bind(config);
            }
        }
    }
    
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
    internal class GameSettingsEditorInitialization
    {
        static GameSettingsEditorInitialization()
        {
            UnityEditor.EditorApplication.update -= EnsureAsset;
            UnityEditor.EditorApplication.update += EnsureAsset;
        }

        internal static void EnsureAsset()
        {
            if (GameSettings.Instance != null)
            {
                ValidateAsset();
                return;
            }
            string[] assets = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(GameSettings)}");
            if (assets.Length == 0) // Create Missing
            {
                GameSettings.Instance = ScriptableObject.CreateInstance<GameSettings>();
                UnityEditor.AssetDatabase.CreateAsset(GameSettings.Instance, $"Assets/Resources/{nameof(GameSettings)}.asset");
                UnityEditor.AssetDatabase.SaveAssets();
                return;
            }

            if (assets.Length > 1) // Too many
            {
                
                GameSettings.Instance = ScriptableObject.CreateInstance<GameSettings>();
                foreach (string assetGuid in assets)
                {
                    Log.Warn($"Too many GameSettings exist! '{UnityEditor.AssetDatabase.GUIDToAssetPath(assetGuid)}'");
                }
                return;
            }

            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(assets[0]);
            GameSettings.Instance = UnityEditor.AssetDatabase.LoadAssetAtPath<GameSettings>(path);
            ValidateAsset();
        }

        private static void ValidateAsset()
        {
            var path = UnityEditor.AssetDatabase.GetAssetPath(GameSettings.Instance);
            GameSettings.Instance.Initialize();
            // TODO: needs to actually be directly under "Resources"
            // TODO: needs to be named nameof(GameConfig)
            if (path.Contains("Resources")) return;
            Log.Error($"GameSettings {path} does not live in a 'Resources' folder!");
        }
    }
#endif
}