using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class AluminumCubeCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is HealingDealtValueChangeException value)
            {
                (effector as IUnit).ShowItem();

                CombatManager.Instance.AddSubAction(new RandomizeHealthColorAction(value.healingUnit));

                value.AddModifier(new PercentageValueModifier(true, 20, true));
            }
            return true;
        }
    }
}
