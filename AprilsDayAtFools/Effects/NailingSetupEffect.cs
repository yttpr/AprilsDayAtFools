using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class NailingSetupEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    target.Unit.SimpleSetStoredValue(NailingHandler.Record, 0);
                    target.Unit.SimpleSetStoredValue(NailingHandler.Initial, 1);
                }
            }
            return true;
        }
    }
    public class NailingTriggerEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    int num4 = target.Unit.SimpleGetStoredValue(NailingHandler.Record);

                    target.Unit.SimpleSetStoredValue(NailingHandler.Record, 0);
                    target.Unit.SimpleSetStoredValue(NailingHandler.Initial, 0);

                    if (num4 <= 0) continue;

                    int num = target.Unit.SlotID;
                    int num2 = target.Unit.SlotID + target.Unit.Size - 1;
                    int targetSlotOffset = (areTargetSlots ? (target.SlotID - target.Unit.SlotID) : (-1));
                    if (targetSlotOffset >= 0)
                    {
                        targetSlotOffset = Mathf.Clamp(target.Unit.SlotID + targetSlotOffset, num, num2);
                        num = targetSlotOffset;
                        num2 = targetSlotOffset;
                    }

                    IntegerReference args = Help.GenerateDamageIntReference(num4, Tools.Utils.GetBasicDamageIDFromAmount(num4), "Basic", target.Unit.SimpleGetStoredValue(NailingHandler.Direct) > 0, target.Unit.SimpleGetStoredValue(NailingHandler.Direct) <= 0, target.SlotID, target.SlotID, caster, target.Unit);

                    CombatManager.Instance.PostNotification(TriggerCalls.OnDamaged.ToString(), target.Unit, args);

                    string notificationName = (target.Unit.SimpleGetStoredValue(NailingHandler.Direct) > 0) ? TriggerCalls.OnDirectDamaged.ToString() : TriggerCalls.OnIndirectDamaged.ToString();
                    CombatManager.Instance.PostNotification(notificationName, target.Unit, args);
                }
            }
            return true;
        }
    }
}
