using System;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable]
    public class AvatarMovementParentConstraint : IAvatarAbility
    {
        public Rigidbody parent;

        public int Priority { get; } = int.MaxValue - 1;

        [ShowInInspector, HorizontalGroup("Options"), ToggleLeft, PropertyOrder(-100)]
        public bool Enabled { get; set; } = true;

        [ShowInInspector, HorizontalGroup("Options"), ToggleLeft, PropertyOrder(-100)]
        public bool Unlocked { get; set; } = true;

        private KinematicCharacterMotor _motor;

        public void OnStart(KinematicCharacterMotor motor)
        {
            _motor = motor;
            _motor.AttachedRigidbodyOverride = parent;
        }

        public void OnUpdate(IInputState input)
        {
            
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            currentRotation = parent.rotation;
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {

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

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {

        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {

        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {

        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {

        }
    }
}