using UnityEngine;

namespace RedOwl.Engine
{
    public class SceneAssetReferenceRestriction : AssetReferenceUIRestriction
    {
#if UNITY_EDITOR
        public override bool ValidateAsset(Object obj)
        {
            return obj is UnityEditor.SceneAsset;
        }
#endif
    }
}