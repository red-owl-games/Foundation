
using RedOwl.Engine;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Editor
{
    public class UnityEventFoldoutAttributeDrawer<T> : OdinAttributeDrawer<UnityEventFoldoutAttribute, T> where T : UnityEventBase
    {
	    private UnityEventDrawer drawer = new UnityEventDrawer();

        protected override void DrawPropertyLayout(GUIContent label)
        {
	        var entry = Property.ValueEntry;
            // We need to find the standard SerializedProperty so that we can access
	    	// the isExpanded property, which we'll use to drive our foldout behaviour
	    	var serializedObject = entry.Property.Tree.UnitySerializedObject;
	    	var serializedProperty = serializedObject.FindProperty( entry.Property.UnityPropertyPath );
	        var lineHeight = EditorGUIUtility.singleLineHeight;
	        
	        var rect = EditorGUILayout.BeginHorizontal( GUILayout.MinHeight( serializedProperty.isExpanded ? drawer.GetPropertyHeight(serializedProperty, label) : lineHeight ) ).Padding(lineHeight - 3, 0f, 0f, 0f);
	        
	        serializedProperty.isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(serializedProperty.isExpanded, new GUIContent(), "Foldout");
	        if (serializedProperty.isExpanded)
		        drawer.OnGUI(rect, serializedProperty, label);

	        EditorGUILayout.EndFoldoutHeaderGroup();
	        EditorGUILayout.EndHorizontal();
	        
	        // TODO: there something weird about this label positioning
	        
	        var lineRect = rect.SetHeight( lineHeight );
	        
	        if (Event.current.type == EventType.Repaint)
	        {
		        ((GUIStyle) "RL Header").Draw(lineRect, false, false, false, false);
	        }
	        GUI.Label(lineRect.Padding(5f, 0f, 0f, 0f), label.text);
	        
	        GUILayout.Space( 5f );
        }
    }
}
