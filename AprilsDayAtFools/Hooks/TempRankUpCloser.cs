using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class TempRankUpCloser
    {
        public static void Setup()
        {
            NotificationHook.AddAction(NotifCheck, true);
        }
        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnCombatEnd.ToString() && sender is CharacterCombat chara)
            {
                int num = chara.SimpleGetStoredValue(IDs.Level);
                if (num > 0)
                {
                    chara._rank -= num;
                }
            }
        }
    }
}
