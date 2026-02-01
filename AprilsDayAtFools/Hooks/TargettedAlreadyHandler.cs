using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class TargettedAlreadyHandler
    {
        public static Dictionary<int, List<int>> Fools;
        public static Dictionary<int, List<int>> Enemies;
        public static int TargettedAlreadyHandler_StartEffect(Func<EffectInfo, CombatStats, IUnit, TargetSlotInfo[], bool, int, int> orig, EffectInfo self, CombatStats stats, IUnit caster, TargetSlotInfo[] possibleTargets, bool areTargetSlots, int previousExitValue)
        {
            if (caster != null && possibleTargets != null)
            {
                foreach (TargetSlotInfo target in possibleTargets)
                {
                    if (caster.IsUnitCharacter == target.IsTargetCharacterSlot) continue;

                    AddSlot(caster, target.SlotID);
                    if (target.HasUnit && !areTargetSlots)
                    {
                        for (int i = 0; i < target.Unit.Size; i++)
                        {
                            AddSlot(caster, target.Unit.SlotID + i);
                        }
                    }
                }
            }
            return orig(self, stats, caster, possibleTargets, areTargetSlots, previousExitValue);
        }
        public static void AddSlot(IUnit caster, int slot)
        {
            Dictionary<int, List<int>> addTo = caster.IsUnitCharacter ? Fools : Enemies;
            if (!addTo.ContainsKey(caster.ID) || addTo[caster.ID] == null)
                addTo[caster.ID] = [];
            if (!addTo[caster.ID].Contains(slot))
                addTo[caster.ID].Add(slot);
        }
        public static bool CheckSlot(IUnit caster, int slot)
        {
            if (caster == null) return false;
            Dictionary<int, List<int>> addTo = caster.IsUnitCharacter ? Fools : Enemies;
            if (!addTo.ContainsKey(caster.ID) || addTo[caster.ID] == null)
                return false;
            return addTo[caster.ID].Contains(slot);
        }

        public static void Setup()
        {
            IDetour hook1 = new Hook(typeof(EffectInfo).GetMethod(nameof(EffectInfo.StartEffect), ~BindingFlags.Default), typeof(TargettedAlreadyHandler).GetMethod(nameof(TargettedAlreadyHandler_StartEffect), ~BindingFlags.Default));
            Fools = new Dictionary<int, List<int>>();
            Enemies = new Dictionary<int, List<int>>();
        }
    }

    public class TargettedAlreadyControllerEffect : EffectSO
    {
        public EffectSO Effect;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            List<TargetSlotInfo> ret = [];
            foreach (TargetSlotInfo target in targets)
            {
                if (target.IsTargetCharacterSlot == caster.IsUnitCharacter) continue;

                bool add = false;

                if (TargettedAlreadyHandler.CheckSlot(caster, target.SlotID)) add = true;

                if (!areTargetSlots && target.HasUnit)
                {
                    for (int i = 0; i < target.Unit.Size; i++)
                    {
                        if (TargettedAlreadyHandler.CheckSlot(caster, target.Unit.SlotID + i)) add = true;
                    }
                }

                if (add) ret.Add(target);
            }

            return Effect.PerformEffect(stats, caster, ret.ToArray(), areTargetSlots, entryVariable, out exitAmount);
        }

        public static TargettedAlreadyControllerEffect Create(EffectSO effect)
        {
            TargettedAlreadyControllerEffect ret = ScriptableObject.CreateInstance<TargettedAlreadyControllerEffect>();
            ret.Effect = effect;
            return ret;
        }
    }
    public class TargettedAlreadyTargetting : BaseCombatTargettingSO
    {
        public override bool AreTargetAllies => false;
        public override bool AreTargetSlots => true;
        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            List<TargetSlotInfo> ret = [];

            IUnit caster = null;
            if (isCasterCharacter && slots.CharacterSlots[casterSlotID].HasUnit) caster = slots.CharacterSlots[casterSlotID].Unit;
            if (!isCasterCharacter && slots.EnemySlots[casterSlotID].HasUnit) caster = slots.EnemySlots[casterSlotID].Unit;

            if (caster == null) return ret.ToArray();

            foreach (CombatSlot slot in isCasterCharacter ? slots.EnemySlots : slots.CharacterSlots)
            {
                if (TargettedAlreadyHandler.CheckSlot(caster, slot.SlotID))
                    ret.Add(slot.TargetSlotInformation);
            }

            return ret.ToArray();
        }
    }
    public class TargettedAlreadyVisualsEffect : EffectSO
    {
        public AttackVisualsSO _visuals;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            List<TargetSlotInfo> ret = [];

            foreach (CombatSlot slot in caster.IsUnitCharacter ? stats.combatSlots.EnemySlots : stats.combatSlots.CharacterSlots)
            {
                if (TargettedAlreadyHandler.CheckSlot(caster, slot.SlotID))
                    ret.Add(slot.TargetSlotInformation);
            }

            CombatManager.Instance.AddUIAction(new PlayAnimationAnywhereAction(_visuals, ret.ToArray()));

            exitAmount = ret.Count;
            return exitAmount > 0;
        }

        public static TargettedAlreadyVisualsEffect Create(AttackVisualsSO visuals)
        {
            TargettedAlreadyVisualsEffect ret = ScriptableObject.CreateInstance<TargettedAlreadyVisualsEffect>();
            ret._visuals = visuals;
            return ret;
        }
    }
}
