using UnityEngine;

namespace RedOwl.Core
{
    public static class AnimationCurveExtensions
    {
        public static float GetDuration(this AnimationCurve self)
        {
            return self.keys[self.length - 1].time;
        }
    }
}