using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [HideMonoScript]
    [RequireComponent(typeof(Collider))]
    public class Checkpoint : IndexedBehaviour<Checkpoint>
    {
        public bool isLevelStart = false;

        private void Start()
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