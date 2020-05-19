using System;
using UnityEngine;

namespace RedOwl.Core
{
    public class StateMachineManager : MonoBehaviour
    {
        [SerializeField] private StateMachineConfig stateMachine;

        private StateMachine _machine;
        private CoroutineWrapper _wrapper;

        private void Awake()
        {
            _machine = new StateMachine(gameObject, stateMachine);
        }

        private void Start()
        {
            _wrapper = _machine.EnterInitialState();
        }

        private void OnDestroy()
        {
            _wrapper.Stop();
        }
    }
}