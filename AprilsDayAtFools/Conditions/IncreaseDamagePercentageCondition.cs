using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class IncreaseDamagePercentageCondition : EffectorConditionSO
    {
        public int Mod;
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is DamageDealtValueChangeException reference)
            {
                (effector as IUnit).ShowItem();
                reference.AddModifier(new PercentageValueModifier(true, Mod, true));
            }
            return true;
        }

        public static IncreaseDamagePercentageCondition Create(int percentageToAdd)
        {
            IncreaseDamagePercentageCondition ret = ScriptableObject.CreateInstance<IncreaseDamagePercentageCondition>();
            ret.Mod = percentageToAdd;
            return ret;
        }
    }
}
