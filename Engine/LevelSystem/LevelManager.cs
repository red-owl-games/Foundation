using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Engine.DataTypes;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    #region Settings
    
    [Serializable]
    public class LevelManagerSettings : Settings
    {
        public string BootstrapSceneName = "Bootstrap";

        public float LoadDelay = 1f;
        
        [AssetsOnly, AssetSelector(Filter = "t:ILoadingScreen")]
        public GameObject loadingScreenPrefab;
    }

    public partial class GameSettings
    {
        [FoldoutGroup("Level Manager"), SerializeField]
        private LevelManagerSettings levelManagerSettings = new LevelManagerSettings();
        public static LevelManagerSettings LevelManagerSettings => Instance.levelManagerSettings;    
    }
    
    #endregion

    public class LevelCollection : BetterKeyedCollection<string, GameLevel>
    {
        protected override string GetKeyForItem(GameLevel item) => item.sceneName;
    }

    public class LevelManager : IServiceInit, IServiceStart
    {
        public event Action<GameLevel> OnLoaded;
        public event Action<GameLevel> OnCompleted;
        
        public readonly LevelCollection Levels;
        public readonly History<GameLevel> LevelHistory;

        [Inject]
        private FmodService _fmodService;

        public LevelManager()
        {
            Levels = new LevelCollection();
            LevelHistory = new History<GameLevel>();
        }

        public void Init()
        {
            if (GameSettings.LevelManagerSettings.loadingScreenPrefab != null)
            {
                Object.DontDestroyOnLoad(Object.Instantiate(GameSettings.LevelManagerSettings.loadingScreenPrefab));
            }
            OnCompleted += AfterLevelLoaded;
        }
        
        public void Start()
        {
            string current = SceneManager.GetActiveScene().name;
            if (Levels.TryGetValue(current, out var level))
            {
                LevelHistory.Set(level);
                Game.LoadLevel(level);
            }
        }

        internal IEnumerator LoadLevelAsync(GameLevel level)
        {
            yield return LoadingScreen.Show();
            if (level.sceneName == GameSettings.LevelManagerSettings.BootstrapSceneName) level = Levels.Next(level);
            var async = SceneManager.LoadSceneAsync(level.sceneName);
            while (!async.isDone)
            {
                yield return null;
            }
            
            LevelHistory.Set(level);
            OnLoaded?.Invoke(level);
            yield return new WaitForSeconds(GameSettings.LevelManagerSettings.LoadDelay);
            OnCompleted?.Invoke(level);
            yield return LoadingScreen.Hide();
        }
        
        private void AfterLevelLoaded(GameLevel level)
        {
            _fmodService.Set(level.fmodEvents);
        }

        public void RegisterLevels(IEnumerable<GameLevel> levels)
        {
            foreach (var level in levels)
            {
                Levels.Add(level);
            }
        }
    }
    
    public partial class Game
    {
        public static LevelManager LevelManager => Find<LevelManager>();

        public static void BindLevelManager() => Bind(new LevelManager());
        
        public static void LoadLevel(GameLevel level)
        {
            if (LevelManager != null) CoroutineManager.StartRoutine(LevelManager.LoadLevelAsync(level));
        }

        public static void LoadNextLevel()
        {
            if (LevelManager != null) CoroutineManager.StartRoutine(LevelManager.LoadLevelAsync(LevelManager.Levels.Next(LevelManager.LevelHistory.Current)));
        }
        
        public static void LoadPreviousLevel()
        {
            if (LevelManager != null) CoroutineManager.StartRoutine(LevelManager.LoadLevelAsync(LevelManager.LevelHistory.Previous));
        }

        public static void LoadMainMenu()
        {
            foreach (var level in LevelManager.Levels)
            {
                if (!level.isMainMenu) continue;
                LoadLevel(level);
                return;
            }
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
        
        public static void EnsureLevelBuildSettings(List<GameLevel> levels)
        {
            var possibleScenes = GetScenes();

            var current = new Dictionary<string, UnityEditor.EditorBuildSettingsScene>(levels.Count);
            foreach (var level in levels)
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