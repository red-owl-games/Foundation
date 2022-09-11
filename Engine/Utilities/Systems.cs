using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    public class Systems : MonoBehaviour
    {
        private void Start()
        {
            foreach (var system in World.Default.All)
            {
                if (system.Enabled)
                    system.DoStartRunning();
            }
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            foreach (var system in World.Default.All)
            {
                system.DoUpdate(dt);
            }
        }

        private void LateUpdate()
        {
            var dt = Time.deltaTime;
            foreach (var system in World.Default.All)
            {
                system.DoLateUpdate(dt);
            }
        }

        private void OnDestroy()
        {
            foreach (var system in World.Default.All)
            {
                if (system.Enabled)
                    system.DoStopRunning();
            }
        }
    }
    
    public class World : Container<SystemBase>
    {
        protected override void OnAdd<T>(T item)
        {
            item.World = this;
            item.DoCreate();
        }

        #region Static

        [ClearOnReload]
        private static World _default;
        public static World Default
        {
            get => _default ??= new World();
            set => _default = value;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            var game = typeof(Game);
            var match = typeof(SystemBase);
            var add = typeof(World).GetMethod("Add", new []{ typeof(string) });
            var parameters = new object[] { null };
            if (add == null) return;
            add.MakeGenericMethod(game).Invoke(Default, parameters);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsAbstract || !match.IsAssignableFrom(type) || type.FullName == game.FullName) continue;
                    add.MakeGenericMethod(type).Invoke(Default, parameters);
                }
            }
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            Object.DontDestroyOnLoad(new GameObject("Systems").AddComponent<Systems>().gameObject);
        }

        #endregion
    }
    
    /// <summary>
    /// Order of Events:
    ///     OnCreate
    ///     OnStartRunning
    ///     OnUpdate
    ///     OnLateUpdate
    ///     OnStopRunning
    /// </summary>
    public abstract class SystemBase
    {
        public World World { get; set; }
        
        private bool _enabled = true;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                switch (_enabled)
                {
                    case false when value == true:
                        ShouldEnable = true;
                        break;
                    case true when value == false:
                        ShouldDisable = true;
                        break;
                }

                _enabled = value;
            }
        }

        private bool ShouldDisable { get; set; }
        private bool ShouldEnable { get; set; }

        internal void DoCreate() => OnCreate();
        
        protected virtual void OnCreate() {}
        
        internal void DoStartRunning() => OnStartRunning();

        protected virtual void OnStartRunning() {}

        internal void DoUpdate(float dt)
        {
            if (ShouldEnable)
            {
                ShouldEnable = false;
                DoStartRunning();
            }

            if (Enabled)
                OnUpdate(dt);
                
            if (ShouldDisable)
            {
                ShouldDisable = false;
                DoStopRunning();
            }
        }

        protected virtual void OnUpdate(float dt) {}
        
        internal void DoLateUpdate(float dt)
        {
            if (Enabled)
                OnLateUpdate(dt);
        }

        protected virtual void OnLateUpdate(float dt) {}

        internal void DoStopRunning() => OnStopRunning();

        protected virtual void OnStopRunning() {}
        
    }

    public abstract class SystemBase<T> : SystemBase where T : SystemBase, new()
    {
        public static T Instance => World.Default.Get<T>();
    }
}