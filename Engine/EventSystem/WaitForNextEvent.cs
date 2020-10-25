using UnityEngine;

namespace RedOwl.Engine
{
    /*
    public class WaitForNextEvent : CustomYieldInstruction
    {
        private bool _keepWaiting;
        public override bool keepWaiting => _keepWaiting;

        private readonly TelegramReference _evt;

        public WaitForNextEvent(TelegramReference evt)
        {
            _evt = evt;
            _evt.Subscribe(HandleEvent);
            _keepWaiting = true;
        }

        private void HandleEvent()
        {
            _evt.Unsubscribe(HandleEvent);
            _keepWaiting = false;
            
        }
    }
    */
}