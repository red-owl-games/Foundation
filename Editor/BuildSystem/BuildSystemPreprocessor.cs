using RedOwl.Core;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace RedOwl.Editor
{
    public class BuildSystemPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        
        public void OnPreprocessBuild(BuildReport report)
        {
            PlayerSettings.bundleVersion = Git.BuildVersion;
            Log.Warn($"Set version to '{PlayerSettings.bundleVersion}'");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}