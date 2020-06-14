using UnityEngine;

namespace RedOwl.Core
{
    public static class RectTransformExtensions
    {
        public static void ResetAnchoredPosition3D(this RectTransform target) { target.anchoredPosition3D = Vector3.zero;}
        public static void ResetLocalPosition(this RectTransform target) {target.localPosition = Vector3.zero; }
        public static void ResetLocalScaleToOne(this RectTransform target) { target.localScale = Vector3.one; }
        public static void AnchorMinToZero(this RectTransform target) { target.anchorMin = Vector2.zero; }
        public static void AnchorMinToCenter(this RectTransform target) { target.anchorMin =  new Vector2(0.5f, 0.5f); }
        public static void AnchorMaxToOne(this RectTransform target) {  target.anchorMax = Vector2.one;}
        public static void AnchorMaxToCenter(this RectTransform target) {  target.anchorMax =  new Vector2(0.5f, 0.5f);}
        public static void CenterPivot(this RectTransform target) { target.pivot = new Vector2(0.5f, 0.5f); }
        public static void SizeDeltaToZero(this RectTransform target) {target.sizeDelta = Vector2.zero;}
        
        public static void Stretch(this RectTransform target, bool resetScaleToOne)
        {
            if(resetScaleToOne) target.ResetLocalScaleToOne();
            target.AnchorMinToZero();
            target.AnchorMaxToOne();
            target.CenterPivot();
            target.SizeDeltaToZero();
            target.ResetAnchoredPosition3D();
            target.ResetLocalPosition();
        }
    }
}