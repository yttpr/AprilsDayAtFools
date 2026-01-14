using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CasterResetStoredValueIfPastEntryEffect : EffectSO
    {
        public string ValueName;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = caster.SimpleGetStoredValue(ValueName);
            if (exitAmount >= entryVariable) caster.SimpleSetStoredValue(ValueName, 0);
            else exitAmount = 0;
            return exitAmount > 0;
        }
    }
}
