using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class ConnectionSource : MonoBehaviour, IConnectionSource, IInteractable
    {
        public bool useTrigger;
        public BoolEvent onTriggered = new BoolEvent();
        
        public event Action ConnectionTriggered;
        public bool ConnectionState { get; private set; }
        private Collider _presser;
        private bool _pressed;

        private void OnTriggerEnter(Collider other)
        {
            if (useTrigger && !_pressed)
            {
                _presser = other;
                _pressed = true;
                InternalInteract();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (useTrigger && !_pressed)
            {
                _presser = other;
                _pressed = true;
                InternalInteract();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (useTrigger && _pressed && other == _presser)
            {
                _presser = null;
                _pressed = false;
                InternalInteract();
            }
        }

        public void Select()
        {
        }

        public void Deselect()
        {
        }

        [Button]
        public void Interact()
        {
            if (!useTrigger) InternalInteract();
        }

        private void InternalInteract()
        {
            ConnectionState = !ConnectionState;
            onTriggered.Invoke(ConnectionState);
            ConnectionTriggered?.Invoke();
        }
    }
}