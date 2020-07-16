using GameClient.Types.Item;
using GameServer.General;
using GameServer.Types.Item;
using Microsoft.Xna.Framework;
using System;
using System.Dynamic;

namespace Server.Types
{
    class CharacterPlayer : CharacterHead
    {
        public WeaponItem[] Equipment; //Added later from the ID
        public WeaponItem[] Inventory;
        public Vector2 physicalPosition = new Vector2(0, 0);

        public CharacterPlayer(float x, float y, string name, WeaponItem[] equipment, WeaponItem[] inventory) : base(x, y, name)
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

        public void AddItemToInventory(WeaponItem item)
        {
            //findst first empty inventory spot
            int lowestPos = Array.IndexOf(Inventory, null);
            //-1 if no found, else add it
            if (lowestPos != -1)
            {
                Inventory[lowestPos] = item;
            }
            else
            {
                //Drop item else
            }
        }

        //GET SET METHODS
        public WeaponItem[] GetEquipment()
        {
            return Equipment;
        }
        public WeaponItem[] GetInventory()
        {
            return Inventory;
        }
    }

}
