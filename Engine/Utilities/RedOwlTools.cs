using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace RedOwl.Engine
{
    public static class RedOwlTools
    {
        public static T Create<T>(GameObject parent = null, string name = "", bool selectGameObjectAfterCreation = true) where T : Component
        {
            var type = typeof(T);
            string label = string.IsNullOrEmpty(name) ? type.Name : name;
            GameObject go = new GameObject(label, type);

#if UNITY_EDITOR
            if (parent != null)
            {
                UnityEditor.GameObjectUtility.SetParentAndAlign(go, parent);
                if (go.GetComponent<RectTransform>() != null) go.GetComponent<RectTransform>().Stretch(true);
            }
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, $"Created {label}");
            if (selectGameObjectAfterCreation) UnityEditor.Selection.activeObject = go;
#endif
            return go.GetComponent<T>();
        }
    }
}