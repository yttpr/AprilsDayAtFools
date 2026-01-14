using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CasterPerformRandomTargetAbilityEffect : EffectSO
    {
        public static void TryPerformGivenAbility(IUnit unit, AbilitySO selectedAbility)
        {
            CombatManager.Instance.AddSubAction(new ShowAttackInformationUIAction(unit.ID, unit.IsUnitCharacter, selectedAbility.GetAbilityLocData().text));
            CombatManager.Instance.AddSubAction(new PlayAbilityAnimationAction(selectedAbility.visuals, selectedAbility.animationTarget, unit));
            CombatManager.Instance.AddSubAction(new SafetyEffectAction(selectedAbility.effects, unit));
            unit.SetVolatileUpdateUIAction();
        }
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    if (target.Unit.AbilityCount <= 0) continue;
                    for (int i = 0; i < entryVariable; i++)
                    {
                        if (target.Unit is EnemyCombat enemy)
                        {
                            TryPerformGivenAbility(caster, enemy.Abilities.GetRandom().ability);
                            exitAmount++;
                        }
                        else if (target.Unit is CharacterCombat chara)
                        {
                            TryPerformGivenAbility(caster, chara.CombatAbilities.GetRandom().ability);
                            exitAmount++;
                        }
                    }
                }
            }

            return exitAmount > 0;
        }
    }
}
