using System.Diagnostics;
using Print = UnityEngine.Debug;

namespace RedOwl.Engine
{
    public enum LogLevel
    {
        Error,
        Warn,
        Info,
        Debug
    }

    public static class Log
    {
        [ClearOnReload] private static Game _game;
        private static LogLevel Current
        {
            get
            {
                _game ??= Game.Instance;
                if (_game == null) return LogLevel.Info;
                return _game.Prefs.LogLevel;
            }
        }

        [Conditional("DEBUG")]
        public static void Always(string message)
        {
            Print.Log($"[RedOwl] <color=lightblue>{message}</color>");
        }

        [Conditional("DEBUG")]
        public static void Debug(string message)
        {
            if (Current >= LogLevel.Debug)
            {
                Print.Log($"[RedOwl] <color=grey>{message}</color>");
            }
        }

        [Conditional("DEBUG")]
        public static void Info(string message)
        {
            if (Current >= LogLevel.Info)
            {
                Print.Log($"[RedOwl] <color=teal>{message}</color>");
            }
        }

        [Conditional("DEBUG")]
        public static void Warn(string message)
        {
            if (Current >= LogLevel.Warn)
            {
                Print.LogWarning($"[RedOwl] <color=yellow>{message}</color>");
            }
        }

        [Conditional("DEBUG")]
        public static void Error(string message)
        {
            if (Current >= LogLevel.Error)
            {
                Print.LogError($"[RedOwl] <color=red>{message}</color>");
            }
        }
    }
}