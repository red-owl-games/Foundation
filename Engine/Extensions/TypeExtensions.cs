using System;
using System.Collections.Generic;
using System.Reflection;

namespace RedOwl.Core
{
    public static class TypeExtensions
    {
        public static Type[] SafeGetTypes(this Assembly self)
        {
            try
            {
                return self.GetTypes();
            }
            catch
            {
                return Type.EmptyTypes;
            }
        }
        
        public static string SafeGetName(this Type self)
        {
            string output = "";
            var type = self;
            while (true)
            {
                output = type.FullName;
                if (string.IsNullOrEmpty(output))
                {
                    output = self.GetGenericTypeDefinition().FullName;
                    if (!string.IsNullOrEmpty(output))
                    {
                        return output;
                    }
                }
                else
                {
                    return output;
                }

                type = type.BaseType;
            }
        }

        public static IEnumerable<Type> GetAllTypes<T>()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.SafeGetTypes())
                {
                    if (typeof(T).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                        yield return type;
                }
            }
        }
    }
}