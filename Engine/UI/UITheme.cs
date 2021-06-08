using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedOwl.Engine
{
    [Serializable]
    public class LabelSettings
    {
        public Font Font;
        public TMP_FontAsset TMP_Font;
        public Color FontColor = Color.gray;
    }
    
    [Serializable]
    public class SelectableSettings
    {
        [Title("Colors")]
        public ColorBlock Colors = ColorBlock.defaultColorBlock;

        [Title("Sprites")] 
        public Color Color;
        public Sprite NormalSprite;
        public SpriteState Sprites;
        //[Title("Animator"), HideLabel]
        //public AnimatorController Animator;
    }
    
    [CreateAssetMenu(menuName = "Red Owl/UI/Theme", fileName = "UI Theme")]
    public class UITheme : RedOwlScriptableObject
    {
        [FoldoutGroup("Labels"), HideLabel]
        public LabelSettings Labels;

        [FoldoutGroup("Buttons"), HideLabel]
        public SelectableSettings Buttons;
        [FoldoutGroup("Toggles"), HideLabel]
        public SelectableSettings Toggles;
        [FoldoutGroup("Sliders"), HideLabel]
        public SelectableSettings Sliders;
        [FoldoutGroup("Scrollbars"), HideLabel]
        public SelectableSettings Scrollbars;
        [FoldoutGroup("Inputs"), HideLabel]
        public SelectableSettings Inputs;
        [FoldoutGroup("Dropdowns"), HideLabel]
        public SelectableSettings Dropdowns;

        [Button(ButtonSizes.Large), PropertyOrder(-100)]
        private void Apply()
        {
            foreach (var ctrl in FindObjectsOfType<UIThemeController>())
            {
                ctrl.ApplyTheme(this);
            }
        }
    }
}