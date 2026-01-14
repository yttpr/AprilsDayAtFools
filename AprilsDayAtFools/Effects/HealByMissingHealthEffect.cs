using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class HealByMissingHealthEffect : HealEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            return base.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable + Math.Max(caster.MaximumHealth - caster.CurrentHealth, 0), out exitAmount);
        }
    }
}
