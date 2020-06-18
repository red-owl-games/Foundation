using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RedOwl.Core.Input
{
    public class ActionToEvent : MonoBehaviour
    {
        [SerializeField, PropertyOrder(-1), ShowIf("@controls != null")]
        private InputActionReference action;

        [SerializeField, HideInInspector]
        private InputActionAsset controls;
        [ShowInInspector, PropertyOrder(-2)]
        public InputActionAsset Controls
        {
            get => controls;
            set
            {
                if (value == controls) return;
                controls = value;
                action = InputUtils.UpdateReference(controls, action);
            }
        }
        
        public GameEvent @event;

        private void OnEnable()
        {
            action?.action?.Enable();
            InputUtils.SetActionCallback(action, OnInput, true);
        }

        private void OnDisable()
        {
            InputUtils.SetActionCallback(action, OnInput, false);
            action?.action?.Disable();
        }

        private void OnInput(InputAction.CallbackContext ctx)
        {
            if (ctx.ReadValueAsButton())
                @event.Raise();
        }
    }
}