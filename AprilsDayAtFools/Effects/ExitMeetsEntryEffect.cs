using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ExitMeetsEntryEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = PreviousExitValue;
            return PreviousExitValue >= entryVariable;
        }
    }
}
