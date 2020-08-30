using System.Collections.Generic;
using System.Linq;

namespace RedOwl.Core
{
    public class AvatarAbilityManager
    {
        private readonly List<IAvatarAbility> _abilities = new List<IAvatarAbility>();

        public IEnumerable<IAvatarAbility> All => _abilities;
        public IEnumerable<IAvatarAbility> Enabled => _abilities.Where(a => a.Enabled);
        public IEnumerable<IAvatarAbility> Unlocked => Enabled.Where(a => a.Unlocked);
        

    }
}