using GameClient.Types.Item;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GameServer.Types.Item
{
    class ItemBase
    {
        //Information
        public int ID { get; set; }
        public string Name { get; set; }

        private Texture2DE Texture;

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public EqupmentType EquipmentType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ItemRarity Rarity { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ItemTypes ItemType { get; set; }

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
        public ItemTypes GetItemType()
        {
            return ItemType;
        }
        public EqupmentType GetEqupmentType()
        {
            return EquipmentType;
        }
        public ItemRarity GetRarity()
        {
            return Rarity;
        }
        public void SetRarity(ItemRarity rarity)
        {
            Rarity = rarity;
        }
    }
}
