using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface IState
    {
        string Name { get; }
        IEnumerable<ITransition> Transitions { get; }
    }
    
    public interface IStateEnter
    {
        void Enter();
    }

    public interface IStateExit
    {
        void Exit();
    }
    
    public interface IStateEnterExit : IStateEnter, IStateExit {}

    public interface IStateUpdate
    {
        void Update(float dt);
    }

    public interface IStateLateUpdate
    {
        void LateUpdate(float dt);
    }

    public interface IStateFixedUpdate
    {
        void FixedUpdate(float dt);
    }

    // Used by TypeFilter in Unity Inspector
    public interface IStateBehaviour : IState {}

    [Serializable, InlineProperty, HideReferenceObjectPicker]
    public abstract partial class BaseState : IState, IStateEnterExit, IStateUpdate
    {
        [SerializeField, HideLabel] protected string _name;
        
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public Enum Id
        {
            set => _name = value.ToString();
        }

        [SerializeReference, LabelText("Transitions To"), ListDrawerSettings(Expanded = true), TypeFilter("PossibleTransitions")]
        private List<ITransition> _transitions;

        public IEnumerable<ITransition> Transitions => _transitions;
        
        public IEnumerable<Type> PossibleTransitions()
        {
#if UNITY_EDITOR
            foreach (var type in UnityEditor.TypeCache.GetTypesDerivedFrom<ITransitionBehaviour>())
            {
                yield return type;
            }
#else
            yield break;
#endif
        }
        
        private StateMachine _engine;

        protected BaseState()
        {
            _name = "";
            _transitions = new List<ITransition>();
        }

        public void Permit(ITransition transition)
        {
            if (transition is TransitionBase transitionBase) transitionBase.Init(_engine, this);
            _transitions.Add(transition);
        }

        internal void Init(StateMachine engine)
        {
            Log.Debug($"Init State '{Name}'");
            _engine = engine;
            foreach (var transition in _transitions)
            {
                if (transition is TransitionBase transitionBase) transitionBase.Init(engine, this);
            }
        }

        public virtual void Enter()
        {
            foreach (var transition in _transitions)
            {
                if (transition is ITransitionEnter casted) casted.OnEnter();
            }
        }
        
        public virtual void Update(float dt)
        {
            foreach (var transition in _transitions)
            {
                if (transition is ITransitionUpdate casted) casted.OnUpdate(dt);
            }
        }

        public virtual void Exit()
        {
            foreach (var transition in _transitions)
            {
                if (transition is ITransitionExit casted) casted.OnExit();
            }
        }
    }
}