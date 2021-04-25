using UnityEngine;

namespace RedOwl.Engine
{
    public interface IAvatarDashInput : IAvatarInput
    {
        ButtonStates InputDash { get; }
    }
    
    public class AvatarDash : AvatarAbility<IAvatarDashInput>
    {
        public override int Priority { get; } = -10;

        public Cooldown cooldown;
        public Vector2 velocity = new Vector2(30, 10f);
        
        public AnimTriggerProperty dashAnimParam = "Dashed";
        
        private bool _dashRequested;

        public override void RegisterAnimatorParams(AnimatorController controller)
        {
            dashAnimParam.Register(controller);
        }

        protected override void ProcessInput(ref IAvatarDashInput input)
        {
            if (input.InputDash == ButtonStates.Pressed && cooldown.IsReady)
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