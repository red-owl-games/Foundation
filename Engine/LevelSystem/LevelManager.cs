using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedOwl.Core
{
    public static class LevelManager
    {
        private static GameLevel _lastLevel;
        
        public static void Initialize()
        {
            if (SceneManager.GetActiveScene().name == "Bootstrap")
            {
                _lastLevel = GameLevel.Levels[0];
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

        private static IEnumerator LoadLevelAsync(GameLevel level)
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
                yield return LoadLevelAsync(GameLevel.Next(level));
                yield break;
            }
            Log.Always($"Finished Loading Level {level.sceneName}");
            _lastLevel = level;
            LevelState.SetState(level.state);
            yield return new WaitForSeconds(0.5f);
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


    }
}