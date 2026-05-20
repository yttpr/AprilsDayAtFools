using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class SwapStatusEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (targets == null || targets.Length < 2) return false;
            if (!targets[0].HasUnit || !targets[1].HasUnit) return false;
            if (targets[0].Unit == targets[1].Unit) return false;
            if (targets[0].Unit.StatusEffectCount <= 0 && targets[1].Unit.StatusEffectCount <= 0) return false;
            //EnemyCombat
            List<StatusEffect_SO> stati = [];
            List<int> amounts = [];

            int split = 0;

            IStatusEffector first = targets[0].Unit as IStatusEffector;
            IStatusEffector second = targets[1].Unit as IStatusEffector;

            for (int i = 0; i < first.StatusEffects.Count; i++)
            {
                if (first.StatusEffects[i].StatusContent <= 0) continue;
                stati.Add((first.StatusEffects[i] as StatusEffect_Holder)._Status);
                amounts.Add(first.StatusEffects[i].StatusContent);

                split++;
            }

            for (int i = 0; i < second.StatusEffects.Count; i++)
            {
                if (second.StatusEffects[i].StatusContent <= 0) continue;
                stati.Add((second.StatusEffects[i] as StatusEffect_Holder)._Status);
                amounts.Add(second.StatusEffects[i].StatusContent);
            }

            if (stati.Count <= 0) return false;

            foreach (TargetSlotInfo target in targets)
                if (target.HasUnit) exitAmount += target.Unit.TryRemoveAllStatusEffects();

            for (int i = 0; i < split; i++)
            {
                targets[1].Unit.ApplyStatusEffect(stati[i], amounts[i]);
            }
            for (int i = split; i < stati.Count; i++)
            {
                targets[0].Unit.ApplyStatusEffect(stati[i], amounts[i]);
            }

            return exitAmount > 0;
        }
    }
}
