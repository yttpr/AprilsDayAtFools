using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ExitByPigmentUsedEffect : EffectSO
    {
        public ManaColorSO mana;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (ManaColorSO pigment in PigmentUsedCollector.lastUsed)
            {
                if (pigment.SharesPigmentColor(mana)) exitAmount++;
            }
            return exitAmount > 0;
        }
    }
}
