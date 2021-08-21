using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedOwl.Engine
{
    [HideMonoScript, ExecuteInEditMode]
    public class UIThemeController : MonoBehaviour
    {
        private List<Text> _labels;
        private List<TMP_Text> _labelsTMP;
        private List<Button> _btns;
        
        private void Awake()
        {
            Log.Always($"Theme Ctrl '{name}' - Awake");
            Ensure();
        }

        [Button]
        public void Ensure()
        {
            _labels = new List<Text>();
            _labelsTMP = new List<TMP_Text>();
            _btns = new List<Button>();

            Walk(transform);
        }

        public void ApplyTheme(UITheme theme)
        {
            ApplyLabels(theme);
            ApplyButtons(theme);
        }

        private void Walk(Transform t)
        {
            var ignore = t.GetComponent<UIThemeIgnore>();
            var hasIgnore = ignore != null;
            if (!hasIgnore)
            {
                FindLabels(t);
                FindButtons(t);
            }
            if (!hasIgnore || ignore.includeChildren == false) t.Children(Walk);
        }

        private void FindLabels(Transform t)
        {
            var text = t.GetComponent<Text>();
            if (text != null)
            {
                //Log.Always($"Theme Ctrl '{name}' | Register Label '{text.gameObject.name}'");
                _labels.Add(text);
            }
            var textTMP = t.GetComponent<TMP_Text>();
            if (textTMP != null)
            {
                //Log.Always($"Theme Ctrl '{name}' | Register TMP Label '{textTMP.gameObject.name}'");
                _labelsTMP.Add(textTMP);
            }
        }

        private void ApplyLabels(UITheme theme)
        {
            foreach (var label in _labels)
            {
                label.font = theme.Labels.Font;
                label.color = theme.Labels.FontColor;
            }

            foreach (var label in _labelsTMP)
            {
                label.font = theme.Labels.TMP_Font;
                label.color = theme.Labels.FontColor;
            }
        }

        private void FindButtons(Transform t)
        {
            var btn = t.GetComponent<Button>();
            if (btn != null)
            {
                //Log.Always($"Theme Ctrl '{name}' | Register Button '{btn.gameObject.name}'");
                _btns.Add(btn);
            }
        }

        private void ApplyButtons(UITheme theme)
        {
            foreach (var btn in _btns)
            {   
                switch (btn.transition)
                {
                    case Selectable.Transition.ColorTint:
                        btn.colors = theme.Buttons.Colors;
                        break;
                    case Selectable.Transition.SpriteSwap:
                        var btnImage = (Image) btn.targetGraphic;
                        if (btnImage != null)
                        {
                            btnImage.color = theme.Buttons.Color;
                            btnImage.sprite = theme.Buttons.NormalSprite;
                        }
                        btn.spriteState = theme.Buttons.Sprites;
                        break;
                    case Selectable.Transition.Animation:
                        break;

                }
            }
        }
    }
}