using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;

namespace RedOwl.Engine
{
    public abstract class Pool<T> : RedOwlScriptableObject
    {
        [HorizontalGroup("Settings")]
        [LabelWidth(50), DisableInPlayMode]
        public int size = 10;

        protected readonly Stack<T> Available = new Stack<T>();

        [BoxGroup("Runtime Info")]
        [ShowInInspector, PropertyOrder(100000)]
        private int AvailableCount => Available.Count;

        protected bool HasBeenPrewarmed { get; private set; }

        protected void Enlarge(int num)
        {
            for (int i = 0; i < num; i++)
            {
                Available.Push(Create());
            }
        }

        [ButtonGroup("Controls")]
        [Button(ButtonSizes.Large), DisableInEditorMode]
        public void Prewarm()
        {
            if (HasBeenPrewarmed) return;
            OnPrewarm();
            HasBeenPrewarmed = true;
        }

        protected virtual void OnPrewarm()
        {
            Enlarge(size);
        }

        protected virtual T BeforeRequest(T member)
        {
            return member;
        }

        public T Request()
        {
            return BeforeRequest(Available.Count > 0 ? Available.Pop() : Create());
        }
        
        public IEnumerable<T> Request(int num)
        {
            math.clamp(num, 0, int.MaxValue);
            var available = Available.Count;
            if (num > available)
            {
                Enlarge(1 + num - available);
            }
            List<T> members = new List<T>(num);
            for (int i = 0; i < num; i++)
            {
                var member = Available.Pop();
                BeforeRequest(member);
                members.Add(member);
            }
            return members;
        }
        
        public void Return(T member)
        {
            AfterReturn(member);
            Game.DelayedCall(() =>
            {
                Available.Push(member);
            }, 1f);
        }

        public void Return(IEnumerable<T> members)
        {
            foreach (T member in members)
            {
                Return(member);
            }
        }
        
        protected virtual void AfterReturn(T member) {}
        
        public virtual void OnDisable()
        {
            HasBeenPrewarmed = false;
        }

        protected abstract T Create();
    }
}