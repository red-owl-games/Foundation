using System;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace RedOwl.Engine
{
    public interface IInputState
    {
        // TODO: should we use more custom types to handle press vs hold on buttons?
        float2 JoystickLeft { get; }
        float2 JoystickRight { get; }
        float2 Dpad { get; }
        
        bool ButtonNorth { get; }
        bool ButtonSouth { get; }
        bool ButtonEast { get; }
        bool ButtonWest { get; }
        
        bool TriggerLeft { get; }
        bool TriggerRight { get; }
        
        bool ShoulderLeft { get; }
        bool ShoulderRight { get; }
        
        bool JoystickLeftButton { get; }
        bool JoystickRightButton { get; }
        
        bool CommandLeft { get; }
        bool CommandRight { get; }
        bool CommandCenter { get; }

        void Vibrate(float duration);
    }

    /// <summary>
    /// PlayerInput is code generated from an Input Action Asset
    /// var Controls = new PlayerInput();
    /// var GamepadState = new InputState(Controls, Controls.GamepadScheme);
    /// var KeyboardState = new InputState(Controls, Controls.KeyboardMouseScheme);
    /// </summary>
    public class InputState : IInputState
    {
        public event Action<float> OnVibrate;

        public InputUser User { get; private set; }

        public InputState(IInputActionCollection actions, InputControlScheme controlScheme)
        {
            User = InputUser.CreateUserWithoutPairedDevices();
            User.AssociateActionsWithUser(actions);
            SwitchControlScheme(controlScheme);
        }

        public void SwitchControlScheme(InputControlScheme controlScheme)
        {
            User.UnpairDevices();
            User.ActivateControlScheme(controlScheme).AndPairRemainingDevices();
        }

        public float2 JoystickLeft { get; set; }
        public float2 JoystickRight { get; set; }
        public float2 Dpad { get; set; }
        public bool ButtonNorth { get; set; }
        public bool ButtonSouth { get; set; }
        public bool ButtonEast { get; set; }
        public bool ButtonWest { get; set; }
        public bool TriggerLeft { get; set; }
        public bool TriggerRight { get; set; }
        public bool ShoulderLeft { get; set; }
        public bool ShoulderRight { get; set; }
        public bool JoystickLeftButton { get; set; }
        public bool JoystickRightButton { get; set; }
        public bool CommandLeft { get; set; }
        public bool CommandRight { get; set; }
        public bool CommandCenter { get; set; }

        public void Vibrate(float duration)
        {
            OnVibrate?.Invoke(duration);
        }
    }
}