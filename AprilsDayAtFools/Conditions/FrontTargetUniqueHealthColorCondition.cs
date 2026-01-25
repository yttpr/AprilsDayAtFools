using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class FrontTargetUniqueHealthColorCondition : EffectConditionSO
    {
        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            foreach (TargetSlotInfo target in Slots.Front.GetTargets(CombatManager.Instance._stats.combatSlots, caster.SlotID, caster.IsUnitCharacter))
            {
                if (target.HasUnit)
                {
                    if (target.Unit.IsUnitCharacter)
                    {
                        foreach (CharacterCombat chara in CombatManager.Instance._stats.CharactersOnField.Values)
                        {
                            if (chara.ID == target.Unit.ID) continue;
                            if (chara.HealthColor == target.Unit.HealthColor) return false;
                        }
                    }
                    else
                    {
                        foreach (EnemyCombat enemy in CombatManager.Instance._stats.EnemiesOnField.Values)
                        {
                            if (enemy.ID == target.Unit.ID) continue;
                            if (enemy.HealthColor == target.Unit.HealthColor) return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
