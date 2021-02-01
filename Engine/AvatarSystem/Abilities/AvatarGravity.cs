using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public class AvatarGravity : AvatarAbility
    {
        public override int Priority { get; } = 1000;

        private float _gravity;
        private float _gravityFactor;

        public override void OnStart()
        {
            _gravity = Game.AvatarSettings.Gravity;
            _gravityFactor = Game.AvatarSettings.GravityFallFactor;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (Motor.GroundingStatus.IsStableOnGround) return;
            currentVelocity.y += currentVelocity.y > 0 ? _gravity * deltaTime : _gravity * _gravityFactor * deltaTime;
        }
    }
}