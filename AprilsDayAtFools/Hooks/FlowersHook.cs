using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class FlowersUnboxer : UnboxUnitHandlerSO
    {
        public static TriggerCalls Trigger => (TriggerCalls)3973519;
        public override bool CanBeUnboxed(CombatStats stats, BoxedUnit unit, object senderData)
        {
            return true;
        }
        public int ID;
        public static FlowersUnboxer GetDefault(int id)
        {
            FlowersUnboxer basic = ScriptableObject.CreateInstance<FlowersUnboxer>();
            basic._unboxConditions = [TriggerCalls.TimelineEndReached];
            basic.ID = id;
            return basic;
        }
        public static IEnumerator Execute(Func<FleetingUnitAction, CombatStats, IEnumerator> orig, FleetingUnitAction self, CombatStats stats)
        {
            bool flag = false;
            if (self._isUnitCharacter && !stats.IsPassiveLocked(IDs.Flowers))
            {
                CharacterCombat characterCombat = stats.TryGetCharacterOnField(self._unitID);
                if (characterCombat != null && characterCombat.CurrentHealth > 0)
                {
                    if (characterCombat != null && characterCombat.ContainsPassiveAbility(IDs.Flowers))
                    {
                        flag = true;
                        CombatManager.Instance.PostNotification(Trigger.ToString(), characterCombat, null);
                        stats.TryBoxCharacter(self._unitID, FlowersUnboxer.GetDefault(self._unitID), CombatType_GameIDs.Exit_Fleeting.ToString());
                    }
                }
            }
            if (flag) yield return null;
            else yield return orig(self, stats);
        }
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(FleetingUnitAction).GetMethod(nameof(FleetingUnitAction.Execute), ~BindingFlags.Default), typeof(FlowersUnboxer).GetMethod(nameof(Execute), ~BindingFlags.Default));
        }

    }
}
