using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AprilsDayAtFools
{
    public static class SidesDamagedHandler
    {
        public static TriggerCalls LeftDamaged => (TriggerCalls)7591331;
        public static TriggerCalls RightDamaged => (TriggerCalls)7591332;
        public static void Setup()
        {
            NotificationHook.AddAction(NotifCheck);
        }
        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnDamaged.ToString() && sender is IUnit unit)
            {
                TargetSlotInfo[] right = Targeting.Slot_AllyRight.GetTargets(CombatManager.Instance._stats.combatSlots, unit.SlotID, unit.IsUnitCharacter);
                foreach (TargetSlotInfo rightally in right)
                {
                    if (rightally.HasUnit) CombatManager.Instance.PostNotification(LeftDamaged.ToString(), rightally.Unit, args);
                }

                TargetSlotInfo[] left = Targeting.Slot_AllyLeft.GetTargets(CombatManager.Instance._stats.combatSlots, unit.SlotID, unit.IsUnitCharacter);
                foreach (TargetSlotInfo leftally in left)
                {
                    if (leftally.HasUnit) CombatManager.Instance.PostNotification(RightDamaged.ToString(), leftally.Unit, args);
                }
            }
        }
    }
}
