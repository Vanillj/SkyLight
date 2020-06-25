using Client.Managers;
using Microsoft.Xna.Framework;
using Server.Types;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace Server.Managers
{
    class CharacterManager
    {
        private static HashSet<LoginManagerServer> LoginManagerServerList = new HashSet<LoginManagerServer>();

        public static void AddLoginManagerServerToList(LoginManagerServer login)
        {
            LoginManagerServerList.Add(login);
        }

        public static void RemoveLoginManagerServerFromList(float UniqueID)
        {
            LoginManagerServerList.RemoveWhere(l => l.GetUniqueID().Equals(UniqueID)); ;
        }
        public static void ChangeCharacterPosition(Vector2 vector, float uniqueID)
        {
            CharacterPlayer c = GetCharacterFromUniqueID(uniqueID);
            if(c != null)
            {
                c._pos.X += vector.X;
                c._pos.Y += vector.Y;
            }

        }

        public static HashSet<LoginManagerServer> GetLoginManagerServerList()
        {
            return LoginManagerServerList;
        }

        public static CharacterPlayer GetCharacterFromUniqueID(float uniqueID)
        {
            foreach (LoginManagerServer l in LoginManagerServerList)
            {
                if (l.GetUniqueID() == uniqueID)
                {
                    return l.GetCharacter();
                }
            }
            return null;
        }

    }
}
