using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class AmbushHook
    {
        public static void Setup()
        {
            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool(IDs.Ambush, "Ambush +{0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Positive));

            NotificationHook.AddAction(NotifCheck);
        }
        public static void NotifCheck(string name, object sender, object args)
        {
            if (sender is IUnit unit && name == TriggerCalls.OnTurnStart.ToString()) unit.SimpleSetStoredValue(IDs.Ambush, 0);
        }
    }
}
