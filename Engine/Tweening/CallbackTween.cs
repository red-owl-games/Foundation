using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    public class CallbackTween : TweenConfig
    {
        [UnityEventFoldout]
        public UnityEvent Callback;
        
        public override void ApplyTween(Sequence sequence)
        {
            sequence.AppendCallback(Callback.Invoke);
        }
    }
}