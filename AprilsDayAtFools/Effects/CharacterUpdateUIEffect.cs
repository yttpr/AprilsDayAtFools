using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CharacterCasterUpdateUIEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (!caster.IsUnitCharacter) return false;
            CombatManager.Instance.AddUIAction(new CharacterUpdateVolatilesUIAction((caster as CharacterCombat).ID, (caster as CharacterCombat).CanSwapNoTrigger, (caster as CharacterCombat).CanUseAbilitiesNoTrigger, shouldPopUp: true));
            return true;
        }
    }
}
