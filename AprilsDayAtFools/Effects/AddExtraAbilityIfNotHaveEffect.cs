using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class AddExtraAbilityIfNotHaveEffect : EffectSO
    {
        public ExtraAbilityInfo _extraAbility;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    if (target.Unit is CharacterCombat chara)
                    {
                        foreach (ExtraAbilityInfo info in chara.ExtraAbilities)
                        {
                            if (info.ability.name == _extraAbility.ability.name) continue;
                        }
                    }
                    if (target.Unit is EnemyCombat enemy)
                    {
                        foreach (ExtraAbilityInfo info in enemy.ExtraAbilities)
                        {
                            if (info.ability.name == _extraAbility.ability.name) continue;
                        }
                    }

                    target.Unit.AddExtraAbility(_extraAbility);
                    exitAmount++;
                }
            }

            return exitAmount > 0;
        }
    }
}
