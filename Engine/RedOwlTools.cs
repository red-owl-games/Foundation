using UnityEngine;

namespace RedOwl.Core
{
    public static class RedOwlTools
    {
        public static T Create<T>(GameObject parent = null, string name = "", bool useRectTransform = false, bool selectGameObjectAfterCreation = true) where T : MonoBehaviour
        {
            var type = typeof(T);
            string label = string.IsNullOrEmpty(name) ? type.Name : name;
            GameObject go = useRectTransform ? new GameObject(label, typeof(RectTransform), type) : new GameObject(label, type);

#if UNITY_EDITOR
            if (parent != null)
            {
                UnityEditor.GameObjectUtility.SetParentAndAlign(go, parent);
                if (useRectTransform) go.GetComponent<RectTransform>().Stretch(true);
            }
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, $"Created {label}");
            if (selectGameObjectAfterCreation) UnityEditor.Selection.activeObject = go;
#endif
            return go.GetComponent<T>();
        }
    }
}