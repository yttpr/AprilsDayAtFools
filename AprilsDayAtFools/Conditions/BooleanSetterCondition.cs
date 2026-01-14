using UnityEngine;

namespace AprilsDayAtFools
{
    public class BooleanSetterCondition : EffectorConditionSO
    {
        public bool returnValue;
        public bool elseValue;
        public bool ShowItem;
        public bool setValue;
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is BooleanReference reference)
            {
                reference.value = setValue;
                if (ShowItem) (effector as IUnit).ShowItem();
                return returnValue;
            }
            return elseValue;
        }
        public static BooleanSetterCondition Create(bool set, bool showItem = false, bool returnAs = true, bool elseReturnAs = false)
        {
            BooleanSetterCondition ret = ScriptableObject.CreateInstance<BooleanSetterCondition>();
            ret.setValue = set;
            ret.ShowItem = showItem;
            ret.returnValue = returnAs;
            ret.elseValue = elseReturnAs;
            return ret;
        }
    }
}
