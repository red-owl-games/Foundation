using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable]
    public class BetterDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] 
        private TKey[] keys;
        [SerializeField] 
        private TValue[] values;

        public BetterDictionary() : base() { }
        public BetterDictionary(int capacity) : base(capacity) { }
        public BetterDictionary(IEqualityComparer<TKey> comparer) : base(0, comparer) { }
        public BetterDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }
        public BetterDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary, null) { }
        public BetterDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary?.Count ?? 0, comparer) { }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            foreach (var value in Values)
            {
                if (value is ISerializationCallbackReceiver callback) callback.OnBeforeSerialize();
            }
            var count = Count;
            keys = new TKey[count];
            values = new TValue[count];
            Keys.CopyTo(keys, 0);
            Values.CopyTo(values, 0);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (keys == null || values == null) return;
            var keyCount = keys.Length;
            if (keyCount != values.Length) return;
            Clear();
            for (var i = 0; i < keyCount; i++) {
                this[keys[i]] = values[i];
            }
            keys = null;
            values = null;
        }
    }
}