using GameServer.Types.Abilities.SharedAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Types.Abilities
{
    class AbilityformaterClient
    {
        public List<AbilityHead> TravelAbility { get; set; }
        public List<AbilityHead> ConeAbility { get; set; }
        public List<AbilityHead> DoTAbility { get; set; }

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
