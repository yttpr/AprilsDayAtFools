using MonoMod.RuntimeDetour;
using System;
using System.Reflection;

namespace AprilsDayAtFools
{
    public static class SolitudeHandler
    {
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(FieldEffect_SO).GetMethod(nameof(FieldEffect_SO.ReduceDuration), ~BindingFlags.Default), typeof(SolitudeHandler).GetMethod(nameof(ReduceDuration_OnFireFE_SO), ~BindingFlags.Default));
        }
        public static void ReduceDuration_OnFireFE_SO(Action<FieldEffect_SO, FieldEffect_Holder> orig, FieldEffect_SO self, FieldEffect_Holder holder)
        {
            if (self.FieldID != StatusField_GameIDs.OnFire_ID.ToString())
            {
                orig(self, holder);
                return;
            }
            if (holder.Effector is CombatSlot slot)
            {
                if (slot.HasUnit && slot.Unit.ContainsPassiveAbility(IDs.Solitude))
                {
                    if (!CombatManager.Instance._stats.IsPassiveLocked(IDs.Solitude)) return;
                }
            }
            orig(self, holder);
        }
    }
}
