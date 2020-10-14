using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public class AvatarGravity : AvatarAbility
    {
        public override int Priority { get; } = 1000;

        private float _gravity;

        public override void OnStart()
        {
            _gravity = AvatarSettings.Instance.Gravity;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (!Motor.GroundingStatus.IsStableOnGround) currentVelocity.y += _gravity * deltaTime;
        }
    }
}