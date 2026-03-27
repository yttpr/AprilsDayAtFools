using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{

    public class InvertTargetHealthEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    int temp = target.Unit.MaximumHealth - target.Unit.CurrentHealth;

                    exitAmount += Math.Abs(temp - target.Unit.CurrentHealth);

                    if (temp <= 0) target.Unit.GenericDirectDeath(caster);
                    else if (temp != target.Unit.CurrentHealth) target.Unit.SetHealthTo(temp);
                }
            }
            return exitAmount > 0;
        }
    }
}
