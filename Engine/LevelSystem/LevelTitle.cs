using DG.Tweening;
using TMPro;
using UnityEngine;

namespace RedOwl.Core
{
    public class LevelTitle : MonoBehaviour
    {
        public CanvasGroup group;
        public TMP_Text title;
        public TMP_Text subTitle;

        private Sequence _sequence;
        private bool _isTitleNotNull;
        private bool _isSubTitleNotNull;

        private void Awake()
        {
            group.alpha = 0f;
            _isTitleNotNull = title != null;
            _isSubTitleNotNull = subTitle != null;
            LevelManager.OnCompleted += OnLoadComplete;
        }

        private void OnDestroy()
        {
            if (_sequence.IsActive()) _sequence.Kill();
        }

        private void OnLoadComplete(GameLevel level)
        {
            if (string.IsNullOrEmpty(level.title)) return;
            if (_isTitleNotNull) title.text = level.title;
            if (_isSubTitleNotNull) subTitle.text = level.subTitle;
            group.alpha = 1f;
            if (_sequence.IsActive()) _sequence.Kill();
            _sequence = DOTween.Sequence().AppendInterval(3).Append(group.DOFade(0, 1f));
            _sequence.Play();
        }
    }
}

