using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public class WaitForNextEvent : CustomYieldInstruction
    {
        private bool _keepWaiting;
        public override bool keepWaiting => _keepWaiting;

        private readonly ITelegram _evt;

        public WaitForNextEvent(ITelegram evt)
        {
            _evt = evt;
            _evt.On += HandleEvent;
            _keepWaiting = true;
        }

        private void HandleEvent()
        {
            _evt.On -= HandleEvent;
            _keepWaiting = false;
            
        }
    }

    public abstract class ITelegram : ScriptableObject
    {
        public event Action On;

        [Button(ButtonSizes.Large)]
        public void Raise()
        {
            On?.Invoke();
            OnRaise();
        }

        protected abstract void OnRaise();
        
        public WaitForNextEvent OnNext()
        {
            return new WaitForNextEvent(this);
        }
    }
    
    public abstract class Telegram<T> : ITelegram where T : struct
    {
        public event Action<T> When;

        public bool sendGlobally;
        
        [InlineProperty, HideLabel]
        public T message;
        
        protected override void OnRaise()
        {
            When?.Invoke(message);
            if (sendGlobally) Telegraph.Send(message);
        }
    }

    [HideMonoScript]
    [CreateAssetMenu(menuName = "Red Owl/Telegram")]
    public class Telegram : Telegram<Signal>
    {
        private void OnValidate()
        {
            message = new Signal(name);
        }
    }
}