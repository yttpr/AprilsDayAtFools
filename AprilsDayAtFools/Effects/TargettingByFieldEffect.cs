using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{

    public class TargettingByFieldEffect : BaseCombatTargettingSO
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

            for (int i = 0; i < targets.Length; i++)
            {
                if ((targets[i].HasUnit || !OnlyUnits) && targets[i].ContainsFieldEffect(FieldID)) ret.Add(targets[i].TargetSlotInformation);
            }

            return ret.ToArray();
        }

        public static TargettingByFieldEffect Create(bool allies, string field, bool only = true)
        {
            TargettingByFieldEffect ret = ScriptableObject.CreateInstance<TargettingByFieldEffect>();
            ret.GetAllies = allies;
            ret.FieldID = field;
            ret.OnlyUnits = only;
            return ret;
        }
    }
}
