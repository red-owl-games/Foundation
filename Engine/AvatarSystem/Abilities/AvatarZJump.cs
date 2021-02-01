using UnityEngine;

namespace RedOwl.Engine
{
    public interface IAvatarInputZJump : IAvatarInput
    {
        ButtonStates ZJumpUp { get; }
        ButtonStates ZJumpDown { get; }
    }
    
    public class AvatarZJump : AvatarAbility<IAvatarInputZJump>
    {
        public override int Priority { get; } = 100;

        public int steps = 1;
        public float stepSize = 5f;

        public AnimTriggerProperty jumpAnimParam = "ZJump";
        public AnimIntProperty jumpIndexAnimParam = "ZJumpIndex";

        private int _zIndex;

        public override void OnStart()
        {
            jumpAnimParam.Register(Avatar.AnimController);
            jumpIndexAnimParam.Register(Avatar.AnimController);
        }

        protected override void HandleInput(ref IAvatarInputZJump input)
        {
            if (input.ZJumpUp == ButtonStates.Pressed && _zIndex < steps)
            {
                jumpAnimParam.Set();
                _zIndex += 1;
            }

            if (input.ZJumpDown == ButtonStates.Pressed && _zIndex > -steps)
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