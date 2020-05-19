using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Core
{
    public class StateMachine : State, IDisposable
    {
        private readonly List<State> _internalStates;
        private readonly List<StateMachine> _subMachines;
        private readonly Dictionary<GameEvent, Action> _callbacks;
        private State _current;

        private State InitialState { get; set; }
        
        // Debug
        private StateMachineConfig _config;

        public StateMachine(GameObject owner, StateMachineConfig configuration, bool reenterable = false)
        {
            _config = configuration;
            int count = configuration.SubStates.Count + configuration.SubStateMachines.Count;
            _internalStates = new List<State>(configuration.States.Count);
            _subMachines = new List<StateMachine>(count);
            _callbacks = new Dictionary<GameEvent, Action>(count);
            Initialize(owner, reenterable);
            ParseStates(owner, configuration);
            ParseStateMachines(owner, configuration);
        }

        private void ParseStates(GameObject owner, StateMachineConfig configuration)
        {
            foreach (string name in configuration.States)
            {
                var state = StateCache.Get(name);
                state.Initialize(owner, false);
                _internalStates.Add(state);
            }
            foreach (var stateConfig in configuration.SubStates)
            {
                stateConfig.State.Initialize(owner, stateConfig.reenterable);
                if (stateConfig.Event != null) _callbacks.Add(stateConfig.Event, () => SetState(stateConfig.State));
                if (stateConfig.initial) InitialState = stateConfig.State;
            }
        }
        
        private void ParseStateMachines(GameObject owner, StateMachineConfig configuration)
        {
            foreach (var stateConfig in configuration.SubStateMachines)
            {
                var machine = new StateMachine(owner, stateConfig.State, stateConfig.reenterable);
                _subMachines.Add(machine);
                if (stateConfig.Event != null) _callbacks.Add(stateConfig.Event, () => { SetState(machine); });
                if (stateConfig.initial) InitialState = machine;
            }
        }

        private void RegisterEvents()
        {
            //Debug.Log($"Registering {_callbacks.Count} Events for State: {_config.name} with Id: {Id}");
            foreach (var kvp in _callbacks)
            {
                kvp.Key.On += kvp.Value;
            }
        }

        private void UnregisterEvents()
        {
            //Debug.Log($"Unregistering {_callbacks.Count} Events for State: {_config.name} with Id: {Id}");
            foreach (var kvp in _callbacks)
            {
                kvp.Key.On -= kvp.Value;
            }
        }

        public void Dispose()
        {
            UnregisterEvents();
            foreach (var machine in _subMachines)
            {
                machine.Dispose();
            }
        }

        public CoroutineWrapper SetState(State nextState)
        {
            if (_current != null)
            {
                if (_current.Id == nextState.Id && !nextState.Reenterable) return new NullCoroutineWrapper();
                _current.Exit().Start();
            }
            _current = nextState;
            var output = _current.Enter();
            output.Start().WhenDone(() =>{ nextState.Update().Start(); });
            return output;
        }

        public override IEnumerator OnEnter()
        {
            //Debug.Log($"Entering State: {_config.name} with Id: {Id}");
            foreach (var state in _internalStates)
            {
                state.Enter().Start().WhenDone(() => { state.Update().Start(); });
            }
            EnterInitialState();
            yield return null;
        }

        public override IEnumerator OnExit()
        {
            //Debug.Log($"Exiting State: {_config.name} with Id: {Id}");
            UnregisterEvents();
            _current?.Exit().Start();
            _current = null;
            foreach (var state in _internalStates)
            {
                state.Exit().Start();
            }
            yield return null;
        }

        public CoroutineWrapper EnterInitialState()
        {
            RegisterEvents();
            return SetState(InitialState);
        }
    }
}