using RedOwl.Engine;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace RedOwl.Editor
{
    public class TagSelectorAttributeDrawer : OdinAttributeDrawer<TagSelectorAttribute, string> // Draw for tag field attributes on string members
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Property.ValueEntry.WeakSmartValue = label != null ?
                EditorGUILayout.TagField(label, (string)Property.ValueEntry.WeakSmartValue) :
                EditorGUILayout.TagField((string)Property.ValueEntry.WeakSmartValue);
        }
    }
}