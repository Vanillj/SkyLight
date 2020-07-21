using GameClient.Types.Item;
using GameServer.Managers;
using GameServer.Types.Item;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.General
{
    class ItemContainer
    {
        public static HashSet<WeaponItem> itemBasesList = new HashSet<WeaponItem>();

        public static HashSet<WeaponItem> itemBaseLegendary = new HashSet<WeaponItem>();
        public static HashSet<WeaponItem> itemBaseNormal = new HashSet<WeaponItem>();

        public void LoadFromFile()
        {
            itemBasesList = FileManager.GetItemInformation("Data/ItemData.json");
            foreach (var item in itemBasesList)
            {
                switch (item.GetRarity())
                {
                    case ItemRarity.Normal:
                        itemBaseNormal.Add(item);
                        break;
                    case ItemRarity.Legendary:
                        itemBaseLegendary.Add(item);
                        break;
                    default:
                        break;
                }
            }
        }
        public void GenerateItems()
        {
            LoadFromFile();
        }
    }

}
