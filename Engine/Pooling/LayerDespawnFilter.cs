using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class LayerDespawnFilter : MonoBehaviour, IDespawnFilter
    {
        public LayerMask layerFilter;
        
        public bool ShouldDespawn(GameObject other)
        {
            return layerFilter == (layerFilter | (1 << other.layer));
        }
    }
}