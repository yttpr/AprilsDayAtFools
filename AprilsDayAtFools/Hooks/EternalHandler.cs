using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace AprilsDayAtFools
{
    public static class EternalHandler
    {
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(CombatStats).GetMethod(nameof(CombatStats.CombatEndTriggered), ~BindingFlags.Default), typeof(EternalHandler).GetMethod(nameof(CombatStats_FinalizeCombat), ~BindingFlags.Default));
        }
        public static void CombatStats_FinalizeCombat(Action<CombatStats> orig, CombatStats self)
        {
            foreach (CharacterCombat chara in self.Characters.Values)
            {
                if (!self.CharactersAlive) break;
                if (!chara.IsAlive && chara.ContainsPassiveAbility(IDs.Eternal) && !self.IsPassiveLocked(IDs.Eternal))
                {
                    if (self.ResurrectDeadCharacter(chara, EmptyCharSlot(self), 1))
                    {
                        CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(chara.ID, chara.IsUnitCharacter, "Eternal", ResourceLoader.LoadSprite("EternityPassive.png")));
                    }
                    else
                    {
                        chara.CurrentHealth = 1;
                        chara.IsAlive = true;
                        chara.HasFled = true;
                    }
                }
            }
            orig(self);
        }
        public static int EmptyCharSlot(CombatStats self)
        {
            return self.GetRandomCharacterSlot();
            for (int i = 0; i < 5; i++)
            {
                foreach (CharacterCombat chara in self.Characters.Values)
                {
                    if (chara.IsAlive && chara.SlotID == i) continue;
                }
                if (self.combatSlots.CharacterSlots[i].HasUnit) continue;
                return i;
            }
            return -1;
        }
    }
}
