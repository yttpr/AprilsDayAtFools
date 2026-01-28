using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools.Effects
{
    public class MaskedDelayedAttackStoredValueEffect : MaskedAddDelayedAttackEffect
    {
        public string ValueName;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            int mod = caster.SimpleGetStoredValue(ValueName);
            if (entryVariable + mod <= 0) return false;

            return base.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable + mod, out exitAmount);
        }
    }
}
