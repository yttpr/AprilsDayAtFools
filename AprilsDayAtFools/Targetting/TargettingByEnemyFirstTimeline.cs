using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class TargettingByEnemyFirstTimeline : BaseCombatTargettingSO
    {
        public override bool AreTargetSlots => false;
        public override bool AreTargetAllies => false;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            CombatStats stats = CombatManager.Instance._stats;
            for (int i = stats.timeline.CurrentTurn; i < stats.timeline.Round.Count; i++)
            {
                if (stats.timeline.Round[i].isPlayer) continue;

                if (stats.timeline.Round[i].turnUnit is EnemyCombat enemy)
                {
                    return slots.EnemySlots[enemy.SlotID].TargetSlotInformation.SelfArray();
                }
            }
            return [];
        }
    }
}
