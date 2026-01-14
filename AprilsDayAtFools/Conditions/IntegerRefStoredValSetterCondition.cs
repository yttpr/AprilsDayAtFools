using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class IntegerRefStoredValSetterCondition : EffectorConditionSO
    {
        public string Value;
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (Value != "" && args is IntegerReference reference && effector is IUnit unit) unit.SimpleSetStoredValue(Value, reference.value);
            return true;
        }
        public static IntegerRefStoredValSetterCondition Create(string value)
        {
            IntegerRefStoredValSetterCondition ret = ScriptableObject.CreateInstance<IntegerRefStoredValSetterCondition>();
            ret.Value = value;
            return ret;
        }
    }
}
