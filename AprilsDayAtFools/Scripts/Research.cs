using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Tools;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace AprilsDayAtFools
{
    public static class Research
    {
        public static void Add()
        {
            //Foundling();
            ExcessNotificationHook.Setup();
        }
        public static void Foundling()
        {
            EnemySO foundling = LoadedAssetsHandler.GetEnemy("Foundling_Derelict_EN");
            if (foundling == null || foundling.Equals(null)) foundling = LoadedAssetsHandler.GetEnemy("DerelictFoundling_EN");

            foreach (EffectInfo info in foundling.enterEffects) Debug.Log(info.effect);

            CasterAddExtraAbilitySetFromPreviousEffect test = foundling.enterEffects[1].effect as CasterAddExtraAbilitySetFromPreviousEffect;
            Debug.Log("going through ABILITY SETS");
            foreach (IntListWrapper wrapper in test._AbilitySets)
            {
                string working = "";
                foreach (int num in wrapper.list)
                {
                    working += num.ToString();
                    working += ", ";
                }
                Debug.Log("int list wrapper: " + working);
            }
            Debug.Log("going through POOLS DATA");
            foreach (ExtraAbilityInfoListWrapper wrapper in test._PoolsData)
            {
                Debug.Log("NEW POOL DATA");
                foreach (ExtraAbilityInfo info in wrapper.list)
                {
                    Debug.Log(info.ability);
                }
            }
        }
    }


    public static class ExcessNotificationHook
    {
        public static void CalculateOverflow(Action<PlayerTurnEndSecondPartAction, CombatStats> orig, PlayerTurnEndSecondPartAction self, CombatStats stats)
        {
            bool startedInOverflow = stats.overflowMana.OverflowManaAmount > 0;
            orig(self, stats);
            if (startedInOverflow && stats.overflowMana.OverflowManaAmount <= 0)
            {
                Debug.Log("overflow should have triggered. check");
                foreach (CharacterCombat chara in CombatManager.Instance._stats.CharactersOnField.Values) CombatManager.Instance.PostNotification(OnExcessTriggered.ToString(), chara, null);
                foreach (EnemyCombat chara in CombatManager.Instance._stats.EnemiesOnField.Values) CombatManager.Instance.PostNotification(OnExcessTriggered.ToString(), chara, null);
            }
        }

        public static TriggerCalls OnExcessTriggered => (TriggerCalls)6682573;
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(PlayerTurnEndSecondPartAction).GetMethod(nameof(PlayerTurnEndSecondPartAction.CalculateOverflow), ~BindingFlags.Default), typeof(ExcessNotificationHook).GetMethod(nameof(CalculateOverflow), ~BindingFlags.Default));
        }
    }

}
