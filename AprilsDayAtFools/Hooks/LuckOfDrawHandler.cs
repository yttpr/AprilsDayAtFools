using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AprilsDayAtFools
{
    public static class LuckOfDrawHandler
    {
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(CharacterCombat).GetMethod(nameof(CharacterCombat.DefaultPassiveAbilityInitialization), ~BindingFlags.Default), typeof(LuckOfDrawHandler).GetMethod(nameof(DefaultPassiveAbilityInitialization_CharacterCombat), ~BindingFlags.Default));
            NotificationHook.AddAction(NotifCheck);
        }
        public static void DefaultPassiveAbilityInitialization_CharacterCombat(Action<CharacterCombat> orig, CharacterCombat self)
        {
            orig(self);
            for (int i = 0; i < self._combatAbilities.Count; i++)
            {
                if (self._combatAbilities[i].ability.name == IDs.Luck)
                {
                    CombatAbility ability = GetRandomCharAbil(self.Rank);
                    if (ability == null) continue;
                    else self._combatAbilities[i] = ability;
                }
            }
        }
        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == IDs.Initialize && sender is CharacterCombat self)
            {
                for (int i = 0; i < self._combatAbilities.Count; i++)
                {
                    if (self._combatAbilities[i].ability.name == IDs.Luck)
                    {
                        CombatAbility ability = GetRandomCharAbil(self.Rank);
                        if (ability == null) continue;
                        else self._combatAbilities[i] = ability;
                    }
                }
                CombatManager.Instance.AddUIAction(new CharacterUpdateAllAttacksUIAction(self.ID, self.CombatAbilities.ToArray()));
            }
        }
        public static CombatAbility GetRandomCharAbil(int rank, int loop = 0)
        {
            if (loop > 99) return null;
            CharacterSO chara = new List<CharacterSO>(LoadedAssetsHandler.LoadedCharacters.Values).GetRandom();
            if (chara == null || !chara.HasRankedData || chara.rankedData.Count <= 0) return GetRandomCharAbil(rank, loop++);
            CharacterRankedData data = chara.rankedData[chara.ClampRank(rank)];
            if (data.rankAbilities.Length <= 0) return GetRandomCharAbil(rank, loop++);
            return new CombatAbility(data.rankAbilities.GetRandom());
        }
    }
}
