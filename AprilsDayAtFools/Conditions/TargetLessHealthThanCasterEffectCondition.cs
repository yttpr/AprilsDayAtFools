using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class TargetLessHealthThanCasterEffectCondition : EffectConditionSO
    {
        public BaseCombatTargettingSO targets;
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            foreach (TargetSlotInfo target in targets.GetTargets(CombatManager.Instance._stats.combatSlots, caster.SlotID, caster.IsUnitCharacter))
            {
                if (target.HasUnit && target.Unit.CurrentHealth < caster.CurrentHealth) return true;
            }
            return false;
        }

        public static TargetLessHealthThanCasterEffectCondition Create(BaseCombatTargettingSO slots)
        {
            TargetLessHealthThanCasterEffectCondition ret = ScriptableObject.CreateInstance<TargetLessHealthThanCasterEffectCondition>();
            ret.targets = slots;
            return ret;
        }
    }
}
