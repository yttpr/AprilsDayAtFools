using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools.Effects
{
    public class CasterChangeMultiStoredValueEffect : CasterStoredValueChangeEffect
    {
        public string[] ValueNames;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (ValueNames == null) return false;

            foreach (string value in ValueNames)
            {
                m_unitStoredDataID = value;

                base.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable, out int exi);
                exitAmount += exi;
            }

            return true;
        }
    }
}
