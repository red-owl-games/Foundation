using UnityEngine;

namespace RedOwl.Engine
{
    public static class AnimationCurveExtensions
    {
        public static float GetDuration(this AnimationCurve self)
        {
            return self.keys[self.length - 1].time;
        }
    }
}