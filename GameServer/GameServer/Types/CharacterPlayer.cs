using GameClient.Types.Item;
using GameServer.General;
using GameServer.Types.Item;
using Microsoft.Xna.Framework;

namespace Server.Types
{
    class CharacterPlayer : CharacterHead
    {
        public EqupmentBase[] Equipment; //Added later from the ID
        public ItemBase[] Inventory;
        public Vector2 physicalPosition = new Vector2(0, 0);

        public CharacterPlayer(float x, float y, string name, EqupmentBase[] equipment, ItemBase[] inventory) : base(x, y, name)
        {
            Equipment = equipment;
            Inventory = inventory;
        }

        //help methods bellow to create character and such
        public void MoveToPos(Vector2 vector)
        {
            //Change later with animations etc.
            _pos = vector;
        }

        public ItemBase[] GetEquipment()
        {
            return Equipment;
        }

        public static CharacterPlayer CreateCharacterFromJson(string json)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<CharacterPlayer>(json);
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
