using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public class TransformTween : TweenConfig<Transform>
    {
        public enum Types
        {
            Move,
            LocalMove,
            Rotate,
            Scale
        }

        public Types type;

        [ShowIf("type", Types.Move)] public Vector3 position;
        [ShowIf("type", Types.LocalMove)] public Vector3 relative;
        [ShowIf("type", Types.Rotate)] public Vector3 rotation;
        [ShowIf("type", Types.Rotate), LabelText("Mode")] public RotateMode rotationMode = RotateMode.WorldAxisAdd;
        [ShowIf("type", Types.Scale)] public Vector3 scale;

        protected override void Children(Action<Transform> callback)
        {
            target.Children(callback);
        }

        protected override Tweener BuildTween(Transform t)
        {
            switch (type)
            {
                case Types.Move:
                    return t.DOMove(position, duration);
                case Types.LocalMove:
                    return t.DOLocalMove(t.localPosition + relative, duration);
                case Types.Rotate:
                    return t.DORotate(rotation, duration, rotationMode);
                case Types.Scale:
                    return t.DOScale(scale, duration);
            }

            return null;
        }
    }
}