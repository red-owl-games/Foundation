using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    public interface ITooltipView
    {
        void Show(Func<Vector3> position, string contentText, string headerText = "");
        void Hide();
    }
    
    [HideMonoScript]
    public class TooltipController : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [LabelWidth(50)]
        public Vector2 offset = new Vector2(0, -15);
        
        [Title("Header"), HideLabel]
        public string header;

        [TextArea, Title("Content"), HideLabel] 
        public string content;

        private bool _useMousePosition;
        private Tween _tween;

        public void OnSelect(BaseEventData eventData)
        {
            _useMousePosition = false;
            _tween = Delayed.Run(Show, GameSettings.TooltipSettings.delay);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Hide();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _useMousePosition = true;
            _tween = Delayed.Run(Show, GameSettings.TooltipSettings.delay);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Hide();
        }

        private Vector3 GetPosition()
        {
            if (_useMousePosition)
            {
                return (Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero) + offset;
            }

            return (Vector2)transform.position + offset;
        }

        private void Show()
        {
            Game.Tooltip?.Show(GetPosition, content, header);
        }

        private void Hide()
        {
            _tween.Kill();
            Game.Tooltip?.Hide();
        }
    }
}