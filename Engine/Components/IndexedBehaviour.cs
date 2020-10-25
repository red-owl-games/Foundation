using System;
using UnityEngine;

namespace RedOwl.Engine
{
    public abstract class IndexedBehaviour<T> : MonoBehaviour, IIndexable where T : IndexedBehaviour<T>
    {
        public static IndexedList<T> All { get; } = new IndexedList<T>();
        
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

        protected void Awake()
        {
            All.Add((T)this);
            AfterAwake();
        }

        protected virtual void AfterAwake()
        {
            
        }

        protected void OnDestroy()
        {
            Remove((T)this);
            AfterDestory();
        }

        protected virtual void AfterDestory()
        {
            
        }
    }
}