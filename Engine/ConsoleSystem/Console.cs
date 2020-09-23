using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        [LabelText("Font Size")]
        private int fontSize = 16;
        public static int FontSize => Instance.fontSize;
        
        [SerializeField]
        [LabelText("Buffer Size")]
        private int bufferLength = 5000;
        public static int BufferLength => Instance.bufferLength;
        
        [SerializeField]
        [LabelText("History Buffer Size")]
        private int historyBufferLength = 100;
        public static int HistoryBufferLength => Instance.historyBufferLength;
        
        [SerializeField]
        private InputAction showConsoleAction = new InputAction();
        public static InputAction ShowConsoleAction => Instance.showConsoleAction;
    }
    
    [Command("clear", "Clears the console GUI's Log Text")]
    public class ClearCommand : ICommand
    {
        public void Invoke(string[] args) => Console.Clear();
    }
    
    [Command("help", "Prints all Commands and their Descriptions")]
    public class HelpCommand : ICommand
    {
        public void Invoke(string[] args) => Console.Help();
    }
    
    public static class Console
    {
        public static RingBuffer<string> Logs;
        
        private static Dictionary<string, ICommandRegistration> _commands;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void InitializeBeforeSplashScreen()
        {
            _commands = new Dictionary<string, ICommandRegistration>();
            // TODO: Could Probably just support Classes
            FindCommandMethods();
            FindCommandClasses();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeBeforeSceneLoad()
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
        
        private static void FindCommandClasses()
        {
            foreach (var type in TypeExtensions.GetAllTypesWithAttribute<Command>())
            {
                var attribute = type.GetCustomAttribute<Command>();
                var command = new ConsoleCommandClass(attribute.Name, attribute.Description, (ICommand)Activator.CreateInstance(type));
                _commands.Add(attribute.Name, command);
            }
        }

        private static void FindCommandMethods()
        {
            foreach (var type in TypeExtensions.GetAllTypes())
            {
                foreach (var methodInfo in type.GetMethodsWithAttribute<Command>())
                {
                    if (methodInfo.IsStatic == false) continue;
                    var attribute = methodInfo.GetCustomAttribute<Command>();
                    Log.Always($"Building Command for: '{attribute.Name}' | '{attribute.Description}'");
                    var command = new ConsoleCommand(attribute.Name, attribute.Description, (Action)methodInfo.CreateDelegate(typeof(Action)));
                    _commands.Add(command.Name, command);
                }
            }
        }
        
        private static void OnUnityLog(string message, string stack, LogType logtype)
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

        
        public static void Clear()
        {
            Logs.Clear();
        }
        
        public static void Help()
        {
            var commands = new List<ICommandRegistration>(_commands.Values);
            commands.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
            Log.Always("----------HELP----------");
            foreach (var command in commands)
            {
                Log.Always($"{command.Name} - {command.Description}");
            }
            Log.Always("----------HELP----------");
        }
        
        public static void Write(string message)
        {
            Logs.PushFront($"<color=grey>[{DateTime.Now:HH:mm:ss}] {message}</color>");
        }

        public static void Run(string[] parsed)
        {
            string name = parsed[0].ToLower();
            if (_commands.TryGetValue(name, out ICommandRegistration command))
            {
                command.Invoke(parsed.Skip(1).ToArray());
            }
            else
            {
                Log.Warn($"Unable to find command: '{name}'");
            }
        }
    }
}