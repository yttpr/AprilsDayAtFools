using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
                    bool has = false;

                    if (target.Unit is CharacterCombat chara)
                    {
                        foreach (CombatAbility info in chara.CombatAbilities)
                        {
                            if (info.ability.name == _extraAbility.ability.name)
                            {
                                has = true;
                                break;
                            }
                        }
                    }
                    if (target.Unit is EnemyCombat enemy)
                    {
                        foreach (CombatAbility info in enemy.Abilities)
                        {
                            if (info.ability.name == _extraAbility.ability.name)
                            {
                                has = true;
                                break;
                            }
                        }
                    }

                    target.Unit.AddExtraAbility(_extraAbility);
                    exitAmount++;
                }
            }

            return exitAmount > 0;
        }

        public static AddExtraAbilityIfNotHaveEffect Create(CharacterAbility ability)
        {
            AddExtraAbilityIfNotHaveEffect ret = ScriptableObject.CreateInstance<AddExtraAbilityIfNotHaveEffect>();
            ret._extraAbility = new ExtraAbilityInfo(ability);
            return ret;
        }
    }
}
