using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Core
{
    public static class TextMeshUtility
    {
        public const int SortingOrderDefault = 5000;
        
        public static TextMesh Create(string text, float3 localPosition = default, Color? color = null, Transform parent = null, int fontSize = 40, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = SortingOrderDefault) {
            if (color == null) color = Color.white;
            GameObject gameObject = new GameObject($"TextMesh ({text.Truncate(12)})", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = (Color)color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
        
        public static void CreatePopup(string text, float popupTime, float3 finalPosition, float3 localPosition = default, Color? color = null, Transform parent = null, int fontSize = 40) {
            TextMesh textMesh = Create(text, localPosition, color, parent, fontSize, TextAnchor.LowerLeft);
            Transform transform = textMesh.transform;
            Vector3 moveAmount = (finalPosition - localPosition) / popupTime;
            CoroutineManager.StartRoutine(() =>
            {
                transform.position += moveAmount * Time.deltaTime;
                popupTime -= Time.deltaTime;
                if (!(popupTime <= 0f)) return false;
                transform.Destroy();
                return true;
            });
        }
    }
}