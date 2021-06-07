using DG.Tweening;
using Sirenix.OdinInspector;

namespace RedOwl.Engine
{
    public class PauseTween : TweenConfig
    {
        [ShowInInspector, ReadOnly, DisplayAsString, HideLabel]
        public readonly string Title = "Pause";
        public override void ApplyTween(Sequence sequence)
        {
            sequence.AppendCallback(() => sequence.Pause());
        }
    }
}