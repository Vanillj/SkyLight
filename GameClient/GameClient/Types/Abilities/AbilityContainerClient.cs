using GameClient.Managers;
using GameServer.General;
using GameServer.Managers;
using GameServer.Types.Abilities.SharedAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Types.Abilities
{
    class AbilityContainerClient
    {
        public static List<AbilityHead> MageAbilities = new List<AbilityHead>();
        public static List<AbilityHead> WarriorAbilities = new List<AbilityHead>();
        public static List<AbilityHead> ArcherAbilities = new List<AbilityHead>();
        public static List<AbilityHead> AllAbilities = new List<AbilityHead>();
        public static void LoadAbilities()
        {
            AbilityformaterClient abilities = FileManager.GetAbilityInformation("Data/" + ConstantValues.AbilityDataFileName);
            AllAbilities = abilities.AllAbilities();
            foreach (var ab in AllAbilities)
            {
                switch (ab.ClassAbility)
                {
                    case Class.Warrior:
                        WarriorAbilities.Add(ab);
                        break;
                    case Class.Mage:
                        MageAbilities.Add(ab);
                        break;
                    case Class.Archer:
                        ArcherAbilities.Add(ab);
                        break;
                    default:
                        break;
                }
            }
        }
        public static AbilityHead GetAbilityByID(int ID)
        {
            if (AllAbilities != null)
            {
                return AllAbilities.Find(a => a.ID.Equals(ID));
            }
            return null;
        }
    }
}
