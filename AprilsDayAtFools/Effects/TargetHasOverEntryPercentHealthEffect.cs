using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class TargetHasOverEntryPercentHealthEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit && (target.Unit.CurrentHealth * 100)/target.Unit.MaximumHealth > entryVariable) exitAmount++;
            }
            return exitAmount > 0;
        }
    }
}
