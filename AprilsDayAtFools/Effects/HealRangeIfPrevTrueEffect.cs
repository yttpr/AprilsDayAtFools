using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class HealRangeIfPrevHasExitEffect : HealEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            //fail!
            return base.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable, out exitAmount);
        }
    }
}
