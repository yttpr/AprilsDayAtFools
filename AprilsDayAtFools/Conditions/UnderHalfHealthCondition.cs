using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class UnderHalfHealthCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            return ((float)effector.CurrentHealth / (float)effector.MaximumHealth) < 0.5f;
        }
    }
}
