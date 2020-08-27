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
        public string LastMultiLocation { get; set; } //Last location that is not solo or group locked
        public int CurrentHealth { get; set; }
        public CharacterPlayer(float x, float y, string name, WeaponItem[] equipment, WeaponItem[] inventory, int maxHealth = 100) : base(x, y, name, maxHealth)
        {
            Equipment = equipment;
            Inventory = inventory;
        }

        //help methods bellow to create character and such
        public void MoveToPos(Vector2 vector)
        {
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

        public override bool Equals(object obj)
        {
            if (obj is CharacterPlayer)
            {
                CharacterPlayer p = obj as CharacterPlayer;
                if (p._name.Equals(this._name))
                {
                    return true;
                }
            }
            return false;
        }
    }

}
