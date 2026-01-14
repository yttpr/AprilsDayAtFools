using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class DamageByMissingHealthCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is DamageDealtValueChangeException reference)
            {
                int num = effector.MaximumHealth - effector.CurrentHealth;
                if (num > 0)
                {
                    (effector as IUnit).ShowItem();
                    reference.AddModifier(new AdditionValueModifier(true, num));
                }
            }
            return true;
        }
    }
}
