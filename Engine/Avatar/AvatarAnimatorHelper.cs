using System;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable]
    // Set Animator Parameters based data about the avatar
    public class AvatarAnimatorHelper : IAvatarAbility, IAvatarAnimator
    {
        [SerializeField]
        private AnimFloatProperty InputLookXAnimParam = "InputLookX";
        [SerializeField]
        private AnimFloatProperty InputLookYAnimParam = "InputLookY";
        [SerializeField]
        private AnimFloatProperty InputMoveXAnimParam = "InputMoveX";
        [SerializeField]
        private AnimFloatProperty InputMoveYAnimParam = "InputMoveY";
        [SerializeField]
        private AnimFloatProperty VelocityXAnimParam = "VelocityX";
        [SerializeField]
        private AnimFloatProperty VelocityYAnimParam = "VelocityY";
        [SerializeField]
        private AnimFloatProperty VelocityZAnimParam = "VelocityZ";
        [SerializeField]
        private AnimFloatProperty RotationXAnimParam = "RotationX";
        [SerializeField]
        private AnimFloatProperty RotationYAnimParam = "RotationY";
        [SerializeField]
        private AnimFloatProperty RotationZAnimParam = "RotationZ";
        [SerializeField]
        private AnimBoolProperty LandedAnimParam = "Landed";
        [SerializeField]
        private AnimBoolProperty GroundedAnimParam = "Grounded";

        public int Priority { get; } = int.MaxValue - 1;
        [ShowInInspector, HorizontalGroup("Options"), ToggleLeft]
        public bool Enabled { get; set; } = true;
        [ShowInInspector, HorizontalGroup("Options"), ToggleLeft]
        public bool Unlocked { get; set; } = true;

        private KinematicCharacterMotor _motor;
        private bool _wasGroundedLastFrame;
        
        public void RegisterAnimatorParams(AnimatorController controller)
        {
            InputLookXAnimParam.Register(controller);
            InputLookYAnimParam.Register(controller);
            InputMoveXAnimParam.Register(controller);
            InputMoveYAnimParam.Register(controller);
            
            VelocityXAnimParam.Register(controller);
            VelocityYAnimParam.Register(controller);
            VelocityZAnimParam.Register(controller);
            RotationXAnimParam.Register(controller);
            RotationYAnimParam.Register(controller);
            RotationZAnimParam.Register(controller);
            LandedAnimParam.Register(controller);
            GroundedAnimParam.Register(controller);
        }
        
        public void OnStart(KinematicCharacterMotor motor)
        {
            _motor = motor;
        }

        public void OnUpdate(IInputState input)
        {
            InputLookXAnimParam.Set(input.JoystickRight.x);
            InputLookYAnimParam.Set(input.JoystickRight.y);
            InputMoveXAnimParam.Set(input.JoystickLeft.x);
            InputMoveYAnimParam.Set(input.JoystickLeft.y);
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            var angles = currentRotation.eulerAngles;
            RotationXAnimParam.Set(angles.x);
            RotationYAnimParam.Set(angles.y);
            RotationZAnimParam.Set(angles.z);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            VelocityXAnimParam.Set(currentVelocity.x);
            VelocityYAnimParam.Set(currentVelocity.y);
            VelocityZAnimParam.Set(currentVelocity.z);
        }

        public void BeforeCharacterUpdate(float deltaTime) {}

        public void PostGroundingUpdate(float deltaTime) {}

        public void AfterCharacterUpdate(float deltaTime)
        {
            bool isGrounded = _motor.GroundingStatus.IsStableOnGround;
            if (!_wasGroundedLastFrame && isGrounded) LandedAnimParam.Trigger();
            _wasGroundedLastFrame = isGrounded;
            GroundedAnimParam.Set(isGrounded);
        }

        public bool IsColliderValidForCollisions(Collider coll) => true;

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {}

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {}

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) {}

        public void OnDiscreteCollisionDetected(Collider hitCollider) {}
    }
}