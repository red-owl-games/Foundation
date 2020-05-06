using Sirenix.OdinInspector;
using Sirenix.Utilities;

namespace RedOwl.Core
{
    [HideMonoScript]
    [GlobalConfig("Resources/RedOwl/Settings", UseAsset = true)]
    public partial class CoreSettings : Database<CoreSettings>
    {
#if UNITY_EDITOR
        public class CoreSettingsEditor : UnitySettingsEditor<CoreSettings>
        {
            [UnityEditor.SettingsProvider]
            public static UnityEditor.SettingsProvider Create() { return CreateCustomSettingsProvider("Core"); }
        }
#endif
    }
}