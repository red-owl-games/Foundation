using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    [RequireComponent(typeof(AvatarGravity))]
    public class AvatarJump : AvatarAbility
    {
        public override int Priority { get; } = -20;

        public Cooldown coyoteTime;
        public Counter airJumpLimit;
        public AvatarInputButtons button = AvatarInputButtons.ButtonSouth;
        
        public AnimBoolProperty jumpAnimParam = "Jumpped";
        public AnimBoolProperty airJumpAnimParam = "AirJumpped";

        private ButtonStates _button;
        private float _maxJumpVelocity;
        //private float _minJumpVelocity;
        private bool _jumpRequested;
        private bool _isFalling;

        private AnimatorManager _manager;

        public override void OnStart()
        {
            _maxJumpVelocity = AvatarSettings.Instance.MaxJumpVelocity;
            jumpAnimParam.Register(Avatar.AnimManager);
            airJumpAnimParam.Register(Avatar.AnimManager);
            airJumpLimit.Reset();
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
            // TODO: this does not work its always evaluating - we need "if was cancelled this frame" kind of thing
            // if (_button == ButtonStates.Cancelled)
            // {
            //     if (currentVelocity.y > _minJumpVelocity) currentVelocity.y = _minJumpVelocity;
            // }
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

        [Button]
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