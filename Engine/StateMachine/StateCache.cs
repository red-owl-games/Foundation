using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Core
{
    internal class StateCache
    {
        private static int _nextId = 0;

        internal static int NextId
        {
            get
            {
                _nextId += 1;
                return _nextId;
            }
        }

        private static Dictionary<string, Type> _cache;
        
        public static IEnumerable<Type> All
        {
            get
            {
                ShouldBuildCache();
                return _cache.Values;
            }
        }
        
        public static IEnumerable<string> AllNames
        {
            get
            {
                ShouldBuildCache();
                return _cache.Keys;
            }
        }
        
        internal static bool TryGet(string name, out Type output)
        {
            ShouldBuildCache();
            return _cache.TryGetValue(name, out output);
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ShouldBuildCache()
        {
            if (_cache == null) BuildCache();
        }

        private static void BuildCache()
        {
            _cache = new Dictionary<string, Type>();
            foreach (var type in TypeExtensions.GetAllTypes<State>())
            {
                if (typeof(StateMachine).IsAssignableFrom(type)) continue;
                _cache.Add(type.Name, type);
            }
        }
    }
}