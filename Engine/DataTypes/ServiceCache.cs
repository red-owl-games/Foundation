using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine.Assertions;

namespace RedOwl.Engine
{
    [AttributeUsage( AttributeTargets.Field )]
    [MeansImplicitUse]
    public sealed class Inject : Attribute { }
    
    public class ServiceCache
    {
        protected readonly Dictionary<Type, object> cache = new Dictionary<Type, object>();

        public void Bind<T>(T instance)
        {
            Log.Info($"Binding: {instance.GetType().FullName}");
            cache[instance.GetType()] = instance;
        }
        
        public void BindAs<T>(T instance)
        {
            Log.Info($"Binding: {typeof(T).FullName}");
            cache[typeof(T)] = instance;
        }

        public void Unbind<T>(T instance)
        {
            cache.Remove(instance.GetType());
        }

        public void UnbindAs<T>(T instance)
        {
            cache.Remove(typeof(T));
        }

        private object Find(Type type)
        {
            if (!cache.TryGetValue(type, out object value))
            {
                Log.Warn($"Unable to find service for: '{type.FullName}'");
            }
            //Log.Info($"Finding: {value?.GetType()}");
            return value;
        }

        public T Find<T>() => cache.TryGetValue(typeof(T), out object output) ? (T)output : default;

        public void Clear() => cache.Clear();

        public void Inject( object obj )
        {
            if (Game.IsShuttingDown) return;
            Log.Info($"Injecting: {obj.GetType().FullName}");
            foreach (var field in Reflector.Reflect(obj.GetType()))
            {
                field.SetValue(obj, Find(field.FieldType));
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