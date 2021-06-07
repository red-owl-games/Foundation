using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class TweenSequence : MonoBehaviour
    {
        public enum StartType
        {
            Never,
            Awake,
            Enable,
            Start
        }
        
        public enum SequenceTypes
        {
            Loops,
            Restart,
            PingPong,
            Toggle
        }

        [HorizontalGroup("Settings"), LabelWidth(60)]
        public StartType StartOn;

        [HorizontalGroup("Settings"), LabelWidth(45)]
        public SequenceTypes type;

        [SerializeReference, ShowInInspector]
        public TweenConfig[] tweens = new TweenConfig[0];

        private Sequence _sequence;
        private bool _started;
        private bool _toggleState;

        private void Awake()
        {
            BuildSequence();
            if (StartOn == StartType.Awake) Play();
        }

        private void OnEnable()
        {
            if (StartOn == StartType.Enable) Play();
        }

        private void Start()
        {
            if (StartOn == StartType.Start) Play();
        }

        private void OnDisable()
        {
            if (StartOn == StartType.Enable)
            {
                if (type == SequenceTypes.Toggle)
                {
                    _sequence.Rewind();
                    _toggleState = false;
                }
            }
        }

        private void BuildSequence()
        {
            _sequence = DOTween.Sequence().SetAutoKill(false);
            switch (type)
            {
                case SequenceTypes.Loops:
                    _sequence.SetLoops(-1, LoopType.Restart);
                    break;
                case SequenceTypes.Restart:
                    _sequence.SetLoops(1, LoopType.Restart);
                    break;
                case SequenceTypes.PingPong:
                    _sequence.SetLoops(-1, LoopType.Yoyo);
                    break;
            }
            _sequence.Pause();
            foreach (var tween in tweens)
            {
                tween.ApplyTween(_sequence);
            }
        }

        [BoxGroup("Controls")]
        [Button(ButtonSizes.Medium), ButtonGroup("Controls/Buttons"), DisableInEditorMode]
        public void Play()
        {
            if (type == SequenceTypes.Toggle)
            {
                Toggle(!_toggleState);
            }
            else
            {
                if (type == SequenceTypes.Restart)
                    _sequence.Restart();
                else
                    _sequence.Play();
            }
        }
        
        [Button(ButtonSizes.Medium), ButtonGroup("Controls/Buttons"), DisableInEditorMode]
        public void Rewind()
        {
            if (type == SequenceTypes.Restart)
            {
                _sequence.Rewind();
            }
        }

        [Button(ButtonSizes.Medium), ButtonGroup("Controls/Buttons"), DisableInEditorMode]
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
        [Button(ButtonSizes.Medium), ButtonGroup("Controls/Buttons"), DisableInPlayMode]
        private void Preview()
        {
            if (_isPreviewing) return;
            _isPreviewing = true;
            int loops = 0;
            BuildSequence();
            if (type == SequenceTypes.Toggle) _sequence.AppendCallback(_sequence.PlayBackwards);
            DG.DOTweenEditor.DOTweenEditorPreview.PrepareTweenForPreview(_sequence);
            _sequence.OnStepComplete(() =>
            {
                switch (type)
                {
                    case SequenceTypes.Loops:
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