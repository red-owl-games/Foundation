using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Engine {
    public interface IPlayerOwnershipService {
        List<PlayerOwnershipRegistration> this[int index] { get; }
        void Register(int player, bool ignoresControl, GameObject target);
        void Unregister(int player, GameObject target);
        void Toggle(int player, bool state);
    }

    [Serializable]
    public struct PlayerOwnershipRegistration {
        public bool ignoresControl;
        public GameObject target;
    }

    public class PlayerOwnershipService : IPlayerOwnershipService, IServiceInit
    {
        private Dictionary<int, bool> _active;
        private Dictionary<int, List<PlayerOwnershipRegistration>> _registry;

        public void Init()
        {
            _active = new Dictionary<int, bool>();
            _registry = new Dictionary<int, List<PlayerOwnershipRegistration>>();
        }

        public List<PlayerOwnershipRegistration> this[int index] {
            get {
                List<PlayerOwnershipRegistration> output;
                if (!_registry.TryGetValue(index, out output))
                {
                    output = new List<PlayerOwnershipRegistration>();
                    _active[index] = false;
                    _registry[index] = output;
                }
                return output;
            }
        }

        public void Register(int player, bool ignoresControl, GameObject target)
        {
            this[player].Add(new PlayerOwnershipRegistration {ignoresControl = ignoresControl, target = target});
            if (ignoresControl == false && _active[player] == false) target.Disable();
        }

        public void Unregister(int player, GameObject target)
        {
            var list = this[player];
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].target == target) list.RemoveAt(i);
            }
        }

        public void Toggle(int player, bool state)
        {
            var list = this[player];
            _active[player] = state;
            foreach(var item in list)
            {
                if (item.ignoresControl) continue;
                if (state)
                {
                    item.target.Enable();
                }
                else
                {
                    item.target.Disable();
                }
            }
        }
    }
    
    public partial class Game
    {
        public static IPlayerOwnershipService PlayerOwnershipService => Find<IPlayerOwnershipService>();

        public static void BindPlayerOwnershipService() => Bind<IPlayerOwnershipService>(new PlayerOwnershipService());
    }
}