using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RedOwl.Engine
{
    public static class TypeExtensions
    {
        public static IEnumerable<Tuple<T, FieldInfo>> GetFieldsWithAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            var attrType = typeof(T);
            foreach (var info in type.GetFields())
            {
                if (info.IsDefined(attrType, inherit))
                    yield return new Tuple<T, FieldInfo>(info.GetCustomAttributes<T>(inherit).FirstOrDefault(), info);
            }
        }
        
        public static IEnumerable<Tuple<T, PropertyInfo>> GetPropertiesWithAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            var attrType = typeof(T);
            foreach (var info in type.GetProperties())
            {
                if (info.IsDefined(attrType, inherit))
                    yield return new Tuple<T, PropertyInfo>(info.GetCustomAttributes<T>(inherit).FirstOrDefault(), info);
            }
        }
        
        public static IEnumerable<Tuple<T, MethodInfo>> GetMethodsWithAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            var attrType = typeof(T);
            foreach (var info in type.GetRuntimeMethods())
            {
                if (info.IsDefined(attrType, inherit))
                    yield return new Tuple<T, MethodInfo>(info.GetCustomAttributes<T>(inherit).FirstOrDefault(), info);
            }
        }
    }
}