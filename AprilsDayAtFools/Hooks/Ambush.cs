using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AprilsDayAtFools
{

    public static class AmbushManager
    {
        public static TriggerCalls Trigger = IDs.AmbushCall;

        public static void PostNotif(IUnit unit)
        {
            foreach (TargetSlotInfo target in Targeting.Slot_Front.GetTargets(CombatManager.Instance._stats.combatSlots, unit.SlotID, unit.IsUnitCharacter))
            {
                if (target.HasUnit)
                {
                    CombatManager.Instance.PostNotification(Trigger.ToString(), target.Unit, null);
                }
            }
        }

        public static void EnemySwap(Action<EnemyCombat, int> orig, EnemyCombat self, int slotID)
        {
            orig(self, slotID);
            PostNotif(self);

        }
        public static void CharacterSwap(Action<CharacterCombat, int> orig, CharacterCombat self, int slotID)
        {
            orig(self, slotID);
            PostNotif(self);
        }

        public static void Setup()
        {
            IDetour EnemySwapTo = new Hook((MethodBase)typeof(EnemyCombat).GetMethod(nameof(EnemyCombat.SwapTo), ~BindingFlags.Default), typeof(AmbushManager).GetMethod(nameof(EnemySwap), ~BindingFlags.Default));
            IDetour EnemySwappedTo = new Hook((MethodBase)typeof(EnemyCombat).GetMethod(nameof(EnemyCombat.SwappedTo), ~BindingFlags.Default), typeof(AmbushManager).GetMethod(nameof(EnemySwap), ~BindingFlags.Default));
            IDetour CharaSwapTo = new Hook((MethodBase)typeof(CharacterCombat).GetMethod(nameof(CharacterCombat.SwapTo), ~BindingFlags.Default), typeof(AmbushManager).GetMethod(nameof(CharacterSwap), ~BindingFlags.Default));
            IDetour CharaSwappedTo = new Hook((MethodBase)typeof(CharacterCombat).GetMethod(nameof(CharacterCombat.SwappedTo), ~BindingFlags.Default), typeof(AmbushManager).GetMethod(nameof(CharacterSwap), ~BindingFlags.Default));
        }
    }
}
