using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class RefreshIfStatusOverEntryEffect : EffectSO
    {
        public string Status;
        public bool Swap_Also;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit && target.Unit.ContainsStatusEffect(Status, entryVariable))
                {
                    exitAmount++;
                    target.Unit.RefreshAbilityUse();
                    if (Swap_Also) target.Unit.RestoreSwapUse();
                }
            }

            return exitAmount > 0;
        }
    }
}
