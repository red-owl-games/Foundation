using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace RedOwl.Engine
{
    public class LevelTitle : IndexedBehaviour<LevelTitle>
    {
        public CanvasGroup group;
        public TMP_Text title;
        public TMP_Text subTitle;

        private Sequence _sequence;
        private bool _isTitleNotNull;
        private bool _isSubTitleNotNull;

        private void OnEnable()
        {
            group.alpha = 0f;
            _isTitleNotNull = title != null;
            _isSubTitleNotNull = subTitle != null;
        }

        private void OnDisable()
        {
            if (_sequence.IsActive()) _sequence.Kill();
        }

        private void Apply(GameLevel level)
        {
            if (string.IsNullOrEmpty(level.title)) return;
            if (_isTitleNotNull) title.text = level.title;
            if (_isSubTitleNotNull) subTitle.text = level.subTitle;
            group.alpha = 1f;
            if (_sequence.IsActive()) _sequence.Kill();
            _sequence = DOTween.Sequence().AppendInterval(3).Append(group.DOFade(0, 1f));
            _sequence.Play();
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Initialize()
        {
            LevelManager.OnCompleted += OnLoaded;
        }

        private static void OnLoaded(GameLevel level)
        {
            foreach (var component in All)
            {
                component.Apply(level);
            }
        }
    }
}

