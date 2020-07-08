using System;

namespace RedOwl.Core
{
    public interface ITransition
    {
        void Enable();
        void Disable();
    }

    public class NullTransition : ITransition
    {
        public void Enable() { }

        public void Disable() { }
    }
    
    public class Transition : ITransition
    {
        private readonly StateMachine _machine;
        private readonly Message _message;
        private readonly Guid _state;

        public Transition(StateMachine machine, Message message, Guid state)
        {
            _machine = machine;
            _message = message;
            _state = state;
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
            _machine.SetState(_state);
        }
    }

    public class GuardedTransition : ITransition
    {
        private readonly StateMachine _machine;
        private readonly Message _message;
        private readonly Guid _state;
        private readonly Func<bool> _isAllowed;

        public GuardedTransition(StateMachine machine, Message message, Guid state, Func<bool> guard)
        {
            _machine = machine;
            _message = message;
            _state = state;
            _isAllowed = guard;
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
            if (!_isAllowed()) return;
            _machine.SetState(_state);
        }
    }

    public enum TransitionTypes
    {
        Normal,
        Guarded
    }

    public class TransitionBuilder
    {
        private readonly TransitionTypes _type;
        private readonly Message _message;
        private readonly Guid _state;
        private readonly Func<bool> _guard;
        
        public TransitionBuilder(Message message, Guid state)
        {
            _type = TransitionTypes.Normal;
            _message = message;
            _state = state;
        }

        public TransitionBuilder(Message message, Guid state, Func<bool> guard)
        {
            _type = TransitionTypes.Guarded;
            _message = message;
            _state = state;
            _guard = guard;
        }

        public ITransition Build(StateMachine machine)
        {
            switch (_type)
            {
                case TransitionTypes.Normal:
                    return new Transition(machine, _message, _state);
                case TransitionTypes.Guarded:
                    return new GuardedTransition(machine, _message, _state, _guard);
                default:
                    return new NullTransition();
            }
        }
    }
}