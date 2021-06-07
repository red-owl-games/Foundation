using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RedOwl.Engine
{
    [HideMonoScript]
    [RequireComponent(typeof(Canvas))]
    public class TooltipView : MonoBehaviour, ITooltipView
    {
        public Transform target;
        public TMP_Text header;
        public TMP_Text content;
        public LayoutElement layout;
        [FormerlySerializedAs("maxWidth")] public int characterWrapLimit;

        [UnityEventFoldout] public UnityEvent onShow;
        [UnityEventFoldout] public UnityEvent onHide;

        private RectTransform _canvas;
        private RectTransform _tooltip;
        private Func<Vector3> _getPosition;
        private Tween _tween;

        private void Awake()
        {
            _canvas = (RectTransform)transform;
            _tooltip = (RectTransform)target;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_getPosition != null) target.position = GetPosition();
            if (Application.isEditor) CalculateSize();
        }

        private void CalculateSize()
        {
            var headerLength = header.text.Length;
            var contentLength = content.text.Length;

            layout.enabled = headerLength > characterWrapLimit || contentLength > characterWrapLimit;
        }

        private Vector3 GetPosition()
        {
            if (_getPosition == null) return Vector3.zero;
            var targetPosition = _getPosition();
            float tooltipRight = targetPosition.x + _tooltip.rect.width;
            if (tooltipRight > _canvas.rect.width)
            {
                // Tooltip left screen on right side
                targetPosition.x -= tooltipRight - _canvas.rect.width;
            }
            float tooltipBottom = targetPosition.y + -_tooltip.rect.height;
            if (tooltipBottom < 0)
            {
                // Tooltip left screen on bottom side
                targetPosition.y -= tooltipBottom;
            }
            return targetPosition;
        }

        public void Show(Func<Vector3> position, string contentText, string headerText = "")
        {
            if (string.IsNullOrEmpty(headerText))
            {
                header.enabled = false;
            }
            else
            {
                header.text = headerText;
                header.enabled = true;
            }

            _getPosition = position;
            content.text = contentText;
            CalculateSize();
            target.position = GetPosition();
            gameObject.SetActive(true);
            onShow.Invoke();
        }

        public void Hide()
        {
            onHide.Invoke();
            _getPosition = null;
            gameObject.SetActive(false);
        }
    } 
}

