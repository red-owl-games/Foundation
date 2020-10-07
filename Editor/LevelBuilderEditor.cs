using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
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
        
        [AssetsOnly, HideIf("@Root == null")]
        public LevelData Data;

        [AssetsOnly, InlineEditor, HideIf("@Root == null")]
        public LookupTableAsset Table;
        
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
            GoogleSheetsEditor.Refresh(Data);
        }
        
        [Button(ButtonSizes.Large), ButtonGroup("Commands"), HideIf("@Root == null || Builder == null")]
        private void Build()
        {
            Root.Builder.Build(Data, Table.Table, Root.transform);
        }
        
        [Button(ButtonSizes.Large), ButtonGroup("Commands"), HideIf("@Root == null || Builder == null")]
        private void RefreshAndBuild()
        {
            Refresh();
            Build();
        }

        [Button, ShowIf("@Root == null")]
        private void CreateLevelRoot()
        {
            var go = new GameObject("Level");
            go.AddComponent<LevelRoot>();
            Root = go.GetComponent<LevelRoot>();
        }
    }
}