using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    public class Avatar2DPlatformerMovement : AvatarAbility
    {
        public override int Priority { get; } = 0;

        public float Speed = 5f;
        public float Drag = .1f;
        public Vector3 forwardRotation = new Vector3(0, -90f, 0);
        public Vector3 backwardRotation = new Vector3(0, 90f, 0);
        
        private float movement;
        private float velocityXSmoothing;

        public override void HandleInput(ref AvatarInput input)
        {
            movement = input.Move.x;
        }
        
        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (movement > 0)
            {
                currentRotation = Quaternion.Slerp(currentRotation, Quaternion.Euler(forwardRotation), 1 - math.exp(-10 * deltaTime));
            } else if (movement < 0)
            {
                currentRotation = Quaternion.Slerp(currentRotation, Quaternion.Euler(backwardRotation), 1 - math.exp(-10 * deltaTime));
            }
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            currentVelocity.x = Mathf.SmoothDamp(currentVelocity.x, movement * Speed, ref velocityXSmoothing, Drag);
        }
    }
}