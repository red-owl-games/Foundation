using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace RedOwl.Core.Editor
{
    public class LevelBuilderEditor : OdinEditorWindow
    {
        [SceneObjectsOnly]
        public LevelRoot Root;
        [AssetsOnly]
        public ScriptableObject LevelData;
        
        [MenuItem("Project/Level Builder")]
        private static void ShowWindow()
        {
            var window = GetWindow<LevelBuilderEditor>();
            window.titleContent = new GUIContent("Level Builder");
            window.Root = FindObjectOfType<LevelRoot>();
            window.Show();
        }
        
        [Button, ButtonGroup("Commands")]
        private void Refresh()
        {
            GoogleSheetsEditor.Refresh(LevelData);
        }
        
        [Button, ButtonGroup("Commands")]
        private void Build()
        {
            Root.Builder.Build(LevelData);
        }
        
        [Button, ButtonGroup("Commands")]
        private void RefreshAndBuild()
        {
            Refresh();
            Build();
        }
    }
}