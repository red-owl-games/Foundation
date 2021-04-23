using UnityEngine;

namespace RedOwl.Engine
{
    public class AvatarGravityTowards : AvatarAbility
    {
        public override int Priority { get; } = 1000;

        public Transform target;

        private float _gravity;
        private float _gravityFactor;

        public override void OnStart()
        {
            _gravity = GameSettings.AvatarSettings.Gravity;
            _gravityFactor = GameSettings.AvatarSettings.GravityFallFactor;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (Motor.GroundingStatus.IsStableOnGround) return;
            var delta = target.position - transform.position;
            currentVelocity = delta / deltaTime;
        }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            currentRotation = target.rotation;
            //var delta = Vector3.Scale(target.position - transform.position, -Vector3.one);
            //currentRotation = Quaternion.FromToRotation((currentRotation * Vector3.up), delta) * currentRotation;
        }
    }
}