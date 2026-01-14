using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class DamageIncreaseIfTargetAboveHalfEffect : DamageEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                float num = entryVariable;
                if (target.HasUnit && 100 * (float)target.Unit.CurrentHealth / target.Unit.MaximumHealth >= 50) num = (float)Math.Ceiling(num * 1.5f);
                if (base.PerformEffect(stats, caster, target.SelfArray(), areTargetSlots, (int)num, out int exi)) exitAmount += exi;
            }
            return exitAmount > 0;
        }
    }
}
