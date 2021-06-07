using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public class AvatarGravity : IAvatarAbility
    {
        public int Priority { get; } = 1000;
        [ShowInInspector, HorizontalGroup("Options"), ToggleLeft]
        public bool Enabled { get; set; } = true;
        [ShowInInspector, HorizontalGroup("Options"), ToggleLeft]
        public bool Unlocked { get; set; } = true;

        private KinematicCharacterMotor _motor;
        private float _gravity;
        private float _gravityFalling;
        
        public void OnStart(KinematicCharacterMotor motor)
        {
            _motor = motor;
            GameSettings.AvatarSettings.Initialize();
            _gravity = GameSettings.AvatarSettings.Gravity;
            _gravityFalling = GameSettings.AvatarSettings.GravityFalling;
        }

        public void OnUpdate(IInputState input)
        {
            
        }
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (_motor.GroundingStatus.IsStableOnGround) return;
            currentVelocity.y += deltaTime * (currentVelocity.y > 0 ? _gravity : _gravityFalling);
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            
        }
    }
}