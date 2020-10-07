using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RedOwl.Core.Editor
{
    public class LevelBuilderEditor : OdinEditorWindow
    {
        private LevelRoot Root;

        [PropertyOrder(-10)]
        [ShowInInspector, HideIf("@Root == null")]
        public ILevelBuilder Builder
        {
            get => Root.Builder;
            set => Root.Builder = value;
        }

        [MenuItem("Project/Level Builder")]
        private static void ShowWindow()
        {
            var window = GetWindow<LevelBuilderEditor>();
            window.titleContent = new GUIContent("Level Builder");
            window.Root = FindObjectOfType<LevelRoot>();
            window.Show();
        }
        
        [Button(ButtonSizes.Large), ButtonGroup("Commands"), HideIf("@Root == null || Builder == null")]
        private void Refresh()
        {
            GoogleSheetsEditor.Refresh(Root.Builder.Data);
        }
        
        [Button(ButtonSizes.Large), ButtonGroup("Commands"), HideIf("@Root == null || Builder == null")]
        private void Build()
        {
            Root.Builder.Build(Root.transform);
        }
        
        [Button(ButtonSizes.Large), ButtonGroup("Commands"), HideIf("@Root == null || Builder == null")]
        private void RefreshAndBuild()
        {
            Refresh();
            Build();
        }
        
        [Button(ButtonSizes.Large), ShowIf("@Root == null")]
        private void GetOrCreateLevelRoot()
        {
            Root = FindObjectOfType<LevelRoot>();
            if (Root != null) return;
            var go = new GameObject("Level");
            Root = go.AddComponent<LevelRoot>();
        }

        [DidReloadScripts]
        private static void OnScriptsReloaded() => GetWindow<LevelBuilderEditor>().GetOrCreateLevelRoot();
    }
}