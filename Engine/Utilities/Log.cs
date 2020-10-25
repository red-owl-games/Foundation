using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Print = UnityEngine.Debug;

namespace RedOwl.Engine
{
    public enum LogLevel {
        Error,
        Warn,
        Info,
        Debug
    }
    
    [Serializable]
    public class LogSettings : Settings
    {
        public LogLevel LogLevel = LogLevel.Warn;
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
            if (Game.LogSettings.LogLevel >= LogLevel.Debug)
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
            if (Game.LogSettings.LogLevel >= LogLevel.Info)
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
            if (Game.LogSettings.LogLevel >= LogLevel.Warn)
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
            if (Game.LogSettings.LogLevel >= LogLevel.Error)
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