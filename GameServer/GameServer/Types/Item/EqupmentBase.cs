
namespace GameServer.Types.Item
{
    class EqupmentBase : ItemBase
    {
        //Attributes
        public int Intelligence { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }

        //Requirements
        public int IntelligenceReq { get; set; }
        public int StrengthReq { get; set; }
        public int DexterityReq { get; set; }
        public int LevelReq { get; set; }

        public EqupmentBase(int id, string name, ItemType Type, int Intelligence, int Strength, int Dexterity, ItemRarity itemRarity, int intreq, int strreq, int dexreq, int levelReq) : base(id, name, Type, itemRarity)
        {
            this.Intelligence = Intelligence;
            this.Strength = Strength;
            this.Dexterity = Dexterity;
            IntelligenceReq = intreq;
            StrengthReq = strreq;
            DexterityReq = dexreq;
            LevelReq = levelReq;
        }
    }
}
