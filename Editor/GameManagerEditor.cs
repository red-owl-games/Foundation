using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace RedOwl.Core.Editor
{
    public class GameManagerEditor : OdinMenuEditorWindow
    {
        private static List<object> _targets;

        private int _currentTarget;
        
        [MenuItem("Red Owl/Game Manager")]
        private static void OpenWindow()
        {
            _targets = FindGameManagers();
            GetWindow<GameManagerEditor>().Show();
        }

        private static List<object> FindGameManagers()
        {
            foreach (var type in TypeExtensions.GetAllTypesWithAttribute<GameManagerAttribute>())
            {
                
            }
            return new List<object>();
        }

        protected override void OnGUI()
        {
            SirenixEditorGUI.Title("Red Owl Managers", "Because every hobby game is overscoped", TextAlignment.Center, true);
            EditorGUILayout.Space();
            
            // Draw non menu tree showing editors base.DrawEditor(_currentTarget)
            base.OnGUI();
        }

        protected override void DrawEditors()
        {
            // Draw menu tree showing editors base.DrawEditor(_currentTarget)
            base.DrawEditors();
        }

        protected override IEnumerable<object> GetTargets()
        {
            var targets = new List<object>();
            targets.Add(base.GetTarget());

            _currentTarget = targets.Count - 1;
            
            return targets;
        }

        protected override void DrawMenu()
        {
            for (int index = 0; index < _targets.Count; index++)
            {
                //If Selected
                //  base.DrawMenu()
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();

            return tree;
        }
    }
}