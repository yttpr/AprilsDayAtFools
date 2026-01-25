using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class DamageTargetsSharingCasterHealthColorEffect : DamageEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            List<TargetSlotInfo> ret = [];
            foreach (TargetSlotInfo target in targets)
            {
                if (!target.HasUnit) continue;
                if (target.Unit.HealthColor.SharesPigmentColor(caster.HealthColor))
                    ret.Add(target);
            }

            return base.PerformEffect(stats, caster, ret.ToArray(), areTargetSlots, entryVariable, out exitAmount);
        }
    }
}
