using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class LessThan10EntropyCondition : EffectConditionSO
    {
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            return caster.GetStatusAmount(Entropy.StatusID) < 10;
        }
    }
}
