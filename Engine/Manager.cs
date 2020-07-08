using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    [HideMonoScript]
    public class Manager : MonoBehaviour
    {
        [SerializeField, AssetsOnly] private ManagerReference reference = null;

        private void Awake()
        {
            reference.Bind();
        }
    }
}