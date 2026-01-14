using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class IncreaseDamageByAmountCondition : EffectorConditionSO
    {
        public int Amount;
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is DamageDealtValueChangeException value)
            {
                (effector as IUnit).ShowItem();
                value.AddModifier(new AdditionValueModifier(true, Amount));
            }
            return true;
        }
    }
}
