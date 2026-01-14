using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class SurgeonsToolsCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is HealingDealtValueChangeException reference)
            {
                (effector as IUnit).ShowItem();
                reference.AddModifier(new MultiplyIntValueModifier(false, 2));
                CombatManager.Instance.AddSubAction(new EffectAction([Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyRupturedEffect>(), 2, Slots.Self)], reference.healingUnit));
            }
            return true;
        }
    }
}
