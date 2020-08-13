using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Core
{
    [Serializable, InlineProperty, HideReferenceObjectPicker]
    public abstract class TweenData
    {
        public abstract void ApplyTween(Sequence sequence);
    }
    
    public class CallbackTween : TweenData
    {
        [FoldoutGroup("Callback")]
        public UnityEvent Callback;
        
        public override void ApplyTween(Sequence sequence)
        {
            sequence.AppendCallback(Callback.Invoke);
        }
    }

    public abstract class TweenData<T> : TweenData
    {
        [HorizontalGroup("Common", 0.25f), LabelWidth(50)]
        public float duration = 1f;
        [HorizontalGroup("Common", 0.2f), LabelWidth(35)]
        public float delay;
        [HorizontalGroup("Common", 0.3f), LabelWidth(30)]
        public Ease ease = Ease.Linear;
        [HorizontalGroup("Common"), LabelWidth(20), ToggleLeft]
        public bool parallel;
        
        public T target;
        
        public override void ApplyTween(Sequence sequence)
        {
            var tween = GetTween()?.SetDelay(delay).SetEase(ease);
            if (parallel)
            {
                sequence.Join(tween);
            }
            else
            {
                sequence.Append(tween);
            }
        }

        protected abstract Tween GetTween();
    }

    public class TransformTween : TweenData<Transform>
    {
        public enum Types
        {
            Move,
            LocalMove,
            Rotate,
            Scale
        }

        public Types type;

        [ShowIf("type", Types.Move)] public Vector3 position;
        [ShowIf("type", Types.LocalMove)] public Vector3 relative;
        [ShowIf("type", Types.Rotate)] public Vector3 rotation;
        [ShowIf("type", Types.Scale)] public Vector3 scale;
        
        protected override Tween GetTween()
        {
            switch (type)
            {
                case Types.Move:
                    return target.DOMove(position, duration);
                case Types.LocalMove:
                    return target.DOLocalMove(relative, duration);
                case Types.Rotate:
                    return target.DORotate(rotation, duration);
                case Types.Scale:
                    return target.DOScale(scale, duration);
            }

            return null;
        }
    }

    public class LightTween : TweenData<Light>
    {
        public enum Types
        {
            Color,
            Intensity
        }
        
        public Types type;
        
        [ShowIf("type", Types.Color)] public Color color;
        [ShowIf("type", Types.Intensity)] public float intensity;
        
        protected override Tween GetTween()
        {
            switch (type)
            {
                case Types.Color:
                    return target.DOColor(color, duration);
                case Types.Intensity:
                    return target.DOIntensity(intensity, duration);
            }

            return null;
        }
    }
}