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
            Log.Always($"Level Player Kill: {other.name}");
            other.WithComponent<AvatarRespawnable>(c => c.Kill());
        }
    }
}