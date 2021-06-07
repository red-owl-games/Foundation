using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public class LightTween : TweenConfig<Light>
    {
        public enum Types
        {
            Color,
            Intensity
        }
        
        public Types type;
        
        [ShowIf("type", Types.Color)] public Color color;
        [ShowIf("type", Types.Intensity)] public float intensity;


        protected override void Children(Action<Light> callback)
        {
            target.Children(callback);
        }

        protected override Tweener BuildTween(Light t)
        {
            switch (type)
            {
                case Types.Color:
                    return t.DOColor(color, duration);
                case Types.Intensity:
                    return t.DOIntensity(intensity, duration);
            }

            return null;
        }
    }
}