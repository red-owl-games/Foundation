using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QFSW.QC;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace RedOwl.Engine
{
    public struct SceneControllerLoadSettings
    {
        public bool isBootstrapping;
        public bool useLoadingScreen;
        public float loadingScreenAnimationDelay;
        public bool useFader;
        public float faderAnimationDelay;
    }
    
    public class SceneController
    {
        [Inject] private FmodService _fmod;
        
        private Dictionary<string, AsyncOperationHandle<SceneInstance>> _loadedScenes;
        private List<Scene> _preloadedScenes;

        public SceneController()
        {
            _loadedScenes = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();
            _preloadedScenes = new List<Scene>();
            CacheCurrentScenes();
            Game.Inject(this);
        }

        public IEnumerator Load(Location next, SceneControllerLoadSettings settings)
        {
            if (settings.isBootstrapping)
            {
                yield return LoadOtherScenes(next);
                yield return WaitForAllLoading();
            }

            // TODO: Location should control how loading screen and fader work
            if (settings.useFader)
            {
                Game.Events.FadeOut.Raise();
                yield return new WaitForSeconds(settings.faderAnimationDelay);
            }

            if (settings.useLoadingScreen)
            {
                Game.Events.ShowLoadingScreen.Raise();
                yield return new WaitForSeconds(settings.loadingScreenAnimationDelay);
            }
            
            _fmod.Ensure(next.Music);

            // TODO: we also need to check that operations are completing successfully
            yield return LoadOtherScenes(next);
            yield return LoadMainScene(next);
            UnloadUnusedAddressableScenes(next);
            UnloadUnusedPreloadedScenes(next);
            
            yield return WaitForAllLoading();

            yield return new WaitForSeconds(next.warmupDelay);

            if (settings.useLoadingScreen)
            {
                Game.Events.HideLoadingScreen.Raise();
            }

            if (settings.useFader)
            {
                yield return new WaitForSeconds(settings.loadingScreenAnimationDelay);
                Game.Events.FadeIn.Raise();
            }
        }
        
        private void CacheCurrentScenes()
        {
            if (SceneManager.sceneCount <= 0) return;
            for (int n = 0; n < SceneManager.sceneCount; ++n)
            {
                Scene scene = SceneManager.GetSceneAt(n);
                if (!scene.isLoaded) continue;
                _preloadedScenes.Add(scene);
            }
        }
        private IEnumerator LoadMainScene(Location location)
        {
            var mainSceneLoadedOp = Addressables.LoadResourceLocationsAsync(location.mainScene.Address);
            yield return mainSceneLoadedOp;
            if (!SceneManager.GetSceneByPath(mainSceneLoadedOp.Result[0].InternalId).isLoaded)
            {
                var mainSceneLoadOp = Addressables.LoadSceneAsync(location.mainScene.Address, LoadSceneMode.Additive);
                _loadedScenes.Add(location.mainScene.Address, mainSceneLoadOp);
                yield return mainSceneLoadOp;
                SceneManager.SetActiveScene(mainSceneLoadOp.Result.Scene);
            }
        }

        private IEnumerator LoadOtherScenes(Location location)
        {
            foreach (var otherScene in location.otherScenes)
            {
                var otherSceneLoadedOp = Addressables.LoadResourceLocationsAsync(otherScene.Address);
                yield return otherSceneLoadedOp;
                if (!SceneManager.GetSceneByPath(otherSceneLoadedOp.Result[0].InternalId).isLoaded)
                {
                    var otherSceneLoadOp = Addressables.LoadSceneAsync(otherScene.Address, LoadSceneMode.Additive);
                    _loadedScenes.Add(otherScene.Address, otherSceneLoadOp);
                }
            }
        }

        private void UnloadUnusedPreloadedScenes(Location location)
        {
            foreach (var editorScene in _preloadedScenes)
            {
                if (IsInLocation(editorScene.path, location)) continue;
                SceneManager.UnloadSceneAsync(editorScene).completed += (ao) =>
                {
                    _preloadedScenes.Remove(editorScene);
                };
            }
        }

        private void UnloadUnusedAddressableScenes(Location location)
        {
            foreach (var kvp in _loadedScenes)
            {
                if (IsInLocation(kvp.Key, location)) continue;
                Addressables.UnloadSceneAsync(kvp.Value).Completed += (handle) =>
                {
                    _loadedScenes.Remove(kvp.Key);
                };
            }
        }
        
        private IEnumerator WaitForAllLoading()
        {
            // TODO: what happens if this never exits?
            bool done = false;
            while (done == false)
            {
                yield return null;
                foreach (var op in _loadedScenes.Values)
                {
                    if (op.Status != AsyncOperationStatus.Succeeded)
                    {
                        done = false;
                        break;
                    }

                    done = true;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsInLocation(string loadedScene, Location location) => 
            IsMainScene(loadedScene, location) || IsInOtherScenes(loadedScene, location);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsMainScene(string loadedScene, Location location) => 
            loadedScene.EndsWith(location.mainScene.Address);

        private bool IsInOtherScenes(string loadedScene, Location location)
        {
            foreach (var otherScene in location.otherScenes)
            {
                if (loadedScene.EndsWith(otherScene.Address)) return true;
            }
            return false;
        }
    }
    
    public class LocationService : IServiceInit, IServiceStart
    {
        public static class Events
        {
            [Telegram] 
            public static LocationMessage LoadLocation => Telegraph.Get<LocationMessage>(nameof(LoadLocation));
            [Telegram] 
            public static Message LoadNextLocation => Telegraph.Get<Message>(nameof(LoadNextLocation));
            [Telegram] 
            public static Message LoadPreviousLocation => Telegraph.Get<Message>(nameof(LoadPreviousLocation));
            
            [Command("ro.next-location")]
            private static void NextLocation() => LoadNextLocation.Raise();
            [Command("ro.prev-location")]
            private static void PreviousLocation() => LoadPreviousLocation.Raise();
        }

        private SceneController _controller;

        private LocationFlow _flow;
        public History<Location> History { get; private set; }
        
        public void Init()
        {
            _controller = new SceneController();
            
            // TODO: should all of this go into the AssetTools callback?
            Events.LoadLocation.On -= LoadLocation;
            Events.LoadLocation.On += LoadLocation;
            Events.LoadNextLocation.On -= LoadNextLocation;
            Events.LoadNextLocation.On += LoadNextLocation;
            Events.LoadPreviousLocation.On -= LoadPreviousLocation;
            Events.LoadPreviousLocation.On += LoadPreviousLocation;
            
            History = new History<Location>();
        }

        public void Start()
        {
            AssetTools.Load<LocationFlow>("GameLocationFlow", false, (flow) =>
            {
                _flow = flow;
                ColdLoadCheck();
            });
        }

        private void ColdLoadCheck()
        {
            var coldLoaded = false;
            var scenePath = SceneManager.GetActiveScene().path;
            foreach (var location in _flow.AllLocations)
            {
                if (scenePath.EndsWith(location.mainScene.Address))
                {
                    InternalLoad(location, true, true);
                    coldLoaded = true;
                }
            }

            if (coldLoaded == false)
            {
                InternalLoad(_flow.Flow[0], true, true);
            }
        }

        public void LoadLocation(LocationRef reference)
        {
            AssetTools.Load<Location>(reference, true, (l) => InternalLoad(l, false, true));
        }

        public void LoadNextLocation()
        {
            int count = _flow.Flow.Length;
            for (int i = 0; i < count; i++)
            {
                if (_flow[i].Id != History.Current.Id) continue;
                int nextIndex = i + 1;
                InternalLoad(nextIndex >= count ? _flow[0] : _flow[nextIndex], false, true);
                break;
            }
        }
        
        public void LoadPreviousLocation()
        {
            int count = _flow.Flow.Length;
            for (int i = 0; i < count; i++)
            {
                if (_flow[i].Id != History.Current.Id) continue;
                int nextIndex = i - 1;
                InternalLoad(nextIndex < 0 ? _flow[0] : _flow[nextIndex], false, false);
                break;
            }
        }

        private void InternalLoad(Location location, bool bootstrap, bool forward)
        {
            History.Push(location);
            if (bootstrap) History.Push(location);
            // TODO: should this just be if any location needs loading screen then its true? same for fader?
            var settings = new SceneControllerLoadSettings
            {
                isBootstrapping = bootstrap,
                useLoadingScreen = forward ? location.useLoadingScreen : History.Previous.useLoadingScreen ,
                loadingScreenAnimationDelay = forward ? location.loadingScreenAnimationDelay : History.Previous.loadingScreenAnimationDelay,
                useFader = forward ? location.useFader : History.Previous.useFader ,
                faderAnimationDelay = forward ? location.faderAnimationDelay : History.Previous.faderAnimationDelay
            };
            Game.StartRoutine(_controller.Load(location, settings));
            
        }
    }

    public partial class Game
    {
        public static LocationService LocationService => FindOrBind<LocationService>();
        
        public static LocationService BindLocationService() => Bind<LocationService>();

    }
}