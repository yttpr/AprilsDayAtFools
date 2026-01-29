using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ImmediateAnimationVisualsEffect : EffectSO
    {
        public AttackVisualsSO _visuals;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            stats.combatUI.PlayAbilityAnimation(_visuals, targets, areTargetSlots);

            return false;
        }
    }
}
