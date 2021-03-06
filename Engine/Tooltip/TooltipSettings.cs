using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable]
    public class TooltipSettings : Settings
    {
        public float delay = 1f;
        // TODO: could add an "auto kill" delay
        [AssetsOnly, AssetSelector(Filter = "t:ITooltipView")]
        public GameObject prefab;
    }
    
    public partial class Game
    {
        [FoldoutGroup("Tooltip"), SerializeField]
        private TooltipSettings tooltipSettings = new TooltipSettings();
        public static TooltipSettings TooltipSettings => Instance.tooltipSettings;
    }
}