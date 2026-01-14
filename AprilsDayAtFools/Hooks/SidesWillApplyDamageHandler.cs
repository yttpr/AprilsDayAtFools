using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class SidesWillApplyDamageHandler
    {
        public static TriggerCalls SidesApply => (TriggerCalls)12947583;

        public static void Setup()
        {
            NotificationHook.AddAction(NotifCheck);
        }

        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnWillApplyDamage.ToString() && sender is IUnit unit)
            {
                foreach (TargetSlotInfo target in Slots.Sides.GetTargets(CombatManager.Instance._stats.combatSlots, unit.SlotID, unit.IsUnitCharacter))
                {
                    if (target.HasUnit) CombatManager.Instance.PostNotification(SidesApply.ToString(), target.Unit, args);
                }
            }
        }
    }
}
