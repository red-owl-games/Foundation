using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    [HideMonoScript]
    public class LevelPlayerKill : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // TODO: Check Tags
            other.WithComponent<AvatarRespawnable>(c => c.Kill());
        }
    }
}