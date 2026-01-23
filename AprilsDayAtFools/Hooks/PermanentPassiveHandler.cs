using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class PermanentPassiveHandler
    {
        public static Sprite Icon;
        public static void Setup()
        {
            Icon = ResourceLoader.LoadSprite("PermanentPassive.png");
            IDetour hook1 = new Hook(typeof(CharacterCombat).GetMethod(nameof(CharacterCombat.MaximizeHealth), ~BindingFlags.Default), typeof(PermanentPassiveHandler).GetMethod(nameof(IUnit_MaximizeHealth), ~BindingFlags.Default));
            IDetour hook2 = new Hook(typeof(EnemyCombat).GetMethod(nameof(EnemyCombat.MaximizeHealth), ~BindingFlags.Default), typeof(PermanentPassiveHandler).GetMethod(nameof(IUnit_MaximizeHealth), ~BindingFlags.Default));
        }
        public static bool IUnit_MaximizeHealth(Func<IUnit, int, bool> orig, IUnit self, int amount)
        {
            if (self.ContainsPassiveAbility(IDs.Permanent))
            {
                CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(self.ID, self.IsUnitCharacter, "Permanent", Icon));
                return false;
            }
            return orig(self, amount);
        }
    }
}
