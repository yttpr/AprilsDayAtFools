using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class WrongPigmentCondition : EffectorConditionSO 
    {
        public string ValueName;
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is IntegerReference reference)
            {
                if (ValueName != "") (effector as IUnit).SimpleSetStoredValue(ValueName, reference.value);
                return reference.value > 0;
            }
            return true;
        }
        public static WrongPigmentCondition Create(string value)
        {
            WrongPigmentCondition ret = ScriptableObject.CreateInstance<WrongPigmentCondition>();
            ret.ValueName = value;
            return ret;
        }
    }
}
