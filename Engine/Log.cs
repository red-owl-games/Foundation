using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Print = UnityEngine.Debug;

namespace RedOwl.Core
{
    public enum LogLevel {
        Error,
        Warn,
        Info,
        Debug
    }
    
    [Serializable]
    public class LogConfig : Settings<LogConfig>
    {
        [SerializeField]
        private LogLevel logLevel = LogLevel.Warn;
        public static LogLevel LogLevel => Instance?.logLevel ?? LogLevel.Debug;
    }
    
    public static class Log
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Always(string message)
        {
#if DEBUG
            Print.Log($"[RedOwl] <color=maroon>{message}</color>");
#else 
            Console.Write($"[RedOwl] <color=maroon>{message}</color>");
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Debug(string message)
        {
            if (LogConfig.LogLevel >= LogLevel.Debug)
            {
#if DEBUG
                Print.Log($"[RedOwl] <color=grey>{message}</color>");
#else 
                Console.Write($"[RedOwl] <color=grey>{message}</color>");
#endif
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Info(string message)
        {
            if (LogConfig.LogLevel >= LogLevel.Info)
            {
#if DEBUG
                Print.Log($"[RedOwl] <color=teal>{message}</color>");
#else 
                Console.Write($"[RedOwl] <color=teal>{message}</color>");
#endif
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Warn(string message)
        {
            if (LogConfig.LogLevel >= LogLevel.Warn)
            {
#if DEBUG
                Print.LogWarning($"[RedOwl] <color=yellow>{message}</color>");
#else 
                Console.Write($"[RedOwl] <color=yellow>{message}</color>");
#endif
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Error(string message)
        {
            if (LogConfig.LogLevel >= LogLevel.Error)
            {
#if DEBUG
                Print.LogError($"[RedOwl] <color=red>{message}</color>");
#else 
                Console.Write($"[RedOwl] <color=red>{message}</color>");
#endif
            }
        }
    }
}