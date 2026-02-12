using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CheckMultiplePassivesEffect : EffectSO
    {
        [PassiveIDsEnumRef]
        public string[] m_PassiveIDs = [];

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit)
                {
                    foreach (string passive in m_PassiveIDs)
                    {
                        if (targetSlotInfo.Unit.ContainsPassiveAbility(passive)) exitAmount++;
                    }
                }
            }

            return exitAmount > 0;
        }
    }
}
