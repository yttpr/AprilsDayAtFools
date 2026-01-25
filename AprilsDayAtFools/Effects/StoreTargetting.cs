using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class StoreTargetting : BaseCombatTargettingSO
    {
        public override bool AreTargetAllies => Source.AreTargetAllies;
        public override bool AreTargetSlots => Source.AreTargetSlots;

        public BaseCombatTargettingSO Source;
        public TargetSlotInfo[] Last;
        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            Last = Source.GetTargets(slots, casterSlotID, isCasterCharacter);
            return Last;
        }

        public static StoreTargetting Create(BaseCombatTargettingSO source)
        {
            StoreTargetting ret = ScriptableObject.CreateInstance<StoreTargetting>();
            ret.Source = source;
            return ret;
        }
    }

    public class GetStoredTargetting : BaseCombatTargettingSO
    {
        public override bool AreTargetAllies => Source.AreTargetAllies;
        public override bool AreTargetSlots => Source.AreTargetSlots;

        public StoreTargetting Source;
        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            if (Source != null && Source.Last != null) return Source.Last;
            return [];
        }

        public static GetStoredTargetting Create(StoreTargetting source)
        {
            GetStoredTargetting ret = ScriptableObject.CreateInstance<GetStoredTargetting>();
            ret.Source = source;
            return ret;
        }
    }
}
