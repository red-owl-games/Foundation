using DG.Tweening;
using Sirenix.OdinInspector;

namespace RedOwl.Engine
{
    public class WaitTween : TweenConfig
    {
        [LabelText("Wait Duration")]
        public float duration;
        
        public override void ApplyTween(Sequence sequence)
        {
            sequence.AppendInterval(duration);
        }
    }
}