using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RedOwl.Engine
{
    // TODO: (this might go in input handler) If gamepad was last input disable cursor / if mouse input enable cursor  - Cursor.visible = false;
    
    [RequireComponent(typeof(EventSystem))]
    public class UIHelper : MonoBehaviour
    {
        [ClearOnReload]
        public static event Action<GameObject> OnSelectionChanged;
        
        private EventSystem _eventSystem;
        private GameObject _lastSelected;

        public GameObject LastSelected => _lastSelected;

        [ClearOnReload]
        private static UIHelper _instance;
        
        public static UIHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    EventSystem.current.EnsureComponent(out _instance);
                }

                return _instance;
            }
        }

        private void Awake()
        {
            _eventSystem = EventSystem.current;
        }

        private void Update()
        {
            if (_eventSystem.currentSelectedGameObject != null &&
                _eventSystem.currentSelectedGameObject != _lastSelected)
            {
                _lastSelected = _eventSystem.currentSelectedGameObject;
                FireSelectionChanged();
            }
            else if (_lastSelected != null && _eventSystem.currentSelectedGameObject == null)
            {
                if (!_lastSelected.activeInHierarchy)
                {
                    if (_eventSystem.firstSelectedGameObject.activeInHierarchy)
                    {
                        _lastSelected = _eventSystem.firstSelectedGameObject;
                    }
                    else
                    {
                        _lastSelected = _eventSystem.gameObject;
                    }
                }
                _eventSystem.SetSelectedGameObject(_lastSelected);
                FireSelectionChanged();
            }
        }

        private void FireSelectionChanged()
        {
            OnSelectionChanged?.Invoke(_lastSelected);
        }
        
        public static void UpdateSelection(GameObject selected)
        {
            Instance._lastSelected = selected;
            Instance.FireSelectionChanged();
        }
    }
}