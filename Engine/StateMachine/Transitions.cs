using System;

namespace RedOwl.Core
{
    public interface ITransition
    {
        IState To { get;}
        int Priority { get;  }
        bool IsReady { get; }
        void Reset();
        void Enable();
        void Disable();
    }

    public class CallbackTransition : ITransition
    {
        public IState To { get; }
        public int Priority { get; }
        private readonly Func<bool> _isAllowed;
        public bool IsReady => _isAllowed();
        
        public CallbackTransition(IState to, int priority, Func<bool> callback)
        {
            To = to;
            Priority = priority;
            _isAllowed = callback;
        }

        public void Reset() { }
        public void Enable() { }
        public void Disable() { }
    }
    
    public class EventTransition : ITransition
    {
        public IState To { get; }
        public int Priority { get; }
        private readonly IMessage _message;
        private readonly Func<bool> _isAllowed;
        private readonly bool _hasGuard;
        private readonly bool _autoReset;

        public EventTransition(IState to, int priority, IMessage message, bool autoReset)
        {
            To = to;
            Priority = priority;
            _message = message;
            _hasGuard = false;
            _autoReset = autoReset;
        }

        public EventTransition(IState to, int priority, IMessage message, Func<bool> guard, bool autoReset)
        {
            To = to;
            Priority = priority;
            _message = message;
            _isAllowed = guard;
            _hasGuard = true;
            _autoReset = autoReset;
        }

        public bool IsReady { get; private set; }

        public void Reset()
        {
            if (_autoReset) IsReady = false;
        }

        public void Enable()
        {
            _message.On += HandleEvent;
        }

        public void Disable()
        {
            _message.On -= HandleEvent;
        }

        private void HandleEvent()
        {
            if (_hasGuard)
            {
                IsReady = _isAllowed();
            }
            else
            {
                IsReady = true;
            }
        }
    }
}