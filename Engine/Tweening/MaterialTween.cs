using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public class MaterialTween : TweenConfig<Material>
    {
        public enum Types
        {
            Color,
        }
        
        public Types type;
        
        [ShowIf("type", Types.Color)] public Color color;

        protected override void Children(Action<Material> callback) {}

        protected override Tweener BuildTween(Material t)
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