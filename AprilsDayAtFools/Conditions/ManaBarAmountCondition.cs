using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class ManaBarAmountCondition : EffectConditionSO
    {
        public int Max;
        public bool Over;
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            int num = 0;
            foreach (ManaBarSlot slots in CombatManager.Instance._stats.MainManaBar.ManaBarSlots)
            {
                if (slots.IsEmpty) continue;
                num++;
            }
            if (num <= Max) return !Over;
            return Over;
        }
        public static ManaBarAmountCondition Create(int checkByMax, bool shouldBeOver)
        {
            ManaBarAmountCondition ret = ScriptableObject.CreateInstance<ManaBarAmountCondition>();
            ret.Max = checkByMax;
            ret.Over = shouldBeOver;
            return ret;
        }
    }
}
