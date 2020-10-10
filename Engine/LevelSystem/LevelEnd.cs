using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    [HideMonoScript]
    public class LevelEnd : MonoBehaviour
    {
        public int count = 2;

        private int current;
        
        private void OnTriggerEnter(Collider other)
        {
            current += 1;
            // TODO: check tags?
            if (current >= count) LevelManager.LoadNextLevel();
        }

        private void OnTriggerExit(Collider other)
        {
            current -= 1;
        }
    }
}