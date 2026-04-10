using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    internal class DistributeHealingEffect : HealEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            List<TargetSlotInfo> valid = [];

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit) valid.Add(target);
            }
            int count = valid.Count;

            float use_still = entryVariable;
            
            for (int i = 0; i < count; i++)
            {
                if (valid.Count <= 0 || use_still <= 0f) break;

                int num2 = Mathf.CeilToInt(use_still / (float)valid.Count);
                int index = Random.Range(0, valid.Count);
                TargetSlotInfo unit = valid[index];
                valid.RemoveAt(index);
                
                if (base.PerformEffect(stats, caster, [unit], areTargetSlots, num2, out int exi))
                {
                    exitAmount += exi;
                }
                
                use_still -= (float)num2;
            }

            return exitAmount > 0;
        }
    }
}
