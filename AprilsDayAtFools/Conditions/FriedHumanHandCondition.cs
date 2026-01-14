using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class FriedHumanHandCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is HealingDealtValueChangeException value)
            {
                if (value.healingUnit.CurrentHealth < value.casterUnit.CurrentHealth)
                {
                    (effector as IUnit).ShowItem();
                    value.AddModifier(new PercentageValueModifier(true, 60, true));
                }
                else if (value.healingUnit.CurrentHealth > value.casterUnit.CurrentHealth)
                {
                    (effector as IUnit).ShowItem();
                    value.AddModifier(new PercentageValueModifier(true, 40, false));
                }
            }
            return true;
        }
    }
}
