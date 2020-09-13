using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace RedOwl.Core.Input
{
    [HideMonoScript]
    public class ActionToEvent : MonoBehaviour
    {
        public ActionReference input = new ActionReference();
        
        public UnityEvent @event = new UnityEvent();

        private void OnEnable()
        {
            input.Bind(OnInput);
        }

        private void OnDisable()
        {
            input.Unbind(OnInput);
        }

        private void OnInput(InputAction.CallbackContext ctx)
        {
            if (ctx.ReadValueAsButton())
                @event.Invoke();
        }
    }
}