using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface IAvatarInput3DTopdown : IAvatarInput
    {
        Vector2 Move { get; }
        ButtonStates Run { get; }
    }
    
    public class AvatarMovement3DTopDown : AvatarAbility<IAvatarInput3DTopdown>
    {
        public override int Priority { get; } = 0;

        public float Speed = 5f;
        public float RunSpeedFactor = 2f;
        public float rotationSpeed = 5f;
        public float Drag = .1f;

        private float runSpeed;
        private Vector2 movement;
        private bool running;
        private float velocityXSmoothing;
        private float velocityZSmoothing;

        public override void OnStart()
        {
            base.OnStart();
            runSpeed = Speed * RunSpeedFactor;
        }

        protected override void HandleInput(ref IAvatarInput3DTopdown input)
        {
            movement = input.Move;
            running = input.Run != ButtonStates.Cancelled;
        }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (movement.magnitude > 0.001f)
                currentRotation = Quaternion.Slerp(currentRotation, Quaternion.LookRotation(new Vector3(-movement.x, 0f, -movement.y)), 1 - math.exp(-rotationSpeed * deltaTime));
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            float speed = running ? runSpeed : Speed;
            currentVelocity.x = Mathf.SmoothDamp(currentVelocity.x, movement.x * speed, ref velocityXSmoothing, Drag);
            currentVelocity.z = Mathf.SmoothDamp(currentVelocity.z, movement.y * speed, ref velocityZSmoothing, Drag);
        }
    }
}