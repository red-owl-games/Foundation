using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class PlayerInputSettings : ScriptableObject
    {
        internal const string DataLocation = "Game/Resources/Config/PlayerInput.asset";
        
        public bool player1And2ShareKeyboard;
        [FoldoutGroup("Player1")]
        public InputActionMap player1;
        [FoldoutGroup("Player2")]
        public InputActionMap player2;
        [FoldoutGroup("Player3")]
        public InputActionMap player3;
        [FoldoutGroup("Player4")]
        public InputActionMap player4;

        [Button, PropertyOrder(-1)]
        internal void Reset()
        {
            player1 = NewActionMap();
            player2 = NewActionMap();
            player3 = NewActionMap();
            player4 = NewActionMap();
        }

        internal static PlayerInputSettings Load()
        {
            return Resources.Load<PlayerInputSettings>("Config/PlayerInput");
        }

        private static InputActionMap NewActionMap()
        {
            var actionMap = new InputActionMap("config");

            var leftStick = actionMap.AddAction("LeftStick", binding: "<Gamepad>/leftStick",
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
            actionMap.AddAction("RightStick", binding: "<Gamepad>/rightStick", expectedControlLayout: "Vector2");

            actionMap.AddAction("DPadNorth", binding: "<Gamepad>/dpad/up", type: InputActionType.Button)
                .AddBinding("<Keyboard>/1");
            actionMap.AddAction("DPadSouth", binding: "<Gamepad>/dpad/down", type: InputActionType.Button)
                .AddBinding("<Keyboard>/2");
            actionMap.AddAction("DPadWest", binding: "<Gamepad>/dpad/left", type: InputActionType.Button)
                .AddBinding("<Keyboard>/3");
            actionMap.AddAction("DPadEast", binding: "<Gamepad>/dpad/right", type: InputActionType.Button)
                .AddBinding("<Keyboard>/4");

            actionMap.AddAction("ButtonNorth", binding: "<Gamepad>/buttonNorth", type: InputActionType.Button)
                .AddBinding("<Keyboard>/e");
            actionMap.AddAction("ButtonSouth", binding: "<Gamepad>/buttonSouth", type: InputActionType.Button)
                .AddBinding("<Keyboard>/space");
            actionMap.AddAction("ButtonWest", binding: "<Gamepad>/buttonWest", type: InputActionType.Button)
                .AddBinding("<Keyboard>/r");
            actionMap.AddAction("ButtonEast", binding: "<Gamepad>/buttonEast", type: InputActionType.Button)
                .AddBinding("<Keyboard>/escape");

            actionMap.AddAction("LeftShoulder", binding: "<Gamepad>/leftShoulder", type: InputActionType.Button);
            actionMap.AddAction("RightShoulder", binding: "<Gamepad>/rightShoulder", type: InputActionType.Button);

            actionMap.AddAction("LeftTrigger", binding: "<Gamepad>/leftTrigger", type: InputActionType.Button)
                .AddCompositeBinding("ButtonWithOneModifier")
                .With("Button", "<Keyboard>/e")
                .With("Modifier", "<Keyboard>/leftShift");
            actionMap.AddAction("RightTrigger", binding: "<Gamepad>/rightTrigger", type: InputActionType.Button)
                .AddCompositeBinding("ButtonWithOneModifier")
                .With("Button", "<Keyboard>/r")
                .With("Modifier", "<Keyboard>/leftShift");

            actionMap.AddAction("LeftStickPress", binding: "<Gamepad>/leftStickPress", type: InputActionType.Button);
            actionMap.AddAction("RightStickPress", binding: "<Gamepad>/RightStickPress", type: InputActionType.Button);

            actionMap.AddAction("Start", binding: "<Gamepad>/start", type: InputActionType.Button)
                .AddBinding("<Keyboard>/tab");
            actionMap.AddAction("Select", binding: "<Gamepad>/select", type: InputActionType.Button)
                .AddCompositeBinding("ButtonWithOneModifier")
                .With("Button", "<Keyboard>/tab")
                .With("Modifier", "<Keyboard>/leftShift");

            actionMap.AddAction("MousePosition", binding: "<Mouse>/position", expectedControlLayout: "Vector2");

            actionMap.AddAction("LeftMouseButton", binding: "<Mouse>/leftButton", type: InputActionType.Button);
            actionMap.AddAction("MiddleMouseButton", binding: "<Mouse>/middleButton", type: InputActionType.Button);
            actionMap.AddAction("RightMouseButton", binding: "<Mouse>/rightButton", type: InputActionType.Button);

            actionMap.AddAction("ScrollWheel", binding: "<Mouse>/scroll/y", expectedControlLayout: "float");

            return actionMap;
        }
    }
}