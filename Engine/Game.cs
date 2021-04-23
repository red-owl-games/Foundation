using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace RedOwl.Engine
{
    public class RedOwlException : Exception
    {
        public RedOwlException() {}
        public RedOwlException(string message) : base(message) {}
        public RedOwlException(string message, Exception inner) : base(message, inner) {}
    }
    
    [Serializable, InlineProperty, HideLabel]
    public abstract class Settings {}

    public class GameInstance : IDisposable
    {
        public Random Random { get; }
        public Container Container { get; }

        private Coroutine _update;

        public GameInstance()
        {
            Random = new Random((uint)Environment.TickCount);
            Container = new Container();
        }

        public IEnumerator Start()
        {
            // TODO: this should move to the FmodService probably
            while (FMODUnity.RuntimeManager.HasBankLoaded("Master Bank"))
            {
                yield return null;
            }
            Log.Info("Master Bank Loaded");
            Container.Start();
            _update = CoroutineManager.StartRoutine(Container.Update());
        }

        public void Dispose()
        {
            CoroutineManager.StopRoutine(_update);
            Container?.Dispose();
        }
    }
    
    public partial class Game
    {
        private static GameInstance _instance;
        public static GameInstance Instance => _instance ?? (_instance = new GameInstance());

        public static bool IsRunning => Application.isPlaying;

        public static bool IsShuttingDown { get; internal set; }

        #region Random

        public static Random Random => Instance.Random;

        #endregion
        
        #region Container

        public static T Bind<T>(string key = null) where T : new() => Instance.Container.Add<T>(key);
        public static T Bind<T>(T system, string key = null) => Instance.Container.Add(system, key);
        public static T Find<T>(string key = null) => Instance.Container.Get<T>(key);
        public static void Inject(object obj) => Instance.Container.Inject(obj);

        #endregion

        public static float ScreenHalfWidth => Screen.width * 0.5f;
        public static float ScreenHalfHeight => Screen.height * 0.5f;
        public static Vector3 ScreenCenter => new Vector3(ScreenHalfWidth, ScreenHalfHeight, 0f);
        
        public static void Initialize()
        {
            IsShuttingDown = false;
            Log.Always("Initialize RedOwl Game!");
            Application.quitting -= HandleQuit;
            Application.quitting += HandleQuit;
            _instance = new GameInstance();
#if REDOWL_DOTS
            DotsInit();
#endif
            // Register Systems Initialize
            // TODO: should these move to Container.Initialize?
            GameSettings.AvatarSettings.Initialize();
        }

        public static void Start()
        {
            CoroutineManager.StartRoutine(Instance.Start());
        }
        
        private static void HandleQuit()
        {
            IsShuttingDown = true;
            CoroutineManager.StopAllRoutines();
            Instance.Dispose();
        }
        
        public static void Quit()
        {
            IsShuttingDown = true;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#elif UNITY_STANDALONE 
            Application.Quit();
#elif UNITY_WEBGL
            Application.OpenURL("about:blank");
#endif
        }
    }
}