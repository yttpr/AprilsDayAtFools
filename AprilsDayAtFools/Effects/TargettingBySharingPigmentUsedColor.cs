using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class TargettingBySharingPigmentUsedColor : Targetting_ByUnit_Side
    {
        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            TargetSlotInfo[] orig = base.GetTargets(slots, casterSlotID, isCasterCharacter);
            List<TargetSlotInfo> ret = [];
            foreach (TargetSlotInfo target in orig)
            {
                if (target.HasUnit)
                {
                    foreach (ManaColorSO mana in PigmentUsedCollector.lastUsed)
                    {
                        if (target.Unit.HealthColor.SharesPigmentColor(mana))
                        {
                            ret.Add(target);
                            break;
                        }
                    }
                }
            }
            return ret.ToArray();
        }
    }
}
