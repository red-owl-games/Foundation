using UnityEngine;

namespace RedOwl.Engine
{
    public interface IAvatarInputZJump : IAvatarInput
    {
        ButtonStates InputZJumpUp { get; }
        ButtonStates InputZJumpDown { get; }
    }
    
    public class AvatarZJump : AvatarAbility<IAvatarInputZJump>
    {
        public override int Priority { get; } = 100;

        public int steps = 1;
        public float stepSize = 5f;

        public AnimTriggerProperty jumpAnimParam = "ZJump";
        public AnimIntProperty jumpIndexAnimParam = "ZJumpIndex";

        private int _zIndex;

        public override void RegisterAnimatorParams(AnimatorController controller)
        {
            jumpAnimParam.Register(controller);
            jumpIndexAnimParam.Register(controller);
        }

        protected override void ProcessInput(ref IAvatarInputZJump input)
        {
            if (input.InputZJumpUp == ButtonStates.Pressed && _zIndex < steps)
            {
                jumpAnimParam.Set();
                _zIndex += 1;
            }

            if (input.InputZJumpDown == ButtonStates.Pressed && _zIndex > -steps)
            {
                jumpAnimParam.Set();
                _zIndex -= 1;
            }
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            jumpIndexAnimParam.Set(_zIndex);
            currentVelocity.z = ((_zIndex * stepSize) - Motor.TransientPosition.z) / deltaTime;
        }
    }
}