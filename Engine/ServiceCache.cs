using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Core
{
    public class ServiceCache
    {
        private readonly Dictionary<Type, object> _cache = new Dictionary<Type, object>();

        public void Set<T>(object instance)
        {
            _cache[typeof(T)] = instance;
        }

        public T Get<T>()
        {
            return (T)_cache[typeof(T)];
        }

        public void Reset()
        {
            _cache.Clear();
        }
    }
}