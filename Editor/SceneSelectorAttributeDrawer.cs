using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace RedOwl.Core.Editor
{
    public class SceneSelectorAttributeStringDrawer : OdinAttributeDrawer<SceneSelectorAttribute, Object>
    {
        private SceneAsset[] _allScenes;
        private SceneAsset _currentScene;

        protected override void Initialize()
        {
            _allScenes = AssetDatabase.FindAssets( "t:Scene" )
                .Select( AssetDatabase.GUIDToAssetPath )
                .Select( AssetDatabase.LoadAssetAtPath<SceneAsset> )
                .ToArray();

            _currentScene = _allScenes.FirstOrDefault( x => x == ValueEntry.SmartValue );
        }

        protected override void DrawPropertyLayout( GUIContent label )
        {
            // TODO: Raise Error Box if not in build settings - https://github.com/starikcetin/unity-scene-reference/blob/upm/SceneReference.cs#L254
            var newScene = SirenixEditorFields.UnityObjectField( label, _currentScene, typeof( SceneAsset ), false ) as SceneAsset;
            if (newScene == _currentScene) return;
            _currentScene = newScene;

            if ( _currentScene == null ) ValueEntry.SmartValue = null;
            else if (newScene != null) ValueEntry.SmartValue = newScene;
        }
    }
}