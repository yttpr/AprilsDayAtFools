using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class DamageTypeCondition : EffectorConditionSO
    {
        public string Type;
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is AdvancedDamageInfo info) return Type == info.Type;
            return false;
        }
        public static DamageTypeCondition Create(string type)
        {
            DamageTypeCondition ret = ScriptableObject.CreateInstance<DamageTypeCondition>();
            ret.Type = type;
            return ret;
        }
    }
}
