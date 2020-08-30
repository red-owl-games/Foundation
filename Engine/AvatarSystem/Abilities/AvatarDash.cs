using UnityEngine;

namespace RedOwl.Core
{
    public class AvatarDash : AvatarAbility
    {
        public override int Priority { get; } = -10;

        public AvatarInputButtons button = AvatarInputButtons.ButtonNorth;
        public Cooldown cooldown;
        public Vector2 velocity = new Vector2(30, 10f);
        
        public AnimTriggerProperty dashAnimParam = "Dashed";
        
        private bool _dashRequested;
        
        public override void OnStart()
        {
            dashAnimParam.Register(Avatar.AnimManager);
        }

        public override void HandleInput(ref AvatarInput input)
        {
            if (input.Get(button) == ButtonStates.Pressed && cooldown.IsReady)
            {
                dashAnimParam.Set();
                _dashRequested = true;
            }
        }
        
        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (!Unlocked) return;
            if (!_dashRequested) return;
            Motor.ForceUnground(0.1f);
            currentVelocity.x += (Motor.CharacterForward.x <= 0 ? 1 : -1) * velocity.x;
            currentVelocity.y += velocity.y;
            _dashRequested = false;
            cooldown.Use();
        }

        public override void AfterCharacterUpdate(float deltaTime)
        {
            cooldown.Tick(deltaTime);
        }
    }
}