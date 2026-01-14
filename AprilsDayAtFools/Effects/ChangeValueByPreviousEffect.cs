using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class ChanageValueByPreviousEffect : CasterStoredValueChangeEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            return base.PerformEffect(stats, caster, targets, areTargetSlots, PreviousExitValue, out exitAmount);
        }
        public static ChanageValueByPreviousEffect Create(string value, bool increase, int min = 0)
        {
            ChanageValueByPreviousEffect ret = ScriptableObject.CreateInstance<ChanageValueByPreviousEffect>();
            ret.m_unitStoredDataID = value;
            ret._increase = increase;
            ret._minimumValue = min;
            return ret;
        }
    }
}
