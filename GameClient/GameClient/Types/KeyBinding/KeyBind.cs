using GameClient.Types.Abilities;
using GameServer.Types.Abilities;
using GameServer.Types.Abilities.SharedAbilities;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Types.KeyBinding
{
    class KeyBind
    {
        private AbilityHead BindedAbility;
        public Keys BindedKey { get; set; }
        public int BindedAbilitityID { get; set; }

        public KeyBind(Keys BindedKey, int BindedAbilitityID)
        {
            this.BindedKey = BindedKey;
            this.BindedAbilitityID = BindedAbilitityID;
            SetAbility();
        }

        private void SetAbility()
        {
            BindedAbility = AbilityContainerClient.GetAbilityByID(BindedAbilitityID);
        }

        public AbilityHead GetAbility()
        {
            return BindedAbility;
        }
    }
}
