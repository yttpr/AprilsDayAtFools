using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AprilsDayAtFools
{
    public class IncreaseDamageBySlotsCondition : EffectorConditionSO
    {
        public int[] ValidSlots;
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is DamageDealtValueChangeException value)
            {
                if (effector.IsUnitCharacter) (effector as IUnit).ShowItem();
                if (!ValidSlots.Contains(effector.SlotID))
                {
                    value.AddModifier(new PercentageValueModifier(true, 25, false));
                }
                else value.AddModifier(new PercentageValueModifier(true, 25, true));
            }
            return true;
        }
    }
}
