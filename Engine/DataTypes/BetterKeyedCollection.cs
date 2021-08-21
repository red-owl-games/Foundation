using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable]
    public abstract class BetterKeyedCollection<TKey, TValue> : ICollection<TValue>
    {
        [SerializeReference]
        private List<TValue> collection;
        [SerializeField]
        private BetterDictionary<TKey, int> lookup;

        protected BetterKeyedCollection()
        {
            collection = new List<TValue>();
            lookup = new BetterDictionary<TKey, int>();
        }

        protected BetterKeyedCollection(int capacity)
        {
            collection = new List<TValue>(capacity);
            lookup = new BetterDictionary<TKey, int>(capacity);
        }
        
        protected BetterKeyedCollection(ICollection<TValue> data) : this(data.Count)
        {
            foreach (var item in data)
            {
                Add(item);
            }
        }
        
        #region Shared
        
        public int Count => collection.Count;

        public bool IsReadOnly => false;

        public void Clear()
        {
            collection.Clear();
            lookup.Clear();
        }

        public IEnumerator<TValue> GetEnumerator() => collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
        
        #region ICollection

        public bool Contains(TValue item) => collection.Contains(item);

        public void CopyTo(TValue[] array, int index) => collection.CopyTo(array, index);

        public void Add(TValue item)
        {
            lookup[GetKeyForItem(item)] = collection.Count;
            collection.Add(item);
        }

        public void Remove(TValue value)
        {
            for (int i = collection.Count - 1; i >= 0; i--)
            {
                if (collection[i].Equals(value))
                {
                    lookup.Remove(GetKeyForItem(collection[i]));
                    collection.RemoveAt(i);
                }
            }
        }

        public void Remove(int index) => collection.RemoveAt(index);
         
        bool ICollection<TValue>.Remove(TValue item) => collection.Remove(item);

        #endregion

        #region IDictionary

        public TValue this[TKey key] => collection[lookup[key]];

        public bool ContainsKey(TKey key) => lookup.ContainsKey(key);

        public bool Remove(TKey key)
        {
            if (!ContainsKey(key))
            {
                return false;
            }
            collection.RemoveAt(lookup[key]);
            lookup.Remove(key);
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            try
            {
                value = this[key];
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }

        public ICollection<TKey> Keys => lookup.Keys;
        public ICollection<TValue> Values => collection;
        
        #endregion
        
        public TValue this[int index] => collection[index];
        
        public TValue Next(TValue item)
        {
            var key = GetKeyForItem(item);
            if (!ContainsKey(key)) return default;
            int next = lookup[key] + 1;
            if (next > collection.Count)
            {
                return collection[0];
            }

            return collection[next];
        }
        
        protected abstract TKey GetKeyForItem(TValue item);
    }
}