using GameClient.Types.Abilities;
using GameServer.Types.Abilities;
using GameServer.Types.Abilities.SharedAbilities;
using Microsoft.Xna.Framework.Input;
using Nez.UI;
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
        private Window BindedWindow;
        public Keys BindedKey { get; set; }
        public int BindedAbilitityID { get; set; }

        /*public KeyBind(Keys BindedKey)
        {
            this.BindedKey = BindedKey;
            BindedAbilitityID = -1;
        }*/

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
