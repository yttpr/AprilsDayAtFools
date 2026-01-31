using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class TargettingCanUseAbilities : Targetting_ByUnit_Side
    {
        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            List<TargetSlotInfo> ret = [];
            TargetSlotInfo[] targets = base.GetTargets(slots, casterSlotID, isCasterCharacter);
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit && target.Unit is CharacterCombat chara && chara.CanUseAbilitiesNoTrigger)
                    ret.Add(target);
            }
            return ret.ToArray();
        }
    }
}
