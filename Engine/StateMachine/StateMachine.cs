using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RedOwl.Core
{
    public class StateMachine : IStateEnter, IStateAsyncExecute, IStateExit, IStateIdentifiable
    {
        private static readonly List<ITransition> Empty = new List<ITransition>(0);
        
        private readonly Dictionary<string, IState> _states;
        private readonly Dictionary<string, List<ITransition>> _transitions;
        private List<ITransition> _currentTransitions;
        private readonly List<ITransition> _anyTransitions;

        private bool _isStarted;
        private IState _initial;
        private IState _current;
        private readonly bool _hasInternal;
        private readonly IState[] _internal;
        private Coroutine _routine;

        public string Id { get; }

        public StateMachine()
        {
            Id = Guid.NewGuid().ToString();
            _states = new Dictionary<string, IState>();
            _transitions = new Dictionary<string, List<ITransition>>();
            _currentTransitions = new List<ITransition>();
            _anyTransitions = new List<ITransition>();
        }

        public StateMachine(params IState[] states) : this()
        {
            _hasInternal = true;
            _internal = states;
        }
        
        #region Helpers

        private string GetId(IState state)
        {
            if (state is IStateIdentifiable s) return s.Id;
            return state.GetType().FullName;
        }
        
        private void Ensure(IState state)
        {
            if (_isStarted)
            {
                Log.Error($"Trying to add state '{GetId(state)}' after StateMachine is already started!");
            }
            string id = GetId(state);
            if (_states.ContainsKey(id)) return;
            _states[id] = state;
            _transitions[id] = new List<ITransition>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddAny(ITransition transition) => _anyTransitions.Add(transition);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Add(IState from, ITransition transition) => _transitions[GetId(from)].Add(transition);

        private ITransition GetNextTransition()
        {
            // TODO: Interweave _any & _current transitions by priority?
            foreach (var transition in _anyTransitions)
            {
                if (!transition.IsReady) continue;
                transition.Reset();
                return transition;
            }

            foreach (var transition in _currentTransitions)
            {
                if (!transition.IsReady) continue;
                transition.Reset();
                return transition;
            }

            return null;
        }

        private void InjectStates()
        {
            if (_hasInternal)
            {
                foreach (var state in _internal)
                {
                    Game.Inject(state);
                }
            }
            foreach (var state in _states.Values)
            {
                Game.Inject(state);
            }
        }
        
        private void SortTransitions()
        {
            int Compare(ITransition x, ITransition y) => x.Priority.CompareTo(y.Priority);
            foreach (var transitions in _transitions.Values)
            {
                transitions.Sort(Compare);
            }
            _anyTransitions.Sort(Compare);
        }
        
        private void EnableAnyTransitions()
        {
            foreach (var transition in _anyTransitions)
            {
                transition.Enable();
            }
        }
        
        private void DisableAnyTransitions()
        {
            foreach (var transition in _anyTransitions)
            {
                transition.Disable();
            }
        }

        private void EnterState(IState state)
        {
            //Log.Always($"Entering State {GetId(state)}");
            _transitions.TryGetValue(GetId(state), out _currentTransitions);
            if (_currentTransitions == null)
                _currentTransitions = Empty;
            foreach (var transition in _currentTransitions)
            {
                transition.Enable();
            }
            if (state is IStateEnter s) s.OnEnter();
            if (state is IStateAsyncEnter a) CoroutineManager.StartRoutine(a.OnEnter());
        }

        private void ExitState(IState state)
        {
            //Log.Always($"Exiting State {GetId(state)}");
            if (state is IStateExit s) s.OnExit();
            if (state is IStateAsyncExit a) CoroutineManager.StartRoutine(a.OnExit());
            foreach (var transition in _currentTransitions)
            {
                transition.Disable();
            }
        }

        private void SetState(IState state)
        {
            if (state == _current)
                return;
            ExitState(_current);
            _current = state;
            EnterState(_current);
        }
        
        private void EnterInternalStates()
        {
            if (!_hasInternal) return;
            foreach (var state in _internal)
            {
                if (state is IStateEnter s) s.OnEnter();
                if (state is IStateAsyncEnter a) CoroutineManager.StartRoutine(a.OnEnter());
            }
        }
        
        private void ExitInternalStates()
        {
            if (!_hasInternal) return;
            foreach (var state in _internal)
            {
                if (state is IStateExit s) s.OnExit();
                if (state is IStateAsyncExit a) CoroutineManager.StartRoutine(a.OnExit());
            }
        }

        private IEnumerator Wrapper()
        {
            while (true)
            {
                yield return OnExecute();
            }
        }
        
        #endregion

        #region IState
        
        public void OnEnter()
        {
            InjectStates();
            SortTransitions();
            EnableAnyTransitions();
            EnterInternalStates();
            if (_initial == null) _initial = new State($"{Id}InitialState");
            if (_current == null) _current = _initial;
            EnterState(_current);
        }

        public IEnumerator OnExecute()
        {
            var transition = GetNextTransition();
            if (transition != null) SetState(transition.To);
            if (_current is IStateExecute e) e.OnExecute();
            if (_current is IStateAsyncExecute a) yield return a.OnExecute();
            if (_hasInternal)
            {
                foreach (var state in _internal)
                {
                    if (state is IStateExecute ie) ie.OnExecute();
                    if (state is IStateAsyncExecute ia) yield return ia.OnExecute();
                }
            }
        }

        public void OnExit()
        {
            ExitInternalStates();
            DisableAnyTransitions();
            ExitState(_current);
            _current = _initial;
        }

        #endregion
        
        #region API
        
        public void Start()
        {
            OnEnter();
            _routine = CoroutineManager.StartRoutine(Wrapper());
            _isStarted = true;
        }
        
        public void Stop()
        {
            OnExit();
            CoroutineManager.StopRoutine(_routine);
            _isStarted = false;
        }

        public void Initial(IState initial)
        {
            Ensure(initial);
            _initial = initial;
        }
        
        public void Permit(IState from, IState to, int priority = 0)
        {
            Ensure(from);
            Ensure(to);
            Add(from, new CallbackTransition(to, priority, () => true));
        }
        
        public void Permit(IState to, Func<bool> guard, int priority = 0)
        {
            Ensure(to);
            AddAny(new CallbackTransition(to, priority, guard));
        }

        public void Permit(IState from, IState to, Func<bool> guard, int priority = 0)
        {
            Ensure(from);
            Ensure(to);
            Add(from, new CallbackTransition(to, priority, guard));
        }
        
        public void Permit(IState to, IMessage message, bool autoReset = true, int priority = 0)
        {
            Ensure(to);
            AddAny(new EventTransition(to, priority, message, autoReset));
        }

        public void Permit(IState from, IState to, IMessage message, bool autoReset = true, int priority = 0)
        {
            Ensure(from);
            Ensure(to);
            Add(from, new EventTransition(to, priority, message, autoReset));
        }
        
        public void Permit(IState to, IMessage message, Func<bool> guard, bool autoReset = true, int priority = 0)
        {
            Ensure(to);
            AddAny(new EventTransition(to, priority, message, guard, autoReset));
        }

        public void Permit(IState from, IState to, IMessage message, Func<bool> guard, bool autoReset = true, int priority = 0)
        {
            Ensure(from);
            Ensure(to);
            Add(from, new EventTransition(to, priority, message, guard, autoReset));
        }
        
        #endregion
    }
}