using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine.Assertions;

namespace RedOwl.Core
{
    [AttributeUsage( AttributeTargets.Field )]
    [MeansImplicitUse]
    public sealed class Inject : Attribute { }
    
    public class ServiceCache
    {
        private readonly Dictionary<Type, object> _cache = new Dictionary<Type, object>();

        public void Bind<T>(T instance) => _cache[typeof(T)] = instance;

        private object Find(Type type)
        {
            if (!_cache.TryGetValue(type, out object value))
            {
                Log.Warn($"Unable to find service for: '{type.FullName}'");
            }
            return value;
        }

        public T Find<T>() => (T)_cache[typeof(T)];

        public void Reset() => _cache.Clear();

        public void Inject( object obj )
        {
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