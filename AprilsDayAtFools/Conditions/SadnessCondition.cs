using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class SadnessCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is StatusFieldApplication value)
            {
                if (value.statusID == "Ruptured_ID" || value.statusID == "OilSlicked_ID" || value.statusID == "Left_ID" || value.statusID == Entropy.StatusID || value.statusID == "Frail_ID" || value.statusID == "Scars_ID" || value.statusID == "Cursed_ID" || value.statusID == "Muted_ID" || value.statusID == Acid.StatusID)
                {
                    value.canBeApplied = false;
                    (effector as IUnit).ShowItem();
                    (effector as IUnit).ApplyStatusEffect(Drowning.Object, value.amount);
                }
            }
            return false;
        }
    }
}
