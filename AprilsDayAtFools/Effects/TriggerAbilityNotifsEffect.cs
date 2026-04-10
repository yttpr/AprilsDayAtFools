using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class TriggerAnyAbilityNotifsEffect : EffectSO
    {
        public AbilitySO abil;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            if (abil == null || abil.Equals(null))
            {
                Ability empty = new Ability("Empty", "Empty_Ability_A");
                empty.GenerateEnemyAbility();
                abil = empty.ability;
            }

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    AbilityUsageReference _abilityReference = new AbilityUsageReference(target.Unit.ID, target.Unit.IsUnitCharacter, abil);

                    foreach (CharacterCombat value in stats.CharactersOnField.Values)
                    {
                        value.AnyAbilityHasFinished(_abilityReference);
                    }

                    foreach (EnemyCombat value2 in stats.EnemiesOnField.Values)
                    {
                        value2.AnyAbilityHasFinished(_abilityReference);
                    }

                    exitAmount++;
                }
            }

            return exitAmount > 0;
        }
    }
}
