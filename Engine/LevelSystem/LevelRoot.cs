using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public interface ILevelBuilder
    {
        void Build(ScriptableObject levelData);
    }
    
    [HideMonoScript]
    public class LevelRoot : MonoBehaviour
    {
        [SerializeReference]
        public ILevelBuilder Builder;
    }
}