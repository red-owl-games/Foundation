using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RedOwl.Engine
{
    [Serializable]
    public class ConsoleSettings : Settings
    {
        [LabelText("Font Size")]
        public int FontSize = 16;
        
        [LabelText("Buffer Size")]
        public int BufferLength = 5000;
        
        [LabelText("History Buffer Size")]
        public int HistoryBufferLength = 100;
        
        public InputAction ShowConsoleAction = new InputAction();
        
        [AssetsOnly, AssetSelector(Filter = "t:IConsoleView")]
        public GameObject prefab;
    }
    
    public partial class GameSettings
    {
        [FoldoutGroup("Console"), SerializeField]
        private ConsoleSettings consoleSettings = new ConsoleSettings();
        public static ConsoleSettings ConsoleSettings => Instance.consoleSettings;
    }
}