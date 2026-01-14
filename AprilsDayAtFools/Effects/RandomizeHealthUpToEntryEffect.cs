using UnityEngine;

namespace AprilsDayAtFools
{
    public class RandomizeHealthUpToEntryEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit && targetSlotInfo.Unit.IsAlive)
                {
                    int healthTo = Random.Range(1, System.Math.Min(entryVariable + 1, targetSlotInfo.Unit.MaximumHealth + 1));
                    if (targetSlotInfo.Unit.SetHealthTo(healthTo))
                    {
                        exitAmount++;
                    }
                }
            }

            return exitAmount > 0;
        }
    }
}
