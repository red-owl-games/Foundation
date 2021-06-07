using UnityEngine;
using UnityEngine.EventSystems;

namespace RedOwl.Engine
{
    // The point of this class is to make button selection state work correctly when you switch between gamepad and mouse
    public class UISelectionHelper : MonoBehaviour, IPointerEnterHandler, ISelectHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void OnSelect(BaseEventData eventData)
        {
            UIHelper.UpdateSelection(gameObject);
        }
    }
}