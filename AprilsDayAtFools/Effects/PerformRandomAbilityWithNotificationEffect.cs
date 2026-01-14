using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class PerformRandomAbilityWithNotificationEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            return false;

            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    TurnFinishedReference args = new TurnFinishedReference(!target.Unit.IsUnitCharacter ? false : target.Unit.HasManuallySwappedThisTurn);
                    CombatManager.Instance.PostNotification(TriggerCalls.OnTurnFinished.ToString(), target.Unit, args);
                }
            }
        }
    }
}
