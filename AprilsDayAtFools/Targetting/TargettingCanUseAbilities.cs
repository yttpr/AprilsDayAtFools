using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class TargettingCanUseAbilities : Targetting_ByUnit_Side
    {
        public BaseCombatTargettingSO Source;
        public override bool AreTargetAllies => Source != null ? Source.AreTargetAllies : base.AreTargetAllies;
        public override bool AreTargetSlots => Source != null ? Source.AreTargetSlots : base.AreTargetSlots;
        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            List<TargetSlotInfo> ret = [];
            TargetSlotInfo[] targets = Source != null ? Source.GetTargets(slots, casterSlotID, isCasterCharacter) : base.GetTargets(slots, casterSlotID, isCasterCharacter);
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit && target.Unit is CharacterCombat chara && chara.CanUseAbilitiesNoTrigger)
                    ret.Add(target);
            }
            return ret.ToArray();
        }
    }
}
