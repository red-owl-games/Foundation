using UnityEngine;
using UnityEngine.InputSystem;

namespace RedOwl.Engine
{
   
    public enum ButtonStates
    {
        Cancelled,
        Pressed,
        Held,
    }

    public interface IAvatarInput
    {
        void AssignAvatar(Avatar avatar);
    }

    public abstract class AvatarInputBase
    {
        private Avatar _avatar;

        public void AssignAvatar(Avatar avatar)
        {
            _avatar = avatar;
        }
        
        protected void ApplyInputImmediately()
        {
            _avatar.ProcessInput();
        }
        
        public static ButtonStates GetState(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                return ButtonStates.Cancelled;
            }
            if (context.started)
            {
                return ButtonStates.Pressed;
            }
            return context.performed ? ButtonStates.Held : ButtonStates.Cancelled;
        }

        public static Vector2 GetAxis(InputAction.CallbackContext context)
        {
            return context.ReadValue<Vector2>();
        }
    }
}