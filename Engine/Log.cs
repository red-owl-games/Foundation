﻿using System;
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
        // These fix a problem with 'dynamic' objects being passed to the API methods
        [Conditional("UNITY_EDITOR")]
        private static void log(string message)
        {
            Print.Log($"[RedOwl] {message}");
        }
        
        [Conditional("UNITY_EDITOR")]
        private static void logWarning(string message)
        {
            Print.LogWarning($"[RedOwl] {message}");
        }
        
        [Conditional("UNITY_EDITOR")]
        private static void logError(string message)
        {
            Print.LogError($"[RedOwl] {message}");
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Always(string message)
        {
            log($"<color=maroon>{message}</color>");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Debug(string message)
        {
            if (RedOwlSettings.LogLevel >= LogLevel.Debug) log($"<color=grey>{message}</color>");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Info(string message)
        {
            if (RedOwlSettings.LogLevel >= LogLevel.Info) log($"<color=teal>{message}</color>");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Warn(string message)
        {
            if (RedOwlSettings.LogLevel >= LogLevel.Warn) logWarning($"<color=yellow>{message}</color>");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Error(string message)
        {
            if (RedOwlSettings.LogLevel >= LogLevel.Error) logError($"<color=red>{message}</color>");
        }
    }
}