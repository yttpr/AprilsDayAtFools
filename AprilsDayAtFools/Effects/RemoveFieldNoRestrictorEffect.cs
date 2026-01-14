using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class RemoveFieldNoRestrictorEffect : EffectSO
    {
        public string _field;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (_field == null)
            {
                return false;
            }

            foreach (TargetSlotInfo target in targets)
            {
                int amountAndRemoveFieldEffect = GetAmountAndRemoveFieldEffect(stats, target);
                exitAmount += amountAndRemoveFieldEffect;
            }

            return exitAmount > 0;
        }

        public int GetAmountAndRemoveFieldEffect(CombatStats stats, TargetSlotInfo target)
        {
            CombatSlot combatSlot = ((!target.IsTargetCharacterSlot) ? stats.combatSlots.EnemySlots[target.SlotID] : stats.combatSlots.CharacterSlots[target.SlotID]);
            int num = 0;
            int restrictor = 0;
            IFieldEffect field = null;
            foreach (IFieldEffect fieldEffect in combatSlot.FieldEffects)
            {
                if (!(fieldEffect.FieldID != _field))
                {
                    num = fieldEffect.FieldContent;
                    restrictor = fieldEffect.Restrictor;
                    field = fieldEffect;
                    break;
                }
            }

            if (num > 0)
            {
                if (restrictor > 0)
                {
                    if (field != null)
                    {
                        int removed = field.JustRemoveAllContent();
                        (field as FieldEffect_Holder).Effector.FieldEffectValuesChanged(_field, useSpecialSound: false, removed * -1, false);
                    }
                }
                else combatSlot.RemoveFieldEffect(_field);
            }

            return num;
        }

        public static RemoveFieldNoRestrictorEffect Create(string field)
        {
            RemoveFieldNoRestrictorEffect ret = ScriptableObject.CreateInstance<RemoveFieldNoRestrictorEffect>();
            ret._field = field;
            return ret;
        }
    }
}
