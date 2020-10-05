using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedOwl.Core
{
    public static class LevelManager
    {
        private static GameLevel _lastLevel;

        public static event Action<GameLevel> OnLevelLoaded;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            _lastLevel = GameLevel.Find(SceneManager.GetActiveScene().name);
            if (_lastLevel.id == GameLevel.Levels[0].id)
            {
                LoadNextLevel();
            }
            else
            {
                CoroutineManager.StartRoutine(HideCurtain());
            }
        }

        private static IEnumerator HideCurtain()
        {
            yield return new WaitForSeconds(1f);
            yield return LoadingScreen.Hide();
        }

        private static IEnumerator LoadLevelAsync(GameLevel level, Action callback = null, Func<IEnumerator> asyncCallback = null)
        {
            yield return LoadingScreen.Show();
            
            var async = SceneManager.LoadSceneAsync(level.sceneName);
            while (!async.isDone)
            {
                yield return null;
            }

            if (level.sceneName == "Bootstrap")
            {
                yield return new WaitForSeconds(0.5f);
                yield return LoadLevelAsync(GameLevel.Next(level), callback, asyncCallback);
                yield break;
            }
            Log.Always($"Finished Loading Level {level.sceneName}");
            _lastLevel = level;
            LevelState.SetState(level.state);
            OnLevelLoaded?.Invoke(level);
            yield return new WaitForSeconds(1.5f);
            callback?.Invoke();
            if (asyncCallback != null) yield return asyncCallback();
            yield return LoadingScreen.Hide();
        }
        
        public static void LoadLevel(GameLevel level)
        {
            CoroutineManager.StartRoutine(LoadLevelAsync(level));
        }
        
        public static void LoadLevel(GameLevel level, Action callback)
        {
            CoroutineManager.StartRoutine(LoadLevelAsync(level, callback));
        }

        public static void LoadLevel(GameLevel level, Func<IEnumerator> callback)
        {
            CoroutineManager.StartRoutine(LoadLevelAsync(level, null, callback));
        }
        
        public static void LoadNextLevel()
        {
            CoroutineManager.StartRoutine(LoadLevelAsync(GameLevel.Next(_lastLevel)));
        }

        public static void LoadNextLevel(Action callback)
        {
            CoroutineManager.StartRoutine(LoadLevelAsync(GameLevel.Next(_lastLevel), callback));
        }

        public static void LoadNextLevel(Func<IEnumerator> callback)
        {
            CoroutineManager.StartRoutine(LoadLevelAsync(GameLevel.Next(_lastLevel), null, callback));
        }

        public static void LoadMainMenu()
        {
            foreach (var level in GameLevel.Levels)
            {
                if (!level.isMainMenu) continue;
                LoadLevel(level);
                return;
            }
        }
    }
}