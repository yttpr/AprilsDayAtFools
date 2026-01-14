using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class DoubleBladedKnifeCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is DamageDealtValueChangeException value)
            {
                bool left = effector.SlotID < value.damagedUnit.SlotID + (value.damagedUnit.Size - 1);
                bool right = effector.SlotID > value.damagedUnit.SlotID;

                if (left && !right)
                {
                    if (effector.IsUnitCharacter) (effector as IUnit).ShowItem();
                    value.AddModifier(new PercentageValueModifier(true, 50, true));
                }
                else if (right && !left)
                {
                    if (effector.IsUnitCharacter) (effector as IUnit).ShowItem();
                    value.AddModifier(new PercentageValueModifier(true, 50, false));
                }
            }
            return true;
        }
    }
}
