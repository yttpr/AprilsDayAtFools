using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ImmuneToShieldCondition : EffectorConditionSO
    {
        //this doesnt work it would need to be before shield. making this a hook.
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is DamageReceivedValueChangeException value)
            {
                if (StatusExtensions.GetFieldAmountFromID(effector.SlotID, effector.IsUnitCharacter, "Shield_ID", true) > 0 && effector.IsUnitCharacter) (effector as IUnit).ShowItem();
                value.ignoreShield = true;
                value.ShouldIgnoreUI = false;
            }
            return true;
        }
    }
}
