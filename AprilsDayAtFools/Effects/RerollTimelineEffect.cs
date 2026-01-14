using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class RerollTimelineEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = stats.timeline.TryReRollAnyRandomTurns(stats.timeline.Round.Count);
            return exitAmount > 0;
        }
    }
}
