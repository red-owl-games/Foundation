using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedOwl.Core
{
    [Serializable]
    public class ManagerSettings : Settings<ManagerSettings>
    {
        [SerializeField]
        private bool enabled = true;

        public static bool Enabled => Instance.enabled;
        
        [AssetsOnly]
        [SerializeField]
        private List<GameObject> managers;

        public static List<GameObject> Managers => Instance.managers;
    }
    
    public static class ManagerInitialization
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BeforeSceneLoad()
        {
            if (!ManagerSettings.Enabled) return;
            foreach (var manager in ManagerSettings.Managers)
            {
                Object.DontDestroyOnLoad(Object.Instantiate(manager));
            }
        }
    }
    
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