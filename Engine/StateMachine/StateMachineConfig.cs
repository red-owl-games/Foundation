using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace RedOwl.Core
{
    [Serializable]
    public struct StateConfig
    {
        [FormerlySerializedAs("initialState")] [TableColumnWidth(45, false)]
        public bool initial;
        [TableColumnWidth(75, false)]
        public bool reenterable;

        public GameEvent Event;
        
        [ValueDropdown("PossibleStates")]
        public string StateSelector;

        private State _state;
        internal State State => _state ?? (_state = StateCache.Get(StateSelector));

        private IList<ValueDropdownItem<string>> PossibleStates
        {
            get
            {
                var output = new ValueDropdownList<string>();
                foreach (string name in StateCache.AllNames)
                {
                    output.Add(name);
                }
                return output;
            }
        }

        public override string ToString()
        {
            return State.GetType().Name;
        }
    }

    [Serializable]
    public struct SubStateMachineConfig
    {
        [FormerlySerializedAs("initialState")]
        [TableColumnWidth(45, false)]
        public bool initial;
        
        [TableColumnWidth(75, false)]
        public bool reenterable;
        
        public GameEvent Event;

        [HideLabel]
        public StateMachineConfig State;

        public override string ToString()
        {
            return State.name;
        }
    }
    
    [HideMonoScript]
    [CreateAssetMenu(fileName = "StateMachine", menuName = "Red Owl/StateMachine")]
    public class StateMachineConfig : ScriptableObject
    {
        [ListDrawerSettings(Expanded = true)]
        [ValueDropdown("PossibleStates")]
        public List<string> States;
        
        [InfoBox("There is no Initial State set.  Please select one!", InfoMessageType.Error, "_hasNoInitialState")]
        [InfoBox("There is more then 1 Initial State set.  Please select only one!", InfoMessageType.Error, "_hasMoreThenOneInitialState")]
        [TableList(AlwaysExpanded = true)]
        public List<StateConfig> SubStates;
        [TableList(AlwaysExpanded = true)]
        public List<SubStateMachineConfig> SubStateMachines;

        private bool _hasNoInitialState;
        private bool _hasMoreThenOneInitialState;
        
        private void OnValidate()
        {
            int found = 0;
            foreach (var state in SubStates)
            {
                if (state.initial) found++;
            }

            foreach (var subState in SubStateMachines)
            {
                if (subState.initial) found++;
            }

            _hasNoInitialState = found == 0;
            _hasMoreThenOneInitialState = found > 1;
        }
        
        private IList<ValueDropdownItem<string>> PossibleStates
        {
            get
            {
                var output = new ValueDropdownList<string>();
                foreach (string name in StateCache.AllNames)
                {
                    output.Add(name);
                }
                return output;
            }
        }
    }
}