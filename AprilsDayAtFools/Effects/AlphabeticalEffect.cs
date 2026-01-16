using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.InputSystem.XR.Haptics;

namespace AprilsDayAtFools
{
    public class AlphabeticalEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            TargetSlotInfo[] opponents = Slots.Front.GetTargets(stats.combatSlots, caster.SlotID, caster.IsUnitCharacter);
            List<AbilitySO> abilities = new List<AbilitySO>();
            foreach (TargetSlotInfo opponent in opponents)
            {
                if (opponent.HasUnit)
                {
                    if (opponent.Unit is EnemyCombat enemy)
                    {
                        foreach (CombatAbility abil in enemy.Abilities) abilities.Add(abil.ability);
                    }
                    else if (opponent.Unit is CharacterCombat chara)
                    {
                        foreach (CombatAbility abil in chara.CombatAbilities) abilities.Add(abil.ability);
                    }
                }
            }
            if (abilities.Count <= 0) return false;

            if (Rarity.GetCustomRarity("Alphabetical_5") == null) Rarity.CreateAndAddCustomRarityToPool("Alphabetical_5", 5);
            ManaColorSO[] first = [Pigments.Red, Pigments.Yellow, Pigments.Blue];
            ManaColorSO[] second = [Pigments.Blue, Pigments.Yellow, Pigments.Purple];

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    UnitStoreDataHolder holder;
                    target.Unit.TryGetStoredData("Alphabetical_A_" + caster.ID.ToString(), out holder);

                    if (holder.m_ObjectData is ExtraAbilityInfo info) target.Unit.TryRemoveExtraAbility(info);

                    ExtraAbilityInfo extra = new ExtraAbilityInfo();
                    extra.ability = abilities.GetRandom();
                    extra.rarity = Rarity.GetCustomRarity("Alphabetical_5");
                    extra.cost = [first.GetRandom(), second.GetRandom()];

                    target.Unit.AddExtraAbility(extra);

                    holder.m_ObjectData = extra;

                    exitAmount++;
                }
            }

            return exitAmount > 0;
        }
    }
}
