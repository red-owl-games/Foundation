using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface ITransition
    {
        IState To { get; }
        bool CanTransition();
    }
    
    public interface ITransitionEnter : ITransition
    {
        void OnEnter();
    }

    public interface ITransitionExit : ITransition
    {
        void OnExit();
    }
    
    public interface ITransitionEnterExit : ITransitionEnter, ITransitionExit {}

    public interface ITransitionUpdate
    {
        void OnUpdate(float dt);
    }
    
    // Used by TypeFilter in Unity Inspector
    public interface ITransitionBehaviour {}
    
    [Serializable, HideReferenceObjectPicker, InlineProperty]
    public abstract class TransitionBase : ITransition
    {
        [SerializeField, HideLabel, ValueDropdown("PossibleStates")]
        private string _to;

        public IState To
        {
            get => _engine.States[_to];
            set => _to = value.Name;
        }

        private StateMachine _engine;

        internal void Init(StateMachine engine, IState state)
        {
            _engine = engine;
        }
        
        private IEnumerable PossibleStates()
        {
            foreach (var state in _engine.States)
            {
                yield return state.Name;
            }
        }

        public abstract bool CanTransition();
    }
}