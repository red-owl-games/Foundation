using System;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface IPoolable<T>
    {
        void Initialize(Action<T> returnAction);
        void ReturnToPool();
    }
    
    public class Poolable : MonoBehaviour, IPoolable<Poolable>
    {
        private Action<Poolable> _returnAction;

        private void OnDisable()
        {
            ReturnToPool();
        }

        public void Initialize(Action<Poolable> returnAction)
        {
            _returnAction = returnAction;
        }

        public void ReturnToPool()
        {
            _returnAction?.Invoke(this);
        }
    }
}