using System.Runtime.CompilerServices;
using UnityEngine;

namespace RedOwl.Engine
{
    public static class GameObjectExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Disable(this GameObject self) => self.SetActive(false);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enable(this GameObject self) => self.SetActive(true);
    }
}