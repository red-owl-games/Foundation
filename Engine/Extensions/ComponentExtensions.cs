using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RedOwl.Core
{
    public static class ComponentExtensions
    {
        private static bool TryGetComponent<T>(this Component self, out T comp) where T : Component
        {
            comp = self.GetComponent<T>();
            return (comp != null);
        }
        
        public static T EnsureComponent<T>(this Component self) where T : Component
        {
            T comp = self.GetComponent(typeof(T)) as T;
            if (!comp) comp = self.gameObject.AddComponent(typeof(T)) as T;
            return comp;
        }

        public static void WithComponent<T>(this Component self, Action<T> callback) where T : Component
        {
            if (self.TryGetComponent<T>(out T comp)) callback(comp);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy(this Component self) => self.gameObject.Destroy();
    }
}