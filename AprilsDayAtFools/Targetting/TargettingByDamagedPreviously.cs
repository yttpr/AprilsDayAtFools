using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class TargettingByDamagedPreviously : Targetting_ByUnit_Side
    {
        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            TargetSlotInfo[] orig = base.GetTargets(slots, casterSlotID, isCasterCharacter);

            List<TargetSlotInfo> ret = new List<TargetSlotInfo>();

            foreach (TargetSlotInfo target in orig)
            {
                if (target.HasUnit)
                {
                    if (target.Unit.IsUnitCharacter) { if (ComaHandler.PastFools.Contains(target.Unit.ID)) ret.Add(target); }
                    else { if (ComaHandler.PastEnemies.Contains(target.Unit.ID)) ret.Add(target); }
                }
            }

            return ret.ToArray();
        }
    }
}
