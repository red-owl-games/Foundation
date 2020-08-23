using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Core
{
    [HideMonoScript]
    public class EventInteractable : MonoBehaviour, IInteractable
    {
        [LabelText("On Interact")]
        public ITelegram onInteractTelegram;
        [LabelText("On Interact")]
        public UnityEvent onInteractUnity = new UnityEvent();

        private bool _usesTelegram;

        private void Awake()
        {
            _usesTelegram = onInteractTelegram != null;
        }

        public void Interact()
        {
            if (_usesTelegram) onInteractTelegram.Raise();
            onInteractUnity?.Invoke();
        }
    }
}