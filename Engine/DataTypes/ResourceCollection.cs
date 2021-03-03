using System.Collections.ObjectModel;
using UnityEngine;

namespace RedOwl.Engine
{
    public class ResourceReference<T> where T : Object
    {
        public string Path;
        private T _asset;
        public T Asset
        {
            get => _asset == null ? Resources.Load<T>(Path) : _asset;
            set => _asset = value;
        }

        public static implicit operator T(ResourceReference<T> reference) => reference.Asset;
    }

    public class ResourceCollection<T> : KeyedCollection<string, ResourceReference<T>> where T : Object
    {
        public ResourceCollection(string path = "", T asset = null)
        {
            Add(new ResourceReference<T>{ Path = path, Asset = asset });
        }
        
        protected override string GetKeyForItem(ResourceReference<T> item) => item.Path;
        
        public int Ensure(string path, T item)
        {
            if (item == null) return 0;
            if (!Dictionary.TryGetValue(path, out var found))
            {
                Debug.Log($"Creating New ResourceReference - '{path}'");
                found = new ResourceReference<T> {Path = path, Asset = item};
                Add(found);
            }
            return IndexOf(found);
        }

        public int Ensure(string path) => Ensure(path, Resources.Load<T>(path));

        public T[] ToArray()
        {
            var count = Count;
            var output = new T[count];
            for (int i = 0; i < count; i++)
            {
                output[i] = this[i].Asset;
            }
            return output;
        }
    }
}