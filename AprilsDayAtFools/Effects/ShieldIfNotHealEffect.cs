using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

namespace AprilsDayAtFools
{
    public class ShieldIfNotHealEffect : HealEffect
    {
        public ApplyShieldSlotEffect Shield;
        public int Amount;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (base.PerformEffect(stats, caster, [target], areTargetSlots, entryVariable, out int exi))
                {
                    exitAmount += exi;
                }
                else
                {
                    Shield.PerformEffect(stats, caster, [target], areTargetSlots, Amount, out int exits);
                }
            }
            return exitAmount > 0;
        }

        public static ShieldIfNotHealEffect Create(int shield, ApplyShieldSlotEffect effect = null)
        {
            ShieldIfNotHealEffect ret = ScriptableObject.CreateInstance<ShieldIfNotHealEffect>();
            ret.Shield = effect != null ? effect : ScriptableObject.CreateInstance<ApplyShieldSlotEffect>();
            ret.Amount = shield;
            return ret;
        }
    }
}
