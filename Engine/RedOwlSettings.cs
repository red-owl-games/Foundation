using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedOwl.Core
{
    internal static class RedOwlInitialization
    {
        // This needs to not be on the scriptable object so that it gets called
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            foreach (var singleton in RedOwlSettings.I.singletons)
            {
                Object.DontDestroyOnLoad(Object.Instantiate(singleton));
            }
        }
    }
    
    [HideMonoScript]
    [GlobalConfig("Assets/Resources/RedOwl/", UseAsset = true)]
    public partial class RedOwlSettings : Database<RedOwlSettings>
    {
#if UNITY_EDITOR
        public class RedOwlSettingsEditor : UnitySettingsEditor<RedOwlSettings>
        {
            [UnityEditor.SettingsProvider]
            public static UnityEditor.SettingsProvider Create() { return CreateCustomSettingsProvider("Core"); }
        }
#endif

        [AssetsOnly]
        public List<GameObject> singletons;
    }
}