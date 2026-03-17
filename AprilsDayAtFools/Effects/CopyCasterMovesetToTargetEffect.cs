using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CopyCasterMovesetToTargetEffect : EffectSO
    {
        public RaritySO BasicRarity;
        public static ManaColorSO[] GenerateBasicCost(int length)
        {
            ManaColorSO[] first = [Pigments.Red, Pigments.Yellow, Pigments.Blue];
            ManaColorSO[] second = [Pigments.Blue, Pigments.Yellow, Pigments.Purple];
            List<ManaColorSO> ret = [];
            ret.Add(second.GetRandom());
            for (int i = 0; i < length - 1; i++) ret.Add(first.GetRandom());
            return ret.ToArray();
        }

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            if (BasicRarity == null)
            {
                BasicRarity = Rarity.CreateAndAddCustomRarityToPool("CopyCasterMovesetToTargetEffect_5", 5);
            }

            List<CombatAbility> abils = [];
            if (caster is CharacterCombat chara) abils = chara.CombatAbilities;
            if (caster is EnemyCombat enemy) abils = enemy.Abilities;

            foreach (CombatAbility abil in abils)
            {
                if (abil.cost == null) abil.cost = GenerateBasicCost(UnityEngine.Random.Range(2, 4));
                if (abil.rarity == null || abil.rarity.Equals(null)) abil.rarity = BasicRarity;
            }

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    if (target.Unit is CharacterCombat character)
                    {
                        character.CombatAbilities.AddRange(abils);
                        exitAmount += abils.Count;

                        CombatManager.Instance.AddUIAction(new CharacterUpdateAllAttacksUIAction(character.ID, character.CombatAbilities.ToArray()));

                        foreach (CombatAbility c in abils)
                        {
                            CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(character.ID, character.IsUnitCharacter, c.ability.GetAbilityLocData().text, c.ability.abilitySprite)); ;
                        }
                    }
                    else if (target.Unit is EnemyCombat enemie)
                    {
                        enemie.Abilities.AddRange(abils);
                        exitAmount += abils.Count;

                        CombatManager.Instance.AddUIAction(new EnemyUpdateAllAttacksUIAction(enemie.ID, enemie.Abilities.ToArray()));

                        foreach (CombatAbility c in abils)
                        {
                            CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(enemie.ID, enemie.IsUnitCharacter, c.ability.GetAbilityLocData().text, c.ability.abilitySprite)); ;
                        }
                    }
                }
            }

            return exitAmount > 0;
        }
    }
}
