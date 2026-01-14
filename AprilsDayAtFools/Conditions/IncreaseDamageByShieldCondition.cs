using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class IncreaseDamageByShieldCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is DamageDealtValueChangeException value)
            {
                if (StatusExtensions.GetFieldAmountFromID(effector.SlotID, effector.IsUnitCharacter, "Shield_ID", true) > 0 && effector.IsUnitCharacter) (effector as IUnit).ShowItem();
                value.AddModifier(new AdditionValueModifier(true, StatusExtensions.GetFieldAmountFromID(effector.SlotID, effector.IsUnitCharacter, "Shield_ID", true)));
            }
            return true;
        }
    }
}
