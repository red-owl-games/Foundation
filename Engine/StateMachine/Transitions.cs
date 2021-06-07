using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public class AlwaysTransition : TransitionBase, ITransitionBehaviour
    {
        public override bool CanTransition() => true;
    }
    
    public class TriggerTransition : TransitionBase, ITransitionBehaviour
    {
        [HideInInspector]
        public bool Value;

        [Button]
        public void Trigger() => Value = true;
        
        public override bool CanTransition()
        {
            if (!Value) return false;
            Value = false;
            return true;
        }
    }
    
    public class OnMessageTransition : TransitionBase, ITransitionEnterExit
    {
        public Message message;

        private bool _wasTriggered;

        public override bool CanTransition()
        {
            if (!_wasTriggered) return false;
            _wasTriggered = false;
            return true;
        }
        
        public void OnEnter() => message.On += Handler;

        public void OnExit() => message.On -= Handler;

        private void Handler() => _wasTriggered = true;
    }

    public class DelayedTransition : TransitionBase, ITransitionEnterExit, ITransitionUpdate, ITransitionBehaviour
    {
        public float delay;

        private float _current;

        public override bool CanTransition()
        {
            if (_current > delay)
            {
                return true;
            }

            return false;
        }
        
        public void OnEnter() => _current = 0;
        
        public void OnUpdate(float dt)
        {
            _current += dt;
        }

        public void OnExit() => _current = 0;

    }

    public class OnChannelTransition : TransitionBase, ITransitionEnterExit, ITransitionBehaviour
    {
        public Channel channel;

        private bool _wasTriggered;

        public override bool CanTransition()
        {
            if (!_wasTriggered) return false;
            _wasTriggered = false;
            return true;
        }
        
        public void OnEnter() => channel.On += Handler;

        public void OnExit() => channel.On -= Handler;

        private void Handler() => _wasTriggered = true;
    }

    public abstract partial class BaseState
    {
        public void Permit(IState to)
        {
            Permit(new AlwaysTransition{To = to});
        }
        
        public void Permit(IState to, Message message)
        {
            Permit(new OnMessageTransition{To = to, message = message});
        }
        
        public void Permit(IState to, float delay)
        {
            Permit(new DelayedTransition(){To = to, delay = delay});
        }
        
        public void Permit(IState to, Channel channel)
        {
            Permit(new OnChannelTransition{To = to, channel = channel});
        }
    }
}