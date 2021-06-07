using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class PlayerOwnership : MonoBehaviour {
        [SerializeField, Range(1,4)] private int player;
        [Tooltip("If true, ignores being enabled/disabled by PlayerOwnershipService")]
        [SerializeField] private bool ignoresControl;

        [Inject]
        private IPlayerOwnershipService _registry;

        public void Awake()
        {
            Game.Inject(this);
            _registry.Register(player, ignoresControl, gameObject);
        }

        public void OnDestroy() {
            _registry.Unregister(player, gameObject);
        }
    }
}