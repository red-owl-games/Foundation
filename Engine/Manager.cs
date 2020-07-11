using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    [HideMonoScript]
    public class Manager : MonoBehaviour
    {
        [SerializeField, AssetsOnly] private ManagerReference reference = null;

        // Performance Helpers
        private bool _needsUpdate;
        private IManagerOnUpdate _update;
        
        private bool _needsFixedUpdate;
        private IManagerOnFixedUpdate _fixedUpdate;
        
        private bool _needsLateUpdate;
        private IManagerOnLateUpdate _lateUpdate;

        private void Awake()
        {
            reference.Bind();
            switch (reference)
            {
                case IManagerOnAwake awake:
                    awake.WhenAwake();
                    break;
                case IManagerOnUpdate update:
                    _needsUpdate = true;
                    _update = update;
                    break;
                case IManagerOnFixedUpdate fixedUpdate:
                    _needsFixedUpdate = true;
                    _fixedUpdate = fixedUpdate;
                    break;
                case IManagerOnLateUpdate lateUpdate:
                    _needsLateUpdate = true;
                    _lateUpdate = lateUpdate;
                    break;
            }
        }

        private void OnEnable()
        {
            if (reference is IManagerOnEnable casted) casted.WhenEnable();
        }

        private void Start()
        {
            if (reference is IManagerOnStart casted) casted.WhenStart();
        }

        private void Update()
        {
            if (_needsUpdate) _update.WhenUpdate();
        }

        private void FixedUpdate()
        {
            if (_needsFixedUpdate) _fixedUpdate.WhenFixedUpdate();
        }

        private void LateUpdate()
        {
            if (_needsLateUpdate) _lateUpdate.WhenLateUpdate();
        }

        private void OnDisable()
        {
            if (reference is IManagerOnEnable casted) casted.WhenDisable();
        }

        private void OnDestroy()
        {
            if (reference is IManagerOnAwake casted) casted.WhenDestroy();
        }
    }
}