using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
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
    
    public partial class RedOwlSettings
    {
        [SerializeField]
        private LogLevel logLevel = LogLevel.Warn;

        public static LogLevel LogLevel => I != null ? I.logLevel : LogLevel.Warn;
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
            if (RedOwlSettings.LogLevel >= LogLevel.Debug)
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
            if (RedOwlSettings.LogLevel >= LogLevel.Info)
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
            if (RedOwlSettings.LogLevel >= LogLevel.Warn)
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
            if (RedOwlSettings.LogLevel >= LogLevel.Error)
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