using System;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    // TODO: Needs a State Machine
    // Grounded -> JumpRequested -> Jumping -> AirJump -> Jumping
    //                                                 -> MinJump -> AirJump
    //                                      -> MinJump
    // TODO: Coyote Time is not working - the Cooldown is wonky and i'm not sure about the logic
    
    [Serializable]
    public class AvatarJump : IAvatarAbility, IAvatarAnimator
    {
        public enum States
        {
            Grounded,
            JumpRequested,
            Falling,
            MinJump,
            AirJump,
        }
        
        [SerializeField]
        private AnimBoolProperty jumpAnimParam = "Jumpped";
        [SerializeField]
        private AnimBoolProperty airJumpAnimParam = "AirJumpped";
        
        [SerializeField]
        private Cooldown coyoteTime = 0.05f;
        [SerializeField]
        private Counter airJumpLimit = 0;
        
        [ShowInInspector]
        private States _state;

        private float _jumpForce;
        private float _maxJumpVelocity;
        private float _minJumpVelocity;

        public int Priority { get; } = -20;
        [ShowInInspector, HorizontalGroup("Options"), ToggleLeft, PropertyOrder(-100)]
        public bool Enabled { get; set; } = true;
        [ShowInInspector, HorizontalGroup("Options"), ToggleLeft, PropertyOrder(-100)]
        public bool Unlocked { get; set; } = true;

        private KinematicCharacterMotor _motor;
        
        [Button]
        private void Jump()
        {
            jumpAnimParam.Trigger();
            _jumpForce = _maxJumpVelocity;
            _state = States.JumpRequested;
        }

        private void AirJump()
        {
            airJumpAnimParam.Trigger();
            airJumpLimit.Use();
            _jumpForce = _maxJumpVelocity;
            _state = States.AirJump;
        }

        public void RegisterAnimatorParams(AnimatorController controller)
        {
            jumpAnimParam.Register(controller);
            airJumpAnimParam.Register(controller);
        }

        public void OnStart(KinematicCharacterMotor motor)
        {
            _motor = motor;
            _state = States.Grounded;
            _maxJumpVelocity = GameSettings.AvatarSettings.MaxJumpVelocity;
            _minJumpVelocity = GameSettings.AvatarSettings.MinJumpVelocity;
            airJumpLimit.Reset();
        }

        public void OnUpdate(IInputState input)
        {
            if (!input.ButtonSouth)
            {
                switch (_state)
                {
                    case States.JumpRequested:
                        _jumpForce = _minJumpVelocity;
                        break;
                    case States.Falling:
                        _state = States.MinJump;
                        break;
                }
            }
            if (input.ButtonSouth)
            {
                if (_state == States.Grounded)
                {
                    // if (!coyoteTime.IsReady)
                    // {
                    //     coyoteTime.Reset();
                    //     Jump();
                    // }
                    Jump();
                }
                else
                {
                    if ((_state == States.Falling || _state == States.MinJump) && airJumpLimit.IsReady)
                    {
                        AirJump();
                    }
                }
            }
        }
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (_state == States.MinJump)
            {
                if (currentVelocity.y > _minJumpVelocity) currentVelocity.y = _minJumpVelocity;
                _state = States.Falling;
            }
            
            if (_state == States.JumpRequested)
            {
                _motor.ForceUnground(0.1f);
                currentVelocity.y = _jumpForce;
                _state = States.Falling;
            }

            if (_state == States.AirJump)
            {
                currentVelocity.y = _jumpForce;
                _state = States.Falling;
            }
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            if (_motor.GroundingStatus.IsStableOnGround && !_motor.MustUnground())
            {
                coyoteTime.Use();
                _state = States.Grounded;
                airJumpAnimParam.Off();
                airJumpLimit.Reset();
            }
            else
            {
                _state = States.Falling;
                coyoteTime.Tick(deltaTime);
            }
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