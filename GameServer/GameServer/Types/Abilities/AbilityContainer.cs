﻿using GameServer.General;
using GameServer.Managers;
using GameServer.Types.Abilities.SharedAbilities;
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
        public static List<AbilityHead> MageAbilities = new List<AbilityHead>();
        public static List<AbilityHead> WarriorAbilities = new List<AbilityHead>();
        public static List<AbilityHead> ArcherAbilities = new List<AbilityHead>();
        public static List<AbilityHead> AllAbilities = new List<AbilityHead>();

        public static void LoadAbilities()
        {
            AbilityFormater abilities = FileManager.GetAbilityInformation("Data/" + ConstantValues.AbilityDataFileName);
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
        public static AbilityHead GetAbilityByName(string name)
        {
            if (AllAbilities != null)
            {
                return AllAbilities.Find(a => a.AbilityName.Equals(name));
            }
            return null;
        }
    }
}
