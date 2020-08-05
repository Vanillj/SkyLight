using Nez;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types.Components.Components
{
    class StatsComponent : Component, IUpdatable
    {
        public int MaximumHealth { get; set; }
        private CharacterPlayer player;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            MaximumHealth = 100;
            player = Entity.GetComponent<PlayerComponent>().loginManager.GetCharacter();
            player.CurrentHealth = MaximumHealth;
        }

        public void DamageHealth(int damage)
        {
            player.CurrentHealth -= damage;
            Console.WriteLine(player._name + " : " + player.CurrentHealth.ToString());
        }

        public void Update()
        {

        }
    }
}
