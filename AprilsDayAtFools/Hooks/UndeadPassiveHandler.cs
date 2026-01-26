using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AprilsDayAtFools
{
    public static class UndeadPassiveHandler
    {
        public static string Room => "Aprils.Lich.Revive";
        public static void Setup()
        {
            IDetour hook1 = new Hook(typeof(CombatStats).GetMethod(nameof(CombatStats.CombatEndTriggered), ~BindingFlags.Default), typeof(UndeadPassiveHandler).GetMethod(nameof(CombatStats_FinalizeCombat), ~BindingFlags.Default));
            //IDetour hook1 = new Hook(typeof(OverworldManagerBG).GetMethod(nameof(OverworldManagerBG.CombatEndTriggered), ~BindingFlags.Default), typeof(UndeadPassiveHandler).GetMethod(nameof(CombatStats_FinalizeCombat), ~BindingFlags.Default));
        }
        public static void CombatStats_FinalizeCombat(Action<CombatStats> orig, CombatStats self)
        {
            orig(self);
            foreach (KeyValuePair<int, CharacterCombat> pair in self.Characters)
            {
                CharacterCombat chara = pair.Value;

                if (!self.CharactersAlive) break;

                if (!chara.IsAlive && chara.ContainsPassiveAbility(IDs.Undead) && !self.IsPassiveLocked(IDs.Undead))
                {
                    int num = UnityEngine.Random.Range(0, 3);
                    int num2 = ((num == 0) ? 1 : 0);
                    int num3 = ((num == 2) ? 1 : 2);

                    //party data
                    OverworldCombatSharedDataSO combatData = LoadedDBsHandler.InfoHolder.CombatData;
                    if (pair.Key < combatData.CharactersData.Length && pair.Key >= 0)
                    {
                        CharacterInGameData data = combatData.CharactersData[pair.Key];

                        if (TestUsedAbilities(data.UsedAbilities.ToArray()))
                        {
                            num2 = chara.UsedAbilities[0];
                            num3 = chara.UsedAbilities[1];
                        }

                        AddRoom(num2, num3, data.Rank, self.BundleDifficulty == BundleDifficulty.Boss);
                        continue;
                    }

                    //combat data
                    if (TestUsedAbilities(chara.UsedAbilities))
                    {
                        num2 = chara.UsedAbilities[0];
                        num3 = chara.UsedAbilities[1];
                    }

                    AddRoom(num2, num3, chara.Rank, self.BundleDifficulty == BundleDifficulty.Boss);

                    continue;
                }
            }
        }
        public static bool TestUsedAbilities(int[] list)
        {
            if (list == null) return false;
            if (list.Length != 2) return false;
            foreach (int num in list)
            {
                if (num < 0 || num > 2) return false;
            }
            return true;
        }

        public static int AddRoom(int first, int second, int rank, bool boss)
        {
            RunDataSO run = LoadedDBsHandler.InfoHolder.Run;
            RunZoneData zone = run.CurrentZoneData;
            int zoneid = run.CurrentZoneID;
            if (boss)
            {
                zoneid = run.CurrentZoneID + 1;
                if (zoneid >= run.ZoneData.Count) return -1;
                zone = run.ZoneData[zoneid];
            }
            FreeFoolEncounterSO freeFoolEncounter = LoadedAssetsHandler.GetFreeFoolEncounter(Room);
            TalkingEntityContentData newEntity = new TalkingEntityContentData(freeFoolEncounter.DialoguePath);
            int idInfo = zone.AddDialoguePathData(newEntity);
            Card card = new Card(zone.CardCount, idInfo, CardType.EventFreeFool, PilePositionType.Last, freeFoolEncounter.signID, freeFoolEncounter.encounterRoom);
            zone.AddCard(card);

            List<int> ints = [];
            for (int i = 0; i < zone.ZonePiles.Length; i++)
            {
                if (zone.ZonePiles[i]._cards.Length <= 0) continue;
                for (int j = zone.ZonePiles[i]._cards.Length - 1; j >= 0; j--)
                {
                    if (zone.ZonePiles[i]._cards[j].PilePosition == PilePositionType.End) continue;
                    if (zone.ZonePiles[i]._cards[j].HasBeenSolved) continue;
                    ints.Add(i);
                    break;
                }
            }

            int pileID = UnityEngine.Random.Range(0, zone.ZonePiles.Length);
            if (ints.Count > 0) pileID = ints.GetRandom();

            Card[] pile = zone.ZonePiles[pileID]._cards;
            List<Card> temp = new List<Card>();
            bool added = boss;
            if (boss) temp.Add(card);
            foreach (Card item in pile)
            {
                if (item.PilePosition != PilePositionType.End)
                {
                    temp.Add(item);
                }
                else
                {
                    if (!added) temp.Add(card);
                    added = true;
                    temp.Add(item);
                }
            }
            zone.ZonePiles[pileID]._cards = temp.ToArray();

            RunInGameData data = run.InGameData as RunInGameData;
            data.SetIntData("Lich_Zone_" + zoneid.ToString() + "_Info_" + idInfo.ToString() + "_FirstAB", first);
            data.SetIntData("Lich_Zone_" + zoneid.ToString() + "_Info_" + idInfo.ToString() + "_SecondAB", second);
            data.SetIntData("Lich_Zone_" + zoneid.ToString() + "_Info_" + idInfo.ToString() + "_Rank", rank);

            //SaveDataManager_2024.FullySaveGameDataToCache(run);

            return idInfo;
        }
    }
}
