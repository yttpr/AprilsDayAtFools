using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class FirstDamageUpItemCondition : EffectorConditionSO
    {
        public string ID;
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is DamageDealtValueChangeException exception && effector is IUnit unit)
            {
                if (unit.SimpleGetStoredValue(ID) <= 0)
                {
                    unit.SimpleSetStoredValue(ID, 1);
                    unit.ShowItem();
                    exception.AddModifier(new AdditionValueModifier(true, 7));
                }
                return false;
            }
            return true;
        }
    }
}
