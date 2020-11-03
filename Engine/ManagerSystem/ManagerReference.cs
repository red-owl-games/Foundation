using System.Collections;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface IManagerReference {}

    public interface IManagerAwake : IManagerReference
    {
        void WhenAwake();
        void WhenDestroy();
    }
    
    public interface IManagerEnable : IManagerReference
    {
        void WhenEnable();
        void WhenDisable();
    }
    
    public interface IManagerStart : IManagerReference
    {
        void WhenStart();
    }
    
    public interface IManagerStartAsync : IManagerReference
    {
        IEnumerator WhenStart();
    }
    
    public interface IManagerUpdate : IManagerReference
    {
        void WhenUpdate();
    }
    
    public interface IManagerLateUpdate : IManagerReference
    {
        void WhenLateUpdate();
    }
    
    public interface IManagerFixedUpdate : IManagerReference
    {
        void WhenFixedUpdate();
    }
    
    public interface IManagerDrawGizmos : IManagerReference
    {
        void WhenDrawGizmos();
    }
    
    public interface IManagerGUI : IManagerReference
    {
        void WhenGUI();
    }
    
    public interface IManager : IManagerAwake, IManagerEnable, IManagerStartAsync {}
    
    public class ManagerReference : MonoBehaviour
    {
        private IManagerReference _reference;

        // Performance Helpers
        private bool _needsUpdate;
        private IManagerUpdate _update;

        private bool _needsLateUpdate;
        private IManagerLateUpdate _lateUpdate;
        
        private bool _needsFixedUpdate;
        private IManagerFixedUpdate _fixedUpdate;
        
        private bool _needsDrawGizmos;
        private IManagerDrawGizmos _gizmos;
        
        private bool _needsGUI;
        private IManagerGUI _gui;

        private void Awake()
        {
            Game.Inject(_reference);
            switch (_reference)
            {
                case IBindable bind:
                    bind.DoBind();
                    break;
                case IInjectable inject:
                    inject.DoInject();
                    break;
                case IManagerAwake awake:
                    awake.WhenAwake();
                    break;
                case IManagerUpdate update:
                    _needsUpdate = true;
                    _update = update;
                    break;
                case IManagerFixedUpdate fixedUpdate:
                    _needsFixedUpdate = true;
                    _fixedUpdate = fixedUpdate;
                    break;
                case IManagerLateUpdate lateUpdate:
                    _needsLateUpdate = true;
                    _lateUpdate = lateUpdate;
                    break;
                case IManagerDrawGizmos gizmos:
                    _needsDrawGizmos = true;
                    _gizmos = gizmos;
                    break;
                case IManagerGUI gui:
                    _needsGUI = true;
                    _gui = gui;
                    break;
            }
        }

        private void OnEnable()
        {
            if (_reference is IManagerEnable casted) casted.WhenEnable();
        }

        private IEnumerator Start()
        {
            if (_reference is IManagerStart casted) casted.WhenStart();
            if (_reference is IManagerStartAsync castedAsync) yield return castedAsync.WhenStart();
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
            if (_reference is IManagerEnable casted) casted.WhenDisable();
        }

        private void OnDestroy()
        {
            if (_reference is IManagerAwake casted) casted.WhenDestroy();
        }

        private void OnDrawGizmos()
        {
            if (_needsDrawGizmos) _gizmos.WhenDrawGizmos();
        }

        private void OnGUI()
        {
            if (_needsGUI) _gui.WhenGUI();
        }

        public static void Create<T>(T manager) where T : IManagerReference
        {
            var go = new GameObject($"[{typeof(T).Name}] Manager");
            go.AddComponent<ManagerReference>()._reference = manager;
            DontDestroyOnLoad(go);
        }
    }
}