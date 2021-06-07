using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace RedOwl.Engine
{
    public interface ISceneController
    {
        History<SceneMetadata> History { get; }
        void LoadNext();
        void LoadPrevious();
        void LoadScene(SceneMetadata scene);
        void AddScene(SceneMetadata scene);
    }
    
    public class SceneController : MonoBehaviour, ISceneController
    {

        public UIView loadingScreen;

        public float loadingScreenHideDelay = 1f;
        
        public History<SceneMetadata> History { get; private set; }

        private SceneCollection _scenes;

        #region Unity
        
        private void Awake()
        {
            BuildSceneCollection();
            BuildSceneHistory();
            PrepareLoadingScreen();
            PrepareSceneEvents();
            
            Game.Bind<ISceneController>(this);
        }
        
        private void BuildSceneCollection()
        {
            _scenes = new SceneCollection(GameSettings.SceneSettings.scenes.Count);
            foreach (var scene in GameSettings.SceneSettings.scenes)
            {
                _scenes.Add(scene);
            }
        }

        private void BuildSceneHistory()
        {
            History = new History<SceneMetadata>();
            var sceneName = SceneManager.GetActiveScene().name;
            if (_scenes.TryGetValue(sceneName, out var scene))
            {
                // TODO: seems theres a weird bug and we need to possible cache the active editor scene somewhere else to handle it on first unload
                Log.Always($"Pushing '{sceneName}' as Current Scene History");
                History.Push(scene);
            }
        }

        private void PrepareLoadingScreen()
        {
            if (loadingScreen != null)
            {
                DontDestroyOnLoad(loadingScreen);
                loadingScreen.HideImmediate();
            }
        }

        private void PrepareSceneEvents()
        {
            if (GameSettings.SceneSettings.loadScene.IsValid()) GameSettings.SceneSettings.loadScene.ReleaseAsset();
            GameSettings.SceneSettings.loadScene.LoadAssetAsync<SceneChannel>().Completed += (AO) => { AO.Result.On += LoadScene; };
            if (GameSettings.SceneSettings.sceneLoaded.IsValid()) GameSettings.SceneSettings.sceneLoaded.ReleaseAsset();
            GameSettings.SceneSettings.sceneLoaded.LoadAssetAsync<SceneChannel>();
        }

        private void OnEnable()
        {
            if (GameSettings.SceneSettings.loadScene.IsValid())
            {
                if (GameSettings.SceneSettings.loadScene.OperationHandle.Result is SceneChannel casted)
                {
                    casted.On += LoadScene;
                }
            }
        }

        private void Start()
        {
            var sceneName = SceneManager.GetActiveScene().name.ToLower();
            bool isBootstrap = sceneName == "bootstrap";
            bool inPersistent = false;
            foreach (var scene in GameSettings.SceneSettings.scenes)
            {
                if (!scene.isPersistent) continue;
#if UNITY_EDITOR
                if (scene.sceneRef.editorAsset.name.ToLower() == sceneName)
                {
                    inPersistent = true;
                    break;
                }
#endif
            }

            if (GameSettings.SceneSettings.firstScene && (isBootstrap || inPersistent))
            {
                Log.Always($"Red Owl Load First Scene! {GameSettings.SceneSettings.firstScene.name}");
                LoadScene(GameSettings.SceneSettings.firstScene);
            }
        }

        private void OnDisable()
        {
            if (GameSettings.SceneSettings.loadScene.IsValid())
            {
                if (GameSettings.SceneSettings.loadScene.OperationHandle.Result is SceneChannel casted)
                {
                    casted.On -= LoadScene;
                }
            }
        }
        
        #endregion
        
        #region ISceneController
        
        public void LoadNext()
        {
            LoadScene(_scenes.Next(History.Current));
        }

        public void LoadPrevious()
        {
            LoadScene(History.Previous);
        }

        public void LoadScene(SceneMetadata scene)
        {
            Game.StartRoutine(LoadSceneAsync(scene, true));
        }

        public void AddScene(SceneMetadata scene)
        {
            Game.StartRoutine(LoadSceneAsync(scene, false, false));
        }
        
        #endregion
        
        private IEnumerator LoadSceneAsync(SceneMetadata scene, bool showLoadingScreen = true, bool mainScene = true)
        {
            var activeSceneName = SceneManager.GetActiveScene().name.ToLower();
            if (scene == null) yield break;
            if (showLoadingScreen)
            {
                yield return loadingScreen.ShowAsync();
            }

            if (mainScene)
            {
                var current = History.Current;
                if (current != null)
                {
                    if (current.sceneRef.IsValid())
                    {
                        current.sceneRef.UnLoadScene();
                    }
                    else
                    {
                        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                    }
                }

                History.Push(scene);
            }

            if (scene.sceneRef.IsValid()) scene.sceneRef.ReleaseAsset();
            var handle = scene.sceneRef.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            while (!handle.IsDone)
            {
                // TODO: Push progress into loading screen? handle.PercentComplete
                yield return null;
            }

            if (mainScene)
            {
                SceneManager.SetActiveScene(handle.Result.Scene);
                if (activeSceneName == "bootstrap")
                {
                    SceneManager.UnloadSceneAsync(0);
                }
            }
            while (!GameSettings.SceneSettings.sceneLoaded.IsValid())
            {
                yield return null;
            }

            if (GameSettings.SceneSettings.sceneLoaded.OperationHandle.Result is SceneChannel casted)
            {
                casted.Raise(scene);
            }
            if (showLoadingScreen)
            {
                yield return new WaitForSeconds(loadingScreenHideDelay);
                yield return loadingScreen.HideAsync();
            }
        }
    }

    public partial class Game
    {
        public static ISceneController SceneController => Find<ISceneController>();
        

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBootstrap()
        {
            // TODO: there is still an out of order initialization with SceneController container binding when you have a other scene open
            // Not sure there is a way to fix this so that the isPersistant scenes get their awake cycle first or at least
            // we need to find a better way to "bind" the SceneController to the container
            // Log.Always("Red Owl Bootstrap!");
            var activeSceneName = SceneManager.GetActiveScene().name.ToLower();
            foreach (var scene in GameSettings.SceneSettings.scenes)
            {
                if (!scene.isPersistent) continue;
#if UNITY_EDITOR
                if (scene.sceneRef.editorAsset.name.ToLower() == activeSceneName)
                {
                    continue;
                }
#endif
                if (scene.sceneRef.IsValid()) scene.sceneRef.ReleaseAsset();
                scene.sceneRef.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            }
        }
    }
}