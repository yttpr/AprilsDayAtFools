using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class TargettingBySameType : BaseCombatTargettingSO
    {
        public int origin;
        public bool get_allies;

        public override bool AreTargetAllies => get_allies;
        public override bool AreTargetSlots => true;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            List<TargetSlotInfo> ret = [];
            TargetSlotInfo base_slot = null;

            int lefting = origin + casterSlotID - 1;
            int righting = origin + casterSlotID + 1;

            CombatSlot[] get_slots = get_allies == isCasterCharacter ? slots.CharacterSlots : slots.EnemySlots;

            if (origin + casterSlotID >= 0 && origin + casterSlotID < 5)
            {
                base_slot = get_slots[origin + casterSlotID].TargetSlotInformation;
                ret.Add(base_slot);
            }
            else
            {
                return ret.ToArray();
            }

            for (int i = lefting; i >= 0; i--)
            {
                if (get_slots[i].HasUnit == base_slot.HasUnit)
                {
                    if (!base_slot.HasUnit)
                    {
                        ret.Add(get_slots[i].TargetSlotInformation);
                        continue;
                    }

                    if (get_slots[i].Unit is CharacterCombat c1 && base_slot.Unit is CharacterCombat c2)
                    {
                        if (c1.Character.name == c2.Character.name)
                        {
                            ret.Add(get_slots[i].TargetSlotInformation);
                            continue;
                        }
                    }
                    if (get_slots[i].Unit is EnemyCombat e1 && base_slot.Unit is EnemyCombat e2)
                    {
                        if (e1.Enemy.name == e2.Enemy.name)
                        {
                            ret.Add(get_slots[i].TargetSlotInformation);
                            continue;
                        }
                    }
                }
                break;
            }
            for (int i = righting; i < 5; i++)
            {
                if (get_slots[i].HasUnit == base_slot.HasUnit)
                {
                    if (!base_slot.HasUnit)
                    {
                        ret.Add(get_slots[i].TargetSlotInformation);
                        continue;
                    }

                    if (get_slots[i].Unit is CharacterCombat c1 && base_slot.Unit is CharacterCombat c2)
                    {
                        if (c1.Character.name == c2.Character.name)
                        {
                            ret.Add(get_slots[i].TargetSlotInformation);
                            continue;
                        }
                    }
                    if (get_slots[i].Unit is EnemyCombat e1 && base_slot.Unit is EnemyCombat e2)
                    {
                        if (e1.Enemy.name == e2.Enemy.name)
                        {
                            ret.Add(get_slots[i].TargetSlotInformation);
                            continue;
                        }
                    }
                }
                break;
            }

            return ret.ToArray();
        }
    }
}
