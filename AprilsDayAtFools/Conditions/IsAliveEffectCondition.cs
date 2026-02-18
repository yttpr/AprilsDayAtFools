using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class IsAliveEffectCondition : EffectConditionSO
    {
        public static IsAliveEffectorCondition check;
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            if (check == null) check = Passives.Slippery.conditions[0] as IsAliveEffectorCondition;
            return check.MeetCondition(caster as IEffectorChecks, null);
        }
    }
}
