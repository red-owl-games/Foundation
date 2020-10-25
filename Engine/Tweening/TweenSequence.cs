using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class TweenSequence : MonoBehaviour
    {
        public enum SequenceTypes
        {
            Restart,
            PingPong,
            Toggle
        }
        [HorizontalGroup("Settings")]
        [ToggleLeft]
        public bool StartOnAwake;

        [HorizontalGroup("Settings", 0.75f), LabelWidth(45)]
        public SequenceTypes type;

        [SerializeReference]
        public TweenData[] tweens = new TweenData[0];

        private Sequence _sequence;
        private bool _started;
        private bool _toggleState;

        private void Awake()
        {
            BuildSequence();
            if (StartOnAwake) Play();
        }

        private void BuildSequence()
        {
            _sequence = DOTween.Sequence().Pause().SetAutoKill(false);
            foreach (var tween in tweens)
            {
                tween.ApplyTween(_sequence);
            }

            switch (type)
            {
                case SequenceTypes.Restart:
                    _sequence.SetLoops(-1, LoopType.Restart);
                    break;
                case SequenceTypes.PingPong:
                    _sequence.SetLoops(-1, LoopType.Yoyo);
                    break;
            }
        }

        [Button(ButtonSizes.Medium), ButtonGroup("Controls"), DisableInEditorMode]
        public void Play()
        {
            if (type == SequenceTypes.Toggle)
            {
                Toggle(!_toggleState);
            }
            else
            {
                _sequence.Play();
            }
        }

        [Button(ButtonSizes.Medium), ButtonGroup("Controls"), DisableInEditorMode]
        public void Stop()
        {
            if (type != SequenceTypes.Toggle)
            {
                _sequence.Pause();
            }
        }

        public void Toggle(bool state)
        {
            _toggleState = state;
            if (_toggleState)
            {
                _sequence.PlayForward();
            }
            else
            {
                _sequence.PlayBackwards();
            }
        }

#if UNITY_EDITOR

        private static bool _isPreviewing;
        [Button(ButtonSizes.Medium), ButtonGroup("Controls"), DisableInPlayMode]
        private void Preview()
        {
            if (_isPreviewing) return;
            _isPreviewing = true;
            int loops = 0;
            BuildSequence();
            if (type == SequenceTypes.Toggle) _sequence.AppendInterval(0.5f);
            DG.DOTweenEditor.DOTweenEditorPreview.PrepareTweenForPreview(_sequence);
            _sequence.OnStepComplete(() =>
            {
                switch (type)
                {
                    case SequenceTypes.Restart:
                        loops = 2;
                        break;
                    case SequenceTypes.PingPong:
                    case SequenceTypes.Toggle:
                        loops += 1;
                        break;
                }

                if (loops > 1)
                {
                    _isPreviewing = false;
                    DG.DOTweenEditor.DOTweenEditorPreview.Stop(true);
                }
            });
            DG.DOTweenEditor.DOTweenEditorPreview.Start();
        }
        #endif
    }
}