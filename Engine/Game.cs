using System;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace RedOwl.Engine
{
    public static partial class Game
    {
        public static bool IsRunning => Application.isPlaying;
        public static Random Random { get; private set; }

        public static void Init()
        {
            Log.Always("Game Initializing...");
            Random = new Random((uint)Environment.TickCount);
            if (!IsRunning) return;
            SetupStateMachines();
            Events.QuitGame.On -= Quit;
            Events.QuitGame.On += Quit;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= HandlePlaymodeChanaged;
            UnityEditor.EditorApplication.playModeStateChanged += HandlePlaymodeChanaged;
#endif
            Log.Always("Game Initialized!");
        }

        public static void Start()
        {
            Log.Always("Game Starting...");
            Events.StartGame.Raise();
            Container.Start();
            UnityBridge.Target = Container;
            Log.Always("Game Started!");
        }

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
        
#if UNITY_EDITOR
        private static void HandlePlaymodeChanaged(UnityEditor.PlayModeStateChange change)
        {
            Log.Always($"Game '{change}'");
            if (change == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                container = null;
            }
        }
#endif
    }
}