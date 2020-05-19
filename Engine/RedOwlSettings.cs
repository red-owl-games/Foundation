using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace RedOwl.Core
{
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
    }
}