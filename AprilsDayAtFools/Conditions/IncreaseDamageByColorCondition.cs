using UnityEngine;

namespace AprilsDayAtFools
{
    public class IncreaseDamageByColorCondition : EffectorConditionSO
    {
        public ManaColorSO color;
        public int percentage;
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is DamageDealtValueChangeException reference)
            {
                if (reference.damagedUnit.HealthColor == color)
                {
                    (effector as IUnit).ShowItem();
                    reference.AddModifier(new PercentageValueModifier(true, percentage, true));
                    return true;
                }
            }
            return false;
        }

        public static IncreaseDamageByColorCondition Create(ManaColorSO color, int percentage)
        {
            IncreaseDamageByColorCondition ret = ScriptableObject.CreateInstance<IncreaseDamageByColorCondition>();
            ret.color = color;
            ret.percentage = percentage;
            return ret;
        }
    }
}
