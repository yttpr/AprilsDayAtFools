using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class DamageNotAssistantsEffect : DamageEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            List<TargetSlotInfo> ret = new List<TargetSlotInfo>();
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit && target.Unit is EnemyCombat enemy && enemy.Enemy.name.Contains("Assistant"))
                {
                    target.Unit.Damage(0, caster, "Basic", (areTargetSlots ? (target.SlotID - target.Unit.SlotID) : (-1)), true, true, false);
                    continue;
                }
                ret.Add(target);
            }

            return base.PerformEffect(stats, caster, ret.ToArray(), areTargetSlots, entryVariable, out exitAmount);
        }
    }
}
