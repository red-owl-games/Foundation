using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class LevelEnd : MonoBehaviour
    {
        [Button]
        public void NextLevel()
        {
            LevelManager.LoadNextLevel();
        }
    }
}