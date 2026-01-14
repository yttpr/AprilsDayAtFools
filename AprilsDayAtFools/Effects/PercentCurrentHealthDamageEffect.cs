using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class PercentCurrentHealthDamageEffect : DamageEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    if (base.PerformEffect(stats, caster, targets, areTargetSlots, Math.Max(1, (int)Math.Floor((float)target.Unit.CurrentHealth / entryVariable)), out int exi)) exitAmount += exi;
                }
            }
            return exitAmount > 0;
        }
    }
}
