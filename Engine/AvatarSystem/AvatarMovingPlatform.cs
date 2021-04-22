using KinematicCharacterController;
using UnityEngine;

namespace RedOwl.Engine
{
    public class AvatarMovingPlatform : MonoBehaviour, IMoverController
    {
        public Vector3 Position;
        public Vector3 Rotation;
        
        public PhysicsMover Mover;
        
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
            Position = _transform.position;
            Rotation = _transform.rotation.eulerAngles;
            Mover.MoverController = this;
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            goalPosition = Position;
            goalRotation = Quaternion.Euler(Rotation);
        }
    }
}