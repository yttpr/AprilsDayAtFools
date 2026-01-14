using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class BroomCheckCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            return (effector as IUnit).ContainsFieldEffect("Slip_ID") || (effector as IUnit).ContainsFieldEffect("Roots_ID") || (effector as IUnit).ContainsFieldEffect("Constricted_ID");
        }
    }
}
