using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

namespace RedOwl.Engine
{
    public class InputHandler
    {
        public InputDevice Mouse { get; set; }
        public InputDevice Keyboard { get; set; }
        public InputDevice Gamepad { get; set; }

        private readonly InputActionMap _state;

        public InputAction LeftStick { get; private set; }
        public InputAction RightStick { get; private set; }

        public InputAction DPadNorth { get; private set; }
        public InputAction DPadSouth { get; private set; }
        public InputAction DPadWest { get; private set; }
        public InputAction DPadEast { get; private set; }

        public InputAction ButtonNorth { get; private set; }
        public InputAction ButtonSouth { get; private set; }
        public InputAction ButtonWest { get; private set; }
        public InputAction ButtonEast { get; private set; }

        public InputAction LeftShoulder { get; private set; }
        public InputAction RightShoulder { get; private set; }

        public InputAction LeftTrigger { get; private set; }
        public InputAction RightTrigger { get; private set; }

        public InputAction LeftStickPress { get; private set; }
        public InputAction RightStickPress { get; private set; }

        public InputAction Start { get; private set; }
        public InputAction Select { get; private set; }

        public InputAction MousePosition { get; private set; }

        public InputAction LeftMouseButton { get; private set; }
        public InputAction MiddleMouseButton { get; private set; }
        public InputAction RightMouseButton { get; private set; }

        public InputAction ScrollWheel { get; private set; }


        public InputHandler(InputActionMap map)
        {
            _state = map.Clone();
            
            LeftStick = _state.FindAction("LeftStick", true);
            RightStick = _state.FindAction("RightStick", true);

            DPadNorth = _state.FindAction("DPadNorth", true);
            DPadSouth = _state.FindAction("DPadSouth", true);
            DPadWest = _state.FindAction("DPadWest", true);
            DPadEast = _state.FindAction("DPadEast", true);

            ButtonNorth = _state.FindAction("ButtonNorth", true);
            ButtonSouth = _state.FindAction("ButtonSouth", true);
            ButtonWest = _state.FindAction("ButtonWest", true);
            ButtonEast = _state.FindAction("ButtonEast", true);

            LeftShoulder = _state.FindAction("LeftShoulder", true);
            RightShoulder = _state.FindAction("RightShoulder", true);

            LeftTrigger = _state.FindAction("LeftTrigger", true);
            RightTrigger = _state.FindAction("RightTrigger", true);

            LeftStickPress = _state.FindAction("LeftStickPress", true);
            RightStickPress = _state.FindAction("RightStickPress", true);

            Start = _state.FindAction("Start", true);
            Select = _state.FindAction("Select", true);

            MousePosition = _state.FindAction("MousePosition", true);

            LeftMouseButton = _state.FindAction("LeftMouseButton", true);
            MiddleMouseButton = _state.FindAction("MiddleMouseButton", true);
            RightMouseButton = _state.FindAction("RightMouseButton", true);

            ScrollWheel = _state.FindAction("ScrollWheel", true);
        }

        [ButtonGroup("Control"), Button, PropertyOrder(-10)]
        public void Enable()
        {
            var devices = new List<InputDevice>();
            if (Mouse != null) devices.Add(Mouse);
            if (Keyboard != null) devices.Add(Keyboard);
            if (Gamepad != null) devices.Add(Gamepad);
            _state.devices = devices.ToArray();
            _state.Enable();
        }

        [ButtonGroup("Control"), Button, PropertyOrder(-10)]
        public void Disable()
        {
            _state.Disable();
        }
    }

    public enum Players
    {
        None,
        Player1,
        Player2,
        Player3,
        Player4
    }

    public class PlayerInputSystem : SystemBase<PlayerInputSystem>
    {
        private InputHandler _player1;
        private InputHandler _player2;
        private InputHandler _player3;
        private InputHandler _player4;
        
        protected override void OnCreate()
        {
            var settings = PlayerInputSettings.Load();
            
            _player1 = new InputHandler(settings.player1)
            {
                Mouse = Mouse.current,
                Keyboard = Keyboard.current,
                Gamepad = Gamepad.all.Count > 0 ? Gamepad.all[0] : null
            };

            _player2 = new InputHandler(settings.player2)
            {
                Mouse = settings.player1And2ShareKeyboard ? Mouse.current : null,
                Keyboard = settings.player1And2ShareKeyboard ? Keyboard.current : null,
                Gamepad = Gamepad.all.Count > 1 ? Gamepad.all[1] : null
            };

            _player3 = new InputHandler(settings.player3)
            {
                Gamepad = Gamepad.all.Count > 2 ? Gamepad.all[2] : null
            };

            _player4 = new InputHandler(settings.player4)
            {
                Gamepad = Gamepad.all.Count > 3 ? Gamepad.all[3] : null
            };
        }

        private void EnableAll()
        {
            _player1.Enable();
            _player2.Enable();
            _player3.Enable();
            _player4.Enable();
        }

        private void DisableAll()
        {
            _player1.Disable();
            _player2.Disable();
            _player3.Disable();
            _player4.Disable();
        }

        protected override void OnStartRunning()
        {
            EnableAll();
        }

        public InputHandler Get(Players player)
        {
            switch (player)
            {
                case Players.Player1:
                default:
                    return _player1;
                case Players.Player2:
                    return _player2;
                case Players.Player3:
                    return _player3;
                case Players.Player4:
                    return _player4;
            }
        }
    }
}
