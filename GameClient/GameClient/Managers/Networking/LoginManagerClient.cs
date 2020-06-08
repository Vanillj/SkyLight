using Client.Types;
using Server.Managers;

namespace Client.Managers
{
    class LoginManagerClient : LoginManagerHead
    {
        private static Character AccountCharacter;
        public LoginManagerClient(string username, string password) : base(username, password)
        { }

        public void SetCharacter(Character character)
        {
            AccountCharacter = character;
        }

        public static Character GetCharacter()
        {
            return AccountCharacter;
        }
    }
}
