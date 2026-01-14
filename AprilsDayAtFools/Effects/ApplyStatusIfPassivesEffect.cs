using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ApplyStatusIfPassivesEffect : StatusEffect_Apply_Effect
    {
        public string[] Passives;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    bool DoIt = false;
                    foreach (string id in Passives)
                    {
                        if (target.Unit.ContainsPassiveAbility(id)) DoIt = true;
                    }

                    if (DoIt)
                    {
                        base.PerformEffect(stats, caster, [target], areTargetSlots, entryVariable, out int exi);
                        exitAmount += exi;
                    }
                }
            }
            return exitAmount > 0;
        }
    }
}
