
using GameClient.Types.Item;

namespace GameServer.Types.Item
{
    class ItemBase
    {
        //Information
        public int ID { get; set; }
        public string Name { get; set; }

        private Texture2DE Texture;

        public EqupmentType EquipmentType { get; }
        public ItemRarity Rarity { get; set; }
        public ItemTypes ItemType { get; }

        public ItemBase(int id, string name, EqupmentType Type, ItemRarity rarity, ItemTypes itemType)
        {
            ID = id;
            Name = name;
            EquipmentType = Type;
            Rarity = rarity;
            ItemType = itemType;
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
