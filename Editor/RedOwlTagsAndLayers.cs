using RedOwl.Engine;
using UnityEditor;

namespace RedOwl.Editor
{
    [InitializeOnLoad]
    public class RedOwlTagsAndLayers
    {
        static RedOwlTagsAndLayers()
        {
            EditorApplication.delayCall += Ensure;
        }

        private static void Ensure()
        {
            Log.Debug("Ensuring Red Owl Tags and Layers");
            TagsAndLayersUtility.CreateLayer("Environment");
            TagsAndLayersUtility.CreateLayer("EnvironmentNoCameraObstruct");
            TagsAndLayersUtility.CreateTag("Enemy");

            for (int i = 1; i < 5; i++)
            {
                var player = $"Player{i}";
                TagsAndLayersUtility.CreateTag(player);
                TagsAndLayersUtility.CreateLayer(player);
                TagsAndLayersUtility.CreateLayer($"{player}OnlyCanSee");
            }
        }
    }
}