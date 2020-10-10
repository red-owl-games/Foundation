using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedOwl.Core
{
    [Serializable]
    public class LevelManagerSettings : Settings<LevelManagerSettings>
    {
        [SerializeField] 
        private string bootstrapSceneName = "Bootstrap";
        public static string BootstrapSceneName => Instance.bootstrapSceneName;

        [SerializeField] 
        private float loadDelay = 1f;
        public static float LoadDelay => Instance.loadDelay;
    }

    public static class LevelManager
    {
        private static GameLevel _lastLevel;

        public static event Action<GameLevel> OnLoaded;
        public static event Action<GameLevel> OnCompleted;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            string current = SceneManager.GetActiveScene().name;
            _lastLevel = GameLevel.Find(current);
            if (current == LevelManagerSettings.BootstrapSceneName)
            {
                LoadNextLevel();
            }
            OnCompleted?.Invoke(GameLevel.Find(current));
        }

        private static IEnumerator LoadLevelAsync(GameLevel level)
        {
            yield return LoadingScreen.Show();
            if (level.sceneName == LevelManagerSettings.BootstrapSceneName) level = GameLevel.Next(level);
            var async = SceneManager.LoadSceneAsync(level.sceneName);
            while (!async.isDone)
            {
                yield return null;
            }

            _lastLevel = level;
            OnLoaded?.Invoke(level);
            yield return new WaitForSeconds(LevelManagerSettings.LoadDelay);
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