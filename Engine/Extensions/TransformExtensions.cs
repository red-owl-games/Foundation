using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    public static class TransformExtensions
    {
        public static void Children(this Transform self, Action<Transform> predicate)
        {
            int count = self.childCount;
            for (int i = 0; i < count; i++)
            {
                predicate(self.GetChild(i));
            }
        }
        
        public static void ChildrenReverse(this Transform self, Action<Transform> predicate)
        {
            int count = self.childCount;
            for (; count > 0; count--)
            {
                predicate(self.GetChild(count - 1));
            }
        }
        
        public static void Clear(this Transform self)
        {
            self.ChildrenReverse(Destroy);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Disable(this Transform self) => self.gameObject.SetActive(false);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enable(this Transform self) => self.gameObject.SetActive(true);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy(this Transform self) => self.gameObject.Destroy();
    }
}