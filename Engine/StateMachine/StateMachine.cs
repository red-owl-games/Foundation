using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Core
{
    public class StateMachine
    {
        private static readonly State Null = new State(new NullState(), new ITransition[0]);

        private Dictionary<Guid, State> _states = new Dictionary<Guid, State>();
        private Guid _initial;
        
        private State _previous;
        private State _current;

        private Coroutine _routine;


        internal void SetInitialState(Guid id) => _initial = id;
        internal void SetStates(Dictionary<Guid, State> states) => _states = states;

        private State GetState(Guid id) => _states.TryGetValue(id, out State output) ? output : Null;

        public void Start()
        {
            foreach (var state in _states.Values)
            {
                state.Inject();
            }
            SetState(_initial);
        }
        
        internal void SetState(Guid id)
        {
            Stop();
            _previous = _current;
            _current = GetState(id);
            _current.Enter();
            _routine = CoroutineManager.StartRoutine(_current.Execute());
        }

        public void Stop()
        {
            if (_current == null) return;
            CoroutineManager.StopRoutine(_routine);
            _current.Exit();
        }
    }

    public class StateMachineBuilder
    {
        private Guid _initial;
        private List<StateBuilder> _builders = new List<StateBuilder>();
        protected StateBuilder Create(IState state)
        {
            var output = new StateBuilder();
            output.WithState(state);
            _builders.Add(output);
            return output;
        }
        
        protected void Initial(StateBuilder builder)
        {
            _initial = builder.Id;
        }
        
        public StateMachine Build()
        {
            var machine = new StateMachine();
            var states = new Dictionary<Guid, State>();
            foreach (var builder in _builders)
            {
                states.Add(builder.Id, builder.Build(machine));
            }
            machine.SetInitialState(_initial);
            machine.SetStates(states);
            return machine;
        }

        public static implicit operator StateMachine(StateMachineBuilder builder) => builder.Build();
    }
}