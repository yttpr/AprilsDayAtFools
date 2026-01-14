using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class FirstAbilityUsedCondition : EffectConditionSO
    {
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            return !FirstPerTurnHandler.FirstAbilityUsed;
        }
    }
}
