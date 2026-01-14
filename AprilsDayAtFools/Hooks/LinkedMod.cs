using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AprilsDayAtFools
{
    public static class LinkedMod
    {
        public static void Setup()
        {
            IDetour hook1 = new Hook(typeof(LinkedSE_SO).GetMethod(nameof(LinkedSE_SO.OnEventCall_02), ~BindingFlags.Default), typeof(LinkedMod).GetMethod(nameof(LinkedSE_SO_OnEventCall_02), ~BindingFlags.Default));
            //IDetour hook2 = new Hook(typeof(CatalystPassiveAbility).GetMethod(nameof(CatalystPassiveAbility.TriggerPassive), ~BindingFlags.Default), typeof(LinkedMod).GetMethod(nameof(CatalystPassiveAbility_TriggerPassive), ~BindingFlags.Default));
        }
        public static void CatalystPassiveAbility_TriggerPassive(Action<CatalystPassiveAbility, object, object> orig, CatalystPassiveAbility self, object sender, object args)
        {
            //doesnt work because passives trigger by triggering a subaction to trigger TriggerPassive
            if (sender is IUnit unit && unit.SimpleGetStoredValue(IDs.DirectHealing) > 0)
            {
                if (unit.SimpleGetStoredValue(IDs.DirectHealAmount) <= 0)
                    CombatManager.Instance.AddSubAction(new PerformLinkedEffectAction(sender, args as IntegerReference, dealsDamage: false, self._LinkedStatus.StatusID, self._dmgTypeID));
                return;
            }
            orig(self, sender, args);
        }
        public static void LinkedSE_SO_OnEventCall_02(Action<LinkedSE_SO, StatusEffect_Holder, object, object> orig, LinkedSE_SO self, StatusEffect_Holder holder, object sender, object args)
        {
            if (sender is IUnit unit && unit.SimpleGetStoredValue(IDs.DirectHealing) > 0)
            {
                return;

                if (unit.ContainsPassiveAbility("Catalyst")) return;

                if (unit.SimpleGetStoredValue(IDs.DirectHealAmount) <= 0)
                    CombatManager.Instance.AddSubAction(new PerformLinkedEffectAction(holder.m_ObjectData, new IntegerReference(unit.SimpleGetStoredValue(IDs.DirectHealAttempt)), dealsDamage: false, self.StatusID, self.healTypeID));
                return;
            }
            orig(self, holder, sender, args);
        }
    }
}
