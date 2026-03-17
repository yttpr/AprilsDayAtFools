using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CalculateHealthAboveCasterHealthEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    int difference = target.Unit.CurrentHealth - caster.CurrentHealth;
                    if (difference <= 0) continue;

                    float result = difference / (float)entryVariable;

                    exitAmount += (int)Math.Floor(result);
                }
            }
            return exitAmount > 0;
        }
    }
}
