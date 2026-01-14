using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class Targetting_ByUnit_Side_Specific_Status : Targetting_ByUnit_Side
    {
        [Header("Status Data")]
        public bool m_GetBySpecificStatus;

        public List<StatusEffect_SO> m_SpecificStatus = new List<StatusEffect_SO>();

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            List<TargetSlotInfo> allUnitTargetSlotsAsList = slots.GetAllUnitTargetSlotsAsList((isCasterCharacter && getAllies) || (!isCasterCharacter && !getAllies), getAllUnitSlots, ignoreCastSlot ? casterSlotID : (-1));
            for (int num = allUnitTargetSlotsAsList.Count - 1; num >= 0; num--)
            {
                TargetSlotInfo targetSlotInfo = allUnitTargetSlotsAsList[num];
                if (!targetSlotInfo.HasUnit)
                {
                    allUnitTargetSlotsAsList.RemoveAt(num);
                }
                else if (m_GetBySpecificStatus)
                {
                    bool flag = false;
                    foreach (StatusEffect_SO item in m_SpecificStatus)
                    {
                        if (targetSlotInfo.Unit.ContainsStatusEffect(item.StatusID))
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        allUnitTargetSlotsAsList.RemoveAt(num);
                    }
                }
                else if (targetSlotInfo.Unit.StatusEffectCount <= 0)
                {
                    allUnitTargetSlotsAsList.RemoveAt(num);
                }
            }

            return allUnitTargetSlotsAsList.ToArray();
        }
    }
}
