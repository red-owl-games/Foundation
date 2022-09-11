using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable]
    public class GamePrefs
    {
        [FoldoutGroup("Application")]
        public ParameterEnum<LogLevel> LogLevel = new(Engine.LogLevel.Info);
        
        [FoldoutGroup("Application")]
        public Parameter<int> MaxFPS = new(60,
            () => Application.targetFrameRate,
            (v) => Application.targetFrameRate = v);
        
        [FoldoutGroup("QualitySettings")] 
        public ParameterEnum<AnisotropicFiltering> AnisotropicFiltering = new(
            () => QualitySettings.anisotropicFiltering,
            (v) => QualitySettings.anisotropicFiltering = v);
        
        [FoldoutGroup("QualitySettings")] 
        public Parameter<int> AntiAliasing = new(
            () => QualitySettings.antiAliasing,
            (v) => QualitySettings.antiAliasing = v);
        
        [FoldoutGroup("QualitySettings")] 
        public Parameter<int> PixelLightCount = new(
            () => QualitySettings.pixelLightCount,
            (v) => QualitySettings.pixelLightCount = v);
        
        [FoldoutGroup("QualitySettings")] 
        public Parameter<float> ShadowDistance = new(
            () => QualitySettings.shadowDistance,
            (v) => QualitySettings.shadowDistance = v);

        [FoldoutGroup("QualitySettings")] 
        public ParameterEnum<ShadowQuality> ShadowQuality = new(
            () => QualitySettings.shadows,
            (v) => QualitySettings.shadows = v);
        
        [FoldoutGroup("QualitySettings")] 
        public ParameterEnum<ShadowResolution> ShadowResolution = new(
            () => QualitySettings.shadowResolution,
            (v) => QualitySettings.shadowResolution = v);
        
        [FoldoutGroup("QualitySettings")] 
        public Parameter<bool> SoftParticles = new(
            () => QualitySettings.softParticles,
            (v) => QualitySettings.softParticles = v);
        
        [FoldoutGroup("QualitySettings")] 
        public Parameter<bool> SoftVegetation = new(
            () => QualitySettings.softVegetation,
            (v) => QualitySettings.softVegetation = v);
        
        [FoldoutGroup("QualitySettings")] 
        public Parameter<int> VSyncCount = new(
            () => QualitySettings.vSyncCount,
            (v) => QualitySettings.vSyncCount = v);
    }

    public partial class Game
    {
        public GamePrefs Prefs = new();
    }
}