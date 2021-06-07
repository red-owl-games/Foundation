using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RedOwl.Engine
{
    [CreateAssetMenu(menuName = "Red Owl/Scene Metadata", fileName = "Scene Metadata")]
    public class SceneMetadata : RedOwlScriptableObject
    {
        public bool isPersistent;

        [SceneAssetReferenceRestriction]
        public AssetReference sceneRef = default;

        public SceneStates state;
    }
}