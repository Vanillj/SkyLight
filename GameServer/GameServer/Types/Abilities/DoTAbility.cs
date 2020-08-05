using GameServer.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types.Abilities
{
    class DoTAbility : Ability
    {
        public float DamageDuration = 2;
        public float DamageTimerDelta = 0;

        public float totalDuration = 20;
        public float durationDelta = 0;

        public DoTAbility()
        {
            BaseDamage = 5;
        }

    }
}
