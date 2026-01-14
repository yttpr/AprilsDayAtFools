using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class OneOrLessEnemyCondition : EffectConditionSO
    {
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            return CombatManager.Instance._stats.EnemiesOnField.Count <= 1;
        }
    }
}
