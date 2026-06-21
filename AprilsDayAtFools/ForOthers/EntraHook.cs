using AprilsDayAtFools;
using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace YourNamespace
{
    public static class AnyBossUnlockSingleItemHook
    {
        public static void AddAchievement(string character, string achievement)
        {
            if (Achievements == null) Achievements = new Dictionary<string, List<string>>();

            if (Achievements.ContainsKey(character)) Achievements[character].Add(achievement);
            else Achievements.Add(character, [achievement]);
        }

        public static Dictionary<string, List<string>> Achievements;


        public static void Setup()
        {
            IDetour hook = new Hook(typeof(UnlockablesManager).GetMethod(nameof(UnlockablesManager.ProcessHardEndingUnlocks), ~BindingFlags.Default), typeof(AnyBossUnlockSingleItemHook).GetMethod(nameof(UnlockablesManager_ProcessHardEndingUnlocks), ~BindingFlags.Default));
        }


        public static void UnlockablesManager_ProcessHardEndingUnlocks(Action<UnlockablesManager, IInformationCheckData, CombatResultsInformation, CharacterInGameData[]> orig, UnlockablesManager self, IInformationCheckData checkData, CombatResultsInformation results, CharacterInGameData[] currentAliveParty)
        {
            orig(self, checkData, results, currentAliveParty);

            if (Achievements == null) return;

            foreach (CharacterInGameData data in currentAliveParty)
            {
                if (Achievements.ContainsKey(data.Character.entityID))
                {
                    foreach (string ach in Achievements[data.Character.entityID])
                    {
                        self.TryUnlockFromID(ach);
                    }
                }
            }
        }
    }

    public static class ExampleKio
    {
        public static void Add()
        {
            //Kio.Add() normal stuff

            UnlockableModData kio_achievement = new UnlockableModData("Kio_Achievement");
            kio_achievement.hasModdedAchievementUnlock = true;
            kio_achievement.moddedAchievementID = "Kio_Achievement";
            kio_achievement.hasItemUnlock = true;
            kio_achievement.items = ["KioItem_TW"];
            ModdedAchievements ach = new ModdedAchievements("Kio Achievement", "Kill Boss. lol", ResourceLoader.LoadSprite("KioAchievementIcon.png"), "Kio_Achievement");
            ach.AddNewAchievementToInGameCategory(AchievementCategoryIDs.StrangersTitleLabel);
            Unlocks.AddUnlock_ByID(kio_achievement);

            AnyBossUnlockSingleItemHook.AddAchievement("Kio_CH", "Kio_Achievement");
        }
    }
}
