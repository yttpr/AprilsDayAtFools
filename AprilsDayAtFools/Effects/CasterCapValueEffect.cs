using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CasterCapValueEffect : EffectSO
    {
        public string value;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = caster.SimpleGetStoredValue(value);
            if (exitAmount >= entryVariable) exitAmount = entryVariable;
            caster.SimpleSetStoredValue(value, exitAmount);
            return exitAmount == entryVariable;
        }
    }
}
