using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace RedOwl.Engine
{
    public enum AvatarInputButtons
    {
        North,
        South,
        East,
        West,
            
        ButtonSouth,
        ButtonEast,
        ButtonWest,
        ButtonNorth,
            
        TriggerRight,
        TriggerLeft,
            
        ShoulderRight,
        ShoulderLeft,
        
        SpecialRight,
        SpecialLeft
    }
    
    public enum ButtonStates
    {
        Cancelled,
        Pressed,
        Held,
    }

    public class AvatarInput
    {
        public float2 Move;
        public float2 Look;
        
        public ButtonStates North;
        public ButtonStates South;
        public ButtonStates East;
        public ButtonStates West;
        
        public ButtonStates ButtonSouth;
        public ButtonStates ButtonEast;
        public ButtonStates ButtonWest;
        public ButtonStates ButtonNorth;
        
        public ButtonStates TriggerRight;
        public ButtonStates TriggerLeft;
        
        public ButtonStates ShoulderRight;
        public ButtonStates ShoulderLeft;
        
        public ButtonStates SpecialRight;
        public ButtonStates SpecialLeft;

        public ButtonStates Get(AvatarInputButtons button)
        {
            switch (button)
            {
                case AvatarInputButtons.North:
                    return North;
                case AvatarInputButtons.South:
                    return South;
                case AvatarInputButtons.East:
                    return East;
                case AvatarInputButtons.West:
                    return West;
                case AvatarInputButtons.ButtonSouth:
                    return ButtonSouth;
                case AvatarInputButtons.ButtonEast:
                    return ButtonEast;
                case AvatarInputButtons.ButtonWest:
                    return ButtonWest;
                case AvatarInputButtons.ButtonNorth:
                    return ButtonNorth;
                case AvatarInputButtons.TriggerRight:
                    return TriggerRight;
                case AvatarInputButtons.TriggerLeft:
                    return TriggerLeft;
                case AvatarInputButtons.ShoulderRight:
                    return ShoulderRight;
                case AvatarInputButtons.ShoulderLeft:
                    return ShoulderLeft;
                case AvatarInputButtons.SpecialRight:
                    return SpecialRight;
                case AvatarInputButtons.SpecialLeft:
                    return SpecialLeft;
            }

            return ButtonStates.Cancelled;
        }
        
        public void Set(AvatarInputButtons button, ButtonStates value)
        {
            switch (button)
            {
                case AvatarInputButtons.North:
                    North = value;
                    break;
                case AvatarInputButtons.South:
                    South = value;
                    break;
                case AvatarInputButtons.East:
                    East = value;
                    break;
                case AvatarInputButtons.West:
                    West = value;
                    break;
                case AvatarInputButtons.ButtonSouth:
                    ButtonSouth = value;
                    break;
                case AvatarInputButtons.ButtonEast:
                    ButtonEast = value;
                    break;
                case AvatarInputButtons.ButtonWest:
                    ButtonWest = value;
                    break;
                case AvatarInputButtons.ButtonNorth:
                    ButtonNorth = value;
                    break;
                case AvatarInputButtons.TriggerRight:
                    TriggerRight = value;
                    break;
                case AvatarInputButtons.TriggerLeft:
                    TriggerLeft = value;
                    break;
                case AvatarInputButtons.ShoulderRight:
                    ShoulderRight = value;
                    break;
                case AvatarInputButtons.ShoulderLeft:
                    ShoulderLeft = value;
                    break;
                case AvatarInputButtons.SpecialRight:
                    SpecialRight = value;
                    break;
                case AvatarInputButtons.SpecialLeft:
                    SpecialLeft = value;
                    break;
            }
        }
    }

    public class AvatarInputManager : AvatarControls.IAvatarActions
    {
        private Avatar _avatar;
        private InputUser _user;
        private AvatarControls _controls;
        private AvatarInput _input;

        public AvatarInputManager(Avatar avatar)
        {
            _avatar = avatar;
            _user = InputUser.CreateUserWithoutPairedDevices();
            _controls = new AvatarControls();
            _controls.Avatar.SetCallbacks(this);
            _input = new AvatarInput();
        }

        public void Enable()
        {
            _user = InputUser.PerformPairingWithDevice(Keyboard.current, _user, InputUserPairingOptions.ForceNoPlatformUserAccountSelection);
            _user.AssociateActionsWithUser(_controls);
            _user.ActivateControlScheme($"Player{_user.index + 1}");
            _controls.Enable();
        }

        public void Disable()
        {
            _controls.Disable();
        }

        private void Apply()
        {
            _avatar.HandleInput(ref _input);
        }
        
        private ButtonStates GetState(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                return ButtonStates.Pressed;
            }
            return context.performed ? ButtonStates.Held : ButtonStates.Cancelled;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _input.Move = context.ReadValue<Vector2>();
            Apply();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _input.Look = context.ReadValue<Vector2>();
            Apply();
        }

        public void OnButtonSouth(InputAction.CallbackContext context)
        {
            _input.ButtonSouth = GetState(context);
            Apply();
        }

        public void OnButtonEast(InputAction.CallbackContext context)
        {
            _input.ButtonEast = GetState(context);
            Apply();
        }

        public void OnButtonWest(InputAction.CallbackContext context)
        {
            _input.ButtonWest = GetState(context);
            Apply();
        }

        public void OnButtonNorth(InputAction.CallbackContext context)
        {
            _input.ButtonNorth = GetState(context);
            Apply();
        }

        public void OnTriggerRight(InputAction.CallbackContext context)
        {
            _input.TriggerRight = GetState(context);
            Apply();
        }

        public void OnTriggerLeft(InputAction.CallbackContext context)
        {
            _input.TriggerLeft = GetState(context);
            Apply();
        }

        public void OnShoulderRight(InputAction.CallbackContext context)
        {
            _input.ShoulderRight = GetState(context);
            Apply();
        }

        public void OnShoulderLeft(InputAction.CallbackContext context)
        {
            _input.ShoulderLeft = GetState(context);
            Apply();
        }

        public void OnSpecialRight(InputAction.CallbackContext context)
        {
            _input.SpecialRight = GetState(context);
            Apply();
        }

        public void OnSpecialLeft(InputAction.CallbackContext context)
        {
            _input.SpecialLeft = GetState(context);
            Apply();
        }

        public void OnNorth(InputAction.CallbackContext context)
        {
            _input.North = GetState(context);
            Apply();
        }

        public void OnSouth(InputAction.CallbackContext context)
        {
            _input.South = GetState(context);
            Apply();
        }

        public void OnEast(InputAction.CallbackContext context)
        {
            _input.East = GetState(context);
            Apply();
        }

        public void OnWest(InputAction.CallbackContext context)
        {
            _input.West = GetState(context);
            Apply();
        }
    }
}