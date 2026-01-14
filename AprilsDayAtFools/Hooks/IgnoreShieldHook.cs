using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AprilsDayAtFools
{
    public static class IgnoreShieldHandler
    {
        public static void Setup()
        {
            NotificationHook.AddAction(NotifCheck, true);
        }
        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnBeingDamaged.ToString() && args is DamageReceivedValueChangeException value && !value.ignoreShield)
            {
                if (sender is IUnit effector && effector.HasUsableItem && effector.HeldItem.IsItemType("IgnoreShield"))
                {
                    if (StatusExtensions.GetFieldAmountFromID(effector.SlotID, effector.IsUnitCharacter, "Shield_ID", true) > 0 && effector.IsUnitCharacter) (effector as IUnit).ShowItem();
                    value.ignoreShield = true;
                }
            }
        }
    }
}
