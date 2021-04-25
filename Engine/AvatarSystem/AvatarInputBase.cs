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
    
    public interface IAvatarInput {}

    public interface IAvatarInputMove : IAvatarInput
    {
        Vector2 InputMove { get; }
    }

    public interface IAvatarInputLook : IAvatarInput
    {
        Vector2 InputLook { get; }
    }
    
    public interface IAvatarInputMoveAndLook : IAvatarInputMove, IAvatarInputLook {}

    public static class InputActionExt
    {
        public static ButtonStates GetState(this InputAction.CallbackContext self)
        {
            if (self.canceled)
            {
                return ButtonStates.Cancelled;
            }
            if (self.started)
            {
                return ButtonStates.Pressed;
            }
            return self.performed ? ButtonStates.Held : ButtonStates.Cancelled;
        }

        public static bool IsPressed(this ButtonStates self)
        {
            return self == ButtonStates.Pressed || self == ButtonStates.Held;
        }

        public static bool WasPressed(this ButtonStates self)
        {
            return self == ButtonStates.Pressed;
        }

        public static Vector2 GetAxis(this InputAction.CallbackContext self)
        {
            return self.ReadValue<Vector2>();
        }
    }
}