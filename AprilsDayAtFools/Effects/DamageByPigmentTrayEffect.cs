using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class DamageByPigmentTrayEffect : DamageEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            int num = 0;
            foreach (ManaBarSlot slots in stats.MainManaBar.ManaBarSlots)
            {
                if (slots.IsEmpty) continue;
                num++;
            }
            return base.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable + num, out exitAmount);
        }
    }
}
