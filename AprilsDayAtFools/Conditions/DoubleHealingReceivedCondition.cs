using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class DoubleHealingReceivedCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is HealingReceivedValueChangeException value)
            {
                value.AddModifier(new MultiplyIntValueModifier(false, 2));
            }
            return true;
        }
    }
}
