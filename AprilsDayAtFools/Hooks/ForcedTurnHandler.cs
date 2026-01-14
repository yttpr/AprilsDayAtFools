using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Reflection;

namespace AprilsDayAtFools
{
    public static class ForcedTurnHandler
    {
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(EnemyDeathAction).GetMethod(nameof(EnemyDeathAction.Execute), ~BindingFlags.Default), typeof(ForcedTurnHandler).GetMethod(nameof(EnemyDeathAction_Execute), ~BindingFlags.Default));
        }
        public static IEnumerator EnemyDeathAction_Execute(Func<EnemyDeathAction, CombatStats, IEnumerator> orig, EnemyDeathAction self, CombatStats stats)
        {
            EnemyCombat enemy = stats.TryGetEnemyOnField(self._enemyID);
            if (enemy != null && enemy.SimpleGetStoredValue(IDs.ForcedTurn) > 0)
            {
                enemy.SimpleSetStoredValue(IDs.ForcedTurn, 0);
                CombatManager.Instance.AddRootAction(self);
                yield return null;
            }
            else yield return orig(self, stats);
        }
    }
}
