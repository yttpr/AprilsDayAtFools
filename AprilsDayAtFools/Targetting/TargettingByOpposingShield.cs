using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class TargettingByOpposingFieldEffect : BaseCombatTargettingSO
    {
        public string FieldID;
        public bool GetAllies;
        public bool OnlyUnits;

        public override bool AreTargetAllies => GetAllies;
        public override bool AreTargetSlots => true;
        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            List<TargetSlotInfo> ret = new List<TargetSlotInfo>();

            CombatSlot[] targets = (GetAllies == isCasterCharacter) ? [.. slots.CharacterSlots] : [.. slots.EnemySlots];
            CombatSlot[] checks = (GetAllies != isCasterCharacter) ?[..slots.CharacterSlots] : [.. slots.EnemySlots];

            for (int i = 0; i <  targets.Length; i++)
            {
                if ((targets[i].HasUnit || !OnlyUnits) && checks[i].ContainsFieldEffect(FieldID)) ret.Add(targets[i].TargetSlotInformation);
            }

            return ret.ToArray();
        }

        public static TargettingByOpposingFieldEffect Create(bool allies, string field, bool only = true)
        {
            TargettingByOpposingFieldEffect ret = ScriptableObject.CreateInstance<TargettingByOpposingFieldEffect>();
            ret.GetAllies = allies;
            ret.FieldID = field;
            ret.OnlyUnits = only;
            return ret;
        }
    }
}
