using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public partial class RedOwlSettings
    {
        [SerializeField]
        [FoldoutGroup("Save Game System")]
        internal bool saveGameEnabled;
        public static bool SaveGameEnabled => I.saveGameEnabled;
    }
}