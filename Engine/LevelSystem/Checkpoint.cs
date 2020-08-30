using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    [HideMonoScript]
    [RequireComponent(typeof(Collider))]
    public class Checkpoint : MonoBehaviour
    {
        public bool isLevelStart = false;

        private void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        [Button]
        private void ForceIsLevelStart()
        {
            isLevelStart = true;
            foreach (var checkpoint in FindObjectsOfType<Checkpoint>())
            {
                if (checkpoint == this) continue;
                if (checkpoint.isLevelStart) checkpoint.isLevelStart = false;
            }
            
        }
    }
}