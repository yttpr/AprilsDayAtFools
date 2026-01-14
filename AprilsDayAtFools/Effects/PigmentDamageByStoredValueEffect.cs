using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class PigmentDamageByStoredValueEffect : EffectSO
    {
        [DeathTypeEnumRef]
        public string _DeathTypeID = "Basic";

        [UnitStoreValueNamesIDsEnumRef]
        public string m_unitStoredDataID = "";

        public int _defaultStoredValue;

        public bool _increaseDamage = true;

        public bool _indirect;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (!caster.TryGetStoredData(m_unitStoredDataID, out var holder))
            {
                holder.m_MainData = _defaultStoredValue;
            }

            int num = holder.m_MainData;
            if (!_increaseDamage)
            {
                num = -num;
            }

            int amount = Mathf.Max(entryVariable + num, 0);
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit)
                {
                    int targetSlotOffset = (areTargetSlots ? (targetSlotInfo.SlotID - targetSlotInfo.Unit.SlotID) : (-1));
                    DamageInfo damageInfo;
                    if (_indirect)
                    {
                        damageInfo = targetSlotInfo.Unit.Damage(amount, null, _DeathTypeID, targetSlotOffset, addHealthMana: false, directDamage: false, ignoresShield: true, "Dmg_Pigment");
                    }
                    else
                    {
                        int amount2 = caster.WillApplyDamage(amount, targetSlotInfo.Unit);
                        damageInfo = targetSlotInfo.Unit.Damage(amount2, caster, _DeathTypeID, targetSlotOffset, true, true, false, "Dmg_Pigment");
                    }

                    exitAmount += damageInfo.damageAmount;
                }
            }

            if (!_indirect && exitAmount > 0)
            {
                caster.DidApplyDamage(exitAmount);
            }

            return exitAmount > 0;
        }
    }
}
