using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    [Serializable]
    public class InterfaceReference<T> where T : class
    {
        public Object reference;

        private T _instance;

        public T Get() {
            if (_instance != null) return _instance;
            var go = reference as GameObject;
            if (go != null) {
                _instance = go.GetComponentInChildren<T> ();
            } else {
                _instance = reference as T;
            }
            return _instance;
        }

        public void Set(T t) {
            _instance = t;
            reference = t as Object;
        }
    }
}