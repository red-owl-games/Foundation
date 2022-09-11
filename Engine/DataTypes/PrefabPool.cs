using System;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    public class PrefabPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private readonly GameObject _prefab;
        private readonly ObjectPool<T> _pool;
        
        public PrefabPool(GameObject prefab, Action<T> onPull = null, Action<T> onPush = null, bool collectionCheck = true, int defaultCapacity = 0, int maxCapacity = 10000)
        {
            _prefab = prefab;
            _pool = new ObjectPool<T>(Create, onPull, onPush, null, collectionCheck, defaultCapacity, maxCapacity);
        }
        
        public int CountAll => _pool.CountAll;
        public int CountActive => _pool.CountActive;
        public int CountInactive => _pool.CountInactive;
        
        public void Clear() => _pool.Clear();

        public T Pull(Vector3? position = null, Quaternion? rotation = null, Vector3? scale = null)
        {
            var output = _pool.Get();
            var t = output.transform;
            t.position = position ?? Vector3.zero;
            t.rotation = rotation ?? Quaternion.identity;
            t.localScale = scale ?? Vector3.one;
            output.Initialize(Push);
            output.gameObject.SetActive(true);
            return output;
        }

        public void Push(T t)
        {
            _pool.Release(t);
            t.gameObject.SetActive(false);
        }

        private T Create()
        {
            var gameObject = Object.Instantiate(_prefab);
            gameObject.SetActive(false);
            return gameObject.GetComponent<T>();
        }
    }
}