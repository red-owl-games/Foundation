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

    public abstract class AvatarInput : IAvatarInput
    {
        private Avatar _avatar;

        public void AssignAvatar(Avatar avatar)
        {
            _avatar = avatar;
        }
        
        protected void ProcessInput()
        {
            _avatar.HandleInput();
        }
        
        public static ButtonStates GetState(InputAction.CallbackContext context)
        {
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