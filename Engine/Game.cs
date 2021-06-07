using System;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace RedOwl.Engine
{
    public static partial class Game
    {
        public static bool IsRunning => Application.isPlaying;
        public static Random Random { get; private set; }

        internal static void Init()
        {
            Log.Always("Game Initialization...");
            Random = new Random((uint)Environment.TickCount);
            Services = new Container();
            if (!IsRunning) return;
            SetupStateMachines();
            Events.StartGame.On -= OnStart;
            Events.StartGame.On += OnStart;
            Events.QuitGame.On -= Quit;
            Events.QuitGame.On += Quit;
            Log.Always("Game Initialized!");
        }

        public static void Start() => Events.StartGame.Raise();

        public static void Pause() => Events.PauseGame.Raise();

        public static void Resume() => Events.ResumeGame.Raise();

        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#elif UNITY_STANDALONE 
            Application.Quit();
#elif UNITY_WEBGL
            HandleQuit();
            Application.OpenURL("about:blank");
#endif
        }

        private static void OnStart()
        {
            Log.Always("Game Started!");
            Services.Start();
            UnityBridge.Target = Services;
        }
    }
    
    public static class GameInit
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnAfterAssembliesLoaded() => Game.Init();
    }
}