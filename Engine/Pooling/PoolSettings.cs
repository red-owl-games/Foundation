using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable]
    public class PoolSetting
    {
        // TODO: Asset Reference?
        [AssetsOnly]
        [HorizontalGroup("Settings", 0.5f), HideLabel]
        public GameObject prefab;
        [HorizontalGroup("Settings"), LabelWidth(50)]
        public int size = 10;
        [HorizontalGroup("lazy"), ToggleLeft, LabelWidth(60)]
        public bool lazyLoad;
        
        [ShowIf("lazyLoad")]
        [HorizontalGroup("lazy"), LabelText("Batch Size"), LabelWidth(65)]
        public int lazyLoadBatchSize;
        [ShowIf("lazyLoad")]
        [HorizontalGroup("lazy"), LabelText("Delay"), LabelWidth(50)]
        public float lazyLoadBatchDelay;
    }
    
    [Serializable]
    public class PoolSettings : Settings
    {
        [ListDrawerSettings(Expanded = true, DraggableItems = false)]
        public PoolSetting[] Pools;
        
    }
    
    public partial class GameSettings
    {
        [FoldoutGroup("Pooling"), SerializeField]
        private PoolSettings poolSettings = new PoolSettings();
        public static PoolSettings PoolSettings => Instance.poolSettings;
    }
}