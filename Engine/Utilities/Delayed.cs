using System;
using DG.Tweening;

namespace RedOwl.Engine
{
    public static class Delayed
    {
        public static Tween Run(TweenCallback callback, float delay = 0f) => DOVirtual.DelayedCall(delay, callback);
    }
}