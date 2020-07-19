using GameServer.Types.Item;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GameClient.Types.Item
{
    class WeaponItem : EqupmentBase
    {
        [JsonProperty("WeaponTypes")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WeaponTypes weaponType { get; set; }

        public int[] WeaponDamageRange { get; set; }
        public WeaponItem(int id, string name, EqupmentType Type, WeaponTypes weaponType, int Intelligence, int Strength, int Dexterity, int Lower, int Upper, ItemRarity rarity, int intreq, int strreq, int dexreq, int LevelReq, ItemTypes itemType) 
            : base(id, name, Type, Intelligence, Strength, Dexterity, rarity, intreq, strreq, dexreq, LevelReq, itemType)
        {
            this.weaponType = weaponType;
            WeaponDamageRange = new int[] { Lower, Upper };
        }

        public WeaponTypes GetWeaponType()
        {
            return weaponType;
        }
    }
}