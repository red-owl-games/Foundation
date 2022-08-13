using System.Collections.Generic;
using JetBrains.Annotations;

namespace RedOwl.Engine
{
    public class Container<TItem>
    {
        private readonly Dictionary<string, TItem> _cache;

        public ICollection<TItem> All => _cache.Values;

        public Container()
        {
            _cache = new Dictionary<string, TItem>();
        }

        [UsedImplicitly]
        public T Add<T>(string key = null) where T : TItem, new()
        {
            return Add(new T (), key);
        }

        public T Add<T>(T item, string key = null) where T : TItem 
        {
            _cache.Add(key ?? typeof(T).FullName ?? typeof(T).Name, item);
            OnAdd(item);
            return item;
        }
        
        protected virtual void OnAdd<T>(T item) where T : TItem {}

        public T Get<T>(string key = null) where T : TItem
        {
            key ??= typeof(T).FullName ?? typeof(T).Name;
            return _cache.ContainsKey(key) ? (T)_cache[key] : default;
        }
    }
}