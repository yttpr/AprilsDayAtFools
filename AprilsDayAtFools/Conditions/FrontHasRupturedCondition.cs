using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class FrontHasRupturedCondition : EffectConditionSO
    {
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            foreach (TargetSlotInfo target in Slots.Front.GetTargets(CombatManager.Instance._stats.combatSlots, caster.SlotID, caster.IsUnitCharacter))
            {
                if (target.HasUnit && target.Unit.ContainsStatusEffect("Ruptured_ID")) return true;
            }
            return false;
        }
    }
}
