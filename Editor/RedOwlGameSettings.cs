using System.IO;
using RedOwl.Engine;
using UnityEditor;
using UnityEngine;

namespace RedOwl.Editor
{
    [InitializeOnLoad]
    public class RedOwlGameSettings
    {
        static RedOwlGameSettings()
        {
            EditorApplication.delayCall += Ensure;
        }
        
        [MenuItem("Red Owl/Init Game Settings")]
        private static void Ensure()
        {
            if (File.Exists(FileController.InstanceInternal.Filepath(Game.DataLocation)) == false)
            {
                FileController.InstanceInternal.Write(Game.DataLocation, new Game());
            }

            if (File.Exists(FileController.InstanceInternal.Filepath(PlayerInputSettings.DataLocation)) == false)
            {
                var asset = ScriptableObject.CreateInstance<PlayerInputSettings>();
                asset.Reset();
                AssetDatabase.CreateAsset(asset, $"Assets/{PlayerInputSettings.DataLocation}");
                AssetDatabase.SaveAssets();
            }
        }
    }
}