using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class ReduceAllNegativeStatusEffect : EffectSO
    {
        [SerializeField]
        public List<string> Exclude = new List<string>();
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            entryVariable = -1 * Math.Abs(entryVariable);
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit)
                {
                    if (targetSlotInfo.Unit is IStatusEffector effector)
                    {
                        foreach (IStatusEffect status in new List<IStatusEffect>(effector.StatusEffects))
                        {
                            if (!status.IsPositive && !Exclude.Contains(status.StatusID))
                            {
                                if (status.StatusContent > Math.Abs(entryVariable))
                                {
                                    if (status.TryAddContent(entryVariable, 0))
                                    {
                                        effector.StatusEffectValuesChanged(status.StatusID, entryVariable, true);
                                        exitAmount += Math.Abs(entryVariable);
                                    }
                                }
                                else
                                {
                                    exitAmount += targetSlotInfo.Unit.TryRemoveStatusEffect(status.StatusID);
                                }
                            }
                        }
                    }
                }
            }
            return exitAmount > 0;
        }
    }
}
