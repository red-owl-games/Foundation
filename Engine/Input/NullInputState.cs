using Unity.Mathematics;

namespace RedOwl.Engine
{
    public class NullInputState : IInputState
    {
        public float2 JoystickLeft => float2.zero;

        public float2 JoystickRight => float2.zero;

        public float2 Dpad => float2.zero;

        public bool ButtonNorth => false;

        public bool ButtonSouth => false;

        public bool ButtonEast => false;

        public bool ButtonWest => false;

        public bool TriggerLeft => false;

        public bool TriggerRight => false;

        public bool ShoulderLeft => false;

        public bool ShoulderRight => false;

        public bool JoystickLeftButton => false;

        public bool JoystickRightButton => false;

        public bool CommandLeft => false;

        public bool CommandRight => false;

        public bool CommandCenter => false;

        public void Vibrate(float duration) {}
    }
}