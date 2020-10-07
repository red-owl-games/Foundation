using System;
using System.Collections.Generic;
using System.IO;

namespace RedOwl.Core
{
    public enum LevelTypes
    {
        SinglePlayer,
        MultiPlayer
    }

    public struct MusicParameters
    {
        public string parameterName;
        public float value;
        public float duration;
    }

    public class GameLevel : Indexed<GameLevel>
    {
        public LevelTypes type;
        public LevelStates state;

        public string sceneName;

        public bool isMainMenu;
        
        //TODO: Localization Key
        public string title;
        public string subTitle;

        public string musicEventPath;
        public List<MusicParameters> musicParameters;

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

            musicEventPath = "";
            musicParameters = new List<MusicParameters>();

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

        public GameLevel Music(string eventPath)
        {
            musicEventPath = eventPath;
            return this;
        }

        public GameLevel MusicParam(string parameterName, float value, float duration = 1f)
        {
            musicParameters.Add(new MusicParameters
            {
                parameterName = parameterName,
                value = value,
                duration = duration
            });
            return this;
        }

        public static GameLevel Find(string sceneName)
        {
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

    public static class GameLevelExtensions
    {
        // TODO: Figure out how to properly deep clone
        public static GameLevel Clone ( this GameLevel v )
        {
            v.id = Guid.NewGuid();
            v.musicParameters = new List<MusicParameters>();
            return v;
        }
    }
}