using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public interface IAvatarAbility : ICharacterController
    {
        string Name { get; }
        bool Enabled { get; }
        int Priority { get; }
        bool Unlocked { get; }
        void OnStart();
        void OnCleanup();
        void HandleInput(ref AvatarInput input);
    }
    
    [HideMonoScript]
    [RequireComponent(typeof(Avatar))]
    public abstract class AvatarAbility : MonoBehaviour, IAvatarAbility
    {
        [field: LabelText("Unlocked")]
        [field: SerializeField]
        public bool Unlocked { get; set; } = true;

        public string Name => GetType().Name;
        
        protected Avatar Avatar;
        protected KinematicCharacterMotor Motor => Avatar.Motor;
        protected Animator Animator => Avatar.animator;

        public bool Enabled => enabled;

        // Higher Numbers Happen Last in Loops
        public abstract int Priority { get; }
        
        private void Awake()
        {
            Avatar = GetComponent<Avatar>();
            Avatar.Add(this);
        }

        private void OnDestroy()
        {
            Avatar.Remove(this);
        }

        public virtual void OnStart() {}
        public virtual void OnCleanup() {}

        public virtual void HandleInput(ref AvatarInput input) {}

        public virtual void BeforeCharacterUpdate(float deltaTime) {}

        public virtual void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {}

        public virtual void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {}

        public virtual void PostGroundingUpdate(float deltaTime) {}

        public virtual void AfterCharacterUpdate(float deltaTime) {}

        public virtual bool IsColliderValidForCollisions(Collider coll) {return true;}

        public virtual void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {}

        public virtual void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {}

        public virtual void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) {}
        
        public virtual void OnDiscreteCollisionDetected(Collider hitCollider) {}
    }
}