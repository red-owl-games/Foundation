using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RedOwl.Engine
{
    #region Settings
    
    [Serializable]
    public class SceneSettings
    {
        public List<SceneMetadata> scenes;
        
        public SceneMetadata firstScene = default;

        [BoxGroup("Listening on")]
        public AssetReference loadScene = default;
        
        [BoxGroup("Broadcasting on")]
        public AssetReference sceneLoaded = default;
    }

    public partial class GameSettings
    {
        [FoldoutGroup("Scene"), SerializeField, InlineProperty, HideLabel]
        private SceneSettings sceneSettings = new SceneSettings();
        
        public static SceneSettings SceneSettings => Instance.sceneSettings;
    }
    
    #endregion
    

}