using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class InfiniteRopeCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is StatusFieldApplication value)
            {
                if (value.statusID == Drowning.StatusID || value.statusID == "DivineProtection_ID" || value.statusID == "Left_ID")
                {
                    (effector as IUnit).ShowItem();
                    value.canBeApplied = false;
                }
            }
            return false;
        }
    }
}
