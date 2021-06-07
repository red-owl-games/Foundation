using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    public static class ComponentExtensions
    {
        public static void EnsureComponent<T>(this Component self, out T comp) where T : Component
        {
            comp = self.GetComponent(typeof(T)) as T;
            if (!comp) comp = self.gameObject.AddComponent(typeof(T)) as T;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WithComponent<T>(this Component self, Action<T> callback) where T : Component
        {
            if (self.TryGetComponent(out T comp)) callback(comp);
        }
        
        public static void Children<T>(this Component self, Action<T> predicate) where T : Component
        {
            self.transform.Children(x => x.WithComponent(predicate));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Disable<T>(this T self) where T : Behaviour => self.enabled = false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enable<T>(this T self) where T : Behaviour => self.enabled = true;
    }
}