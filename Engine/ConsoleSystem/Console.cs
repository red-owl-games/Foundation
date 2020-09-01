using System;
using UnityEngine;

namespace RedOwl.Core
{
    public static class Console
    {
        public static RingBuffer<string> Logs;
        
        internal static void OnUnityLog(string message, string stack, LogType logtype)
        {
            switch (logtype)
            {
                case LogType.Log:
                    if (RedOwlSettings.LogLevel >= LogLevel.Info) Write(message);
                    break;
                case LogType.Warning:
                    if (RedOwlSettings.LogLevel >= LogLevel.Warn) Write(message);
                    break;
                case LogType.Error:
                    if (RedOwlSettings.LogLevel >= LogLevel.Error) Write(message);
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