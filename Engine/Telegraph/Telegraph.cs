using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace RedOwl.Engine
{
    public interface ITelegram {}
    
    public static class Telegraph
    {
        private static Dictionary<Type, Dictionary<string, ITelegram>> _cache = new Dictionary<Type, Dictionary<string, ITelegram>>();

        private static Dictionary<string, ITelegram> GetTable(Type type)
        {
            if (!_cache.TryGetValue(type, out var table))
            {
                table = new Dictionary<string, ITelegram>();
                _cache[type] = table;
            }

            return table;
        }

        public static T Get<T>(string name) where T : class, ITelegram, new()
        {
            var type = typeof(T);
            var table = GetTable(type);

            if (!table.TryGetValue(name, out var telegram))
            {
                Debug.Log($"Creating New Telegram '{name}'");
                telegram = new T();
                table[name] = telegram;
            }

            return (T)telegram;
        }

        public static T Get<T>(Enum name) where T : class, ITelegram, new()
        {
            return Get<T>(name.ToString());
        }
        
        public static IEnumerable GetEvents<T>() where T : class, ITelegram
        {
            foreach (var kvp in GetTable(typeof(T)))
            {
                if (kvp.Value is T) yield return kvp.Key;
            }
        }
        
        internal const string MENU_PATH = "Red Owl/Channel/";

        [Conditional("UNITY_EDITOR")]
        internal static void RenameChannel(RedOwlScriptableObject channel, string name)
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.RenameAsset(UnityEditor.AssetDatabase.GetAssetPath(channel.GetInstanceID()), $"{UnityEditor.ObjectNames.NicifyVariableName(name.Replace("/", "_"))} Channel");
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}