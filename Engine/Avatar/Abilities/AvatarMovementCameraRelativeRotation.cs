using System;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable]
    public class AvatarMovementCameraRelativeRotation : IAvatarAbility
    {
        // TODO: should probably pickup a "PlayerCamera"
        public Transform targetCamera;
        
        public float rotationPowerHorizontal = 50f;
        public float rotationPowerVertical = 30f;
        
        
        public int Priority { get; } = 0;
        [ShowInInspector, HorizontalGroup("Options"), ToggleLeft, PropertyOrder(-100)]
        public bool Enabled { get; set; } = true;
        [ShowInInspector, HorizontalGroup("Options"), ToggleLeft, PropertyOrder(-100)]
        public bool Unlocked { get; set; } = true;
        
        private KinematicCharacterMotor _motor;
        private Transform _transform;
        private Quaternion _rotationOffset;
        
        private float2 _move;
        private float2 _look;
        
        public void OnStart(KinematicCharacterMotor motor)
        {
            _motor = motor;
            _transform = _motor.transform;
            _rotationOffset = _transform.localRotation;
        }

        public void OnUpdate(IInputState input)
        {
            _move = math.normalizesafe(input.JoystickLeft);
            _look = math.normalizesafe(input.JoystickRight);
        }
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            float deltaX = -_move.x * rotationPowerHorizontal;
            float deltaY = _move.y * rotationPowerVertical;
            float deltaZ = _look.x * rotationPowerHorizontal;
            float deltaW = _look.y * rotationPowerVertical;

            Vector3 localScale = _transform.localScale;
            Quaternion target = (Quaternion.AngleAxis(deltaX / localScale.x, Vector3.up) * Quaternion.AngleAxis((deltaY + deltaW) / localScale.y, Vector3.right) * Quaternion.AngleAxis(deltaZ / localScale.z, Vector3.forward)) * currentRotation;
            
            currentRotation = Quaternion.Slerp(currentRotation, target, deltaTime);
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