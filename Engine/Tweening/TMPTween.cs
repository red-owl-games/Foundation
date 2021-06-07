using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace RedOwl.Engine
{
    public class TMPTween : TweenConfig<TMP_Text>
    {
        public enum Types
        {
            Fade,
            Color,
        }
        
        public Types type;
        
        [ShowIf("type", Types.Fade)] public float alpha;
        [ShowIf("type", Types.Color)] public Color color;


        protected override void Children(Action<TMP_Text> callback)
        {
            target.Children(callback);
        }

        protected override Tweener BuildTween(TMP_Text t)
        {
            switch (type)
            {
                case Types.Fade:
                    return t.DOFade(alpha, duration);
                case Types.Color:
                    return t.DOColor(color, duration);
            }

            return null;
        }
    }
}