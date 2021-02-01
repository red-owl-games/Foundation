using UnityEngine;

namespace RedOwl.Engine
{
    public interface IAvatarDashInput : IAvatarInput
    {
        ButtonStates Dash { get; }
    }
    
    public class AvatarDash : AvatarAbility<IAvatarDashInput>
    {
        public override int Priority { get; } = -10;

        public Cooldown cooldown;
        public Vector2 velocity = new Vector2(30, 10f);
        
        public AnimTriggerProperty dashAnimParam = "Dashed";
        
        private bool _dashRequested;
        
        public override void OnStart()
        {
            dashAnimParam.Register(Avatar.AnimController);
        }

        protected override void HandleInput(ref IAvatarDashInput input)
        {
            if (input.Dash == ButtonStates.Pressed && cooldown.IsReady)
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