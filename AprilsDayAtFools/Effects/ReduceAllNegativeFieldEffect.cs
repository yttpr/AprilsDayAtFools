using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class ReduceAllNegativeFieldEffect : EffectSO
    {
        [SerializeField]
        public List<string> Exclude = new List<string>();
        public int ReduceFieldEffects(int amount, CombatStats stats, TargetSlotInfo target)
        {
            CombatSlot combatSlot = ((!target.IsTargetCharacterSlot) ? stats.combatSlots.EnemySlots[target.SlotID] : stats.combatSlots.CharacterSlots[target.SlotID]);
            int ret = 0;
            int num = 0;
            int restrictor = 0;
            foreach (IFieldEffect fieldEffect in new List<IFieldEffect>(combatSlot.FieldEffects))
            {
                if (fieldEffect.IsPositive || Exclude.Contains(fieldEffect.FieldID)) continue;

                num = fieldEffect.FieldContent;
                restrictor = fieldEffect.Restrictor;

                if (num > 0)
                {
                    if (num > Math.Abs(amount))
                    {
                        if (fieldEffect.TryAddContent(amount, 0))
                        {
                            combatSlot.FieldEffectValuesChanged(fieldEffect.FieldID, false, amount, true);
                            ret += Math.Abs(amount);
                        }
                    }
                    else
                    {
                        if (restrictor > 0)
                        {
                            int removed = fieldEffect.JustRemoveAllContent();
                            (fieldEffect as FieldEffect_Holder).Effector.FieldEffectValuesChanged(fieldEffect.FieldID, useSpecialSound: false, removed * -1, false);
                        }
                        else combatSlot.RemoveFieldEffect(fieldEffect.FieldID);

                        ret += Math.Max(1, num);
                    }
                }
            }

            return ret;
        }
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            entryVariable = -1 * Math.Abs(entryVariable);
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                exitAmount += ReduceFieldEffects(entryVariable, stats, targetSlotInfo);
            }
            return exitAmount > 0;
        }
    }
}
