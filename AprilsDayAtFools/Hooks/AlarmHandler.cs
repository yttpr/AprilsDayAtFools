using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Reflection;

namespace AprilsDayAtFools
{
    public static class AlarmHandler
    {
        public static string Value => "6AMAlarm_SW";

        public static void Setup()
        {
            IDetour hook = new Hook(typeof(CombatStats).GetMethod(nameof(CombatStats.PlayerTurnStart), ~BindingFlags.Default), typeof(AlarmHandler).GetMethod(nameof(CombatStats_PlayerTurnStart), ~BindingFlags.Default));
        }

        public static void CombatStats_PlayerTurnStart(Action<CombatStats> orig, CombatStats self)
        {
            orig(self);

            foreach (CharacterCombat chara in self.Characters.Values)
            {
                if (!chara.IsAlive && chara.HasUsableItem && chara.HeldItem.name == "Aprils_6AMAlarm_SW")
                {
                    chara.SimpleSetStoredValue(Value, chara.SimpleGetStoredValue(Value) + 1);

                    if (chara.SimpleGetStoredValue(Value) >= 6)
                    {
                        if (self.ResurrectDeadCharacter(chara, self.GetRandomCharacterSlot(), 1))
                        {
                            chara.SimpleSetStoredValue(Value, 0);

                            chara.ShowItem();
                        }
                    }
                }
            }
        }
    }
}
