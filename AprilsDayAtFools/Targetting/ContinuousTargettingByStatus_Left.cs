using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ContinuousTargettingByStatus_Left : BaseCombatTargettingSO
    {
        public bool _getAllies;
        public bool _getAllSlots;
        public override bool AreTargetAllies => _getAllies;
        public override bool AreTargetSlots => _getAllSlots;
        public string _statusID;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            List<TargetSlotInfo> ret = [];

            if (_getAllies == isCasterCharacter)
            {
                for (int i = casterSlotID; i >= 0 && i < 5; i--)
                {
                    if (slots.CharacterSlots[i].HasUnit) ret.Add(slots.CharacterSlots[i].TargetSlotInformation);
                    if (slots.CharacterSlots[i].HasUnit && slots.CharacterSlots[i].Unit.ContainsStatusEffect(_statusID)) continue;
                    break;
                }
            }
            else
            {
                for (int i = casterSlotID; i >= 0 && i < 5; i--)
                {
                    if (slots.EnemySlots[i].HasUnit) ret.Add(slots.EnemySlots[i].TargetSlotInformation);
                    if (slots.EnemySlots[i].HasUnit && slots.EnemySlots[i].Unit.ContainsStatusEffect(_statusID))
                    {
                        IUnit unit = slots.EnemySlots[i].Unit;
                        i = unit.SlotID;
                        continue;
                    }
                    break;
                }
            }

            return ret.ToArray();
        }
    }
}
