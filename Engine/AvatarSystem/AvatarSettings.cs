using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    [Serializable]
    public class AvatarSettings : Settings<AvatarSettings>
    {
        [SerializeField, DisableInPlayMode]
        private float maxJumpHeight = 4f;
        [SerializeField, DisableInPlayMode]
        private float minJumpHeight = 2f;
        [SerializeField, DisableInPlayMode]
        private float jumpTime = .4f;
        
        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public float Gravity { get; private set; }
        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public float MaxJumpVelocity { get; private set; }
        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public float MinJumpVelocity { get; private set; }
        
        private void CalculateGravity()
        {
            Gravity = Kinematic.CalculateGravity_Jump(maxJumpHeight, jumpTime);
            MaxJumpVelocity = Kinematic.CalculateVelocity_Jump(maxJumpHeight, Gravity);
            MinJumpVelocity = Kinematic.CalculateVelocity_Jump(minJumpHeight, Gravity);
        }

        internal static void Initialize()
        {
            var i = Instance;
            i.CalculateGravity();
        }
    }

    public static class AvatarSystemInitialization
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void InitializeBeforeSplashScreen()
        {
            AvatarSettings.Initialize();
        }
    }
}