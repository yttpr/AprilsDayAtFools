using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class DialCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is CascadeSpecialBooleanReference reference)
            {
                if (reference.Info.Target != null && reference.Info.Target.ContainsStatusEffect("Pale_ID"))
                {
                    reference.value = true;
                    if (effector is IUnit unit) unit.ShowItem();
                }
            }
            return true;
        }
    }
}
