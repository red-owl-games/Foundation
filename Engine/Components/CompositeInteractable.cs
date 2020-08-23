using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public interface IInteractable
    {
        void Interact();
    }
    
    [HideMonoScript]
    public class CompositeInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private List<GameObject> interactables;

        private List<IInteractable> _cache;

        public void Start()
        {
            _cache = new List<IInteractable>();
            foreach (var interactable in interactables)
            {
                var component = interactable.GetComponent<IInteractable>();
                if (component == null) continue;
                _cache.Add(component);
            }
        }

        public void Interact()
        {
            foreach (var interactable in _cache)
            {
                interactable.Interact();
            }
        }
    }
}