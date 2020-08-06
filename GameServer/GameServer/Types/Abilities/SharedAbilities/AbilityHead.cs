using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types.Abilities.SharedAbilities
{
    class AbilityHead
    {
        public string AbilityName { get; set; }
        public int ID { get; set; }
        public int BaseDamage { get; set; }
        public int LevelRequirement { get; set; }
        public int ChannelTime { get; set; }
        public int TravelTime { get; set; }
        public int TravelSpeed { get; set; }
        public Class ClassAbility { get; set; }
        public AbilityType AbilityType { get; set; }
    }

    public enum AbilityType
    {
        TravelAbility,
        ConeAbility,
        DoTAbility
    }

    public enum Class
    {
        Warrior,
        Mage,
        Archer
    }
}
