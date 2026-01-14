using UnityEngine;

namespace AprilsDayAtFools
{
    public class EraserCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            CombatStats stats = CombatManager.Instance._stats;
            if (args is AdvancedDamageInfo info)
            {
                if (info.Target != null && info.Target is EnemyCombat enemy)
                {
                    (effector as IUnit).ShowItem();
                    int range = Random.Range(0, 100);
                    if (range < 33) stats.timeline.TryRemoveRandomEnemyTurns(enemy, 1);
                    else if (range > 66) stats.timeline.TryAddNewExtraEnemyTurns(enemy, 1);
                    else stats.timeline.TryReRollRandomEnemyTurns(enemy, 1, false);
                    return true;
                }
            }
            return false;
        }
    }
}
