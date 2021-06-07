using System;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace RedOwl.Engine
{
    [Flags]
    public enum TweenSettings
    {
        UseTargetsChildren = 0b10,
        Parallel = 0b100,
        From = 0b1000
    }
    
    [Serializable, InlineProperty, HideReferenceObjectPicker]
    public abstract class TweenConfig
    {
        public abstract void ApplyTween(Sequence sequence);
    }

    [LabelWidth(60)]
    public abstract class TweenConfig<T> : TweenConfig
    {
        [HorizontalGroup("Common", 0.3f)]
        public float duration = 1f;
        [HorizontalGroup("Common", 0.25f), LabelWidth(40)]
        public float delay;
        [HorizontalGroup("Common"), LabelWidth(30)]
        public Ease ease = Ease.Linear;
        public TweenSettings settings;
        public T target;
        
        public override void ApplyTween(Sequence sequence)
        {
            var tween = GetTween();
            if (settings.HasFlag(TweenSettings.Parallel))
            {
                sequence.Join(tween);
            }
            else
            {
                sequence.Append(tween);
            }
        }

        private Tween GetTween()
        {
            if (settings.HasFlag(TweenSettings.UseTargetsChildren))
            {
                var sequence = DOTween.Sequence().SetLoops(-1, LoopType.Restart);
                Children(x => sequence.Append(ConfigureTween(BuildTween(x))));
                return sequence;
            }
            return ConfigureTween(BuildTween(target));
        }

        private Tween ConfigureTween(Tweener tweener)
        {
            if (tweener == null) return null;
            if (settings.HasFlag(TweenSettings.From)) tweener.From();
            if (delay > 0) tweener.SetDelay(delay);
            tweener.SetEase(ease);
            return tweener;
        }

        protected abstract void Children(Action<T> callback);
        
        protected abstract Tweener BuildTween(T t);
    }
}