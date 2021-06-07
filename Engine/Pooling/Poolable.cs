using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class Poolable : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        internal PrefabPool pool;

        private bool _isBeingReturned;

        public void Return()
        {
            if (_isBeingReturned) return;
            _isBeingReturned = true;
            if (pool != null) pool.Return(gameObject);
        }

        private void OnEnable()
        {
            _isBeingReturned = false;
        }

        private void OnDisable()
        {
            Return();
        }
    }
}