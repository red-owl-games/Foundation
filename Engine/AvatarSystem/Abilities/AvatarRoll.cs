using UnityEngine;

namespace RedOwl.Core
{
    public class AvatarRoll : AvatarAbility
    {
        public override int Priority { get; } = 10;

        public float capsuleRadius = 0.1f;
        public float capsuleHeight = 0.5f;
        public float capsuleYOffset = 0.25f;

        public AvatarInputButtons button = AvatarInputButtons.ButtonNorth;
        public Cooldown cooldown = 2;
        public Cooldown duration = 1;
        public float distance = 5; 
        
        public AnimBoolProperty animParam = "Rolled";
        
        private float _velocity;
        private float _originalRadius;
        private float _originalHeight;
        private float _originalYOffset;
        private float _direction;
        
        public override void OnStart()
        {
            animParam.Register(Avatar.AnimManager);
            _originalRadius = Motor.Capsule.radius;
            _originalHeight = Motor.Capsule.height;
            _originalYOffset = Motor.Capsule.center.y;
            cooldown.Reset();
            duration.Reset();
            _velocity = distance / duration;
        }

        public override void HandleInput(ref AvatarInput input)
        {
            if (input.Get(button) == ButtonStates.Pressed && cooldown.IsReady)
            {
                Roll();
            }
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (!Unlocked) return;
            if (duration.IsActive) currentVelocity.x = _direction * _velocity;
        }

        public override void AfterCharacterUpdate(float deltaTime)
        {
            cooldown.Tick(deltaTime);
            duration.Tick(deltaTime);
            if (!duration.IsActive)
            {
                animParam.Off();
                Motor.SetCapsuleDimensions(_originalRadius, _originalHeight, _originalYOffset);
            }
        }
        
        private void Roll()
        {
            animParam.On();
            cooldown.Use();
            duration.Use();
            Motor.SetCapsuleDimensions(capsuleRadius, capsuleHeight, capsuleYOffset);
            _direction = (Motor.CharacterForward.x <= 0 ? 1 : -1);
        }
    }
}