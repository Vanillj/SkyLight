using GameServer.Types.Abilities.SharedAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types.Abilities
{
    class AbilityFormater
    {
        public List<TravelAbility> TravelAbility { get; set; }
        public List<ConeAbility> ConeAbility { get; set; }
        public List<DoTAbility> DoTAbility { get; set; }

        public List<AbilityHead> AllAbilities()
        {
            List<AbilityHead> l = new List<AbilityHead>();
            l.AddRange(TravelAbility);
            l.AddRange(ConeAbility);
            l.AddRange(DoTAbility);
            return l;
        }
    }
}
