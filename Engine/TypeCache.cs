using System.Collections.Generic;

namespace RedOwl.Core
{
    public class TypeCache
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

        public T Get<T>()
        {
            return (T)_cache[typeof(T).SafeGetName()];
        }

        public void Set<T>(T obj)
        {
            _cache[typeof(T).SafeGetName()] = obj;
        }
        
        public void Reset()
        {
            _cache.Clear();
        }
    }
}