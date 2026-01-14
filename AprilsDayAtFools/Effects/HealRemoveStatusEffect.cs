using System;
using System.Collections.Generic;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class HealRemoveStatusEffect : EffectSO
    {
        public bool usePreviousExitValue;

        public bool entryAsPercentage;

        [SerializeField]
        public bool _directHeal = true;

        public bool _onlyIfHasHealthOver0;

        public bool _negativeOnly;

        public bool _useTypes;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (usePreviousExitValue)
            {
                entryVariable *= base.PreviousExitValue;
            }

            exitAmount = 0;
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit && (!_onlyIfHasHealthOver0 || targetSlotInfo.Unit.CurrentHealth > 0))
                {
                    int num = entryVariable;
                    if (entryAsPercentage)
                    {
                        num = targetSlotInfo.Unit.CalculatePercentualAmount(num);
                    }

                    if (_directHeal)
                    {
                        num = caster.WillApplyHeal(num, targetSlotInfo.Unit);
                    }

                    int statusCount = 0;
                    List<string> negatives = new List<string>();
                    foreach (IStatusEffect status in (targetSlotInfo.Unit as IStatusEffector).StatusEffects)
                    {
                        if (!_negativeOnly || !status.IsPositive)
                        {
                            if (_useTypes) statusCount++;
                            else statusCount += Math.Max(1, status.StatusContent);
                        }
                        if (!status.IsPositive) negatives.Add(status.StatusID);
                    }

                    if (_negativeOnly)
                    {
                        foreach (string statusTo in negatives)
                            targetSlotInfo.Unit.TryRemoveStatusEffect(statusTo);
                    }
                    else targetSlotInfo.Unit.TryRemoveAllStatusEffects();

                        exitAmount += targetSlotInfo.Unit.Heal(Math.Max(0, num - statusCount), caster, _directHeal);
                }
            }

            return exitAmount > 0;
        }
    }
}
