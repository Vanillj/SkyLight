using GameServer.General;
using GameServer.Types.Item;
using Microsoft.Xna.Framework;

namespace Server.Types
{
    class CharacterPlayer : CharacterHead
    {
        private ItemBase[] Equipment = new ItemBase[StaticConstantValues.EquipmentLength];
        public int[] EquipmentInt = new int[StaticConstantValues.EquipmentLength];
        public Vector2 physicalPosition = new Vector2(0, 0);

        public CharacterPlayer(float x, float y, string name) : base(x, y, name)
        {
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
