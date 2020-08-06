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
        public float DamageDuration { get; set; }
        private float DamageTimerDelta { get; set; }

        public float TotalDuration { get; set; }
        private float durationDelta { get; set; }

        public DoTAbility()
        {

        }

        public void AddToDurationDelta(float durationDelta)
        {
            this.durationDelta += durationDelta;
        }
        public float getDurationDelta()
        {
            return durationDelta;
        }
        public void AddToDamageTimerDelta(float DamageTimerDelta)
        {
            this.DamageTimerDelta += DamageTimerDelta;
        }
        public float getDamageTimerDelta()
        {
            return DamageTimerDelta;
        }

    }
}
