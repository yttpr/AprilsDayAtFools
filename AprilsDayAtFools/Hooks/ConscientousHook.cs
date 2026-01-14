using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AprilsDayAtFools
{
    public static class ConscientiousHandler
    {
        public static void Setup()
        {
            IDetour hook1 = new Hook(typeof(ConstrictedFE_SO).GetMethod(nameof(ConstrictedFE_SO.ReduceDuration), ~BindingFlags.Default), typeof(ConscientiousHandler).GetMethod(nameof(ReduceDuration_ConstrictedFE_SO), ~BindingFlags.Default));
            IDetour hook2 = new Hook(typeof(ConstrictedFE_SO).GetMethod(nameof(ConstrictedFE_SO.OnTriggerAttached), ~BindingFlags.Default), typeof(ConscientiousHandler).GetMethod(nameof(OnTrigger_ConstrictedFE_SO), ~BindingFlags.Default));
            IDetour hook3 = new Hook(typeof(ConstrictedFE_SO).GetMethod(nameof(ConstrictedFE_SO.OnTriggerDettached), ~BindingFlags.Default), typeof(ConscientiousHandler).GetMethod(nameof(OnTrigger_ConstrictedFE_SO), ~BindingFlags.Default));
        }
        public static void ReduceDuration_ConstrictedFE_SO(Action<ConstrictedFE_SO, FieldEffect_Holder> orig, ConstrictedFE_SO self, FieldEffect_Holder holder)
        {
            if (holder.Effector is CombatSlot slot)
            {
                if (slot.HasUnit && slot.Unit.ContainsPassiveAbility(IDs.Conscientious))
                {
                    if (!CombatManager.Instance._stats.IsPassiveLocked(IDs.Conscientious)) return;
                }
            }
            orig(self, holder);
        }
        public static void OnTrigger_ConstrictedFE_SO(Action<ConstrictedFE_SO, FieldEffect_Holder, IUnit> orig, ConstrictedFE_SO self, FieldEffect_Holder holder, IUnit caller)
        {
            if (caller.ContainsPassiveAbility(IDs.Conscientious))
            {
                if (!CombatManager.Instance._stats.IsPassiveLocked(IDs.Conscientious)) return;
            }
            orig(self, holder, caller);
        }
    }
}
