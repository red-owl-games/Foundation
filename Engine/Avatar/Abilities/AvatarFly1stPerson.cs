using System;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable]
    public class AvatarFly1stPerson : IAvatarAbility
    {
        [SerializeField] private float speed = 2.0f;
        [SerializeField] private float drag = .1f;
        [SerializeField] private Transform head;

        [TitleGroup("Camera")]
        [SerializeField, HorizontalGroup("Camera/Split"), LabelText("Horizontal Power")]
        private float horizontalRotationPower = 90f;
        [SerializeField, HorizontalGroup("Camera/Split"), LabelText("Vertical Power")]
        private float verticalRotationPower = 45f;
        [SerializeField] private float minCameraAngle = -50;
        [SerializeField] private float maxCameraAngle = 50;
        
        public int Priority { get; } = 0;
        [ShowInInspector, HorizontalGroup("Options"), ToggleLeft, PropertyOrder(-100)]
        public bool Enabled { get; set; } = true;
        [ShowInInspector, HorizontalGroup("Options"), ToggleLeft, PropertyOrder(-100)]
        public bool Unlocked { get; set; } = true;
        
        private KinematicCharacterMotor _motor;

        private float2 _move;
        private float2 _look;
        private float _headAngleX;
        private float _headAngleY;
        private float _moveVelocityX;
        private float _moveVelocityY;
        private float _moveVelocityZ;
        
        public void OnStart(KinematicCharacterMotor motor)
        {
            _motor = motor;
        }

        public void OnUpdate(IInputState input)
        {
            _move = input.JoystickLeft;
            _look = input.JoystickRight;
        }
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {

        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (_motor.GroundingStatus.IsStableOnGround) _motor.ForceUnground();
            var inputVelocity = (head.forward * _move.y + head.right * _move.x) * speed;
            currentVelocity.x = Mathf.SmoothDamp(currentVelocity.x, inputVelocity.x, ref _moveVelocityX, drag);
            currentVelocity.y = Mathf.SmoothDamp(currentVelocity.y, inputVelocity.y, ref _moveVelocityY, drag);
            currentVelocity.z = Mathf.SmoothDamp(currentVelocity.z, inputVelocity.z, ref _moveVelocityZ, drag);
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            _headAngleX += _look.y * verticalRotationPower * deltaTime;
            _headAngleY += _look.x * horizontalRotationPower * deltaTime;
            
            _headAngleX = math.clamp(_headAngleX, minCameraAngle, maxCameraAngle);
            _headAngleY = mathExt.CClamp(_headAngleY);
            
            head.localRotation = Quaternion.Euler(_headAngleX, _headAngleY, 0);
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