using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface ILevelBuilder
    {
        ScriptableObject Data { get; }
        void Build(Transform root);
    }
    
    [HideMonoScript]
    public class LevelRoot : MonoBehaviour
    {
        [SerializeReference]
        public ILevelBuilder Builder;
    }
}