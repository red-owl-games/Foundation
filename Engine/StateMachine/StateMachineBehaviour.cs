using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public class StateCollection : BetterKeyedCollection<string, IState>
    {
        protected override string GetKeyForItem(IState item) => item.Name;
    }
    
    public abstract class StateMachineBaseBehaviour : MonoBehaviour
    {
        private StateMachine _machine;

        [ShowInInspector, DisplayAsString, PropertyOrder(-100)]
        public string CurrentState => _machine?.CurrentState?.Name ?? "";

        public StateCollection States => _machine.States;

        protected virtual void OnValidate()
        {
            if (!Game.IsRunning) BuildStateMachine();
        }

        protected virtual void Awake()
        {
            BuildStateMachine();
        }

        protected virtual void Start()
        {
            _machine?.Start();
        }

        protected virtual void Update()
        {
            _machine?.Update(Time.deltaTime);
        }

        protected virtual void LateUpdate()
        {
            _machine?.LateUpdate(Time.deltaTime);
        }

        protected virtual void FixedUpdate()
        {
            _machine?.FixedUpdate(Time.deltaTime);
        }

        protected void BuildStateMachine()
        {
            _machine = new StateMachine(name);
            ConfigureStateMachine(_machine);
        }

        protected abstract void ConfigureStateMachine(StateMachine machine);
    }

    [HideMonoScript]
    public class StateMachineBehaviour : StateMachineBaseBehaviour
    {
        [SerializeField, ValueDropdown("PossibleStates")]
        private string initialState = StateMachine.NO_STATE;
        
        [SerializeReference, TypeFilter("PossibleStatesToAdd")]
        private List<IState> _states;

        protected IEnumerable PossibleStates()
        {
            yield return StateMachine.NO_STATE;
            if (_states == null) yield break;
            foreach (var state in _states)
            {
                if (state is IStateBehaviour) yield return state.Name;
            }
        }
        
        private IEnumerable PossibleStatesToAdd()
        {
            if (_states == null) yield break;
#if UNITY_EDITOR
            foreach (var state in UnityEditor.TypeCache.GetTypesDerivedFrom<IStateBehaviour>())
            {
                yield return state;
            }
#endif
        }

        protected override void OnValidate()
        {
            if (_states == null) _states = new List<IState>();
            base.OnValidate();
        }

        protected override void ConfigureStateMachine(StateMachine machine)
        {
            foreach (var state in _states)
            {
                machine.Add(state);
            }
            machine.SetInitialState(initialState);
        }
    }
}