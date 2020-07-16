using Microsoft.Xna.Framework;
using Server.Managers;
using Server.Types;
using System.Collections.Generic;

namespace Client.Managers
{
    class LoginManagerClient : LoginManagerHead
    {
        private static CharacterPlayer AccountCharacter;
        public static List<CharacterPlayer> OtherCharacters = new List<CharacterPlayer>();
        private static Vector2 RecievedPosition;
        public LoginManagerClient(string username, string password) : base(username, password)
        {}

        public void SetCharacter(CharacterPlayer character)
        {
            AccountCharacter = character;
        }

        public static CharacterPlayer GetCharacter()
        {
            return AccountCharacter;
        }

        public static Vector2 GetRecievedPosition()
        {
            return RecievedPosition;
        }

        internal static void SetRecievedPosition(Vector2 pos)
        {
            RecievedPosition = pos;
        }
        
        public static void SetCharacterStatic(CharacterPlayer character)
        {
            AccountCharacter = character;
        }
        
    }
}
