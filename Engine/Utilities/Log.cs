using System;
using System.Diagnostics;
using Sirenix.OdinInspector;
using Print = UnityEngine.Debug;

namespace RedOwl.Engine
{
    public enum LogLevel {
        Error,
        Warn,
        Info,
        Debug
    }
    
    #region Settings
    
    [Serializable, InlineProperty, HideLabel]
    public class LogSettings
    {
        public LogLevel LogLevel = LogLevel.Info;
    }

    #endregion
    
    public static class Log
    {
        private static LogSettings _settings;

        public static LogSettings Settings
        {
            get => _settings ??= new LogSettings();
            set => _settings = value;
        }

        [Conditional("DEBUG")]
        public static void Always(string message)
        {
            Print.Log($"[RedOwl] <color=lightblue>{message}</color>");
        }

        [Conditional("DEBUG")]
        public static void Debug(string message)
        {
            if (Settings.LogLevel >= LogLevel.Debug)
            {
                Print.Log($"[RedOwl] <color=grey>{message}</color>");
            }
        }

        [Conditional("DEBUG")]
        public static void Info(string message)
        {
            if (Settings.LogLevel >= LogLevel.Info)
            {
                Print.Log($"[RedOwl] <color=teal>{message}</color>");
            }
        }

        [Conditional("DEBUG")]
        public static void Warn(string message)
        {
            if (Settings.LogLevel >= LogLevel.Warn)
            {
                Print.LogWarning($"[RedOwl] <color=yellow>{message}</color>");
            }
        }

        [Conditional("DEBUG")]
        public static void Error(string message)
        {
            if (Settings.LogLevel >= LogLevel.Error)
            {
                Print.LogError($"[RedOwl] <color=red>{message}</color>");
            }
        }
    }
}