using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ApplyAcidByPreviousEffect : ApplyAcidEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            return base.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable * PreviousExitValue, out exitAmount);
        }
    }
    
}
