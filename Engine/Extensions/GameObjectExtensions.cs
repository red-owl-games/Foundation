using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedOwl.Core
{
    public static class GameObjectExtensions
    {
        private static bool TryGetComponent<T>(this GameObject self, out T comp) where T : Component
        {
            comp = self.GetComponent<T>();
            return (comp != null);
        }
        
        public static T EnsureComponent<T>(this GameObject self) where T : Component
        {
            T comp = self.GetComponent(typeof(T)) as T;
            if (!comp) comp = self.AddComponent(typeof(T)) as T;
            return comp;
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