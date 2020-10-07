using System;
using UnityEngine;

namespace RedOwl.Core
{
    public abstract class IndexedBehaviour<T> : MonoBehaviour, IIndexable, ISerializationCallbackReceiver where T : IndexedBehaviour<T>
    {
        public static readonly IndexedList<T> All = new IndexedList<T>();
        
        public static int Count => All.Count;
        public static void Clear() => All.Clear();
        public static void Add(T item) => All.Add(item);
        public static void Remove(int index) => All.Remove(index);
        public static void Remove(Guid id) => All.Remove(id);
        public static void Remove(T item) => All.Remove(item);
        public static T Get(int index) => All[index];
        public static T Get(Guid id) => All.Get(id);
        public static T Next(T item) => All.Next(item);
        
        [SerializeField]
        private string id;
        public Guid Id { get; private set; }

        public virtual void Awake()
        {
            All.Add((T)this);
        }

        public virtual void OnDestroy()
        {
            Remove((T)this);
        }
        
        public virtual void OnBeforeSerialize()
        {
            id = Id.ToString();
        }

        public virtual void OnAfterDeserialize()
        {
            Id = Guid.Parse(id);
        }
    }
}