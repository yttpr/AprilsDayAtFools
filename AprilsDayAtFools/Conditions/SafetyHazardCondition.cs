using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class SafetyHazardCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is DamageDealtValueChangeException value)
            {
                if (value.damagedUnit != null && value.damagedUnit.ContainsFieldEffect("Slip_ID"))
                {
                    (effector as IUnit).ShowItem();
                    value.AddModifier(new PercentageValueModifier(true, 25, true));
                }
            }
            return false;
        }
    }
}
