using Sirenix.OdinInspector;
using UnityEngine;
using YamlDotNet.Serialization;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public abstract class RedOwlScriptableObject : ScriptableObject
    {
        [YamlIgnore]
        private string developerTitle => GetType().Name;
        [YamlIgnore]
        [SerializeField, Title("@developerTitle"), TextArea(1, 6), HideLabel, PropertyOrder(-1001), HideInInlineEditors] 
        public string developerDescription;
    }
}