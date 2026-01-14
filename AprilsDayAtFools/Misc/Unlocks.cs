using BrutalAPI;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Unlocking
    {
        public static void SetUpUnlocks(string characterID, string heavenitemID, string osmanitemID, string heavenACH, string osmanACH, string heavenunlock, string osmanunlock)
        {
            SetUpHeavenUnlock(characterID, heavenitemID, heavenACH, heavenunlock);
            SetUpOsmanUnlock(characterID, osmanitemID, osmanACH, osmanunlock);
            if (LoadedAssetsHandler.GetWearable(osmanitemID) == null) Debug.LogWarning("osmanitemid wrong " + osmanitemID);
            if (LoadedAssetsHandler.GetWearable(heavenitemID) == null) Debug.LogWarning("heavenitemid wrong " + heavenitemID);
        }
        public static void SetUpOsmanUnlock(string characterID, string itemID, string ACH, string unlock)
        {
            BrutalAPI.Unlocks.GetUnlock_OsmanFinalBoss().AddUnlockData(characterID, Unlocks.GenerateUnlockData(unlock, ACH, "", "", [itemID]));
        }
        public static void SetUpHeavenUnlock(string characterID, string itemID, string ACH, string unlock)
        {
            BrutalAPI.Unlocks.GetUnlock_HeavenFinalBoss().AddUnlockData(characterID, Unlocks.GenerateUnlockData(unlock, ACH, "", "", [itemID]));
        }
        public static void SetUpSingleUnlock(string boss, string characterID, string itemID, string ACH, string unlock)
        {
            BrutalAPI.Unlocks.GetOrCreateUnlock_CustomFinalBoss(boss).AddUnlockData(characterID, Unlocks.GenerateUnlockData(unlock, ACH, "", "", [itemID]));
        }
        public static void GenerateAchievements(string character, string heavenitem, string osmanitem, string heavenACH, string osmanACH)
        {
            GenerateDivineAchievement(character, heavenitem, heavenACH);
            GenerateWitnessAchievement(character, osmanitem, osmanACH);
        }
        public static void GenerateWitnessAchievement(string character, string item, string ACH)
        {
            new ModdedAchievements(item, "Unlocked a new item.", ResourceLoader.LoadSprite("witness_" + character.ToLower() + ".png"), ACH).AddNewAchievementToInGameCategory(AchievementCategoryIDs.WitnessTitleLabel);
        }
        public static void GenerateDivineAchievement(string character, string item, string ACH)
        {
            new ModdedAchievements(item, "Unlocked a new item.", ResourceLoader.LoadSprite("divine_" + character.ToLower() + ".png"), ACH).AddNewAchievementToInGameCategory(AchievementCategoryIDs.DivineTitleLabel);
        }
        public static void GenerateSingleAchievement(string prefix, string character, string item, string ACH, string achLabel, string achName)
        {
            new ModdedAchievements(item, "Unlocked a new item.", ResourceLoader.LoadSprite(prefix + character.ToLower() + ".png"), ACH).AddNewAchievementToCUSTOMCategory(achLabel, achName);
        }
        public static void GenerateSingleAchievementByID(string prefix, string character, string item, string ACH, string achLabel, string achName)
        {
            new ModdedAchievements(LoadedAssetsHandler.GetWearable(item)._itemName, "Unlocked a new item.", ResourceLoader.LoadSprite(prefix + character.ToLower() + ".png"), ACH).AddNewAchievementToCUSTOMCategory(achLabel, achName);
        }
        public static void AddSinglePearl(string charID, string boss, string ACH)
        {
            LoadedAssetsHandler.GetCharacter(charID).m_BossAchData.Add(new CharFinalBossAchData(boss, ACH));
        }
    }
}
