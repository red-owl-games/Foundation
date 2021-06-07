using System;
using System.Collections.Generic;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface IAvatarAbility : ICharacterController
    {
        /// <summary>
        /// Positive Numbers Happen Last in Loops
        /// </summary>
        int Priority { get; }
        bool Enabled { get; }
        bool Unlocked { get; }
        void OnStart(KinematicCharacterMotor motor);
        void OnUpdate(IInputState input);
        
        // Unneeded?
        //string Name { get; }
        //void ProcessInput(ref IAvatarInput input);
    }

    public interface IAvatarAnimator
    {
        // Called before OnStart
        void RegisterAnimatorParams(AnimatorController controller);
    }

    [HideMonoScript]
    public class Avatar : MonoBehaviour, ICharacterController
    {
        [SerializeField] 
        private KinematicCharacterMotor motor;
        
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private InputReference input;
        
        [SerializeReference]
        public List<IAvatarAbility> abilities;

        private bool _hasAnimator;
        private AnimatorController _animController;
        
        public IEnumerable<IAvatarAbility> AbilitiesEnabled
        {
            get
            {
                foreach (var item in abilities)
                {
                    if (item.Enabled) yield return item;
                }
            }
        }

        public IEnumerable<IAvatarAbility> AbilitiesUnlocked
        {
            get
            {
                foreach (var item in abilities)
                {
                    if (item.Enabled && item.Unlocked) yield return item;
                }
            }
        }

        #region Unity

        private void OnValidate()
        {
            if (motor == null) motor = GetComponent<KinematicCharacterMotor>();
            if (animator == null) animator = GetComponent<Animator>();
            if (abilities == null) abilities = new List<IAvatarAbility>();
            SortAbilities();
            //this.Requires(motor);

        }

        private void Awake()
        {
            _hasAnimator = animator != null;
            motor.CharacterController = this;
        }

        private void OnEnable()
        {
            SortAbilities();
            if (!_hasAnimator) return;
            _animController = new AnimatorController(animator);
            foreach (var ability in abilities)
            {
                if (ability is IAvatarAnimator casted) casted.RegisterAnimatorParams(_animController);
            }

        }

        private void Start()
        {
            // TODO: why do i start in reverse loop order?
            for (int i = abilities.Count - 1; i >= 0; i--)
            {
                abilities[i].OnStart(motor);
            }
        }

        private void Update()
        {
            var state = input.State;
            if (state == null) return;
            foreach (var ability in abilities)
            {
                ability.OnUpdate(state);
            }
        }

        #endregion
        
        public void SortAbilities()
        {
            abilities.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }
        
        #region ICharacterController

        public void BeforeCharacterUpdate(float deltaTime)
        {
            foreach (var ability in AbilitiesEnabled) ability.BeforeCharacterUpdate(deltaTime);
        }
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            foreach (var ability in AbilitiesUnlocked) ability.UpdateRotation(ref currentRotation, deltaTime);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            foreach (var ability in AbilitiesUnlocked) ability.UpdateVelocity(ref currentVelocity, deltaTime);
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            foreach (var ability in AbilitiesUnlocked) ability.PostGroundingUpdate(deltaTime);
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            foreach (var ability in AbilitiesEnabled) ability.AfterCharacterUpdate(deltaTime);
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            // TODO: All or Any ?
            foreach (var ability in AbilitiesUnlocked)
            {
                if (!ability.IsColliderValidForCollisions(coll)) 
                    return false;
            }
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            foreach (var ability in AbilitiesEnabled) ability.OnGroundHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            foreach (var ability in AbilitiesEnabled) ability.OnMovementHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            foreach (var ability in AbilitiesUnlocked) ability.ProcessHitStabilityReport(hitCollider, hitNormal, hitPoint, atCharacterPosition, atCharacterRotation, ref hitStabilityReport);
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            foreach (var ability in AbilitiesUnlocked) ability.OnDiscreteCollisionDetected(hitCollider);
        }
        
        #endregion
    }
}