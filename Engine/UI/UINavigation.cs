using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RedOwl.Engine
{
    public enum NavigationModes
    {
        Vertical = 1,
        Horizontal,
        Grid
    }
    
    [ExecuteInEditMode]
    public class UINavigation : Selectable
    {
        [SerializeField]
        private NavigationModes childMode = NavigationModes.Vertical;

        [SerializeField] 
        private bool wrap;

        [SerializeField, ShowIf("isGridMode")]
        private int gridWidthItemCount = 1;

        private bool isGridMode => childMode == NavigationModes.Grid;

        private List<GameObject> _selectableObjects;
        private List<Selectable> _selectables;

        private GameObject _lastSelected;
        
        private Selectable _upMost;
        private Selectable _downMost;
        private Selectable _leftMost;
        private Selectable _rightMost;

        protected override void OnValidate()
        {
            if (transition != Transition.None) transition = Transition.None;
            if (navigation.mode != Navigation.Mode.Explicit) navigation = new Navigation { mode = Navigation.Mode.Explicit };
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            CacheSelectables();
            BuildNavigation();
            UIHelper.OnSelectionChanged += HandleSelectionChanged;
        }

        private void OnTransformChildrenChanged()
        {
            Game.DelayedCall(() =>
            {
                CacheSelectables();
                BuildNavigation();
            });
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UIHelper.OnSelectionChanged -= HandleSelectionChanged;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            Game.DelayedCall(() =>
            {
                if (UIHelper.Instance.LastSelected == null)
                {
                    switch (childMode)
                    {
                        case NavigationModes.Vertical:
                            EventSystem.current.SetSelectedGameObject(_upMost.gameObject);
                            break;
                        case NavigationModes.Horizontal:
                            EventSystem.current.SetSelectedGameObject(_leftMost.gameObject);
                            break;
                        case NavigationModes.Grid:
                            EventSystem.current.SetSelectedGameObject(_lastSelected != null
                                ? _lastSelected
                                : _selectables[0].gameObject);
                            break;
                    }
                }
                else
                {
                    switch (childMode)
                    {
                        case NavigationModes.Vertical:
                            EventSystem.current.SetSelectedGameObject(
                                UIHelper.Instance.LastSelected.transform.position.y < transform.position.y
                                    ? _downMost.gameObject
                                    : _upMost.gameObject);
                            break;
                        case NavigationModes.Horizontal:
                            EventSystem.current.SetSelectedGameObject(
                                UIHelper.Instance.LastSelected.transform.position.x < transform.position.x
                                    ? _leftMost.gameObject
                                    : _rightMost.gameObject);
                            break;
                        case NavigationModes.Grid:
                            EventSystem.current.SetSelectedGameObject(_lastSelected != null
                                ? _lastSelected
                                : _selectables[0].gameObject);
                            break;
                    }
                }
            });
        }

        private void HandleSelectionChanged(GameObject selected)
        {
            if (_selectableObjects.Contains(selected))
            {
                _lastSelected = selected;
            }
        }

        public void Rebuild()
        {
            CacheSelectables();
            BuildNavigation();
        }

        private void CacheSelectables()
        {
            _selectableObjects = new List<GameObject>();
            _selectables = new List<Selectable>();
            var trans = transform;
            var childCount = trans.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = trans.GetChild(i) as RectTransform;
                if (child == null) continue;
                var selectables = child.GetComponents<Selectable>();
                if (selectables.Length > 0)
                {
                    _selectableObjects.Add(child.gameObject);
                    _selectables.Add(selectables[0]);
                    if (_upMost == null || _upMost.transform.position.y < child.position.y)
                    {
                        _upMost = selectables[0];
                    }
                    if (_downMost == null || _downMost.transform.position.y > child.position.y)
                    {
                        _downMost = selectables[0];
                    }
                    if (_leftMost == null || _leftMost.transform.position.x > child.position.x)
                    {
                        _leftMost = selectables[0];
                    }
                    if (_rightMost == null || _rightMost.transform.position.x < child.position.x)
                    {
                        _rightMost = selectables[0];
                    }
                }
            }
        }

        private void BuildNavigation()
        {
            var upMostPosition = _upMost.transform.position;
            var downMostPosition = _downMost.transform.position;
            var leftMostPosition = _leftMost.transform.position;
            var rightMostPosition = _rightMost.transform.position;
            var count = _selectables.Count;
            for (int i = 0; i < count; i++)
            {
                switch (childMode)
                {
                    case NavigationModes.Vertical:
                        _selectables[i].navigation = new Navigation
                        {
                            mode = Navigation.Mode.Explicit,
                            selectOnUp = i == 0 ? (wrap ? _downMost : navigation.selectOnUp) : _selectables[i - 1],
                            selectOnDown =
                                i == count - 1 ? (wrap ? _upMost : navigation.selectOnDown) : _selectables[i + 1],
                            selectOnLeft = navigation.selectOnLeft,
                            selectOnRight = navigation.selectOnRight,
                        };
                        break;
                    case NavigationModes.Horizontal:
                        _selectables[i].navigation = new Navigation
                        {
                            mode = Navigation.Mode.Explicit,
                            selectOnUp = navigation.selectOnUp,
                            selectOnDown = navigation.selectOnDown,
                            selectOnLeft = i == 0 ? (wrap ? _rightMost : navigation.selectOnLeft) : _selectables[i - 1],
                            selectOnRight = i == count - 1
                                ? (wrap ? _leftMost : navigation.selectOnRight)
                                : _selectables[i + 1],
                        };
                        break;
                    case NavigationModes.Grid:
                    {
                        var selectablePosition = _selectables[i].transform.position;
                        _selectables[i].navigation = new Navigation
                        {
                            mode = Navigation.Mode.Explicit,
                            selectOnUp = Math.Abs(selectablePosition.y - upMostPosition.y) < 0.1f
                                ? navigation.selectOnUp
                                : (i - gridWidthItemCount >= 0 && i - gridWidthItemCount < count - 1  ? _selectables[i - gridWidthItemCount] : navigation.selectOnUp),
                            selectOnDown = Math.Abs(selectablePosition.y - downMostPosition.y) < 0.1f
                                ? navigation.selectOnDown
                                : (i + gridWidthItemCount > count - 1 ? navigation.selectOnDown : _selectables[i + gridWidthItemCount]),
                            selectOnLeft = Math.Abs(selectablePosition.x - leftMostPosition.x) < 0.1f
                                ? navigation.selectOnLeft
                                : (i - 1 >= 0 ? _selectables[i - 1] : navigation.selectOnLeft),
                            selectOnRight = Math.Abs(selectablePosition.x - rightMostPosition.x) < 0.1f
                                ? navigation.selectOnRight
                                : (i + 1 > count - 1 ? navigation.selectOnRight :_selectables[i + 1]),
                        };
                        break;
                    }
                }
            }
        }
    }
}