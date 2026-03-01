using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class EffectChainPerTargetEffect : EffectSO
    {
        public EffectSO Primary;
        public EffectSO Secondary;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (Primary.PerformEffect(stats, caster, [target], areTargetSlots, entryVariable, out int exi))
                {
                    exitAmount += exi;
                    Secondary.PerformEffect(stats, caster, [target], areTargetSlots, exi, out int _);
                }
            }
            return exitAmount > 0;
        }
    }
}
