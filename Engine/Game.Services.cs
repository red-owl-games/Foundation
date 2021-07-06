using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine.Assertions;

namespace RedOwl.Engine
{
    [AttributeUsage( AttributeTargets.Field )]
    [MeansImplicitUse]
    public sealed class Inject : Attribute { }
    
    public interface IService {}

    public interface IServiceInit : IService
    {
        void Init();
    }

    public interface IServiceStart : IService
    {
        void Start();
    }
    
    public interface IServiceAsync : IService
    {
        IEnumerator AsyncUpdate(float dt);
    }

    public interface IServiceUpdate : IService
    {
        void Update(float dt);
    }
    
    public interface IServiceLateUpdate : IService
    {
        void LateUpdate(float dt);
    }
    
    public interface IServiceFixedUpdate : IService
    {
        void FixedUpdate(float dt);
    }
    
    public class Container : IUnityBridgeTarget
    {
        protected readonly Dictionary<string, object> Cache;
        
        private bool _isStarted;

        public Container()
        {
            Cache = new Dictionary<string, object> ();
        }

        public bool Has<T>(string key = null)
        {
            return Cache.ContainsKey(key ?? typeof(T).Name);
        }

        public T Add<T>(string key = null) where T : new() => Add(new T (), key);
        public T Add<T>(T target, Enum key) where T : new() => Add(target, key.ToString());

        public T Add<T>(T target, string key = null)
        {
            //Log.Info($"Binding {key} {typeof(T).Name} {target.GetType().Name}");
            Cache.Add(key ?? typeof(T).Name, target);
            Inject(target);
            if (target is IServiceInit init) init.Init();
            if (_isStarted && target is IServiceStart start) start.Start();
            return target;
        }

        public T Get<T>(Enum key) => Get<T>(key.ToString());
        public T Get<T>(string key = null)
        {
            if (Cache.TryGetValue(key ?? typeof(T).Name, out var item))
            {
                return (T) item;
            }
            Log.Warn($"Unable to find '{key ?? typeof(T).FullName}'");
            return default;
        }

        public void Remove<T>() => Remove(typeof(T).Name);
        public void Remove(Enum key) => Remove(key.ToString());
        
        public void Remove(string key)
        {
            if (Cache.TryGetValue(key, out var item))
            {
                if (item is IDisposable disposable) disposable.Dispose();
            }
            Cache.Remove(key);
        }

        public void Inject( object obj )
        {
            //Log.Info($"Injecting: {obj.GetType().FullName}");
            foreach (var field in Reflector.Reflect(obj.GetType()))
            {
                if (Cache.TryGetValue(field.FieldType.Name, out var item))
                {
                    field.SetValue(obj, item);
                }
            }
        }
        
        
        #region IUnityBridgeTarget

        public void Start()
        {
            _isStarted = true;
            foreach (var item in Cache.Values)
            {
                if (item is IServiceStart service) service.Start();
            }
        }

        public IEnumerator AsyncUpdate(float dt)
        {
            foreach (var item in Cache.Values)
            {
                if (item is IServiceAsync service) yield return service.AsyncUpdate(dt);
            }
        }
        
        public void Update(float dt)
        {
            foreach (var item in Cache.Values)
            {
                if (item is IServiceUpdate service) service.Update(dt);
            }
        }
        
        public void LateUpdate(float dt)
        {
            foreach (var item in Cache.Values)
            {
                if (item is IServiceLateUpdate service) service.LateUpdate(dt);
            }
        }
        
        public void FixedUpdate(float dt)
        {
            foreach (var item in Cache.Values)
            {
                if (item is IServiceFixedUpdate service) service.FixedUpdate(dt);
            }
        }

        #endregion
        
        private static class Reflector
        {
            private static readonly Type InjectAttributeType = typeof(Inject);
            [ClearOnReload(true)]
            private static readonly Dictionary<Type, FieldInfo[]> CachedFieldInfos = new Dictionary<Type, FieldInfo[]>();
            [ClearOnReload(true)]
            private static readonly List<FieldInfo> ReusableList = new List<FieldInfo>( 1024 );

            public static FieldInfo[] Reflect( Type type )
            {
                Assert.AreEqual( 0, ReusableList.Count, "Reusable list in Reflector was not empty!" );

                if (CachedFieldInfos.TryGetValue( type, out var cachedResult ))
                {
                    return cachedResult;
                }

                var fields = type.GetFields( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy );
                foreach (var field in fields)
                {
                    if (field.IsDefined( InjectAttributeType, inherit: false ))
                    {
                        ReusableList.Add( field );
                    }
                }
                var resultAsArray = ReusableList.ToArray();
                ReusableList.Clear();
                CachedFieldInfos[ type ] = resultAsArray;
                return resultAsArray;
            }
        }
    }
    
    public partial class Game
    {
        [ClearOnReload]
        private static Container container;
        public static Container Container => container ?? (container = new Container());

        public static T Bind<T>(string key = null) where T : new() => Container.Add<T>(key);
        public static T Bind<T>(T obj, string key = null) => Container.Add(obj, key);
        public static T Bind<T>(T obj, Enum key) => Container.Add(obj, key.ToString());
        public static T Find<T>(string key = null) => Container.Get<T>(key);
        public static T FindOrBind<T>(string key = null) where T : new() => Container.Has<T>(key) ? Find<T>(key) : Bind<T>(key);
        public static void Inject(object obj) => Container.Inject(obj);
    }
}