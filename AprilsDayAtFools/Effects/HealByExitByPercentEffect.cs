using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class HealByExitByPercentEffect : HealEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            float mod = entryVariable;
            int num = (int)Math.Ceiling((mod * PreviousExitValue) / 100);
            return base.PerformEffect(stats, caster, targets, areTargetSlots, num, out exitAmount);
        }
    }
}
