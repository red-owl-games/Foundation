using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    // TODO: Needs a State Machine
    // Grounded -> JumpRequested -> Jumping -> AirJump -> Jumping
    //                                                 -> MinJump -> AirJump
    //                                      -> MinJump
    // TODO: Coyote Time is not working - the Cooldown is wonky and i'm not sure about the logic

    public interface IAvatarInputJump : IAvatarInput
    {
        ButtonStates Jump { get; }
    }

    [RequireComponent(typeof(AvatarGravity))]
    public class AvatarJump : AvatarAbility<IAvatarInputJump>
    {
        public enum States
        {
            Grounded,
            JumpRequested,
            Falling,
            MinJump,
            AirJump,
        }
        
        public override int Priority { get; } = -20;

        public Cooldown coyoteTime;
        public Counter airJumpLimit;
        
        public AnimBoolProperty jumpAnimParam = "Jumpped";
        public AnimBoolProperty airJumpAnimParam = "AirJumpped";

        [ShowInInspector]
        private States _state;

        private float _jumpForce;
        private float _maxJumpVelocity;
        private float _minJumpVelocity;
        
        public override void RegisterAnimatorParams(AnimatorController controller)
        {
            jumpAnimParam.Register(controller);
            airJumpAnimParam.Register(controller);
        }

        public override void OnStart()
        {
            _state = States.Grounded;
            _maxJumpVelocity = GameSettings.AvatarSettings.MaxJumpVelocity;
            _minJumpVelocity = GameSettings.AvatarSettings.MinJumpVelocity;
            airJumpLimit.Reset();
        }

        protected override void ProcessInput(ref IAvatarInputJump input)
        {
            if (input.Jump == ButtonStates.Cancelled)
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
            if (input.Jump == ButtonStates.Pressed)
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

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (!Unlocked) return;
            if (_state == States.MinJump)
            {
                if (currentVelocity.y > _minJumpVelocity) currentVelocity.y = _minJumpVelocity;
                _state = States.Falling;
            }
            
            if (_state == States.JumpRequested)
            {
                Motor.ForceUnground(0.1f);
                currentVelocity.y = _jumpForce;
                _state = States.Falling;
            }

            if (_state == States.AirJump)
            {
                currentVelocity.y = _jumpForce;
                _state = States.Falling;
            }
        }

        public override void AfterCharacterUpdate(float deltaTime)
        {
            if (Motor.GroundingStatus.IsStableOnGround && !Motor.MustUnground())
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
    }
}