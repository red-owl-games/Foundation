using System;
using System.Collections;
using System.Collections.Generic;

namespace RedOwl.Engine
{
    public abstract class Factory<T> : Container, IEnumerable<string> where T : class, new()
    {
        private readonly Dictionary<string, Type> _types;

        protected Factory()
        {
            var types = new List<Type>(TypeExtensions.GetAllSubclasses<T>());
            _types = new Dictionary<string, Type>(types.Count);
            foreach (var type in types)
            {
                if (!(Activator.CreateInstance(type) is T tmp)) continue;
                _types.Add(type.Name, type);
            }
        }

        public IEnumerator<string> GetEnumerator() => _types.Keys.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}