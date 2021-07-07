using System;
using System.Collections;

namespace RedOwl.Engine
{
    public class StateMachine : BaseState, IServiceInit, IServiceUpdate, IStateLateUpdate, IStateFixedUpdate
    {
        public const string NO_STATE = "NO STATE";
        
        private IState _initialState;
        private History<IState> _history;
        
        public IState CurrentState
        {
            get => _history?.Current;
            private set
            {
                ExitState(Name, _history.Current);
                EnterState(Name, value);
                _history.Push(value);
            }
        }

        public StateCollection States { get; }

        public StateMachine(string name)
        {
            _name = name;
            _history = new History<IState>();
            States = new StateCollection();
        }
        
        public StateMachine(Enum name)
        {
            _name = name.ToString();
            _history = new History<IState>();
            States = new StateCollection();
        }

        public T Add<T>(T state) where T : IState
        {
            if (state is BaseState baseState) baseState.Init(this);
            States.Add(state);
            return state;
        }

        public void SetInitialState(IState initialState)
        {
            SetInitialState(initialState.Name);
        }

        public void SetInitialState(string initialState)
        {
            if (States.Count <= 0) return;
            _initialState = initialState == NO_STATE ? States[0] : States.ContainsKey(initialState) ? States[initialState] : States[0];
        }
        
        public void Init()
        {
            Enter();
        }

        public override void Enter()
        {
            base.Enter();
            CurrentState = _initialState ?? States[0];
        }

        public override void Exit()
        {
            base.Exit();
            ExitState(Name, _history.Current);
        }

        public override void Update(float dt)
        {
            // TODO: why does this not call base.Update() ?
            foreach (var transition in _history.Current.Transitions)
            {
                if (transition.CanTransition())
                {
                    CurrentState = transition.To;
                }
            }

            if (CurrentState is IStateUpdate casted) casted.Update(dt);
        }

        public void LateUpdate(float dt)
        {
            if (CurrentState is IStateLateUpdate casted) casted.LateUpdate(dt);
        }

        public void FixedUpdate(float dt)
        {
            if (CurrentState is IStateFixedUpdate casted) casted.FixedUpdate(dt);
        }

        public static void EnterState(string name, IState state)
        {
            if (state is IStateEnter enterable)
            {
                Log.Debug($"StateMachine '{name}' Entering State '{state.Name}'");
                enterable.Enter();
            }
        }

        private static void ExitState(string name, IState state)
        {
            if (state is IStateExit exitable)
            {
                Log.Debug($"StateMachine '{name}' Exiting State '{state.Name}'");
                exitable.Exit();
            }
        }
    }
}