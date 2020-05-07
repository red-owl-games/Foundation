using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RedOwl.Core
{
    public static class AnimatorExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasParameter(this Animator self, int parameter)
        {
            return Array.Exists(self.parameters, p => p.nameHash == parameter);
        }
    }
}