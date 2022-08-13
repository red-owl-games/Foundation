using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public partial class GamePrefs
    {
        [FoldoutGroup("QualitySettings")] 
        public Parameter<AnisotropicFiltering> AnisotropicFiltering = new(
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
        public Parameter<int> VSyncCount = new(
            () => QualitySettings.vSyncCount,
            (v) => QualitySettings.vSyncCount = v);
    }
}