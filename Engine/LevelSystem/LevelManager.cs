using System;
using System.Collections;
using System.Collections.Generic;
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

    public partial class Game
    {
        [FoldoutGroup("Level Manager"), SerializeField]
        private LevelManagerSettings levelManagerSettings = new LevelManagerSettings();
        public static LevelManagerSettings LevelManagerSettings => Instance.levelManagerSettings;    
    }
    
    #endregion
    
    public static class LevelManager
    {
        private static GameLevel _lastLevel;

        public static event Action<GameLevel> OnLoaded;
        public static event Action<GameLevel> OnCompleted;

        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            if (Game.LevelManagerSettings.loadingScreenPrefab != null)
            {
                Object.DontDestroyOnLoad(Object.Instantiate(Game.LevelManagerSettings.loadingScreenPrefab));
            }

            OnLoaded += LevelLoaded;
            
            string current = SceneManager.GetActiveScene().name;
            _lastLevel = GameLevel.Find(current);
            if (_lastLevel == null) return;
            if (current == Game.LevelManagerSettings.BootstrapSceneName)
            {
                LoadNextLevel();
            }
            else
            {
                OnLoaded?.Invoke(_lastLevel);
                OnCompleted?.Invoke(_lastLevel);
            }
        }

        private static void LevelLoaded(GameLevel level)
        {
            FmodController.Set(level.fmodEvents);
        }

        private static IEnumerator LoadLevelAsync(GameLevel level)
        {
            while (FMODUnity.RuntimeManager.HasBankLoaded("Master Bank"))
            {
                yield return null;
            }
            Log.Always("Master Bank Loaded");
            yield return LoadingScreen.Show();
            if (level.sceneName == Game.LevelManagerSettings.BootstrapSceneName) level = GameLevel.Next(level);
            var async = SceneManager.LoadSceneAsync(level.sceneName);
            while (!async.isDone)
            {
                yield return null;
            }
            
            _lastLevel = level;
            OnLoaded?.Invoke(level);
            yield return new WaitForSeconds(Game.LevelManagerSettings.LoadDelay);
            OnCompleted?.Invoke(level);
            yield return LoadingScreen.Hide();
        }

        public static void LoadLevel(GameLevel level)
        {
            CoroutineManager.StartRoutine(LoadLevelAsync(level));
        }

        public static void LoadNextLevel()
        {
            CoroutineManager.StartRoutine(LoadLevelAsync(GameLevel.Next(_lastLevel)));
        }

        public static void LoadMainMenu()
        {
            foreach (var level in GameLevel.All)
            {
                if (!level.isMainMenu) continue;
                LoadLevel(level);
                return;
            }
        }
    }
}