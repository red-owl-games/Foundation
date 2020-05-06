using System.Runtime.CompilerServices;
using UnityEngine;

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