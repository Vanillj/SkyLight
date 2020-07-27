using GameServer.Scenes;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types.Abilities
{
    class TravelAbility : Ability
    {
        private Entity Target;
        public int Speed { get; set; }

        public TravelAbility()
        {

        }

        public TravelAbility(Entity SourcePlayer)
        {
            
        }
        public TravelAbility(Entity SourcePlayer, Entity Target)
        {
            this.Target = Target;
        }
    }
}
