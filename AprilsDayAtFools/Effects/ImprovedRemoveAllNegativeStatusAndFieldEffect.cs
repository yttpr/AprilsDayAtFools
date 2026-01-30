using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class ImprovedRemoveAllNegativeStatusAndFieldEffect : EffectSO
    {
        [SerializeField]
        public List<string> Exclude = new List<string>();
        public int RemoveFieldEffects(CombatStats stats, TargetSlotInfo target)
        {
            CombatSlot combatSlot = ((!target.IsTargetCharacterSlot) ? stats.combatSlots.EnemySlots[target.SlotID] : stats.combatSlots.CharacterSlots[target.SlotID]);
            int ret = 0;
            int num = 0;
            int restrictor = 0;
            foreach (IFieldEffect fieldEffect in new List<IFieldEffect>(combatSlot.FieldEffects))
            {
                if (fieldEffect.IsPositive) continue;

                num = fieldEffect.FieldContent;
                restrictor = fieldEffect.Restrictor;

                if (num > 0)
                {
                    if (restrictor > 0)
                    {
                        int removed = fieldEffect.JustRemoveAllContent();
                        (fieldEffect as FieldEffect_Holder).Effector.FieldEffectValuesChanged(fieldEffect.FieldID, useSpecialSound: false, removed * -1, false);
                    }
                    else combatSlot.RemoveFieldEffect(fieldEffect.FieldID);

                    ret += num;
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
                if (targetSlotInfo.HasUnit)
                {
                    if (targetSlotInfo.Unit is IStatusEffector effector)
                    {
                        foreach (IStatusEffect status in new List<IStatusEffect>(effector.StatusEffects))
                        {
                            if (!status.IsPositive && !Exclude.Contains(status.StatusID))
                            {
                                exitAmount += Math.Max(1, targetSlotInfo.Unit.TryRemoveStatusEffect(status.StatusID));
                            }
                        }
                    }

                    exitAmount += RemoveFieldEffects(stats, targetSlotInfo);
                }
            }
            return exitAmount > 0;
        }
    }
}
