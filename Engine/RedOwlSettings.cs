using Sirenix.OdinInspector;
using Sirenix.Utilities;

namespace RedOwl.Core
{
    [HideMonoScript]
    [GlobalConfig("Resources/RedOwl", UseAsset = true)]
    public partial class RedOwlSettings : Database<RedOwlSettings>
    {
#if UNITY_EDITOR
        public class RedOwlSettingsEditor : UnitySettingsEditor<RedOwlSettings>
        {
            [UnityEditor.SettingsProvider]
            public static UnityEditor.SettingsProvider Create() { return CreateCustomSettingsProvider("Core"); }
        }
#endif
    }
}