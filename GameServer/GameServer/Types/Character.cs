using Client.Managers;
using Microsoft.Xna.Framework;
using Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Types
{
    class Character
    {
        //public
        public Vector2 _pos;
        public string _name;

        public Character(int x, int y)
        {
            _pos.X = x;
            _pos.Y = y;
            _name = "adminName";
        }

        //help methods bellow to create character and such

        public static Character CreateCharacterFromJson(string json)
        {
            try
            {
                Character tempCharacter = Newtonsoft.Json.JsonConvert.DeserializeObject<Character>(json);
                //CharacterManager.AddCharacterToList(tempCharacter);
                return tempCharacter;
            }
            catch
            {
                return null;
            }
        }

        public string CreateJsonFromCharacter()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }

}
