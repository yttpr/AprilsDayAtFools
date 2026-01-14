using MonoMod.RuntimeDetour;
using System;
using System.Reflection;

namespace AprilsDayAtFools
{
    public static class DeterminedHandler
    {
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(IsAliveEffectorCondition).GetMethod(nameof(IsAliveEffectorCondition.MeetCondition), ~BindingFlags.Default), typeof(DeterminedHandler).GetMethod(nameof(IsAliveEffectorCondition_MeetCondition), ~BindingFlags.Default));
        }
        public static bool IsAliveEffectorCondition_MeetCondition(Func<IsAliveEffectorCondition, IEffectorChecks, object, bool> orig, IsAliveEffectorCondition self, IEffectorChecks effector, object args)
        {
            bool ret = orig(self, effector, args);
            if (!ret)
            {
                if (effector is IUnit unit)
                {
                    int determined = unit.GetStatusAmount("Determined_ID", true);
                    if (determined > 0 && unit.CanHeal(true, determined)) return true;
                }
            }
            return ret;
        }
    }
}
