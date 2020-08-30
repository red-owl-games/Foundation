using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public interface IVerandaView
    {
        IEnumerator DoShow();
        IEnumerator DoHide();
    }

    [HideMonoScript]
    [RequireComponent(typeof(RectTransform)), RequireComponent(typeof(CanvasGroup))]
    public class VerandaView : MonoBehaviour, IVerandaView
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("GameObject/Red Owl/UI/View", false, 13)]
        private static void Create(UnityEditor.MenuCommand menuCommand)
        {
            var component = RedOwlTools.Create<VerandaView>(menuCommand.context as GameObject, "UIView");
            var flexibleLayout = component.gameObject.AddComponent<FlexibleLayout>();
            flexibleLayout.fitType = FlexibleLayout.FitType.FixedColumns;
            flexibleLayout.fitX = true;
            flexibleLayout.fitY = true;
            flexibleLayout.columns = 1;
            RedOwlTools.Create<RectTransform>(component.gameObject, "Group", false);
        }
#endif
        public bool HideAtStart;
        public TelegramReference ToggleView;

        private RectTransform RectTransform;
        private CanvasGroup Group;

        private void Awake()
        {
            if (ToggleView != null) ToggleView.Subscribe(Toggle);
            RectTransform = GetComponent<RectTransform>();
            Group = GetComponent<CanvasGroup>();
            RectTransform.anchoredPosition3D = Vector3.zero;
            if (!HideAtStart) return;
            Group.alpha = 0;
            Group.interactable = false;
            Group.blocksRaycasts = false;
        }

        private void OnDestroy()
        {
            if (ToggleView != null) ToggleView.Subscribe(Toggle);
        }

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

        [Button, ButtonGroup("Control")]
        public void Show()
        {
#if UNITY_EDITOR
            Awake();
#endif
            StartCoroutine(DoShow());
        }
        public IEnumerator DoShow()
        {
            yield return Group.DOFade(1.0f, 0.5f).OnComplete(OnShowComplete).WaitForCompletion();
        }

        private void OnShowComplete()
        {
            Group.interactable = true;
            Group.blocksRaycasts = true;
        }

        [Button, ButtonGroup("Control")]
        public void Hide()
        {
#if UNITY_EDITOR
            Awake();
#endif
            StartCoroutine(DoHide());
        }
        public IEnumerator DoHide()
        {
            Group.interactable = false;
            Group.blocksRaycasts = false;
            yield return Group.DOFade(0f, 0.5f).WaitForCompletion();
        }
    }
}