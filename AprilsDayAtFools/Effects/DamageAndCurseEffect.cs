using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class DamageAndCurseEffect : DamageEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            int num = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    target.Unit.ApplyStatusEffect(StatusField.Cursed, 1);
                    num++;
                }
            }
            base.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable, out exitAmount);
            return num > 0;
        }
    }
}
