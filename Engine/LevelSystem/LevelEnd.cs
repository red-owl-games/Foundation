using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    [HideMonoScript]
    public class LevelEnd : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            LevelManager.LoadNextLevel();
        }
    }
}