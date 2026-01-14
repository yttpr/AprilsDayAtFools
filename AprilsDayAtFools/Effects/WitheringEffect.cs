using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class WitheringEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (caster.IsUnitCharacter)
            {
                CombatManager.Instance.AddSubAction(new CharacterWitheringAction());
            }
            else
            {
                CombatManager.Instance.AddSubAction(new EnemyWitheringAction());
            }
            return true;
        }
    }
}
