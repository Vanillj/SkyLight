using GameClient.Types.Item;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nez.Textures;

namespace GameServer.Types.Item
{
    class ItemBase
    {
        //Information
        public int ID { get; set; }
        public string Name { get; set; }

        private Sprite Sprite;
        public string TextureName { get; set; }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public EqupmentType EquipmentType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ItemRarity Rarity { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ItemTypes ItemType { get; set; }

        public ItemBase(int id, string name, EqupmentType Type, ItemRarity rarity, string TextureName, ItemTypes itemType)
        {
            ID = id;
            Name = name;
            EquipmentType = Type;
            Rarity = rarity;
            ItemType = itemType;
            this.TextureName = TextureName;
        }

        public void SetSprite(Sprite sprite)
        {
            Sprite = sprite;
        }
        public Sprite GetSprite()
        {
            return Sprite;
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
