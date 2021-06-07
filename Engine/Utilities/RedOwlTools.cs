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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FromJson<T>(string json) => JsonUtility.FromJson<T>(json);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FromBytes<T>(byte[] bytes) => FromJson<T>(Encoding.UTF8.GetString(bytes));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToJson<T>(T item) => JsonUtility.ToJson(item);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes<T>(T item) => Encoding.UTF8.GetBytes(ToJson(item));
    }
}