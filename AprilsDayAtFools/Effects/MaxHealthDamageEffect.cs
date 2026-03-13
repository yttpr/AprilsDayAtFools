using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class MaxHealthDamageEffect : EffectSO
    {
        public bool dontKill;
        public bool returnKill;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            int kills = 0;

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    int start = target.Unit.MaximumHealth;
                    target.Unit.MaximizeHealth(start - entryVariable);
                    exitAmount += start - target.Unit.MaximumHealth;

                    if (!dontKill)
                    {
                        if (start <= entryVariable && start - target.Unit.MaximumHealth < entryVariable)
                        {
                            if (target.Unit.DirectDeath(caster, false, out int add))
                            {
                                exitAmount += add;
                                kills++;
                            }
                        }
                    }
                }
            }

            return returnKill ? kills > 0 : exitAmount > 0;
        }
    }
}
