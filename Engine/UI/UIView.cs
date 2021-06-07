using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class UIView : MonoBehaviour
    {
        public string title;
        public bool HideAtStart;
        public bool DisableChildren = true;

        [SerializeField]
        private RectTransform RectTransform;
        [SerializeField]
        private CanvasGroup Group;

        private void Awake()
        {
            if (HideAtStart) HideImmediate();
        }

        private void OnEnable()
        {
            if (RectTransform == null) RectTransform = GetComponent<RectTransform>();
            if (Group == null) Group = GetComponent<CanvasGroup>();
        }

        private void OnValidate()
        {
            if (RectTransform == null) RectTransform = GetComponent<RectTransform>();
            if (Group == null) Group = GetComponent<CanvasGroup>();
            RectTransform.anchoredPosition3D = Vector3.zero;
            gameObject.name = $"View - {title}";
        }

        [Button(), ButtonGroup("Control")]
        public void Toggle()
        {
            if (Group.alpha > 0.9f)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        [ButtonGroup("Control")]
        public void Show()
        {
            if (Game.IsRunning)
            {
                StartCoroutine(ShowAsync());
            }
            else
            {
                ShowImmediate();
            }
        }
        public IEnumerator ShowAsync()
        {
            if (DisableChildren)
            {
                transform.Children(t => t.Enable());
            }
            yield return Group.DOFade(1.0f, 0.5f).SetEase(Ease.OutQuad).OnComplete(ShowImmediate).WaitForCompletion();
        }

        public void ShowImmediate()
        {
            Group.alpha = 1f;
            Group.interactable = true;
            Group.blocksRaycasts = true;
            if (DisableChildren)
            {
                transform.Children(t => t.Enable());
            }
        }

        [ButtonGroup("Control")]
        public void Hide()
        {
            if (Game.IsRunning)
            {
                StartCoroutine(HideAsync());
            }
            else
            {
                HideImmediate();
            }
        }
        public IEnumerator HideAsync()
        {
            Group.interactable = false;
            Group.blocksRaycasts = false;
            yield return Group.DOFade(0f, 0.5f).SetEase(Ease.InExpo).OnComplete(HideImmediate).WaitForCompletion();
        }

        public void HideImmediate()
        {
            Group.interactable = false;
            Group.blocksRaycasts = false;
            Group.alpha = 0;
            if (DisableChildren)
            {
                transform.Children(t => t.Disable());
            }
        }
    }
}