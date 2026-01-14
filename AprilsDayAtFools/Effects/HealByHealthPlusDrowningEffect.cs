using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class HealByHealthPlusDrowningEffect : HealEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    base.PerformEffect(stats, caster, [target], areTargetSlots, target.Unit.CurrentHealth + target.Unit.GetStatusAmount(Drowning.StatusID, true) + entryVariable, out int exi);
                    exitAmount += exi;
                }
            }

            return exitAmount > 0;
        }
    }
}
