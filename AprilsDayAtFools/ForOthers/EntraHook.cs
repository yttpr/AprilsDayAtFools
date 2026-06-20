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
}
