using System;
using System.Collections.Generic;
using RedOwl.Engine;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    // public partial class GamePrefs
    // {
    //     public InputHandler Player1;
    // }
    
    [Serializable]
    [CreateAssetMenu(fileName = "InputHandler", menuName = "Red Owl/Input Handler")]
    public class InputHandler : ScriptableObject
    {
        [SerializeField] private InputActionMap config;

        public InputDevice Mouse { get; set; }
        public InputDevice Keyboard { get; set; }
        public InputDevice Gamepad { get; set; }

        private InputActionMap state;

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

        [NonSerialized] private bool isInitialized;

        public void Init()
        {
            if (isInitialized) return;
            state = config.Clone();

            LeftStick = state.FindAction("LeftStick", true);
            RightStick = state.FindAction("RightStick", true);

            DPadNorth = state.FindAction("DPadNorth", true);
            DPadSouth = state.FindAction("DPadSouth", true);
            DPadWest = state.FindAction("DPadWest", true);
            DPadEast = state.FindAction("DPadEast", true);

            ButtonNorth = state.FindAction("ButtonNorth", true);
            ButtonSouth = state.FindAction("ButtonSouth", true);
            ButtonWest = state.FindAction("ButtonWest", true);
            ButtonEast = state.FindAction("ButtonEast", true);

            LeftShoulder = state.FindAction("LeftShoulder", true);
            RightShoulder = state.FindAction("RightShoulder", true);

            LeftTrigger = state.FindAction("LeftTrigger", true);
            RightTrigger = state.FindAction("RightTrigger", true);

            LeftStickPress = state.FindAction("LeftStickPress", true);
            RightStickPress = state.FindAction("RightStickPress", true);

            Start = state.FindAction("Start", true);
            Select = state.FindAction("Select", true);

            MousePosition = state.FindAction("MousePosition", true);

            LeftMouseButton = state.FindAction("LeftMouseButton", true);
            MiddleMouseButton = state.FindAction("MiddleMouseButton", true);
            RightMouseButton = state.FindAction("RightMouseButton", true);

            ScrollWheel = state.FindAction("ScrollWheel", true);

            isInitialized = true;
        }

        [ButtonGroup("Control"), Button, PropertyOrder(-10)]
        public void Enable()
        {
            Init();
            var devices = new List<InputDevice>();
            if (Mouse != null) devices.Add(Mouse);
            if (Keyboard != null) devices.Add(Keyboard);
            if (Gamepad != null) devices.Add(Gamepad);
            state.devices = devices.ToArray();
            state.Enable();
        }

        [ButtonGroup("Control"), Button, PropertyOrder(-10)]
        public void Disable()
        {
            Init();
            state.Disable();
        }

#if UNITY_EDITOR
        private void Reset()
        {
            Initialize();
        }
#endif

        [ButtonGroup("Controls"), Button]
        private void Initialize()
        {
            config?.Dispose();
            config = new InputActionMap("config");

            var leftStick = config.AddAction("LeftStick", binding: "<Gamepad>/leftStick",
                expectedControlLayout: "Vector2");
            leftStick.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            leftStick.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");
            config.AddAction("RightStick", binding: "<Gamepad>/rightStick", expectedControlLayout: "Vector2");

            config.AddAction("DPadNorth", binding: "<Gamepad>/dpad/up", type: InputActionType.Button)
                .AddBinding("<Keyboard>/1");
            config.AddAction("DPadSouth", binding: "<Gamepad>/dpad/down", type: InputActionType.Button)
                .AddBinding("<Keyboard>/2");
            config.AddAction("DPadWest", binding: "<Gamepad>/dpad/left", type: InputActionType.Button)
                .AddBinding("<Keyboard>/3");
            config.AddAction("DPadEast", binding: "<Gamepad>/dpad/right", type: InputActionType.Button)
                .AddBinding("<Keyboard>/4");

            config.AddAction("ButtonNorth", binding: "<Gamepad>/buttonNorth", type: InputActionType.Button)
                .AddBinding("<Keyboard>/e");
            config.AddAction("ButtonSouth", binding: "<Gamepad>/buttonSouth", type: InputActionType.Button)
                .AddBinding("<Keyboard>/space");
            config.AddAction("ButtonWest", binding: "<Gamepad>/buttonWest", type: InputActionType.Button)
                .AddBinding("<Keyboard>/r");
            config.AddAction("ButtonEast", binding: "<Gamepad>/buttonEast", type: InputActionType.Button)
                .AddBinding("<Keyboard>/escape");

            config.AddAction("LeftShoulder", binding: "<Gamepad>/leftShoulder", type: InputActionType.Button);
            config.AddAction("RightShoulder", binding: "<Gamepad>/rightShoulder", type: InputActionType.Button);

            config.AddAction("LeftTrigger", binding: "<Gamepad>/leftTrigger", type: InputActionType.Button)
                .AddCompositeBinding("ButtonWithOneModifier")
                .With("Button", "<Keyboard>/e")
                .With("Modifier", "<Keyboard>/leftShift");
            config.AddAction("RightTrigger", binding: "<Gamepad>/rightTrigger", type: InputActionType.Button)
                .AddCompositeBinding("ButtonWithOneModifier")
                .With("Button", "<Keyboard>/r")
                .With("Modifier", "<Keyboard>/leftShift");

            config.AddAction("LeftStickPress", binding: "<Gamepad>/leftStickPress", type: InputActionType.Button);
            config.AddAction("RightStickPress", binding: "<Gamepad>/RightStickPress", type: InputActionType.Button);

            config.AddAction("Start", binding: "<Gamepad>/start", type: InputActionType.Button)
                .AddBinding("<Keyboard>/tab");
            config.AddAction("Select", binding: "<Gamepad>/select", type: InputActionType.Button)
                .AddCompositeBinding("ButtonWithOneModifier")
                .With("Button", "<Keyboard>/tab")
                .With("Modifier", "<Keyboard>/leftShift");

            config.AddAction("MousePosition", binding: "<Mouse>/position", expectedControlLayout: "Vector2");

            config.AddAction("LeftMouseButton", binding: "<Mouse>/leftButton", type: InputActionType.Button);
            config.AddAction("MiddleMouseButton", binding: "<Mouse>/middleButton", type: InputActionType.Button);
            config.AddAction("RightMouseButton", binding: "<Mouse>/rightButton", type: InputActionType.Button);

            config.AddAction("ScrollWheel", binding: "<Mouse>/scroll/y", expectedControlLayout: "float");
        }
    }
    
    public class PlayerInputSystem : SystemBase
        {
            public InputHandler Player1 { get; private set; }
            public InputHandler Player2 { get; private set; }
            public InputHandler Player3 { get; private set; }
            public InputHandler Player4 { get; private set; }

            protected override void OnCreate()
            {
                //if (Settings.player1 != null)
                {
                    Player1 = Object.Instantiate(Resources.Load<InputHandler>("Player1"));
                    Player1.Init();
                    Player1.Mouse = Mouse.current;
                    Player1.Keyboard = Keyboard.current;
                    Player1.Gamepad = Gamepad.all.Count > 0 ? Gamepad.all[0] : null;
                }
                // //if (Settings.player2 != null)
                // {
                //     Player2 = Object.Instantiate(Resources.Load<InputHandler>("Player2"));
                //     Player2.Init();
                //     // TODO: If split keyboard then assign keyboard too
                //     Player2.Gamepad = Gamepad.all.Count > 1 ? Gamepad.all[1] : null;
                // }
                // //if (Settings.player3 != null)
                // {
                //     Player3 = Object.Instantiate(Resources.Load<InputHandler>("Player3"));
                //     Player3.Init();
                //     Player3.Gamepad = Gamepad.all.Count > 2 ? Gamepad.all[2] : null;
                // }
                // //if (Settings.player4 != null)
                // {
                //     Player4 = Object.Instantiate(Resources.Load<InputHandler>("Player4"));
                //     Player4.Init();
                //     Player4.Gamepad = Gamepad.all.Count > 3 ? Gamepad.all[3] : null;
                // }
            }

            private void EnableAll()
            {
                if (Player1 != null) Player1.Enable();
                if (Player2 != null) Player2.Enable();
                if (Player3 != null) Player3.Enable();
                if (Player4 != null) Player4.Enable();
            }

            private void DisableAll()
            {
                if (Player1 != null) Player1.Disable();
                if (Player2 != null) Player2.Disable();
                if (Player3 != null) Player3.Disable();
                if (Player4 != null) Player4.Disable();
            }

            protected override void OnStartRunning()
            {
                EnableAll();
            }
        }
}
