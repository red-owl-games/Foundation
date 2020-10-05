using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    [RequireComponent(typeof(AvatarGravity))]
    public class AvatarTrampoline : AvatarAbility
    {
        public override int Priority { get; } = -20;

        [DisableInPlayMode] public float height = 4f;
        
        [ShowInInspector]
        private float _launchVelocity;
        private bool _launchRequested;

        public override void OnStart()
        {
            CalculateLaunchVelocity();
        }

        private void CalculateLaunchVelocity()
        {
            var currentGravity = Avatar.Abilities.Find<AvatarGravity>().Gravity;
            float duration = math.sqrt(-(2 * height) / currentGravity);
            _launchVelocity = math.abs(currentGravity) * duration;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (!Unlocked) return;
            if (_launchRequested && Motor.GroundingStatus.FoundAnyGround)
            {
                Motor.ForceUnground(0.3f);
                currentVelocity.y = _launchVelocity;
                _launchRequested = false;
            }
        }

        [Button]
        public void Launch()
        {
            _launchRequested = true;
        }
    }
}