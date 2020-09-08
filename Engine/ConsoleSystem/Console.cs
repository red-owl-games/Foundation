using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace RedOwl.Core
{
    [Serializable]
    public class ConsoleSettings : Settings<ConsoleSettings>
    {
        [SerializeField]
        [LabelWidth(85), LabelText("Font Size")]
        private int fontSize = 16;
        public static int FontSize => Instance.fontSize;
        
        [SerializeField]
        [LabelWidth(85), LabelText("Buffer Size")]
        private int bufferLength = 5000;
        public static int BufferLength => Instance.bufferLength;
        
        [SerializeField]
        [LabelWidth(120), LabelText("History Buffer Size")]
        private int historyBufferLength = 100;
        public static int HistoryBufferLength => Instance.historyBufferLength;
        
        [SerializeField]
        [LabelWidth(80), LabelText("Key To Show")]
        private InputAction showConsoleKey = new InputAction();
        public static InputAction ShowConsoleKey => Instance.showConsoleKey;
    }
    
    public static class Console
    {
        public static RingBuffer<string> Logs;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeBeforeSplashScreen()
        {
            Logs = new RingBuffer<string>(ConsoleSettings.BufferLength);
            Application.logMessageReceived += OnUnityLog;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializeAfterSceneLoad()
        {
            Object.DontDestroyOnLoad(new GameObject("Console").AddComponent<ConsoleUI>());
            Log.Always("Console Initialization!");
        }
        
        internal static void OnUnityLog(string message, string stack, LogType logtype)
        {
            switch (logtype)
            {
                case LogType.Log:
                    if (LogConfig.LogLevel >= LogLevel.Info) Write(message);
                    break;
                case LogType.Warning:
                    if (LogConfig.LogLevel >= LogLevel.Warn) Write(message);
                    break;
                case LogType.Error:
                    if (LogConfig.LogLevel >= LogLevel.Error) Write(message);
                    break;
                case LogType.Assert:
                    break;
                case LogType.Exception:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logtype), logtype, null);
            }
        }
        
        public static void Write(string message)
        {
            Logs.PushFront($"<color=grey>[{DateTime.Now:HH:mm:ss}] {message}</color>");
        }

        public static void Run(string command) { Run(command.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries));}
        public static void Run(string[] command)
        {
            // if (CommandCache.Get(command[0], out CommandRegistration cmd))
            //     cmd.Invoke(command);
            // else
            //     Log.Warn($"Unable to find command: '{command[0]}'");
        }
    }
}