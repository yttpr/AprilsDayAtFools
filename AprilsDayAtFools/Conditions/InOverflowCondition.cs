using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class InOverflowCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            return CombatManager.Instance._stats.overflowMana.StoredSlots.Count > 0;
        }
    }
}
