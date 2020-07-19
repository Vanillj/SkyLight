using GameClient.Types.Item;
using GameServer.General;
using GameServer.Types.Item;
using System;
using System.Linq;

namespace GameServer.Managers
{
    class ItemManager
    {
        public static Random random = new Random();
        public static WeaponItem GenerateItem()
        {
            //TODO: Change probabilities!!

            //generate rarity
            int itemRarity = random.Next(Enum.GetValues(typeof(ItemRarity)).Length);

            //Generate Item Type
            int itemType = random.Next(Enum.GetValues(typeof(EqupmentType)).Length);
            EqupmentType itemTypeC = (EqupmentType)itemType;

            //generate base type
            //From basetype we get things such as ID, texture, name, possible roll values, etc
            int i = random.Next(ItemContainer.itemBaseNormal.Count);
            ItemBase baseitem = ItemContainer.itemBaseNormal.ElementAt(i);
            while (baseitem.GetEqupmentType() != itemTypeC)
            {
                i = random.Next(ItemContainer.itemBaseNormal.Count);
                baseitem = ItemContainer.itemBaseNormal.ElementAt(i);
            }

            //Set values depending on rarity

            baseitem.SetRarity((ItemRarity)itemRarity);

            if (EqupmentType.Weapon == itemTypeC)
            {
                return (WeaponItem)baseitem;
            }
            else
            {
                return (WeaponItem)baseitem;
            }
        }
    }
}
