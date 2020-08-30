using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    [RequireComponent(typeof(AvatarGravity))]
    public class AvatarJump : AvatarAbility
    {
        public override int Priority { get; } = -20;
        
        [DisableInPlayMode]
        public float maxJumpHeight = 4f;
        [DisableInPlayMode]
        public float minJumpHeight = 2f;
        [DisableInPlayMode]
        public float jumpTime = .4f;

        public Cooldown coyoteTime;
        public Counter airJumpLimit;
        public AvatarInputButtons button = AvatarInputButtons.ButtonSouth;
        
        public AnimBoolProperty jumpAnimParam = "Jumpped";
        public AnimBoolProperty airJumpAnimParam = "AirJumpped";

        private ButtonStates _button;
        private float _gravity;
        private float _maxJumpVelocity;
        private float _minJumpVelocity;
        private bool _jumpRequested;
        private bool _isFalling;

        private AnimatorManager _manager;

        public override void OnStart()
        {
            CalculateJumpVelocity();
            Avatar.Abilities.Find<AvatarGravity>().Gravity = _gravity;
            jumpAnimParam.Register(Avatar.AnimManager);
            airJumpAnimParam.Register(Avatar.AnimManager);
            airJumpLimit.Reset();
        }

        private void CalculateJumpVelocity()
        {
            _gravity = -(2 * maxJumpHeight) / math.pow(jumpTime, 2);
            _maxJumpVelocity = math.abs(_gravity) * jumpTime;
            _minJumpVelocity = math.sqrt(2 * math.abs(_gravity) * minJumpHeight);
        }

        public override void HandleInput(ref AvatarInput input)
        {
            _button = input.Get(button);
            if (_button == ButtonStates.Pressed)
            {
                if (Motor.GroundingStatus.FoundAnyGround || !coyoteTime.IsReady)
                {
                    coyoteTime.Reset();
                    Jump();
                }
                else
                {
                    if (airJumpLimit.IsReady)
                    {
                        AirJump();
                    }
                }
            }
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (!Unlocked) return;
            if (_button == ButtonStates.Cancelled)
            {
                if (currentVelocity.y > _minJumpVelocity) currentVelocity.y = _minJumpVelocity;
            }
            if (_jumpRequested)
            {
                Motor.ForceUnground(0.1f);
                currentVelocity.y = _maxJumpVelocity;
                _jumpRequested = false;
            }
        }

        public override void AfterCharacterUpdate(float deltaTime)
        {
            if (Motor.GroundingStatus.IsStableOnGround)
            {
                if (_isFalling) coyoteTime.Use();
                _isFalling = false;
                airJumpAnimParam.Off();
                airJumpLimit.Reset();
            }
            else
            {
                _isFalling = true;
                coyoteTime.Tick(deltaTime);
            }
        }

        private void Jump()
        {
            jumpAnimParam.Trigger();
            _jumpRequested = true;
        }

        private void AirJump()
        {
            airJumpAnimParam.Trigger();
            airJumpLimit.Use();
            _jumpRequested = true;
        }
    }
}