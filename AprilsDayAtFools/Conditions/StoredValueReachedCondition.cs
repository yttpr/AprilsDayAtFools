using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class StoredValueReachedCondition : EffectConditionSO
    {
        public string Value;
        public int Num;
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            return caster.SimpleGetStoredValue(Value) >= Num;
        }
        public static StoredValueReachedCondition Create(string val, int num)
        {
            StoredValueReachedCondition ret = ScriptableObject.CreateInstance<StoredValueReachedCondition>();
            ret.Value = val;
            ret.Num = num;
            return ret;
        }
    }
}
