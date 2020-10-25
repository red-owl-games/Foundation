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
        
        public static void Children(this GameObject self, Action<GameObject> predicate)
        {
            self.transform.Children(t => predicate(t.gameObject));
        }

        public static void SetLayer(this GameObject self, int layer)
        {
            self.layer = layer;
            self.Children(o => o.SetLayer(layer));
        }
        
        public static void Clear(this GameObject self)
        {
            self.transform.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy(this GameObject self)
        {
#if UNITY_EDITOR
            if (Application.isPlaying) Object.Destroy(self); else Object.DestroyImmediate(self);
#else
            Object.Destroy(self);
#endif
        }
    }
}