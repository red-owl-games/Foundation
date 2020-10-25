using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class ActionToEvent : MonoBehaviour
    {
        public ActionReference input = new ActionReference();
        
        public GameEvent @event = new GameEvent();

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