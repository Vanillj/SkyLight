using GameServer.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types.Abilities
{
    class AbilityContainer
    {
        List<Ability> MageAbilities = new List<Ability>();
        List<Ability> WarriorAbilities = new List<Ability>();
        List<Ability> ArcherAbilities = new List<Ability>();

        public void LoadAbilities()
        {

        }

        public void CreateAbility(MainScene scene, string Name)
        {

        }
    }
}
