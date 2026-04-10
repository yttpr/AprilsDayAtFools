using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class TriggerNotifsEffect : EffectSO
    {
        public (string, object)[] List;

        public int TriggerNotifs(string notif, object args, TargetSlotInfo[] targets)
        {
            int ret = 0;

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    CombatManager.Instance.PostNotification(notif, target.Unit, args);
                    ret++;
                }
            }

            return ret;
        }

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            
            foreach ((string, object) notif in List)
            {
                exitAmount += TriggerNotifs(notif.Item1, notif.Item2, targets);
            }

            return exitAmount > 0;
        }
    }
}
