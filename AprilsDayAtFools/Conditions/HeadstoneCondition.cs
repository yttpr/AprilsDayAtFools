using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class HeadstoneCondition : EffectorConditionSO
    {
        public string Value = "Headstone_TW";
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if ((effector as IUnit).SimpleGetStoredValue(Value) > 0) return false;

            if (args is BooleanReference reference)
            {
                if (StatusExtensions.GetFieldAmountFromID(effector.SlotID, effector.IsUnitCharacter, "Shield_ID", true) > 0)
                {
                    if (effector.IsUnitCharacter) (effector as IUnit).ShowItem();

                    if ((effector as IUnit).Heal(StatusExtensions.GetFieldAmountFromID(effector.SlotID, effector.IsUnitCharacter, "Shield_ID", true), effector as IUnit, true) > 0)
                    {
                        reference.value = false;

                        (effector as IUnit).SimpleSetStoredValue(Value, 1);
                    }
                }
            }
            return true;
        }
    }
}
