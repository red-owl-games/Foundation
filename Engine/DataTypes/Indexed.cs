using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Core
{
    public interface IIndexable
    {
        BetterGuid Id { get; }
    }
    
    public class IndexedList<T> : IEnumerable<T> where T : IIndexable
    {
        [NonSerialized]
        private List<T> _data;
        [NonSerialized]
        private Dictionary<BetterGuid, int> _lookupTable;

        public IndexedList(int count = 0)
        {
            _data = new List<T>(count);
            _lookupTable = new Dictionary<BetterGuid, int>(count);
        }

        public T this[int i] => i > _data.Count ? _data[0] : _data[i];
        public T this[BetterGuid id] => Get(id);

        public int Count => _data.Count;

        public void Clear()
        {
            _data.Clear();
            _lookupTable.Clear();
        }

        public void Add(T item)
        {
            _data.Add(item);
            
            try
            {
                _lookupTable.Add(item.Id, _data.Count);
            }
            catch (Exception e)
            {
                Log.Always($"Failed to Add {item.Id}");
                foreach (var key in _lookupTable.Keys)
                {
                    Log.Always(key.ToString());
                }
            }

        }

        public void Remove(int index)
        {
            if (index > _data.Count) return;
            Remove(_data[index]);
        }

        public void Remove(Guid id)
        {
            if (!_lookupTable.TryGetValue(id, out int index)) return;
            _data.RemoveAt(index);
            RebuildLookupTable();
        }

        public void Remove(T item)
        {
            if (!_lookupTable.TryGetValue(item.Id, out int index)) return;
            _data.RemoveAt(index);
            RebuildLookupTable();
        }

        public T Get(BetterGuid id)
        {
            return _lookupTable.TryGetValue(id, out int index) ? _data[index] : _data[0];
        }

        public T Next(T next)
        {
            int length = _data.Count;
            for (int i = 0; i < _data.Count; i++)
            {
                if (_data[i].Id != next.Id) continue;
                if (i + 1 < length)
                    return _data[i + 1];
            }

            return _data[0];
        }
        
        private void RebuildLookupTable()
        {
            _lookupTable.Clear();
            for (int i = 0; i < _data.Count; i++)
            {
                _lookupTable.Add(_data[i].Id, i);
            }
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [Serializable]
    public abstract class Indexed<T> : IIndexable where T : Indexed<T>
    {
        public static readonly IndexedList<T> All = new IndexedList<T>();
        public static int Count => All.Count;
        public static void Clear() => All.Clear();
        public static void Add(T item) => All.Add(item);
        public static void Remove(int index) => All.Remove(index);
        public static void Remove(BetterGuid id) => All.Remove(id);
        public static void Remove(T item) => All.Remove(item);
        public static T Get(int index) => All[index];
        public static T Get(BetterGuid id) => All.Get(id);
        public static T Next(T item) => All.Next(item);
        
        [SerializeField]
        private BetterGuid id;
        public BetterGuid Id => id;

        protected Indexed()
        {
            NewId();
        }

        public void NewId()
        {
            id = Guid.NewGuid();
        }
    }
}