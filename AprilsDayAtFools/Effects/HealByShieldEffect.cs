using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class HealByShieldEffect : HealEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                int num = target.GetFieldAmount("Shield_ID", true);
                if (base.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable * num, out int exi)) exitAmount += exi;
            }
            return exitAmount > 0;
        }
    }
}
