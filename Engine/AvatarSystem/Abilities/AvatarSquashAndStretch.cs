using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    public class AvatarSquashAndStretch : AvatarAbility
    {
        public override int Priority { get; } = 100000000;

        // TODO: Would be nice to control SquashFact via an Animation curve
        public Vector2 VelocityThreshold = new Vector2(10f, 10f);
        public float SquashFactor = 0.2f;
        public Transform container;
        
        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            // vx == 10f  then amount = 1
            // vy = 10f then amount = 1 scale = 0.8, 1.2, 1
            // vy = 0f then scale = 1, 1, 1
            // vy = -10f then scale = 1.2f, 0.8f, 1

            var factorX = math.abs(mathExt.Remap(currentVelocity.x, -VelocityThreshold.x, VelocityThreshold.x, -1, 1));
            var factorY = mathExt.Remap(currentVelocity.y, -VelocityThreshold.y, VelocityThreshold.y, -1, 1);
            
            container.localScale = new Vector3(
                1 + (factorY * -SquashFactor) + (factorX * SquashFactor),
                1 + (factorY * SquashFactor) + (factorX * -SquashFactor),
                1 + (factorY * -SquashFactor) + + (factorX * SquashFactor));
        }
    }
}