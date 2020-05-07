using System;
using System.Collections.Generic;

namespace RedOwl.Core
{
    public class ServiceCache
    {
        private readonly Dictionary<Type, object> _cache = new Dictionary<Type, object>();

        public void Set<T>(object serviceInstance)
        {
            _cache[typeof(T)] = serviceInstance;
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