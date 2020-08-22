using GameServer.Types.Abilities;
using Nez;
using System.Collections.Generic;

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
                if (dot != null)
                {
                    dot.AddToDurationDelta(Time.DeltaTime);
                    dot.AddToDamageTimerDelta(Time.DeltaTime);

                    if (dot.getDurationDelta() > dot.TotalDuration)
                    {
                        TempDots.Add(dot);
                    }
                    else
                    {
                        if (dot.getDamageTimerDelta() > dot.DamageDuration)
                        {
                            //TODO: damage calculations later
                            DealDamageToEntity(dot.BaseDamage);
                            dot.AddToDamageTimerDelta(-dot.DamageDuration);
                        }
                    }
                }

            }
            DoTs.RemoveAll(d => TempDots.Contains(d));
        }

        public void DealDamageToEntity(int damage)
        {
            Entity.GetComponent<StatsComponent>().DamageHealth(damage);
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
