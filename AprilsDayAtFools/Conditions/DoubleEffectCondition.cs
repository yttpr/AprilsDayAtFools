using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class DoubleEffectCondition : EffectConditionSO
    {
        public EffectConditionSO first;
        public EffectConditionSO second;
        public bool And;

        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex) => this.And ? this.first.MeetCondition(caster, effects, currentIndex) && this.second.MeetCondition(caster, effects, currentIndex) : this.first.MeetCondition(caster, effects, currentIndex) || this.second.MeetCondition(caster, effects, currentIndex);
    }
}
