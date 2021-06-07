using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    public static class GameObjectExtensions
    {
        public static void Children(this GameObject self, Action<GameObject> predicate)
        {
            self.transform.Children(t => predicate(t.gameObject));
        }
        
        public static void ChildrenReverse(this GameObject self, Action<GameObject> predicate)
        {
            self.transform.ChildrenReverse(t => predicate(t.gameObject));
        }

        public static void SetLayer(this GameObject self, int layer, bool recursive = false)
        {
            self.layer = layer;
            if (recursive) self.Children(o => o.SetLayer(layer, true));
        }
        
        public static void Clear(this GameObject self)
        {
            self.transform.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy(this GameObject self)
        {
#if UNITY_EDITOR
            if (Game.IsRunning) Object.Destroy(self); else Object.DestroyImmediate(self);
#else
            Object.Destroy(self);
#endif
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Disable(this GameObject self) => self.SetActive(false);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enable(this GameObject self) => self.SetActive(true);
    }
}