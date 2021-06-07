using System;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable]
    public class AvatarMovement3rdPerson : IAvatarAbility
    {
        [SerializeField] private float speed = 2.0f;
        [SerializeField] private float drag = .1f;
        [SerializeField] private float rotationSmoothSpeed = 0.1f;
        [SerializeField] private Transform head;
        
        [TitleGroup("Camera")]
        [SerializeField, HorizontalGroup("Camera/Split"), LabelText("Horizontal Power")]
        private float cameraHorizontalRotationPower = 90f;
        [SerializeField, HorizontalGroup("Camera/Split"), LabelText("Vertical Power")]
        private float cameraVerticalRotationPower = 45f;
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
        private float _angle;
        private float _velocityXSmoothing;
        private float _velocityZSmoothing;
        private float _rotationVelocity;
        private float _rotationHeadVelocity;
        
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
            if (mathExt.Magnitude(_move) < 0.0001f) return;
            var alignment = math.dot(Quaternion.Euler(0, head.rotation.eulerAngles.y, 0) * new Vector3(_move.x, 0, _move.y), head.forward);

            // Moving in the direction the camera is looking
            if (alignment > 0.7f)
            {
                _angle = math.degrees(math.atan2(_move.x, _move.y)) + head.eulerAngles.y;
                _headAngleY = Mathf.SmoothDampAngle(_headAngleY, 0, ref _rotationHeadVelocity, rotationSmoothSpeed);
                currentRotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(currentRotation.eulerAngles.y, _angle, ref _rotationVelocity, rotationSmoothSpeed), 0);
                Log.Always("Running");
            }
            // Moving towards the camera - backpeddle
            else if (alignment < -0.6f)
            {
                Log.Always("Backpeddle");
            }
            // Moving Sideways - strafe
            else
            {
                Log.Always("Strafing");
            }

            // var headAngles = head.localRotation.eulerAngles;
            // 
            // var desiredHeadAngle = Mathf.SmoothDampAngle(headAngles.y, 0, ref _rotationHeadVelocity, rotationSmoothSpeed);
            // head.localRotation = Quaternion.Euler(headAngles.x, desiredHeadAngle, headAngles.z);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            var headAngles = head.rotation.eulerAngles;
            Vector3 movement = Quaternion.Euler(0, headAngles.y, 0) * new Vector3(_move.x, 0, _move.y);
            
            currentVelocity.x = Mathf.SmoothDamp(currentVelocity.x, movement.x * speed, ref _velocityXSmoothing, drag);
            currentVelocity.z = Mathf.SmoothDamp(currentVelocity.z, movement.z * speed, ref _velocityZSmoothing, drag);
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            _headAngleX += _look.y * cameraVerticalRotationPower * deltaTime;
            _headAngleY += _look.x * cameraHorizontalRotationPower * deltaTime;

            _headAngleX = math.clamp(_headAngleX, minCameraAngle, maxCameraAngle);
            _headAngleY = mathExt.CClamp(_headAngleY);
            
            //_headAngle = math.degrees(math.atan2(_look.x, _look.y)) + head.eulerAngles.y;
            
            // Update Camera Head
            // head.rotation *= Quaternion.AngleAxis(_look.x * 90f * cameraHorizontalRotationPower * deltaTime, Vector3.up) * Quaternion.AngleAxis(_look.y * 90f * cameraVerticalRotationPower * deltaTime, Vector3.right);
            //
            // var angles = head.localRotation.eulerAngles;
            // angles.z = 0;
            //
            // //Clamp the Up/Down rotation

            //
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