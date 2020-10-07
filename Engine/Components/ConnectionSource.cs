using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    [HideMonoScript]
    public class ConnectionSource : MonoBehaviour, IConnectionSource, IInteractable
    {
        public bool useTrigger;
        public BoolEvent onTriggered = new BoolEvent();
        
        public event Action ConnectionTriggered;
        public bool ConnectionState { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (useTrigger) InternalInteract();
        }

        private void OnTriggerExit(Collider other)
        {
            if (useTrigger) InternalInteract();
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