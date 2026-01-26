using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Text;
using Yarn.Unity;
using System.Reflection;
using System.Xml.Linq;
using Tools;
using Yarn;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking.Types;
using BrutalAPI;

namespace AprilsDayAtFools
{
    public static class CustomDialogueHandler
    {
        public static void Setup()
        {
            MethodInfo method = typeof(OverworldManagerBG).GetMethod(nameof(OverworldManagerBG.InitializeDialogueFunctions), ~BindingFlags.Default);
            if (method.GetParameters()[0].ParameterType == typeof(DialogueRunner))
            {
                IDetour diologo = new Hook(method, typeof(CustomDialogueHandler).GetMethod(nameof(OverworldManagerBG_InitializeDialogueFunctions), ~BindingFlags.Default));
            }
            else
            {
                IDetour diologo = new Hook(method, typeof(CustomDialogueHandler).GetMethod(nameof(OverworldManagerBG_InitializeDialogueFunctionsNEW), ~BindingFlags.Default));
            }
        }

        public static void OverworldManagerBG_InitializeDialogueFunctions(Action<OverworldManagerBG, DialogueRunner> orig, OverworldManagerBG self, DialogueRunner dialogueRunner)
        {
            orig(self, dialogueRunner);
            dialogueRunner.AddFunction("Aprils_WonRuns", 0, delegate (Value[] parameters)
            {
                if (WonARunWithEachChar()) return true;
                return HasAnAchievementWithEachChar();
            });
        }
        public static void OverworldManagerBG_InitializeDialogueFunctionsNEW(Action<OverworldManagerBG, DialogueRunner_BO> orig, OverworldManagerBG self, DialogueRunner_BO dialogueRunner)
        {
            orig(self, dialogueRunner);
            dialogueRunner.AddFunction("Aprils_WonRuns", 0, delegate (Value[] parameters)
            {
                if (WonARunWithEachChar()) return true;
                return HasAnAchievementWithEachChar();
            });
            dialogueRunner.AddCommandHandler("VomitTreasureItem", GenerateItemPresent);
            dialogueRunner.AddCommandHandler("AddStoredLichFool", AddLich);
        }

        public static OverworldManagerBG World;
        public static void GenerateItemPresent(string[] info)
        {
            World = UnityEngine.Object.FindObjectOfType<OverworldManagerBG>();
            World.StartCoroutine(ProcessPresent(BronzoPresentType.TreasureItem));
        }
        public static IEnumerator ProcessPresent(BronzoPresentType type)
        {
            yield return null;
            World.SaveProgress(saveRunToo: false);
            BaseWearableSO item = null;
            RunDataSO run = World._informationHolder.Run;
            ItemPoolDataBase itemPoolDB = LoadedDBsHandler.ItemPoolDB;
            switch (type)
            {
                case BronzoPresentType.TreasureItem:
                    item = itemPoolDB.TryGetPrizeItem(run.PrizesInRun, World._informationHolder.Game);
                    break;
                case BronzoPresentType.ShopItem:
                    item = itemPoolDB.TryGetShopItem(run.ShopItemsInRun, World._informationHolder.Game);
                    break;
            }
            if (type == BronzoPresentType.Combat)
            {
                string uIData = LocUtils.GameLoc.GetUIData(UILocID.PrizeGetLabel);
                string uIData2 = LocUtils.GameLoc.GetUIData(UILocID.ContinueButton);
                ConfirmDialogReference dialogReference = new ConfirmDialogReference(uIData, uIData2, "", null);
                NtfUtils.notifications.PostNotification(Utils.showConfirmDialogNtf, null, dialogReference);
                while (dialogReference.result == DialogResult.Abort)
                {
                    yield return null;
                    NtfUtils.notifications.PostNotification(Utils.showConfirmDialogNtf, null, dialogReference);
                }
                int currentZoneID = run.CurrentZoneID;
                int max = run.zoneData.Count - 1;
                int bronzoZoneChance = DataUtils.GetBronzoZoneChance();
                bronzoZoneChance = Mathf.Clamp(bronzoZoneChance, currentZoneID, max);
                World.AddSpecialPileEnemy(SignType_GameIDs.BronzoEnemy.ToString(), bronzoZoneChance);
                World._informationHolder.Run.inGameData.SetBoolData(DataUtils.bronzoIsGiftCombatVar, variable: true);
                while (dialogReference.result == DialogResult.None)
                {
                    yield return null;
                }
            }
            else if (item != null)
            {
                bool hasItemSpace = run.playerData.HasItemSpace;
                StringTrioData itemLocData = item.GetItemLocData();
                string text = string.Format(LocUtils.GameLoc.GetUIData(UILocID.PrizeGetLabel), itemLocData.text);
                if (!hasItemSpace)
                {
                    text = text + "\n" + LocUtils.GameLoc.GetUIData(UILocID.ItemNotEnoughSpace);
                }
                string uIData3 = LocUtils.GameLoc.GetUIData(UILocID.ContinueButton);
                ConfirmDialogReference dialogReference = new ConfirmDialogReference(text, uIData3, "", item.wearableImage, itemLocData.description);
                NtfUtils.notifications.PostNotification(Utils.showConfirmDialogNtf, null, dialogReference);
                while (dialogReference.result == DialogResult.Abort)
                {
                    yield return null;
                    NtfUtils.notifications.PostNotification(Utils.showConfirmDialogNtf, null, dialogReference);
                }
                World._soundManager.PlayOneshotSound(World._soundManager.itemGet);
                while (dialogReference.result == DialogResult.None)
                {
                    yield return null;
                }
                if (hasItemSpace)
                {
                    run.playerData.AddNewItem(item);
                }
                else
                {
                    World._extraItemMenuIsOpen = true;
                    World._extraUIHandler.OpenItemExchangeMenu(new BaseWearableSO[1] { item });
                    while (World._extraItemMenuIsOpen)
                    {
                        yield return null;
                    }
                }
            }
            else
            {
                int prize = DataUtils.GetBronzoMoney(type);
                string dialog = string.Format(LocUtils.GameLoc.GetUIData(UILocID.BronzoMoneyGetLabel), prize);
                string uIData4 = LocUtils.GameLoc.GetUIData(UILocID.ContinueButton);
                Sprite coinSprite = World._spritesDB.GetCoinSprite(prize);
                ConfirmDialogReference dialogReference = new ConfirmDialogReference(dialog, uIData4, "", coinSprite);
                NtfUtils.notifications.PostNotification(Utils.showConfirmDialogNtf, null, dialogReference);
                while (dialogReference.result == DialogResult.Abort)
                {
                    yield return null;
                    NtfUtils.notifications.PostNotification(Utils.showConfirmDialogNtf, null, dialogReference);
                }
                run.playerData.AddCurrency(prize);
                while (dialogReference.result == DialogResult.None)
                {
                    yield return null;
                }
                if (World._informationHolder.UnlockableManager.TryProcessSingleUnlockableFromID(UnlockableID.Over100Coins, World._informationHolder))
                {
                    World.StartCoroutine(World.ProcessOverworldAchievements());
                }
            }
            World._informationHolder.Run.inGameData.SetBoolData(DataUtils.bronzoTimeTravelVar, variable: false);
            World.SaveProgress(saveRunToo: true);
        }

        public static bool WonARunWithEachChar()
        {
            if (!CheckCharHasBosses("Merced_CH")) return false;
            if (!CheckCharHasBosses("Cora_CH")) return false;
            if (!CheckCharHasBosses("Xet_CH")) return false;
            if (!CheckCharHasBosses("Saline_CH")) return false;
            if (!CheckCharHasBosses("Didion_CH")) return false;
            if (!CheckCharHasBosses("Rose_CH")) return false;
            if (!CheckCharHasBosses("Prayer_CH")) return false;
            if (!CheckCharHasBosses("Kafka_CH")) return false;
            if (!CheckCharHasBosses("Bodybag_CH")) return false;
            if (!CheckCharHasBosses("Hangman_CH")) return false;
            if (!CheckCharHasBosses("Saturn_CH")) return false;
            if (!CheckCharHasBosses("Hare_CH")) return false;
            if (!CheckCharHasBosses("Moon_CH")) return false;
            if (!CheckCharHasBosses("Clerk_CH")) return false;
            if (!CheckCharHasBosses("Esther_CH")) return false;
            if (!CheckCharHasBosses("Rhys_CH")) return false;
            if (!CheckCharHasBosses("Wtmiyr_CH")) return false;
            if (!CheckCharHasBosses("Joy_CH")) return false;
            if (!CheckCharHasBosses("Patch_CH")) return false;
            if (!CheckCharHasBosses("Six_CH")) return false;
            if (!CheckCharHasBosses("Rotcore_CH")) return false;
            if (!CheckCharHasBosses("Catten_CH")) return false;
            if (!CheckCharHasBosses("Sunflower_CH")) return false;

            return true;
        }
        public static bool HasAnAchievementWithEachChar()
        {
            if (!CheckBaseGameAch(Merced.HeavenACH, Merced.OsmanACH)) return false;
            if (!CheckBaseGameAch(Cora.HeavenACH, Cora.OsmanACH)) return false;
            if (!CheckBaseGameAch(Xet.HeavenACH, Xet.OsmanACH)) return false;
            if (!CheckBaseGameAch(Saline.HeavenACH, Saline.OsmanACH)) return false;
            if (!CheckBaseGameAch(Didion.HeavenACH, Didion.OsmanACH)) return false;
            if (!CheckBaseGameAch(Rose.HeavenACH, Rose.OsmanACH)) return false;
            if (!CheckBaseGameAch(Prayer.HeavenACH, Prayer.OsmanACH)) return false;
            if (!CheckBaseGameAch(Kafka.HeavenACH, Kafka.OsmanACH)) return false;
            if (!CheckBaseGameAch(Bodybag.HeavenACH, Bodybag.OsmanACH)) return false;
            if (!CheckBaseGameAch(Hangman.HeavenACH, Hangman.OsmanACH)) return false;
            if (!CheckBaseGameAch(Saturn.HeavenACH, Saturn.OsmanACH)) return false;
            if (!CheckBaseGameAch(Hare.HeavenACH, Hare.OsmanACH)) return false;
            if (!CheckBaseGameAch(Moon.HeavenACH, Moon.OsmanACH)) return false;
            if (!CheckBaseGameAch(Clerk.HeavenACH, Clerk.OsmanACH)) return false;
            if (!CheckBaseGameAch(Esther.HeavenACH, Esther.OsmanACH)) return false;
            if (!CheckBaseGameAch(Rhys.HeavenACH, Rhys.OsmanACH)) return false;
            if (!CheckBaseGameAch(Wtmiyr.HeavenACH, Wtmiyr.OsmanACH)) return false;
            if (!CheckBaseGameAch(Joy.HeavenACH, Joy.OsmanACH)) return false;
            if (!CheckBaseGameAch(Patch.HeavenACH, Patch.OsmanACH)) return false;
            if (!CheckBaseGameAch(Six.HeavenACH, Six.OsmanACH)) return false;
            if (!CheckBaseGameAch(Rotcore.HeavenACH, Rotcore.OsmanACH)) return false;
            if (!CheckBaseGameAch(Catten.HeavenACH, Catten.OsmanACH)) return false;
            if (!CheckBaseGameAch(Sunflower.HeavenACH, Sunflower.OsmanACH)) return false;
            
            return true;
        }
        public static bool CheckCharHasBosses(string charID)
        {
            List<string> pearls = LoadedDBsHandler.InfoHolder.Game.GetBossPearlDataCopy(charID);
            if (pearls == null || pearls.Count <= 0)
            {
                //UnityEngine.Debug.Log("need unlocks for:" + charID);
                return false;
            }
            return true;
        }
        public static bool CheckBaseGameAch(string heaven, string osman)
        {
            bool first = LoadedDBsHandler.AchievementDB.GetModdedAchievementInfo(heaven).m_bAchieved || LoadedDBsHandler.AchievementDB.GetModdedAchievementInfo(heaven).m_offlinebAchieved;
            bool second = LoadedDBsHandler.AchievementDB.GetModdedAchievementInfo(osman).m_bAchieved || LoadedDBsHandler.AchievementDB.GetModdedAchievementInfo(osman).m_offlinebAchieved;
            return first || second;
        }


        public static void AddLich(string[] info)
        {
            RunDataSO run = LoadedDBsHandler.InfoHolder.Run;
            RunZoneData currentZoneData = run.CurrentZoneData;
            Card card = currentZoneData.GetCard(run._currentCardID);

            int first = run.InGameData.GetIntData("Lich_Zone_" + run.CurrentZoneID.ToString() + "_Info_" + card.IDInfo.ToString() + "_FirstAB");
            int second = run.InGameData.GetIntData("Lich_Zone_" + run.CurrentZoneID.ToString() + "_Info_" + card.IDInfo.ToString() + "_SecondAB");
            int rank = run.InGameData.GetIntData("Lich_Zone_" + run.CurrentZoneID.ToString() + "_Info_" + card.IDInfo.ToString() + "_Rank");

            run.playerData.AddNewCharacter(LoadedAssetsHandler.GetCharacter("Lich_CH"), rank, [first, second], false);
            NtfUtils.notifications.PostNotification(Utils.updateCharacterVisuals);

            SaveDataManager_2024.FullySaveGameDataToCache(run);
        }
    }
}
