using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class BeforeSpecificTurnCondition : EffectConditionSO
    {
        public int Turn;
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            return CombatManager.Instance._stats.TurnsPassed < Turn;
        }

        public static BeforeSpecificTurnCondition Create(int turn)
        {
            BeforeSpecificTurnCondition ret = ScriptableObject.CreateInstance<BeforeSpecificTurnCondition>();
            ret.Turn = turn;
            return ret;
        }
    }
}
