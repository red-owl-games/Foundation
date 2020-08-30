using UnityEngine;

namespace RedOwl.Core
{
    public class AvatarZJump : AvatarAbility
    {
        public override int Priority { get; } = 100;

        public AvatarInputButtons upButton = AvatarInputButtons.North;
        public AvatarInputButtons downButton = AvatarInputButtons.South;

        public int steps = 1;
        public float stepSize = 5f;

        public AnimTriggerProperty jumpAnimParam = "ZJump";
        public AnimIntProperty jumpIndexAnimParam = "ZJumpIndex";

        private int _zIndex;

        public override void OnStart()
        {
            jumpAnimParam.Register(Avatar.AnimManager);
            jumpIndexAnimParam.Register(Avatar.AnimManager);
        }

        public override void HandleInput(ref AvatarInput input)
        {
            if (input.Get(upButton) == ButtonStates.Pressed && _zIndex < steps)
            {
                jumpAnimParam.Set();
                _zIndex += 1;
            }

            if (input.Get(downButton) == ButtonStates.Pressed && _zIndex > -steps)
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