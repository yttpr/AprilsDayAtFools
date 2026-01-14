using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CascadeHealPercentEffect : EffectSO
    {
        public float _cascadeDecay = 0.5f;

        public bool _consistentCascade = true;

        public bool _usePreviousExitValue;

        public bool _directHeal = true;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (_usePreviousExitValue)
            {
                entryVariable += base.PreviousExitValue;
            }

            exitAmount = 0;
            int orig = entryVariable;
            int num = entryVariable;

            TargetSlotInfo start = targets[0];
            if (!start.HasUnit)
            {
                return false;
            }

            List<TargetSlotInfo> list = new List<TargetSlotInfo>();
            List<TargetSlotInfo> list2 = new List<TargetSlotInfo>();
            for (int i = 1; i < targets.Length; i++)
            {
                if (targets[i].Unit != start.Unit)
                {
                    if (targets[i].SlotID > start.SlotID)
                    {
                        list2.Add(targets[i]);
                    }
                    else if (targets[i].SlotID < start.SlotID)
                    {
                        list.Add(targets[i]);
                    }
                }
            }

            if (_directHeal)
            {
                num = caster.WillApplyHeal(num, start.Unit);
            }

            orig = num;
            exitAmount += start.Unit.Heal(num, caster, _directHeal);

            num = orig;
            num = (int)Math.Ceiling((float)num * _cascadeDecay);
            foreach (TargetSlotInfo targetSlotInfo in list)
            {
                if (!targetSlotInfo.HasUnit)
                {
                    if (_consistentCascade)
                    {
                        break;
                    }

                    continue;
                }

                if (_directHeal)
                {
                    num = caster.WillApplyHeal(num, targetSlotInfo.Unit);
                }

                exitAmount += targetSlotInfo.Unit.Heal(num, caster, _directHeal);
                num = (int)Math.Ceiling((float)num * _cascadeDecay);
                if (num <= 0)
                {
                    break;
                }
            }

            num = orig;
            num = (int)Math.Ceiling((float)num * _cascadeDecay);
            foreach (TargetSlotInfo targetSlotInfo in list2)
            {
                if (!targetSlotInfo.HasUnit)
                {
                    if (_consistentCascade)
                    {
                        break;
                    }

                    continue;
                }

                if (_directHeal)
                {
                    num = caster.WillApplyHeal(num, targetSlotInfo.Unit);
                }

                exitAmount += targetSlotInfo.Unit.Heal(num, caster, _directHeal);
                num = (int)Math.Ceiling((float)num * _cascadeDecay);
                if (num <= 0)
                {
                    break;
                }
            }

            return exitAmount > 0;
        }
    }
}
