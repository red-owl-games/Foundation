using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public class SpriteTween : TweenConfig<SpriteRenderer>
    {
        public enum Types
        {
            Color,
        }
        
        public Types type;
        
        [ShowIf("type", Types.Color)] public Color color;

        protected override void Children(Action<SpriteRenderer> callback)
        {
            target.Children(callback);
        }

        protected override Tweener BuildTween(SpriteRenderer t)
        {
            switch (type)
            {
                case Types.Color:
                    return t.DOColor(color, duration);
            }

            return null;
        }
    }
}