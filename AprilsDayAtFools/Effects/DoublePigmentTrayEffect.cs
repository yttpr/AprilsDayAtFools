using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class GeneratePigmentTrayEffect : GenerateColorManaEffect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            List<ManaColorSO> copy = new List<ManaColorSO>();
            foreach (ManaBarSlot slots in stats.MainManaBar.ManaBarSlots)
            {
                if (slots.IsEmpty) continue;
                copy.Add(slots.ManaColor);
            }
            foreach (ManaColorSO mana in copy)
            {
                this.mana = mana;
                if (base.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable, out int exi)) exitAmount += exi;
            }
            return exitAmount > 0;
        }
    }
}
