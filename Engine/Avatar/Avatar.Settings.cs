using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    #region Settings
    
    [Serializable]
    public class AvatarSettings
    {
        [TitleGroup("Jump Height"), HorizontalGroup("Jump Height/Options")]
        [SerializeField, DisableInPlayMode, OnValueChanged("Calculate"), LabelWidth(30), LabelText("Min")]
        private float minJumpHeight = 2f;
        [HorizontalGroup("Jump Height/Options")]
        [SerializeField, DisableInPlayMode, OnValueChanged("Calculate"), LabelWidth(30), LabelText("Max")]
        private float maxJumpHeight = 4f;
        [SerializeField, DisableInPlayMode, OnValueChanged("Calculate")]
        private float jumpTime = .4f;
        [SerializeField, DisableInPlayMode, OnValueChanged("Calculate")]
        private float gravityFallFactor = 3f;

        public float GravityFallFactor => gravityFallFactor;

        [NonSerialized] private bool _isInitialized;
        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public float Gravity { get; private set; }
        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public float GravityFalling { get; private set; }
        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public float MaxJumpVelocity { get; private set; }
        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public float MinJumpVelocity { get; private set; }

        private void Calculate()
        {
            _isInitialized = false;
            Initialize();
        }

        public void Initialize()
        {
            if (_isInitialized) return;
            Gravity = Kinematic.CalculateGravity_Jump(maxJumpHeight, jumpTime);
            GravityFalling = Gravity * gravityFallFactor;
            MaxJumpVelocity = Kinematic.CalculateVelocity_Jump(maxJumpHeight, Gravity);
            MinJumpVelocity = Kinematic.CalculateVelocity_Jump(minJumpHeight, Gravity);
            _isInitialized = true;
        }
    }

    public partial class GameSettings
    {
        [FoldoutGroup("Avatar"), SerializeField, InlineProperty, HideLabel]
        private AvatarSettings avatarSettings = new AvatarSettings();
        public static AvatarSettings AvatarSettings => Instance.avatarSettings;
    }
    
    #endregion
}