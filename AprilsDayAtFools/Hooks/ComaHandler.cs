using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class ComaHandler
    {
        public static List<int> PresentFools;
        public static List<int> PastFools;
        public static List<int> PresentEnemies;
        public static List<int> PastEnemies;
        public static void Setup()
        {
            PresentFools = new List<int>();
            PastFools = new List<int>();
            PresentEnemies = new List<int>();
            PastEnemies = new List<int>();
            NotificationHook.AddAction(PostNotifCheck, false);
            NotificationHook.AddAction(PreNotifCheck, true);
        }

        public static void PostNotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnDamaged.ToString() && sender is IUnit unit)
            {
                if (unit.IsUnitCharacter) PresentFools.Add(unit.ID);
                else PresentEnemies.Add(unit.ID);
            }
        }
        public static void PreNotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnBeforeCombatStart.ToString())
            {
                PresentFools.Clear();
                PastFools.Clear();
                PresentEnemies.Clear();
                PastEnemies.Clear();
            }
            if (name == TriggerCalls.TimelineEndReached.ToString() && sender == null)
            {
                PastFools = new List<int>(PresentFools);
                PresentFools.Clear();
                PastEnemies = new List<int>(PresentEnemies);
                PresentEnemies.Clear();
            }
        }
    }
}
