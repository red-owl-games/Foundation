using System;
using System.Collections.Generic;
using System.IO;

namespace RedOwl.Engine
{
    public enum LevelTypes
    {
        SinglePlayer,
        MultiPlayer
    }

    public class GameLevel : Indexed<GameLevel>
    {
        public LevelTypes type { get; private set; }
        public LevelStates state { get; private set; }

        public string sceneName { get; private set; }

        public bool isMainMenu { get; private set; }
        
        //TODO: Localization Key
        public string title { get; private set; }
        public string subTitle { get; private set; }

        public List<FmodEvent> fmodEvents { get; private set; }

        public bool IsSkippable { get; private set; }

        public GameLevel(string name) : base()
        {
            sceneName = name;
            isMainMenu = false;
            title = "";
            subTitle = "";

            type = LevelTypes.MultiPlayer;
            state = LevelStates.None;

            fmodEvents = new List<FmodEvent>();

            IsSkippable = false;
        }
        
        public GameLevel Suffix(string value)
        {
            sceneName = $"{sceneName}{value}";
            return this;
        }

        public GameLevel Skippable()
        {
            IsSkippable = true;
            return this;
        }

        public GameLevel MainMenu()
        {
            isMainMenu = true;
            return this;
        }

        public GameLevel Title(string _title = "", string _subTitle = "")
        {
            title = _title;
            subTitle = _subTitle;
            return this;
        }

        public GameLevel State(LevelStates value)
        {
            state = value;
            return this;
        }

        public GameLevel FMOD(string path, params FmodParam[] parameters)
        {
            fmodEvents.Add(new FmodEvent(path, parameters: parameters));
            return this;
        }

        public static GameLevel Find(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return null;
            if (All.Count == 0)
            {
                Log.Warn($"Unable to find sceneName '{sceneName}' because the count of registered Game Levels is 0");
                return null;
            }
            foreach (var level in All)
            {
                if (level.sceneName == sceneName) return level;
            }

            return All[0];
        }
        
        #region SceneBuildSettings
#if UNITY_EDITOR
        // The following ensures that the designed level order levels are in the build settings
        private static Dictionary<string, UnityEditor.GUID> GetScenes()
        {
            var output = new Dictionary<string, UnityEditor.GUID>();
            foreach (string scene in UnityEditor.AssetDatabase.FindAssets("t:Scene"))
            {
                UnityEditor.GUID.TryParse(scene, out UnityEditor.GUID id);
                string key = Path.GetFileName(UnityEditor.AssetDatabase.GUIDToAssetPath(scene)).Replace(".unity", "");
                if (!output.TryGetValue(key, out UnityEditor.GUID _))
                {
                    output.Add(key, id);
                }
            }
            return output;
        }
        
        public static void EnsureBuildSettings()
        {
            var possibleScenes = GetScenes();

            var current = new Dictionary<string, UnityEditor.EditorBuildSettingsScene>(All.Count);
            foreach (var level in All)
            {
                if (possibleScenes.TryGetValue(level.sceneName, out var id))
                {
                    if (!current.TryGetValue(level.sceneName, out var _))
                        current.Add(level.sceneName, new UnityEditor.EditorBuildSettingsScene(id, true));
                }
                else
                {
                    Log.Warn($"Unable to Find Scene: '{level.sceneName}'");
                }
            }
            UnityEditor.EditorBuildSettings.scenes = new List<UnityEditor.EditorBuildSettingsScene>(current.Values).ToArray();
        }
#endif
        #endregion
    }
}