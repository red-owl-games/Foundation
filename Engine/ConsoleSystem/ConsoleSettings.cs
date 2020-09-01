using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RedOwl.Core
{
    public partial class RedOwlSettings
    {
        [SerializeField]
        [FoldoutGroup("Console System")]
        [LabelWidth(85), LabelText("Font Size")]
        internal int consoleFontSize = 16;
        public static int ConsoleFontSize => I.consoleFontSize;
        
        [SerializeField]
        [FoldoutGroup("Console System")]
        [LabelWidth(85), LabelText("Buffer Size")]
        private int consoleBufferLength = 5000;
        public static int ConsoleBufferLength => I.consoleBufferLength;
        
        [SerializeField]
        [FoldoutGroup("Console System")]
        [LabelWidth(120), LabelText("History Buffer Size")]
        private int consoleHistoryBufferLength = 100;
        public static int ConsoleHistoryBufferLength => I.consoleHistoryBufferLength;
        
        [SerializeField]
        [FoldoutGroup("Console System")]
        [LabelWidth(80), LabelText("Key To Show")]
        private InputAction showConsoleKey = new InputAction();
        public static InputAction ShowConsoleKey => I.showConsoleKey;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void InitializeBeforeSplashScreen()
        {
            Console.Logs = new RingBuffer<string>(ConsoleBufferLength);
            Application.logMessageReceived += Console.OnUnityLog;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializeAfterSceneLoad()
        {
            DontDestroyOnLoad(new GameObject("Console").AddComponent<ConsoleUI>());
            Log.Always("Console Initialization!");
        }
    }
}