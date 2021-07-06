using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public class RedOwlIdentityObject : RedOwlScriptableObject
    {
        [SerializeField, HideInInspector] private string id;

        [ShowInInspector, DisplayAsString, PropertyOrder(-1000), HideInInlineEditors]
        public string Id
        {
            get
            {
#if UNITY_EDITOR
                if (string.IsNullOrEmpty(id))
                {
                    id = UnityEditor.AssetDatabase.AssetPathToGUID(UnityEditor.AssetDatabase.GetAssetPath(this));
                    UnityEditor.EditorUtility.SetDirty(this);
                }
#endif
                return id;
            }
        }
    }
}