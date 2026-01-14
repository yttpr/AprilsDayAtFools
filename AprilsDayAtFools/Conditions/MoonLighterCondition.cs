using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class MoonLighterCondition : EffectConditionSO
    {
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            return caster.SimpleGetStoredValue(IDs.Lighter) <= 0;
        }
    }
}
