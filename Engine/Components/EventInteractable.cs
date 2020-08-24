using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Core
{
    [HideMonoScript]
    public class EventInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        [FoldoutGroup("Telegram", false)]
        [LabelText("On Select"), DisableInPlayMode]
        private ITelegram onSelectTelegram;
        [SerializeField]
        [FoldoutGroup("Telegram", false)]
        [LabelText("On Deselect"), DisableInPlayMode]
        private ITelegram onDeselectTelegram;
        [SerializeField]
        [FoldoutGroup("Telegram", false)]
        [LabelText("On Interact"), DisableInPlayMode]
        private ITelegram onInteractTelegram;
        
        [FoldoutGroup("Unity", false)]
        [LabelText("On Select")]
        public UnityEvent onSelectUnity;
        [FoldoutGroup("Unity", false)]
        [LabelText("On Deselect")]
        public UnityEvent onDeselectUnity;
        [FoldoutGroup("Unity", false)]
        [LabelText("On Interact")]
        public UnityEvent onInteractUnity;

        private bool _usesTelegramSelect;
        private bool _usesTelegramDeselect;
        private bool _usesTelegramInteract;

        private void Awake()
        {
            _usesTelegramSelect = onSelectTelegram != null;
            _usesTelegramDeselect = onDeselectTelegram != null;
            _usesTelegramInteract = onInteractTelegram != null;
            if (onSelectUnity == null) onSelectUnity = new UnityEvent();
            if (onDeselectUnity == null) onDeselectUnity = new UnityEvent();
            if (onInteractUnity == null) onInteractUnity = new UnityEvent();
        }

        public void Select()
        {
            if (_usesTelegramSelect) onSelectTelegram.Raise();
            onSelectUnity?.Invoke();
        }

        public void Deselect()
        {
            if (_usesTelegramDeselect) onDeselectTelegram.Raise();
            onDeselectUnity?.Invoke();
        }

        public void Interact()
        {
            if (_usesTelegramInteract) onInteractTelegram.Raise();
            onInteractUnity?.Invoke();
        }
    }
}