using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Text;
using Yarn.Unity;
using System.Reflection;
using System.Xml.Linq;
using Tools;
using Yarn;

namespace AprilsDayAtFools
{
    public static class CustomDialogueHandler
    {
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(OverworldManagerBG).GetMethod(nameof(OverworldManagerBG.InitializeDialogueFunctions), ~BindingFlags.Default), typeof(CustomDialogueHandler).GetMethod(nameof(OverworldManagerBG_InitializeDialogueFunctions), ~BindingFlags.Default));
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
    }
}
