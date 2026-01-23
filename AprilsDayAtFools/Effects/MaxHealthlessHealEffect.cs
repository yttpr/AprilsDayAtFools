using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class MaxHealthlessHealEffect : HealEffect
    {
        public static void Listener(object sender, object args)
        {
            IUnit unit = sender as IUnit;
            int num = (args as IntegerReference).value + unit.CurrentHealth;
            if (num > unit.MaximumHealth)
            {
                unit.MaximizeHealth(num);
            }
        }
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

                    CombatManager.Instance.AddObserver(Listener, TriggerCalls.OnWillBeHealed.ToString(), targetSlotInfo.Unit);

                    exitAmount += targetSlotInfo.Unit.Heal(num, caster, _directHeal);

                    CombatManager.Instance.RemoveObserver(Listener, TriggerCalls.OnWillBeHealed.ToString(), targetSlotInfo.Unit);
                }
            }

            return exitAmount > 0;
        }
    }
}
