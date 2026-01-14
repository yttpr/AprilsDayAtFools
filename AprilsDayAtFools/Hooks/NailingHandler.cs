using JetBrains.Annotations;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Text;

//note for if this ever fucks up with the advanced damage trigger caller:
//alternative solution: set WillApplyHeal as the stored value
//hook IsAliveCondition like the determined handler for CanHeal(StoredValue)

namespace AprilsDayAtFools
{
    public static class NailingHandler
    {
        public static string Initial => "Nailing_A_Intitial";
        public static string Record => "Nailing_A_Record";
        public static string Direct => "Nailing_A_IsDirect";
        public static void PostNotification(Action<CombatManager, string, object, object> orig, CombatManager self, string notificationName, object sender, object args)
        {
            if (sender is IUnit unit)
            {
                if (notificationName == TriggerCalls.OnDirectDamaged.ToString()) unit.SimpleSetStoredValue(Direct, 1);
                else if (notificationName == TriggerCalls.OnIndirectDamaged.ToString()) unit.SimpleSetStoredValue(Direct, 0);

                if (notificationName == TriggerCalls.OnDamaged.ToString() || notificationName == TriggerCalls.OnDirectDamaged.ToString() || notificationName == TriggerCalls.OnIndirectDamaged.ToString())
                {
                    if (unit.SimpleGetStoredValue(Record) > 0) return;

                    if (unit.SimpleGetStoredValue(Initial) > 0)
                    {
                        unit.SimpleSetStoredValue(Initial, 0);
                        if (unit.CurrentHealth <= 0 && args is IntegerReference reference)
                        {
                            unit.SimpleSetStoredValue(Record, reference.value);
                            return;
                        }
                    }
                }
            }

            orig(self, notificationName, sender, args);
        }
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(CombatManager).GetMethod(nameof(CombatManager.PostNotification), ~BindingFlags.Default), typeof(NailingHandler).GetMethod(nameof(PostNotification), ~BindingFlags.Default));
        }
    }
}
