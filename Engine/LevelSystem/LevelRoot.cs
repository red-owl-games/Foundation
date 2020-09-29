using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public interface ILevelBuilder
    {
        void Build(ScriptableObject levelData, LookupTable lookupTable, Transform root);
    }
    
    [HideMonoScript]
    public class LevelRoot : MonoBehaviour
    {
        [SerializeReference]
        public ILevelBuilder Builder;
    }
}