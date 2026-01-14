using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AprilsDayAtFools
{
    public static class ObliterateHook
    {
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(CombatVisualizationController).GetMethod(nameof(CombatVisualizationController.TrySetCharacterDeathStatus), ~BindingFlags.Default), typeof(ObliterateHook).GetMethod(nameof(CombatVisualizationController_TrySetCharacterDeathStatus), ~BindingFlags.Default));
        }

        public static IEnumerator CombatVisualizationController_TrySetCharacterDeathStatus(Func<CombatVisualizationController, int, bool, bool, IEnumerator> orig, CombatVisualizationController self, int id, bool playDeathSound, bool obliteration)
        {
            foreach (CharacterCombat chara in CombatManager.Instance._stats.Characters.Values)
            {
                if (chara.ID == id && chara.HasUsableItem && chara.HeldItem.name == "Aprils_ThePeopleOfPaper_TW") obliteration = true;
            }

            return orig(self, id, playDeathSound, obliteration);
        }
    }
}
