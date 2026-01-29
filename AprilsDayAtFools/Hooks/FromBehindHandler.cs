using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class FromBehindHandler
    {
        public static string AttackedBy => "Unit_AttackedBy_Data";
        public static void Setup() => NotificationHook.AddAction(NotifCheck);
        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnDirectDamaged.ToString() && args is IntegerReference_Damage value && sender is IUnit unit)
            {
                if (value.possibleSourceUnit != null)
                {
                    unit.TryGetStoredData(AttackedBy, out UnitStoreDataHolder holder);

                    if (holder.m_MainData > 0)
                    {
                        (holder.m_ObjectData as List<IUnit>).Add(value.possibleSourceUnit);
                    }
                    else
                    {
                        holder.m_ObjectData = new List<IUnit>() { value.possibleSourceUnit };
                        holder.m_MainData = 1;
                    }
                }
            }
        }
    }

    public class TargettingByAlreadyAttacked : BaseCombatTargettingSO
    {
        public BaseCombatTargettingSO Source;
        public override bool AreTargetAllies => Source.AreTargetAllies;
        public override bool AreTargetSlots => Source.AreTargetSlots;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            TargetSlotInfo[] targets = Source.GetTargets(slots, casterSlotID, isCasterCharacter);

            List<TargetSlotInfo> ret = [];

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    if (target.Unit.TryGetStoredData(FromBehindHandler.AttackedBy, out UnitStoreDataHolder holder, false))
                    {
                        if (holder.m_MainData > 0)
                        {
                            foreach (IUnit unit in holder.m_ObjectData as List<IUnit>)
                            {
                                if (unit.SlotID == casterSlotID && unit.IsUnitCharacter == isCasterCharacter)
                                {
                                    ret.Add(target);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return ret.ToArray();
        }

        public static TargettingByAlreadyAttacked Create(BaseCombatTargettingSO source)
        {
            TargettingByAlreadyAttacked ret = ScriptableObject.CreateInstance<TargettingByAlreadyAttacked>();
            ret.Source = source;
            return ret;
        }
    }
}
