using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public abstract class RedOwlScriptableObject : ScriptableObject
    {
        private string developerTitle => GetType().Name;
        [SerializeField, Title("@developerTitle"), TextArea(1, 6), HideLabel, HideInInlineEditors] 
        public string developerDescription;
    }
}