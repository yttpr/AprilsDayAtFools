using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class FirstPerTurnHandler
    {
        public static void Setup()
        {
            NotificationHook.AddAction(NotifCheck, true);
        }

        public static bool FirstAbilityUsed;

        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnBeforeCombatStart.ToString()) FirstAbilityUsed = false;
            else if (name == TriggerCalls.TimelineEndReached.ToString()) FirstAbilityUsed = false;
            else if (name == TriggerCalls.OnAbilityUsed.ToString()) FirstAbilityUsed = true;
        }
    }
}
