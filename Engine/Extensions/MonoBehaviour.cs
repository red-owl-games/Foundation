using UnityEngine;

namespace RedOwl.Core
{
    public static class MonoBehaviourExtensions
    {
        public static void Destroy(this MonoBehaviour self) => self.gameObject.Destroy();
    }
}