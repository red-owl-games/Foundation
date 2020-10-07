using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    [HideMonoScript]
    public class LevelRoot : MonoBehaviour
    {
        [SerializeReference]
        public ILevelBuilder Builder;
    }
}