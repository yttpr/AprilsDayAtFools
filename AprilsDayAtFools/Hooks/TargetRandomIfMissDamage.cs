using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AprilsDayAtFools
{
    public static class TargetRandomIfMissDamage
    {
        public static string Item;

        public static void Setup()
        {
            SpecialEffectExtensions.AddAction(CheckActionInfo, true);

            IDetour hook1 = new Hook(typeof(EffectInfo).GetMethod(nameof(EffectInfo.StartEffect), ~BindingFlags.Default), typeof(SpecialEffectExtensions).GetMethod(nameof(EffectInfo_StartEffect), ~BindingFlags.Default));
        }


        public static void CheckActionInfo(EffectAction self, CombatStats stats)
        {
            if (self._effects != null && !self._effects.Equals(null) && self._caster != null && self._caster.HasUsableItem && self._caster.HeldItem.name.Contains(Item))
            {
                self._caster.ShowItem();

                EffectGroups = new int[self._effects.Length];
                IsDamageEffects = new bool[self._effects.Length];
                NewTargets = new TargetSlotInfo[self._effects.Length][];

                for (int i = 0; i < self._effects.Length; i++)
                {
                    int group = i;

                    for (int j = 0; j < i; j++)
                    {
                        if (self._effects[i].targets.Matches(self._effects[j].targets))
                        {
                            group = j;
                            break;
                        }
                    }

                    EffectGroups[i] = group;

                    if (self._effects[i].effect.ContainsWillApplyDamage())
                        IsDamageEffects[group] = true;
                }
            }
            else
            {
                EffectGroups = [];
                IsDamageEffects = [];
                NewTargets = [];
            }
        }

        public static int[] EffectGroups;
        public static bool[] IsDamageEffects;
        public static TargetSlotInfo[][] NewTargets;


        public static int EffectInfo_StartEffect(Func<EffectInfo, CombatStats, IUnit, TargetSlotInfo[], bool, int, int> orig, EffectInfo self, CombatStats stats, IUnit caster, TargetSlotInfo[] possibleTargets, bool areTargetSlots, int previousExitValue)
        {
            if (areTargetSlots && caster != null && caster.HasUsableItem && caster.HeldItem.name.Contains(Item) && self.InCurrentAction() && EffectGroups.Length > SpecialEffectExtensions.ActionsTracked && IsDamageEffects[EffectGroups[SpecialEffectExtensions.ActionsTracked]])
            {
                if (NewTargets[EffectGroups[SpecialEffectExtensions.ActionsTracked]] == null)
                {
                    List<TargetSlotInfo> makenew = [..possibleTargets];

                    foreach (TargetSlotInfo target in possibleTargets)
                    {
                        if (!target.HasUnit && UnityEngine.Random.Range(0, 100) < 50) makenew.Add(RandomTarget(stats, target.IsTargetCharacterSlot));
                    }

                    NewTargets[EffectGroups[SpecialEffectExtensions.ActionsTracked]] = makenew.ToArray();
                }

                return orig(self, stats, caster, NewTargets[EffectGroups[SpecialEffectExtensions.ActionsTracked]], areTargetSlots, previousExitValue);
            }

            return orig(self, stats, caster, possibleTargets, areTargetSlots, previousExitValue);
        }

        public static TargetSlotInfo RandomTarget(CombatStats stats, bool partySide)
        {
            if (partySide)
            {
                List<TargetSlotInfo> slots = [];

                foreach (CombatSlot slot in stats.combatSlots.CharacterSlots)
                {
                    if (slot.HasUnit) slots.Add(slot.TargetSlotInformation);
                }

                if (slots.Count > 0) return slots.GetRandom();
                return null;
            }
            else
            {
                List<TargetSlotInfo> slots = [];

                foreach (CombatSlot slot in stats.combatSlots.EnemySlots)
                {
                    if (slot.HasUnit) slots.Add(slot.TargetSlotInformation);
                }

                if (slots.Count > 0) return slots.GetRandom();
                return null;
            }
        }

    }
}
