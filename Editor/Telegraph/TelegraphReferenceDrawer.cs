using System;
using System.Linq;
using RedOwl.Engine;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace RedOwl.Editor
{
    public class TelegraphReferenceDrawer : OdinAttributeDrawer<TelegraphReferenceAttribute, string>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();

            string[] possible = (from type in TypeCache.GetTypesWithAttribute<TelegraphAttribute>()
                from propertyTuple in type.GetPropertiesWithAttribute<TelegramAttribute>()
                select string.IsNullOrEmpty(propertyTuple.Item1.NameOverride)
                    ? propertyTuple.Item2.Name
                    : propertyTuple.Item1.NameOverride).ToArray();

            string current = ValueEntry.SmartValue;
            int selected = SirenixEditorFields.Dropdown(rect, label, Array.IndexOf(possible, current), possible);
            if (selected > -1) ValueEntry.SmartValue = possible[selected];
        }
    }
}