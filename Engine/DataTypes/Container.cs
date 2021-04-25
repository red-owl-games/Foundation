using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace RedOwl.Engine
{
    [AttributeUsage( AttributeTargets.Field )]
    [MeansImplicitUse]
    public sealed class Inject : Attribute { }

    public interface IServiceInit
    {
        void Init();
    }

    public interface IServiceStart
    {
        void Start();
    }

    public interface IServiceUpdate
    {
        IEnumerator Update(float dt);
    }
    
    public interface IContainer : IDisposable
    {
        T Add<T>(string key = null) where T : new ();
        T Add<T>(T service, string key = null);
        T Get<T>(string key = null);
        void Inject(object obj);
    }

    public class Container : IContainer
    {
        protected Action OnAdded;
        protected Action OnRemoved;
        
        protected readonly Dictionary<string, object> cache = new Dictionary<string, object> ();
        
        private bool isStarted;

        public T Add<T>(string key = null) where T : new() => Add(new T (), key);

        public T Add<T>(T target, string key = null)
        {
            cache.Add(key ?? target.GetType().Name, target);
            OnAdded?.Invoke();
            Inject(target);
            if (target is IServiceInit init) init.Init();
            if (isStarted && target is IServiceStart start) start.Start();
            return target;
        }

        public T Get<T>(string key = null) => 
            cache.TryGetValue(key ?? typeof(T).Name, out var item) ? (T) item : default;

        public void Remove<T>(T instance, string key = null) => Remove(key ?? instance.GetType().Name);
        
        public void Remove(string key)
        {
            if (cache.TryGetValue(key, out var item))
            {
                if (item is IDisposable disposable) disposable.Dispose();
            }
            cache.Remove(key);
            OnRemoved?.Invoke();
        }

        public void Dispose()
        {
            foreach (var item in cache.Values)
            {
                if (item is IDisposable disposable) disposable.Dispose();
            }
        }

        public void Start()
        {
            isStarted = true;
            foreach (var item in cache.Values)
            {
                if (item is IServiceStart service) service.Start();
            }
        }
        
        public IEnumerator Update()
        {
            while (true)
            {
                float dt = Time.deltaTime;
                foreach (var item in cache.Values)
                {
                    if (item is IServiceUpdate service) yield return service.Update(dt);
                }
            }
        }

        public void Inject( object obj )
        {
            if (Game.IsShuttingDown) return;
            Log.Info($"Injecting: {obj.GetType().FullName}");
            foreach (var field in Reflector.Reflect(obj.GetType()))
            {
                if (cache.TryGetValue(field.FieldType.Name, out var item))
                {
                    field.SetValue(obj, item);
                }
            }
        }
        
        private static class Reflector
        {
            private static readonly Type InjectAttributeType = typeof(Inject);
            private static readonly Dictionary<Type, FieldInfo[]> CachedFieldInfos = new Dictionary<Type, FieldInfo[]>();
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
}