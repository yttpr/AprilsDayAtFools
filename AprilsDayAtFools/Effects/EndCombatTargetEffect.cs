using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class EndCombatTargetEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    CombatManager.Instance.PostNotification(TriggerCalls.OnCombatEnd.ToString(), target.Unit, null);
                    exitAmount++;
                }
            }
            return exitAmount > 0;
        }
    }
}
