using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class TargetsBelowHalfHealthEffectCondition : EffectConditionSO
    {
        public BaseCombatTargettingSO targets;
        public bool require_all;
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            bool all = true;
            foreach (TargetSlotInfo target in targets.GetTargets(CombatManager.Instance._stats.combatSlots, caster.SlotID, caster.IsUnitCharacter))
            {
                if (target.HasUnit)
                {
                    if (target.Unit.CurrentHealth > (int)Math.Ceiling(target.Unit.MaximumHealth / 2f))
                    {
                        all = false;
                    }
                    else if (!require_all)
                    {
                        return true;
                    }
                }
            }

            return all;
        }
    }
}
