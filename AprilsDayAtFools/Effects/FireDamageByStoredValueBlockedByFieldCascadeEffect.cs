using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class FireDamageByStoredValueBlockedByFieldCascadeEffect : EffectSO
    {
        [Header("Cascade Data")]
        public int _cascadeDecrease = 1;

        public bool _consistentCascade = true;

        public bool _decreaseAsPercentage;

        public bool _cascadeIsIndirect;

        [DeathTypeEnumRef]
        public string _DeathTypeID = "Basic";

        public bool _usePreviousExitValue;

        [UnitStoreValueNamesIDsEnumRef]
        public string m_unitStoredDataID = "";

        public int _defaultStoredValue;

        public bool _increaseDamage = true;

        public bool _indirect;

        public bool _returnKillAsSuccess;

        [Header("Field")]
        public FieldEffect_SO _Field;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (_usePreviousExitValue)
            {
                entryVariable *= base.PreviousExitValue;
            }

            exitAmount = 0;
            bool flag = false;
            if (targets.Length == 0)
            {
                return false;
            }

            TargetSlotInfo targetSlotInfo = targets[0];
            if (!targetSlotInfo.HasUnit)
            {
                return false;
            }

            List<TargetSlotInfo> list = new List<TargetSlotInfo>();
            List<TargetSlotInfo> list2 = new List<TargetSlotInfo>();
            for (int i = 1; i < targets.Length; i++)
            {
                if (targets[i].Unit != targetSlotInfo.Unit)
                {
                    if (targets[i].SlotID > targetSlotInfo.SlotID)
                    {
                        list2.Add(targets[i]);
                    }
                    else if (targets[i].SlotID < targetSlotInfo.SlotID)
                    {
                        list.Add(targets[i]);
                    }
                }
            }

            if (!caster.TryGetStoredData(m_unitStoredDataID, out var holder))
            {
                holder.m_MainData = _defaultStoredValue;
            }

            int temp = holder.m_MainData;
            if (!_increaseDamage)
            {
                temp = -temp;
            }

            int damageAmount = ((!stats.combatSlots.UnitInSlotContainsFieldEffect(targetSlotInfo.SlotID, targetSlotInfo.IsTargetCharacterSlot, _Field.FieldID)) ? Mathf.Max(entryVariable + temp, 0) : 0);

            if (targetSlotInfo.Unit.ContainsStatusEffect("OilSlicked_ID") && damageAmount > 0)
            {
                targetSlotInfo.Unit.TryRemoveStatusEffect("OilSlicked_ID");
                damageAmount *= 3;
            }

            DamageInfo damageInfo = DealDamageToTarget(caster, targetSlotInfo, areTargetSlots, damageAmount, _indirect);
            damageAmount = damageInfo.damageAmount;
            flag |= damageInfo.beenKilled;
            exitAmount += damageInfo.damageAmount;
            damageAmount = ((!_decreaseAsPercentage) ? (damageAmount - _cascadeDecrease) : CalculatePercentualDamageAmount(damageAmount));
            if (damageAmount <= 0)
            {
                if (!_indirect && exitAmount > 0)
                {
                    caster.DidApplyDamage(exitAmount);
                }

                if (!_returnKillAsSuccess)
                {
                    return exitAmount > 0;
                }

                return flag;
            }

            int num = 0;
            bool flag2 = true;
            bool flag3 = true;
            for (int j = 0; j < list.Count || j < list2.Count; j++)
            {
                if (flag2)
                {
                    if (j >= list.Count || !list[j].HasUnit)
                    {
                        if (_consistentCascade)
                        {
                            flag2 = false;
                        }
                    }
                    else
                    {
                        damageInfo = DealDamageToTarget(caster, list[j], areTargetSlots, damageAmount, _cascadeIsIndirect);
                        flag |= damageInfo.beenKilled;
                        num += damageInfo.damageAmount;
                    }
                }

                if (flag3)
                {
                    if (j >= list2.Count || !list2[j].HasUnit)
                    {
                        if (_consistentCascade)
                        {
                            flag3 = false;
                        }
                    }
                    else
                    {
                        damageInfo = DealDamageToTarget(caster, list2[j], areTargetSlots, damageAmount, _cascadeIsIndirect);
                        flag |= damageInfo.beenKilled;
                        num += damageInfo.damageAmount;
                    }
                }

                damageAmount = ((!_decreaseAsPercentage) ? (damageAmount - _cascadeDecrease) : CalculatePercentualDamageAmount(damageAmount));
                if (damageAmount <= 0)
                {
                    break;
                }
            }

            int num2 = ((!_indirect) ? exitAmount : 0);
            num2 += ((!_cascadeIsIndirect) ? num : 0);
            if (num2 > 0)
            {
                caster.DidApplyDamage(num2);
            }

            exitAmount += num;
            if (!_returnKillAsSuccess)
            {
                return exitAmount > 0;
            }

            return flag;
        }

        public int CalculatePercentualDamageAmount(int currentDamage)
        {
            float f = (float)_cascadeDecrease * 1f * (float)currentDamage / 100f;
            return Mathf.Max(0, Mathf.FloorToInt(f));
        }

        public DamageInfo DealDamageToTarget(IUnit caster, TargetSlotInfo target, bool areTargetSlots, int damageAmount, bool useIndirect)
        {
            int targetSlotOffset = (areTargetSlots ? (target.SlotID - target.Unit.SlotID) : (-1));
            if (useIndirect)
            {
                return target.Unit.Damage(damageAmount, null, _DeathTypeID, targetSlotOffset, addHealthMana: false, directDamage: false, ignoresShield: true);
            }

            damageAmount = caster.WillApplyDamage(damageAmount, target.Unit);
            return target.Unit.Damage(damageAmount, caster, _DeathTypeID, targetSlotOffset, addHealthMana: true, directDamage: true, false, "Dmg_Fire");
        }
    }
}
