using System;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    public class NullState : BaseState { }
    
    public class CallbackState : BaseState
    {
        public Action WhenEnter;
        public Action WhenExit;

        public override void Enter()
        {
            base.Enter();
            WhenEnter?.Invoke();
        }

        public override void Exit()
        {
            base.Exit();
            WhenExit?.Invoke();
        }
    }

    public class UnityEventState : BaseState, IStateBehaviour
    {
        [FoldoutGroup("Callbacks")]
        [UnityEventFoldout]
        public UnityEvent WhenEnter;
        [FoldoutGroup("Callbacks")]
        [UnityEventFoldout]
        public UnityEvent WhenExit;

        public override void Enter()
        {
            base.Enter();
            WhenEnter.Invoke();
        }

        public override void Exit()
        {
            base.Exit();
            WhenExit.Invoke();
        }
    }
}