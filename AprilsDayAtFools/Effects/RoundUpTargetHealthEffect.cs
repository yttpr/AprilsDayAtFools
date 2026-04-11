using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class RoundUpTargetHealthEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    int start = target.Unit.CurrentHealth;
                    if (start >= entryVariable) continue;

                    if (target.Unit.SetHealthTo(entryVariable))
                    {
                        exitAmount += target.Unit.CurrentHealth - start;
                    }
                }
            }

            return exitAmount > 0;
        }
    }
}
