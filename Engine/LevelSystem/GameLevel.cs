using System;
using System.Collections.Generic;
using System.IO;
using RedOwl.Core;

namespace RedOwl.Core
{
    public enum LevelTypes
    {
        SinglePlayer,
        MultiPlayer
    }

    public struct GameLevel
    {
        public static readonly List<GameLevel> Levels = new List<GameLevel>();

        public static void Clear() => Levels.Clear();
        public static void Add(GameLevel level) => Levels.Add(level);
        public static GameLevel Next(GameLevel next)
        {
            int length = Levels.Count;
            for (int i = 0; i < Levels.Count; i++)
            {
                if (Levels[i].id != next.id) continue;
                if (i + 1 < length)
                    return Levels[i + 1];
            }

            return Levels[0];
        }
        
        public Guid id;
        
        public LevelTypes type;
        public LevelStates state;

        public string sceneName;

        public bool isMainMenu;
        
        //TODO: Localization Key
        public string title;
        public string subTitle;

        public bool IsSkippable;

        public GameLevel(string name)
        {
            id = Guid.NewGuid();
            
            sceneName = name;
            isMainMenu = false;
            title = "";
            subTitle = "";

            type = LevelTypes.MultiPlayer;
            state = LevelStates.None;

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

            var current = new Dictionary<string, UnityEditor.EditorBuildSettingsScene>(Levels.Count);
            foreach (var level in Levels)
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

        public static GameLevel Find(string name)
        {
            foreach (var level in Levels)
            {
                if (level.sceneName == name) return level;
            }

            return Levels[0];
        }
    }

    public static class GameLevelExtensions
    {
        public static GameLevel Clone ( this GameLevel v )
        {
            v.id = Guid.NewGuid();
            return v;
        }
    }
}