using System.Runtime.CompilerServices;
using UnityEngine;

namespace RedOwl.Core
{
    public static class TransformExtensions
    {
        public static void Clear(this Transform self)
        {
            int count = self.childCount;
            for (; count > 0; count--)
            {
                self.GetChild(count - 1).Destroy();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy(this Transform self) => self.gameObject.Destroy();
    }
}