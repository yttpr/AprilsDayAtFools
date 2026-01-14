using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class HasStatusEffect : EffectSO
    {
        public string StatusID;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit && target.Unit.ContainsStatusEffect(StatusID)) exitAmount++; 
            }
            return exitAmount > 0;
        }
    
        public static HasStatusEffect Create(string status)
        {
            HasStatusEffect ret = ScriptableObject.CreateInstance<HasStatusEffect>();
            ret.StatusID = status;
            return ret;
        }
    }
}
