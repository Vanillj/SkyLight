using Microsoft.Xna.Framework.Graphics;

namespace GameServer.Types.Item
{
    class ItemBase
    {
        public int id { get; set; }
        public string name { get; set; }

        //
        public string TextureString { get; set; }
        private Texture2D Texture;

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
            int[] temp = { Lower, Upper };
            WeaponDamageRange = temp;
        }

        public Texture2D GetTexture()
        {
            return Texture;
        }
    }
}
