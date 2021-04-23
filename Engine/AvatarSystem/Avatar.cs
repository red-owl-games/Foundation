using System;
using System.Collections.Generic;
using System.Linq;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem.Users;

namespace RedOwl.Engine
{
    // TODO: Remove Linq?
    
    public class AbilityCache : Container
    {
        // TODO: ServiceCache should have maintain a list which can be sorted
        public List<IAvatarAbility> All = new List<IAvatarAbility>();
        
        public IEnumerable<IAvatarAbility> Enabled => All.Where(a => a.Enabled);
        public IEnumerable<IAvatarAbility> Unlocked => Enabled.Where(a => a.Unlocked);

        public AbilityCache() : base()
        {
            OnAdded += Sort;
            OnRemoved += Sort;
        }

        private void Sort()
        {
            All = new List<IAvatarAbility>(cache.Values.Cast<IAvatarAbility>());
            All.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }
    }
    
    // The purpose of the Avatar is to Tie together the KinematicCharacterMotor with Abilities that provides the actual functionality
    [HideMonoScript]
    public class Avatar : MonoBehaviour, ICharacterController
    {
        public static List<Avatar> Players { get; } = new List<Avatar>(4);

        [SerializeField] 
        internal KinematicCharacterMotor Motor;

        [SerializeField]
        internal Animator animator;

        public bool isPlayer;

        public AnimFloatProperty VelocityXAnimParam = "VelocityX";
        public AnimFloatProperty VelocityYAnimParam = "VelocityY";
        public AnimFloatProperty VelocityZAnimParam = "VelocityZ";
        public AnimTriggerProperty LandedAnimParam = "Landed";
        public AnimBoolProperty GroundedAnimParam = "Grounded";

        public AbilityCache Abilities { get; } = new AbilityCache();
        public Guid Id { get; private set; }
        private bool _hasAnimator;
        private AnimatorController _animController;
        private IAvatarInput _inputHandler;
        private bool _isInitialized;
        private bool _wasGroundedLastFrame;
        //private AvatarInputManager _input;

        private void Awake()
        {
            Id = Guid.NewGuid();
            this.Requires(Motor);
            _hasAnimator = animator != null;
            Motor.CharacterController = this;
            if (isPlayer) Players.Add(this);
        }

        private void Start()
        {
            if (_hasAnimator)
            {
                _animController = new AnimatorController(animator);
                VelocityXAnimParam.Register(_animController);
                VelocityYAnimParam.Register(_animController);
                VelocityZAnimParam.Register(_animController);
                LandedAnimParam.Register(_animController);
                GroundedAnimParam.Register(_animController);
            }
            foreach (var ability in Abilities.All.ToArray().Reverse())
            {
                if (_hasAnimator) ability.RegisterAnimatorParams(_animController);
                ability.OnStart();
            }
            _isInitialized = true;
            Respawn();
        }

        private void Update()
        {
            ProcessInput();
        }

        internal void Respawn()
        {
            foreach (var ability in Abilities.All.ToArray())
            {
                ability.OnReset();
            }
        }

        private void OnDestroy()
        {
            Abilities.Dispose();
        }

        internal void Add(AvatarAbility ability)
        {
            Abilities.Add(ability);
            if (_isInitialized) ability.OnStart();
        }

        internal void Remove(AvatarAbility ability)
        {
            Abilities.Remove(ability);
        }
        
        public void SetInputHandler<T>(T handler) where T : class, IAvatarInput
        {
            handler.AssignAvatar(this);
            _inputHandler = handler;
        }

        public void ProcessInput()
        {
            if (_inputHandler == null) return;
            foreach (var ability in Abilities.Unlocked) ability.ProcessInput(ref _inputHandler);
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
            VelocityZAnimParam.Set(currentVelocity.z);
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