using GameServer.Types.Abilities;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types.Components.Components
{
    class DamageComponent : Component, IUpdatable
    {
        List<Debuff> Debuffs = new List<Debuff>();
        List<DoTAbility> DoTs = new List<DoTAbility>();

        public void Update()
        {
            UpdateDoTS();
        }

        #region DoTs
        private void UpdateDoTS()
        {
            List<DoTAbility> TempDots = new List<DoTAbility>();
            foreach (var dot in DoTs)
            {
                dot.durationDelta += Time.DeltaTime;
                dot.DamageTimerDelta += Time.DeltaTime;

                if (dot.durationDelta > dot.totalDuration)
                {
                    TempDots.Add(dot);
                }
                else
                {
                    if (dot.DamageTimerDelta > dot.DamageDuration)
                    {
                        //TODO: damage calculations later
                        Entity.GetComponent<StatsComponent>().DamageHealth(dot.BaseDamage);
                        dot.DamageTimerDelta -= dot.DamageDuration;
                    }
                }
            }
            DoTs.RemoveAll(d => TempDots.Contains(d));
        }

        #endregion

        public void AddDebuff(Debuff debuff)
        {
            Debuffs.Add(debuff);
        }
        public void AddDoTAbility(DoTAbility doTAbility)
        {
            DoTs.Add(doTAbility);
        }
    }
}
