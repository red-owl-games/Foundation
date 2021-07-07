using System;
using System.Collections;
using QFSW.QC;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace RedOwl.Engine
{
    public static partial class Game
    {
        public static bool IsRunning => Application.isPlaying;
        public static Random Random { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            Log.Always("Game Initializing...");
            Random = new Random((uint)Environment.TickCount);
            if (!IsRunning)
            {
                Log.Always("Why is this being called?! We should probably remove any edit time calls to this");
                return;
            }
            
            Events.QuitGame.On += HandleQuit;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= HandlePlaymodeChanaged;
            UnityEditor.EditorApplication.playModeStateChanged += HandlePlaymodeChanaged;
#endif
            SetupStateMachines();
            Log.Always("Game Initialized!");
        }

        public static void Start()
        {
            StartRoutine(HandleStart());
        }

        [Command("ro.pause")]
        public static void Pause() => Events.PauseGame.Raise();

        [Command("ro.resume")]
        public static void Resume() => Events.ResumeGame.Raise();
        
        [Command("ro.show-loading")]
        public static void ShowLoadingScreen() => Events.ShowLoadingScreen.Raise();
        
        [Command("ro.hide-loading")]
        public static void HideLoadingScreen() => Events.HideLoadingScreen.Raise();
        
        [Command("ro.fade-out")]
        public static void FadeOut() => Events.FadeOut.Raise();
        
        [Command("ro.fade-in")]
        public static void FadeIn() => Events.FadeIn.Raise();

        [Command("ro.quit")]
        public static void Quit() => Events.QuitGame.Raise();

        private static IEnumerator HandleStart()
        {
            Log.Always("Game Starting...");
            yield return Container.Start();
            Log.Always("Game Started!");
        }

        private static void HandleQuit()
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