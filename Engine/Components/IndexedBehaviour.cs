using System;
using UnityEngine;

namespace RedOwl.Core
{
    public abstract class IndexedBehaviour<T> : MonoBehaviour, IIndexable where T : IndexedBehaviour<T>
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
        private BetterGuid id = Guid.NewGuid();

        public BetterGuid Id => id;

        public virtual void Awake()
        {
            All.Add((T)this);
        }

        public virtual void OnDestroy()
        {
            Remove((T)this);
        }
    }
}