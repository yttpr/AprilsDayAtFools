using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CasterHealthEffectorCondition : EffectorConditionSO
    {
        public int Amount;
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            return effector.CurrentHealth > Amount;
        }
    }
}
