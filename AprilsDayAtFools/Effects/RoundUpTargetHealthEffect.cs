using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class RoundUpTargetHealthEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    int start = target.Unit.CurrentHealth;
                    if (start >= entryVariable) continue;

                    if (target.Unit.ContainsStatusEffect(StatusField_GameIDs.Gutted_ID.ToString()))
                    {
                        target.Unit.MaximizeHealth(entryVariable);
                    }
                    if (target.Unit.SetHealthTo(Math.Min(entryVariable, target.Unit.MaximumHealth)))
                    {
                        exitAmount += target.Unit.CurrentHealth - start;
                    }
                }
            }

            return exitAmount > 0;
        }
    }
}
