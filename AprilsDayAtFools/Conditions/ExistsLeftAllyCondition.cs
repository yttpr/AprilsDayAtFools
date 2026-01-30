using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ExistsLeftAllyCondition : EffectConditionSO
    {
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            foreach (TargetSlotInfo target in Targeting.Slot_AllyLeft.GetTargets(CombatManager.Instance._stats.combatSlots, caster.SlotID, caster.IsUnitCharacter))
            {
                if (target.HasUnit) return true;
            }
            return false;
        }
    }
    public class ExistsRightAllyCondition : EffectConditionSO
    {
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            foreach (TargetSlotInfo target in Targeting.Slot_AllyRight.GetTargets(CombatManager.Instance._stats.combatSlots, caster.SlotID, caster.IsUnitCharacter))
            {
                if (target.HasUnit) return true;
            }
            return false;
        }
    }
}
