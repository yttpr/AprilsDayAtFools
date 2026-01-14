using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class HealDecreaseByStoredValueEffect : EffectSO
    {
        [UnitStoreValueNamesIDsEnumRef]
        public string m_unitStoredDataID = "";

        public int _defaultStoredValue;

        public bool _directHeal = true;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (!caster.TryGetStoredData(m_unitStoredDataID, out var holder))
            {
                holder.m_MainData = _defaultStoredValue;
            }

            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit)
                {
                    int amount = Math.Max(0, entryVariable - holder.m_MainData);
                    if (_directHeal)
                    {
                        amount = caster.WillApplyHeal(amount, targetSlotInfo.Unit);
                    }
                    exitAmount += targetSlotInfo.Unit.Heal(amount, caster, _directHeal);
                }
            }

            return exitAmount > 0;
        }
    }
}
