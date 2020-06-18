using GameServer.Types.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemGenerator
{
    class ItemBase
    {
        public int id { get; set; }
        public string name { get; set; }
        public string TextureString { get; set; }
        public ItemType Type { get; set; }
        public WeaponTypes weaponType { get; set; }
        public int Intelligence { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int[] WeaponDamageRange { get; set; }

        public ItemBase(int id, string name, string TextureString, ItemType Type, int Intelligence, int Strength, int Dexterity)
        {
            this.id = id;
            this.name = name;
            this.TextureString = TextureString;
            this.Type = Type;
            this.Intelligence = Intelligence;
            this.Strength = Strength;
            this.Dexterity = Dexterity;
        }

        public ItemBase(int id, string name, string TextureString, ItemType Type, WeaponTypes weaponType, int Intelligence, int Strength, int Dexterity, int Lower, int Upper)
        {
            this.id = id;
            this.name = name;
            this.TextureString = TextureString;
            this.Type = Type;
            this.weaponType = weaponType;
            this.Intelligence = Intelligence;
            this.Strength = Strength;
            this.Dexterity = Dexterity;
            int[] temp = { Lower, Upper};
            WeaponDamageRange = temp;
        }
    }
}
