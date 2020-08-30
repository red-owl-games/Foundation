using System.Collections.Generic;
using System.Linq;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    // TODO: Remove Linq?
    
    public class AbilityCache : ServiceCache
    {
        // TODO: ServiceCache should have maintain a list which can be sorted
        public List<IAvatarAbility> All = new List<IAvatarAbility>();
        
        public IEnumerable<IAvatarAbility> Enabled => All.Where(a => a.Enabled);
        public IEnumerable<IAvatarAbility> Unlocked => Enabled.Where(a => a.Unlocked);
        
        public void Add<T>(T ability) where T : IAvatarAbility
        {
            Bind(ability);
            Sort();
        }

        public void Remove<T>(T ability) where T : IAvatarAbility
        {
            Unbind(ability);
            Sort();
        }

        private void Sort()
        {
            All = new List<IAvatarAbility>(_cache.Values.Cast<IAvatarAbility>());
            All.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }
    }
    
    // The purpose of the Avatar is to Tie together the KinematicCharacterMotor with Abilities that provides the actual functionality
    [HideMonoScript]
    public class Avatar : MonoBehaviour, ICharacterController
    {
        [SerializeField]
        internal Animator animator;

        public AnimFloatProperty VelocityXAnimParam = "VelocityX";
        public AnimFloatProperty VelocityYAnimParam = "VelocityY";
        public AnimTriggerProperty LandedAnimParam = "Landed";
        public AnimBoolProperty GroundedAnimParam = "Grounded";

        public bool IsInitialized { get; private set; }
        public AbilityCache Abilities { get; } = new AbilityCache();
        public AnimatorManager AnimManager { get; private set; }
        public KinematicCharacterMotor Motor { get; private set; }

        private bool _wasGroundedLastFrame;

        private void Awake()
        {
            Motor = GetComponent<KinematicCharacterMotor>();
            if (animator == null) animator = this.EnsureComponent<Animator>();
            AnimManager = new AnimatorManager(animator);
            VelocityXAnimParam.Register(AnimManager);
            VelocityYAnimParam.Register(AnimManager);
            LandedAnimParam.Register(AnimManager);
            GroundedAnimParam.Register(AnimManager);
        }

        private void Start()
        {
            Motor.CharacterController = this;
            foreach (var ability in Abilities.All.ToArray().Reverse())
            {
                ability.OnStart();
            }

            IsInitialized = true;
        }

        private void OnDestroy()
        {
            foreach (var ability in Abilities.All.ToArray())
            {
                ability.OnCleanup();
            }
        }

        public void HandleInput(ref AvatarInput input)
        {
            foreach (var ability in Abilities.Unlocked) ability.HandleInput(ref input);
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            foreach (var ability in Abilities.Enabled) ability.BeforeCharacterUpdate(deltaTime);
        }
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            foreach (var ability in Abilities.Unlocked) ability.UpdateRotation(ref currentRotation, deltaTime);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            foreach (var ability in Abilities.Unlocked) ability.UpdateVelocity(ref currentVelocity, deltaTime);
            VelocityXAnimParam.Set(currentVelocity.x);
            VelocityYAnimParam.Set(currentVelocity.y);
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            foreach (var ability in Abilities.Unlocked) ability.PostGroundingUpdate(deltaTime);
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            foreach (var ability in Abilities.Enabled) ability.AfterCharacterUpdate(deltaTime);
            bool isGrounded = Motor.GroundingStatus.IsStableOnGround;
            if (!_wasGroundedLastFrame && isGrounded) LandedAnimParam.Set();
            _wasGroundedLastFrame = isGrounded;
            GroundedAnimParam.Set(isGrounded);
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            // TODO: All or Any ?
            return Abilities.Unlocked.All(a => a.IsColliderValidForCollisions(coll));
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            foreach (var ability in Abilities.Enabled) ability.OnGroundHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            foreach (var ability in Abilities.Enabled) ability.OnMovementHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            foreach (var ability in Abilities.Unlocked) ability.ProcessHitStabilityReport(hitCollider, hitNormal, hitPoint, atCharacterPosition, atCharacterRotation, ref hitStabilityReport);
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            foreach (var ability in Abilities.Unlocked) ability.OnDiscreteCollisionDetected(hitCollider);
        }
    }
}