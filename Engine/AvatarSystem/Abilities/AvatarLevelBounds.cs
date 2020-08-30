using UnityEngine;

namespace RedOwl.Core
{
    public class AvatarLevelBounds : AvatarAbility
    {
        public override int Priority { get; } = 10000;
            
        public enum BoundsBehavior 
        {
            Nothing,
            Constrain,
            Kill
        }
        
        public BoundsBehavior Top = BoundsBehavior.Constrain;
        public BoundsBehavior Bottom = BoundsBehavior.Kill;
        public BoundsBehavior Left = BoundsBehavior.Constrain;
        public BoundsBehavior Right = BoundsBehavior.Constrain;
        public BoundsBehavior Front = BoundsBehavior.Constrain;
        public BoundsBehavior Back = BoundsBehavior.Constrain;

        private ILevelBounds _levelBounds;

        public override void OnStart()
        {
            _levelBounds = Game.Find<ILevelBounds>();
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (_levelBounds.Bounds.size == Vector3.zero) return;

            var nextPosition = Motor.TransientPosition + currentVelocity * deltaTime;
            if (ShouldApplyConstrain(Top, nextPosition.y > _levelBounds.Bounds.max.y)) currentVelocity.y = 0;
            if (ShouldApplyConstrain(Bottom, nextPosition.y < _levelBounds.Bounds.min.y)) currentVelocity.y = 0;
            if (ShouldApplyConstrain(Left, nextPosition.x < _levelBounds.Bounds.min.x)) currentVelocity.x = 0;
            if (ShouldApplyConstrain(Right, nextPosition.x > _levelBounds.Bounds.max.x)) currentVelocity.x = 0;
            if (ShouldApplyConstrain(Front, nextPosition.z < _levelBounds.Bounds.min.z)) currentVelocity.z = 0;
            if (ShouldApplyConstrain(Back, nextPosition.z > _levelBounds.Bounds.max.z)) currentVelocity.z = 0;
        }

        private bool ShouldApplyConstrain(BoundsBehavior behvaiour, bool constrain)
        {
            switch (behvaiour)
            {
                case BoundsBehavior.Constrain:
                    return constrain;
                case BoundsBehavior.Kill:
                    if (constrain) KillPlayer();
                    break;
            }
            return false;
        }

        private void KillPlayer()
        {
            // TODO: Kill Player
            //Log.Always($"Kill Player Due To Level Bounds");
        }
    }
}