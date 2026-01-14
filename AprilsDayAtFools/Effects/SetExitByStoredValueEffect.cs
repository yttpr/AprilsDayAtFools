using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class SetExitByStoredValueEffect : EffectSO
    {
        public string ValueName;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (ValueName == "") return false;
            exitAmount = caster.SimpleGetStoredValue(ValueName);
            return exitAmount > 0;
        }
        public static SetExitByStoredValueEffect Create(string value)
        {
            SetExitByStoredValueEffect ret = ScriptableObject.CreateInstance<SetExitByStoredValueEffect>();
            ret.ValueName = value;
            return ret;
        }
    }
}
