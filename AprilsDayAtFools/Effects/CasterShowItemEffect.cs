using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CasterShowItemEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            caster.ShowItem();
            exitAmount = 0;
            return true;
        }
    }
}
