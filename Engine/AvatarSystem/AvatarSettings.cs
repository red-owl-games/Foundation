using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable]
    public class AvatarSettings : Settings
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

        public void Initialize()
        {
            CalculateGravity();
        }

        private void CalculateGravity()
        {
            Gravity = Kinematic.CalculateGravity_Jump(maxJumpHeight, jumpTime);
            MaxJumpVelocity = Kinematic.CalculateVelocity_Jump(maxJumpHeight, Gravity);
            MinJumpVelocity = Kinematic.CalculateVelocity_Jump(minJumpHeight, Gravity);
        }
    }

    public partial class Game
    {
        [FoldoutGroup("Avatar System"), SerializeField]
        private AvatarSettings avatarSettings = new AvatarSettings();
        public static AvatarSettings AvatarSettings => Instance.avatarSettings;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        internal static void InitializeAvatarSettings()
        {
            Log.Always("[Avatar Settings] Initialize RedOwl Game!");
            AvatarSettings.Initialize();
        }
    }
}