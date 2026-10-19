using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class NightmareRefreshEffect : EffectSO
    {
        public bool AlsoMovement;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    if (target.Unit.GetStatusAmount(Drowning.StatusID, true) <= entryVariable)
                    {
                        target.Unit.RefreshAbilityUse();
                        if (AlsoMovement) target.Unit.RestoreSwapUse();

                        if (!target.Unit.ContainsStatusEffect(Drowning.StatusID))
                            target.Unit.ApplyStatusEffect(Drowning.Object, 1);
                        else
                            target.Unit.ApplyStatusEffect(Drowning.Object, target.Unit.GetStatusAmount(Drowning.StatusID, true));

                        exitAmount++;
                    }
                    else
                    {
                        int num = target.Unit.TryRemoveStatusEffect(Drowning.StatusID);
                        if (target.IsTargetCharacterSlot)
                            stats.combatSlots.CharacterSlots[target.SlotID].ApplyFieldEffect(Water.Object, num, 0);
                        else
                            stats.combatSlots.EnemySlots[target.SlotID].ApplyFieldEffect(Water.Object, num, 0);
                    }
                }
            }
            return exitAmount > 0;
        }
    }
}
