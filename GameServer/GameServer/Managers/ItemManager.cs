using GameClient.Types.Item;
using GameServer.General;
using GameServer.Types.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class ItemManager
    {
        public static Random random = new Random();
        public static ItemBase GenerateItem()
        {
            //TODO: Change probabilities!!

            //generate rarity
            int itemRarity = random.Next(Enum.GetValues(typeof(ItemRarity)).Length);

            //Generate Item Type
            int itemType = random.Next(Enum.GetValues(typeof(ItemType)).Length);
            ItemType itemTypeC = (ItemType)itemType;

            //generate base type
            //From basetype we get things such as ID, texture, name, possible roll values, etc
            int i = random.Next(ItemContainer.itemBaseNormal.Count);
            ItemBase baseitem = ItemContainer.itemBaseNormal.ElementAt(i);
            while (baseitem.Type != itemTypeC)
            {
                i = random.Next(ItemContainer.itemBaseNormal.Count);
                baseitem = ItemContainer.itemBaseNormal.ElementAt(i);
            }

            //Set values depending on rarity

            baseitem.Rarity = (ItemRarity)itemRarity;


            if (ItemType.Weapon == itemTypeC)
            {
                return ((WeaponItem)baseitem);
            }
            else
            {

                return ((EqupmentBase)baseitem);
            }
        }
    }
}
