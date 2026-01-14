using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AprilsDayAtFools
{
    public static class PauseHook
    {
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(CombatManager).GetMethod(nameof(CombatManager.OnPauseTriggered), ~BindingFlags.Default), typeof(PauseHook).GetMethod(nameof(CombatManager_OnPauseTriggered), ~BindingFlags.Default));
        }
        public static TriggerCalls Trigger => (TriggerCalls)87112234;
        public static void CombatManager_OnPauseTriggered(Action<CombatManager, object, object> orig, CombatManager self, object sender, object args)
        {
            if (args is BooleanReference value && value.value)
            {
                foreach (EnemyCombat enemy in CombatManager.Instance._stats.EnemiesOnField.Values) enemy.TriggerNotification(Trigger.ToString(), null);
                foreach (CharacterCombat chara in CombatManager.Instance._stats.CharactersOnField.Values) chara.TriggerNotification(Trigger.ToString(), null);
            }

            orig(self, sender, args);
        }
    }
}
