using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CasterReduceHealthEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            int orig = caster.CurrentHealth;
            int num = caster.CurrentHealth - entryVariable;
            if (num <= 1) caster.SetHealthTo(1);
            else caster.SetHealthTo(num);
            exitAmount = orig - caster.CurrentHealth;
            return exitAmount > 0;
        }
    }
}
