using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class ItemPostLoader
    {
        public static List<KeyValuePair<ItemModdedUnlockInfo, string>> Items;

        public static void Add(KeyValuePair<ItemModdedUnlockInfo, string> item)
        {
            if (Items == null) Items = [];
            Items.Add(item);
        }

        public static void Load()
        {
            if (Items == null) Items = [];
            foreach (KeyValuePair<ItemModdedUnlockInfo, string> item in Items)
            {
                if (item.Key.achievementID.Contains("March"))
                {
                    if (MarchCheck.IsMarch || LoadedDBsHandler.InfoHolder.Game.IsItemUnlocked(item.Key.itemID)) AprilsDayAtFools.Items.AddItemData(item.Key, item.Value);
                }
                else
                {
                    AprilsDayAtFools.Items.AddItemData(item.Key, item.Value);
                }
            }
        }
    }
}
