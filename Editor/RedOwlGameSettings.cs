using RedOwl.Engine;
using UnityEditor;

namespace RedOwl.Editor
{
    public static class RedOwlGameSettings
    {
        [MenuItem("Red Owl/Init Game Settings")]
        private static void CreateGameSettings()
        {
            FileController.InstanceInternal.Write("game.yaml", new Game());
        }
    }
}