using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace GameServer.Types.Item
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EqupmentType
    {
        Helm,
        Chest,
        Leg,
        Weapon,
        Cape,
        Glove,
        Ring,
        Boot,
        Quiver
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemTypes
    {
        Normal,
        Equipment,
        Quest
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemRarity
    {
        Normal, //White
        Magical, //Blue
        Exotic, //Yellow
        Legendary //Orange, green, purple or black
    }
}
