using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class TagDespawnFilter : MonoBehaviour, IDespawnFilter
    {
        [CustomValueDrawer("TagFilterDrawer")]
        public string tagFilter;
        
#if UNITY_EDITOR
        public static string TagFilterDrawer(string value, GUIContent label)
        {
            return UnityEditor.EditorGUILayout.TagField(label, value);
        }
#endif
        
        public bool ShouldDespawn(GameObject other)
        {
            return other.tag.Equals(tagFilter);
        }
    }
}