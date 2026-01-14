using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class MolarsCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is AdvancedDamageInfo info)
            {
                if (info.Target != null && info.Target is IStatusEffector holder)
                {
                    List<IStatusEffect> statuses = [.. holder.StatusEffects];

                    if (statuses.Count <= 0) return false;

                    (effector as IUnit).ShowItem();

                    foreach (IStatusEffect status in statuses)
                    {
                        if (status is StatusEffect_Holder data)
                            info.Target.ApplyStatusEffect(data._Status, data.StatusContent, data.Restrictor);
                    }
                }
            }
            return true;
        }
    }
}
