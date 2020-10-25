using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RedOwl.Engine
{
    [Serializable, InlineProperty]
    public class ActionReference
    {
        [SerializeField, PropertyOrder(-1), LabelWidth(45), EnableIf("@controls != null")]
        private InputActionReference action;

        [SerializeField, HideInInspector]
        private InputActionAsset controls;
        [ShowInInspector, PropertyOrder(-2), LabelText("Asset"), LabelWidth(45)]
        public InputActionAsset Controls
        {
            get => controls;
            set
            {
                if (value == controls) return;
                controls = value;
                action = UpdateReference(controls, action);
            }
        }


        public void Bind(Action<InputAction.CallbackContext> callback)
        {
            if (action == null) return;
            action.action.Enable();
            SetActionCallback(action, callback, true);

        }

        public void Unbind(Action<InputAction.CallbackContext> callback)
        {
            if (action == null) return;
            action.action.Disable();
            SetActionCallback(action, callback, true);
        }
        
        internal static InputActionReference UpdateReference(InputActionAsset asset, InputActionReference actionReference)
        {
            if (asset == null) return null;
            
            var oldAction = actionReference?.action;
            if (oldAction == null)
                return null;

            var oldActionMap = oldAction.actionMap;
            Debug.Assert(oldActionMap != null, "Not expected to end up with a singleton action here");

            var newActionMap = asset.FindActionMap(oldActionMap.name);
            if (newActionMap == null)
                return null;

            var newAction = newActionMap.FindAction(oldAction.name);
            if (newAction == null)
                return null;

            return InputActionReference.Create(newAction);
        }
        
        internal static void SetActionCallback(InputActionReference actionReference, Action<InputAction.CallbackContext> callback, bool install)
        {
            if (!install && callback == null)
                return;

            if (actionReference == null)
                return;

            var action = actionReference.action;
            if (action == null)
                return;

            if (install)
            {
                action.performed += callback;
                action.canceled += callback;
            }
            else
            {
                action.performed -= callback;
                action.canceled -= callback;
            }
        }
    }
}