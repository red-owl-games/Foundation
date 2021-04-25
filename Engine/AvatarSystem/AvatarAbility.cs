using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface IAvatarAbility : ICharacterController
    {
        string Name { get; }
        bool Enabled { get; }
        int Priority { get; }
        bool Unlocked { get; }
        void OnStart();
        void OnReset();
        void RegisterAnimatorParams(AnimatorController controller);
        void ProcessInput(ref IAvatarInput input);
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

        /// <summary>
        /// Positive Numbers Happen Last in Loops
        /// </summary>
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

        public virtual void RegisterAnimatorParams(AnimatorController controller) {}
        public virtual void OnStart() {}
        public virtual void OnReset() {}
        public virtual void ProcessInput(ref IAvatarInput input) {}

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

    public abstract class AvatarAbility<TInput> : AvatarAbility where TInput : IAvatarInput
    {
        public override void ProcessInput(ref IAvatarInput input)
        {
            if (input is TInput casted) ProcessInput(ref casted);
        }

        protected abstract void ProcessInput(ref TInput input);
    }
}