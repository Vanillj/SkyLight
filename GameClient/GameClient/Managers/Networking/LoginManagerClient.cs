using Client.Types;
using Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Managers
{
    class LoginManagerClient : LoginManagerHead
    {
        private Character AccountCharacter;
        public LoginManagerClient(string username, string password) : base(username, password)
        { }

        public void SetCharacter(Character character)
        {
            AccountCharacter = character;
        }

        public Character GetCharacter()
        {
            return AccountCharacter;
        }
    }
}
