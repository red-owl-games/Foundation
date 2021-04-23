using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    public interface IConsoleService
    {
        void Clear();
        IEnumerable<ICommandRegistration> Commands { get; }
    }
    
    public class ConsoleService : IServiceInit, IConsoleService, IDisposable
    {
        private Dictionary<string, ICommandRegistration> _commands;
        private RingBuffer<string> _logs;
        private RingBuffer<string> _commandHistory;
        private int _commandHistoryIndex = -1;
        private IConsoleView _consoleView;

        public IEnumerable<ICommandRegistration> Commands => _commands.Values;
        
        public void Init()
        {
            //Log.Always("Console Initialization!");
            SetupView();
            FindCommands();
            SetupLogCollection();
        }

        public void Dispose()
        {
            GameSettings.ConsoleSettings.ShowConsoleAction.Disable();
        }

        #region View
        
        private void SetupView()
        {
            if (GameSettings.ConsoleSettings.prefab != null)
            {
                var go = Object.Instantiate(GameSettings.ConsoleSettings.prefab);
                _consoleView = go.GetComponent<IConsoleView>();
                Object.DontDestroyOnLoad(go);
                GameSettings.ConsoleSettings.ShowConsoleAction.performed += ctx => _consoleView.Toggle();
                GameSettings.ConsoleSettings.ShowConsoleAction.Enable();
            }
        }
        
        #endregion

        #region Commands
        
        private void FindCommands()
        {
            _commands = new Dictionary<string, ICommandRegistration>();
            _commandHistory = new RingBuffer<string>(GameSettings.ConsoleSettings.HistoryBufferLength);
            _commandHistory.PushFront("help");
            _commandHistory.PushFront("clear");
            // TODO: Could Probably just support Classes
            FindCommandMethods();
            FindCommandClasses();
        }

        private void FindCommandClasses()
        {
            foreach (var type in TypeExtensions.GetAllTypesWithAttribute<Command>())
            {
                var attribute = type.GetCustomAttribute<Command>();
                var command = new ConsoleCommandClass(attribute.Name, attribute.Description, (ICommand)Activator.CreateInstance(type));
                _commands.Add(attribute.Name, command);
            }
        }

        private void FindCommandMethods()
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
        
        public void RunCommand(string[] parsed)
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
        
        #endregion
        
        #region Logs
        
        private void SetupLogCollection()
        {
            _logs = new RingBuffer<string>(GameSettings.ConsoleSettings.BufferLength);
            Application.logMessageReceived += OnUnityLog;
        }
        
        public void Write(string message)
        {
            _logs.PushFront($"<color=grey>[{DateTime.Now:HH:mm:ss}] {message}</color>");
        }
        
        private void OnUnityLog(string message, string stack, LogType logtype)
        {
            switch (logtype)
            {
                case LogType.Log:
                    if (GameSettings.LogSettings.LogLevel >= LogLevel.Info) Write(message);
                    break;
                case LogType.Warning:
                    if (GameSettings.LogSettings.LogLevel >= LogLevel.Warn) Write(message);
                    break;
                case LogType.Error:
                    if (GameSettings.LogSettings.LogLevel >= LogLevel.Error) Write(message);
                    break;
                case LogType.Assert:
                    break;
                case LogType.Exception:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logtype), logtype, null);
            }
        }

        public void Clear() => _logs.Clear();

        #endregion
    }
    
    public partial class Game
    {
        public static IConsoleService Console => Find<IConsoleService>();

        public static void BindConsoleService() => Bind<IConsoleService>(new ConsoleService());
    }
}