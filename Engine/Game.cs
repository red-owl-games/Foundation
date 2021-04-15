using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Entities;
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
    
    [Singleton]
    public partial class Game : Asset<Game>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            Log.Always("OnSubsystemRegistration RedOwl Game!");
            Container = new Container();
            Services = new ServiceCache();
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void OnBeforeSplashScreen()
        {
            Log.Always("OnBeforeSplashScreen RedOwl Game!");
            Application.quitting += HandleQuit;
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        internal static void Initialize()
        {
            Log.Always("Initialize RedOwl Game!");
            DotsInit();
        }

        public static bool IsRunning => Application.isPlaying;

        public static bool IsShuttingDown { get; internal set; }
        
        private static void HandleQuit()
        {
            IsShuttingDown = true;
            CoroutineManager.StopAllRoutines();
            Dispose();
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
        
        #region Random

        public static Random Random => new Random((uint)Environment.TickCount);

        #endregion
        
        #region Container

        public static Container Container { get; private set; }
        
        public static T Add<T>(string key = null) where T : IService, new() => Container.AddService<T>(key);

        public static T Add<T>(T system, string key = null) where T : IService => Container.AddService(system, key);

        public static T Get<T>(string key = null) where T : IService => Container.GetService<T>(key);

        public static ICollection<IService> Systems() => Container.Services();
        
        public static void Dispose() => Container.Dispose();

        #endregion
        
        #region Services
        
        //TODO: Combind "container" and "service cache"?
        public static ServiceCache Services { get; private set; }

        public static void Bind<T>(T instance) => Services.Bind(instance);
        public static void BindAs<T>(T instance) => Services.BindAs(instance);
        public static T Find<T>() => Services.Find<T>();
        public static void Inject(object obj) => Services.Inject(obj);
        
        #endregion

        public static float ScreenHalfWidth => Screen.width * 0.5f;
        public static float ScreenHalfHeight => Screen.height * 0.5f;
        public static Vector3 ScreenCenter => new Vector3(ScreenHalfWidth, ScreenHalfHeight, 0f);

        public static void Start()
        {
            Container.Start();
        }
    }
}