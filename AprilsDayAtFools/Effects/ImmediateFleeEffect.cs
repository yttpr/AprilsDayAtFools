using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ImmediateFleeEffect : FleeTargetEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            throw new NotImplementedException();
            //FleetingPassiveAbility
            exitAmount = 0;
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit)
                {
                    exitAmount++;
                    targetSlotInfo.Unit.UnitWillFlee();
                    CombatManager.Instance.AddSubAction(new FleetingUnitAction(targetSlotInfo.Unit.ID, targetSlotInfo.Unit.IsUnitCharacter));
                }
            }

            return exitAmount > 0;
        }
    }
}
