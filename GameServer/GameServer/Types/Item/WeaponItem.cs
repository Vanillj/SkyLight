using GameServer.Types.Item;

namespace GameClient.Types.Item
{
    class WeaponItem : EqupmentBase
    {
        public WeaponTypes weaponType { get; }
        public int[] WeaponDamageRange { get; }
        public WeaponItem(int id, string name, EqupmentType Type, WeaponTypes weaponType, int Intelligence, int Strength, int Dexterity, int Lower, int Upper, ItemRarity rarity, int intreq, int strreq, int dexreq, int LevelReq, ItemTypes itemType) 
            : base(id, name, Type, Intelligence, Strength, Dexterity, rarity, intreq, strreq, dexreq, LevelReq, itemType)
        {
            this.weaponType = weaponType;
            WeaponDamageRange = new int[] { Lower, Upper };
        }
    }
}