using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RedOwl.Core.Input
{
    public static class InputUtils
    {
        public static InputActionReference UpdateReference(InputActionAsset asset, InputActionReference actionReference)
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
        
        public static void SetActionCallback(InputActionReference actionReference, Action<InputAction.CallbackContext> callback, bool install)
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