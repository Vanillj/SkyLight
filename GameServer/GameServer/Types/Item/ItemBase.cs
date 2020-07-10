
using GameClient.Types.Item;

namespace GameServer.Types.Item
{
    class ItemBase
    {
        //Information
        public int id { get; set; }
        public string name { get; set; }

        private Texture2DE Texture;

        public ItemType Type { get; set; }
        public ItemRarity Rarity { get; set; }


        public ItemBase(int id, string name, ItemType Type, ItemRarity rarity)
        {
            this.id = id;
            this.name = name;
            this.Type = Type;
            Rarity = rarity;
        }

        public Texture2DE GetTexture()
        {
            return Texture;
        }
        public ItemBase SetTexture(Texture2DE texture2DE)
        {
            Texture = texture2DE;
            return this;
        }
    }
}
