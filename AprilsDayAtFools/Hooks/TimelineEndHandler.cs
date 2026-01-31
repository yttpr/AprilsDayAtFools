using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class TimelineEndHandler
    {
        public static TriggerCalls Before => (TriggerCalls)86856122;

        public static TriggerCalls After => (TriggerCalls)86856123;
        public static TriggerCalls Late => (TriggerCalls)86856124;

        public static void Setup()
        {
            NotificationHook.AddAction(PreNotifCheck, true);
            NotificationHook.AddAction(PostNotifCheck, false);
        }


        public static void PreNotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.TimelineEndReached.ToString() && sender == null)
            {
                CombatStats stats = CombatManager.Instance._stats;
                foreach (EnemyCombat enemy in stats.EnemiesOnField.Values)
                    enemy.TriggerNotification(Before.ToString(), null);
                foreach (CharacterCombat chara in stats.CharactersOnField.Values)
                    chara.TriggerNotification(Before.ToString(), null);
            }
        }
        public static void PostNotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.TimelineEndReached.ToString() && sender == null)
            {
                CombatStats stats = CombatManager.Instance._stats;
                foreach (EnemyCombat enemy in stats.EnemiesOnField.Values)
                    enemy.TriggerNotification(After.ToString(), null);
                foreach (CharacterCombat chara in stats.CharactersOnField.Values)
                    chara.TriggerNotification(After.ToString(), null);

                CombatManager.Instance.AddSubAction(new TimelineLateNotifAction());
            }
        }

        public class TimelineLateNotifAction : CombatAction
        {
            public override IEnumerator Execute(CombatStats stats)
            {
                CombatManager.Instance.PostNotification(Late.ToString(), null, null);
                yield return null;
            }
        }
    }
}
