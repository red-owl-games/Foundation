using System;
using System.Collections.Generic;

namespace RedOwl.Core
{
    public abstract class TypeCache<T>
    {
        private Dictionary<string, Type> _cache;
        
        public IEnumerable<Type> All
        {
            get
            {
                ShouldBuildCache();
                return _cache.Values;
            }
        }
        
        public void ShouldBuildCache()
        {
            if (_cache == null) BuildCache();
        }

        private void BuildCache()
        {
            _cache = new Dictionary<string, Type>();
            foreach (var type in TypeExtensions.GetAllTypes<T>())
            {
                if (ShouldCache(type))
                    _cache.Add(type.Name, type);
            }
        }

        protected abstract bool ShouldCache(Type type);
    }
}