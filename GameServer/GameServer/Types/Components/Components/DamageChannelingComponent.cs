using GameServer.Types.Abilities;
using GameServer.Types.Abilities.SharedAbilities;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types.Components.Components
{
    class DamageChannelingComponent : ChannelingComponent
    {
        private Entity target;
        private AbilityHead ability;
        public DamageChannelingComponent(PlayerComponent pc, float ChannelTime, AbilityHead ability) : base(pc, ChannelTime)
        {
            target = pc.Target;
            this.ability = ability;
        }

        public override void ChannelExecute()
        {

            switch (ability.AbilityType)
            {
                case AbilityType.DoTAbility:
                    if (target != null)
                    {
                        target.GetComponent<DamageComponent>().AddDoTAbility(ability as DoTAbility);
                    }
                    break;
                case AbilityType.TravelAbility:
                    if (target != null)
                    {
                        target.GetComponent<DamageComponent>().DealDamageToEntity(ability.BaseDamage);
                    }
                    break;
                default:
                    break;
            }
            
            base.ChannelExecute();
        }
    }
}
