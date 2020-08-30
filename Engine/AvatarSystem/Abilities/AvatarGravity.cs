using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public class AvatarGravity : AvatarAbility
    {
        public override int Priority { get; } = 1000;
        
        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public float Gravity { get; set; }

        public override void OnStart()
        {
            Gravity = Physics.gravity.y;
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (!Motor.GroundingStatus.IsStableOnGround) currentVelocity.y += Gravity * deltaTime;
        }
    }
}