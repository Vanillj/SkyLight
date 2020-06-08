using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Client.Types
{
    class Character
    {
        public Vector2 _pos { get; set; }
        public string _name { get; set; }

        public Character(int x, int y)
        {
            _pos = new Vector2(x, y);
            _name = "admin";
        }

        public void MoveToPos(Vector2 vector)
        {
            //Change later with animations etc.
            _pos = vector;
        }

        public string CreateJsonFromCharacter()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public void CreateCharacterFromJson(string json)
        {
            try
            {
                Character tempCharacter = Newtonsoft.Json.JsonConvert.DeserializeObject<Character>(json);
            }
            catch(Exception)
            {

            }
        }
    }
}
