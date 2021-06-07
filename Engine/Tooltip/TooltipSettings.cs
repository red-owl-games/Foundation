using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable, InlineProperty, HideLabel]
    public class TooltipSettings
    {
        public float delay = 1f;
        // TODO: could add an "auto kill" delay
        [AssetsOnly, AssetSelector(Filter = "t:ITooltipView")]
        public GameObject prefab;
    }
    
    public partial class GameSettings
    {
        [FoldoutGroup("Tooltip"), SerializeField]
        private TooltipSettings tooltipSettings = new TooltipSettings();
        public static TooltipSettings TooltipSettings => Instance.tooltipSettings;
    }
}