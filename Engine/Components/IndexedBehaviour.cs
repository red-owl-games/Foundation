using System;
using UnityEngine;

namespace RedOwl.Engine
{
    public abstract class IndexedBehaviour<T> : MonoBehaviour, IIndexable where T : IndexedBehaviour<T>
    {
        private static IndexedList<T> all;

        public static IndexedList<T> All
        {
            get => all ?? (all = new IndexedList<T>());
            set => all = value;
        }
        
        public static int Count => All.Count;
        public static void Clear() => All.Clear();
        public static void Add(T item) => All.Add(item);
        public static void Remove(int index) => All.Remove(index);
        public static void Remove(BetterGuid id) => All.Remove(id);
        public static void Remove(T item) => All.Remove(item);
        public static T Get(int index) => All[index];
        public static T Get(BetterGuid id) => All.Get(id);
        public static T Next(T item) => All.Next(item);

        public BetterGuid Id { get; private set; }

        protected void Awake()
        {
            Id = Guid.NewGuid();
            All.Add((T)this);
            AfterAwake();
        }

        protected virtual void AfterAwake() {}

        protected void OnDestroy()
        {
            Remove((T)this);
            AfterDestory();
        }

        protected virtual void AfterDestory() {}

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            All = null;
        }
    }
}