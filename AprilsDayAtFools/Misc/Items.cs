using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Items
    {
        public static void Add()
        {
            
        }

        public static void AddItem(this BaseWearableSO item, string lockedSprite, string linkedACH)
        {
            if (linkedACH.Contains("March") && !MarchCheck.IsMarch)
            {
                if (item.isShopItem)
                {
                    ItemUtils.AddItemToShopStatsCategoryAndGamePool(item);
                    ItemPostLoader.Add(new KeyValuePair<ItemModdedUnlockInfo, string>(new ItemModdedUnlockInfo(item.name, ResourceLoader.LoadSprite(lockedSprite), linkedACH), "Shop"));
                }
                else
                {
                    ItemUtils.AddItemToTreasureStatsCategoryAndGamePool(item); 
                    ItemPostLoader.Add(new KeyValuePair<ItemModdedUnlockInfo, string>(new ItemModdedUnlockInfo(item.name, ResourceLoader.LoadSprite(lockedSprite), linkedACH), "Treasure"));
                }
                return;
            }


            if (item.isShopItem)
            {
                ItemUtils.AddItemToShopStatsCategoryAndGamePool(item, new ItemModdedUnlockInfo(item.name, ResourceLoader.LoadSprite(lockedSprite), linkedACH));
            }
            else
            {
                ItemUtils.AddItemToTreasureStatsCategoryAndGamePool(item, new ItemModdedUnlockInfo(item.name, ResourceLoader.LoadSprite(lockedSprite), linkedACH));
            }
        }
        public static void AddFishItem(this BaseWearableSO item, int rarity, string lockedSprite, string linkedACH)
        {
            ItemUtils.AddItemFishingRodPool(item, rarity, true);
            ItemUtils.AddItemCanOfWormsPool(item, rarity, true);

            if (linkedACH.Contains("March") && !MarchCheck.IsMarch)
            {
                ItemUtils.JustAddItemSoItCanBeLoaded(item);
                ItemPostLoader.Add(new KeyValuePair<ItemModdedUnlockInfo, string>(new ItemModdedUnlockInfo(item.name, ResourceLoader.LoadSprite(lockedSprite), linkedACH), "Fish"));
                return;
            }

            ItemUtils.AddItemToCustomStatsCategoryAndGamePool(item, "Fish", "Fish", new ItemModdedUnlockInfo(item.name, ResourceLoader.LoadSprite(lockedSprite), linkedACH));
        }
        public static void AddItemData(ItemModdedUnlockInfo unlockInfo, string categoryID)
        {
            bool flag = false;
            foreach (ModdedItemCategory moddedItemCategory2 in LoadedDBsHandler.ItemUnlocksDB._ModdedItemCategories)
            {
                if (moddedItemCategory2.HasSameID(categoryID))
                {
                    flag = true;
                    moddedItemCategory2.lockedItemNames.Add(unlockInfo);
                }
            }

            if (!flag)
            {
                ModdedItemCategory moddedItemCategory = new ModdedItemCategory(categoryID, categoryID);
                moddedItemCategory.lockedItemNames.Add(unlockInfo);

                LoadedDBsHandler.ItemUnlocksDB._ModdedItemCategories.Add(moddedItemCategory);
            }
        }
        public static void Test()
        {
        }
    }
}
