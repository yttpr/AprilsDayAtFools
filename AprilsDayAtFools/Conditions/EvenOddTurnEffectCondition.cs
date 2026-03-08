using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class EvenOddTurnEffectCondition : EffectConditionSO
    {
        public bool Odd;
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            return (CombatManager.Instance._stats.TurnsPassed + 1) % 2 > 0 == Odd;
        }

        public static EvenOddTurnEffectCondition Create(bool odd)
        {
            EvenOddTurnEffectCondition ret = ScriptableObject.CreateInstance<EvenOddTurnEffectCondition>();
            ret.Odd = odd;
            return ret;
        }
    }
}
