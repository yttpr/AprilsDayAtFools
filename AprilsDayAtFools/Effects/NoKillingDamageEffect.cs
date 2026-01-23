using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class NoKillingDamageEffect : EffectSO
    {
        [DeathTypeEnumRef]
        public string _DeathTypeID = "Basic";

        public bool _usePreviousExitValue;

        public bool _ignoreShield;

        public bool _indirect;

        public bool _returnKillAsSuccess;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (_usePreviousExitValue)
            {
                entryVariable *= base.PreviousExitValue;
            }

            exitAmount = 0;
            bool flag = false;
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit)
                {
                    targetSlotInfo.Unit.SimpleSetStoredValue(WontKillDamageHook.Value, 1);
                    int targetSlotOffset = (areTargetSlots ? (targetSlotInfo.SlotID - targetSlotInfo.Unit.SlotID) : (-1));
                    int amount = entryVariable;
                    DamageInfo damageInfo;
                    if (_indirect)
                    {
                        damageInfo = targetSlotInfo.Unit.Damage(amount, null, _DeathTypeID, targetSlotOffset, addHealthMana: false, directDamage: false, ignoresShield: true);
                    }
                    else
                    {
                        amount = caster.WillApplyDamage(amount, targetSlotInfo.Unit);
                        damageInfo = targetSlotInfo.Unit.Damage(amount, caster, _DeathTypeID, targetSlotOffset, addHealthMana: true, directDamage: true, _ignoreShield);
                    }

                    flag |= damageInfo.beenKilled;
                    exitAmount += damageInfo.damageAmount;
                    targetSlotInfo.Unit.SimpleSetStoredValue(WontKillDamageHook.Value, 0);
                }
            }

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
    }
    public static class WontKillDamageHook
    {
        public static string Value => "Dmg_Spare";
        public static int DamageReceivedValueChangeException_GetModifiedValue(Func<DamageReceivedValueChangeException, int> orig, DamageReceivedValueChangeException self)
        {
            int ret = orig(self);
            if (self.damagedUnit.SimpleGetStoredValue(Value) > 0 && ret > self.damagedUnit.CurrentHealth) return self.damagedUnit.CurrentHealth - 1;
            return ret;
        }
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(DamageReceivedValueChangeException).GetMethod(nameof(DamageReceivedValueChangeException.GetModifiedValue), ~System.Reflection.BindingFlags.Default), typeof(WontKillDamageHook).GetMethod(nameof(DamageReceivedValueChangeException_GetModifiedValue), ~System.Reflection.BindingFlags.Default));
        }
    }
    public static class WontKillDamageExtension
    {
        public static DamageInfo NoKillDamageCH(this CharacterCombat self, int amount, IUnit killer, string deathTypeID, int targetSlotOffset = -1, bool addHealthMana = true, bool directDamage = true, bool ignoresShield = false, string specialDamage = "")
        {
            int num = self.SlotID;
            int num2 = self.SlotID + self.Size - 1;
            if (targetSlotOffset >= 0)
            {
                targetSlotOffset = Mathf.Clamp(self.SlotID + targetSlotOffset, num, num2);
                num = targetSlotOffset;
                num2 = targetSlotOffset;
            }

            DamageReceivedValueChangeException ex = Help.GenerateDamageTakenException(amount, specialDamage, deathTypeID, directDamage, ignoresShield, num, num2, killer, self);
            CombatManager.Instance.PostNotification(TriggerCalls.OnBeingDamaged.ToString(), self, ex);
            int modifiedValue = ex.GetModifiedValue();
            if (modifiedValue >= self.CurrentHealth) modifiedValue = self.CurrentHealth - 1;
            if (killer != null && !killer.Equals(null))
            {
                CombatManager.Instance.ProcessImmediateAction(new UnitDamagedImmediateAction(modifiedValue, killer.IsUnitCharacter));
            }

            int num3 = Mathf.Max(self.CurrentHealth - modifiedValue, 0);
            int num4 = self.CurrentHealth - num3;
            if (num4 != 0)
            {
                self.GetHit();
                self.CurrentHealth = num3;
                if (specialDamage == "")
                {
                    specialDamage = Tools.Utils.GetBasicDamageIDFromAmount(modifiedValue);
                }

                CombatManager.Instance.AddUIAction(new CharacterDamagedUIAction(self.ID, self.CurrentHealth, self.MaximumHealth, modifiedValue, specialDamage));
                if (addHealthMana && self.IsAlive)
                {
                    CombatManager.Instance.ProcessImmediateAction(new AddManaToManaBarAction(self.HealthColor, LoadedDBsHandler.CombatData.CharacterPigmentAmount, self.IsUnitCharacter, self.ID));
                }

                IntegerReference args = Help.GenerateDamageIntReference(num4, specialDamage, directDamage, ignoresShield, num, num2, killer, self);

                CombatManager.Instance.PostNotification(TriggerCalls.OnDamaged.ToString(), self, args);
                string notificationName = (directDamage ? TriggerCalls.OnDirectDamaged.ToString() : TriggerCalls.OnIndirectDamaged.ToString());
                CombatManager.Instance.PostNotification(notificationName, self, args);
            }
            else if (!ex.ShouldIgnoreUI)
            {
                CombatManager.Instance.AddUIAction(new CharacterNotDamagedUIAction(self.ID, CombatType_GameIDs.Dmg_Weak.ToString()));
            }

            bool flag = self.IsAlive && self.CurrentHealth == 0 && num4 != 0;
            if (flag)
            {
                CombatManager.Instance.AddSubAction(new CharacterDeathAction(self.ID, killer, deathTypeID));
            }

            return Help.GenerateDamageInfo(num4, modifiedValue, flag);
        }
        public static DamageInfo NoKillDamageEN(this EnemyCombat self, int amount, IUnit killer, string deathTypeID, int targetSlotOffset = -1, bool addHealthMana = true, bool directDamage = true, bool ignoresShield = false, string specialDamage = "")
        {
            int num = self.SlotID;
            int num2 = self.SlotID + self.Size - 1;
            if (targetSlotOffset >= 0)
            {
                targetSlotOffset = Mathf.Clamp(self.SlotID + targetSlotOffset, num, num2);
                num = targetSlotOffset;
                num2 = targetSlotOffset;
            }

            DamageReceivedValueChangeException ex = Help.GenerateDamageTakenException(amount, specialDamage, deathTypeID, directDamage, ignoresShield, num, num2, killer, self);
            CombatManager.Instance.PostNotification(TriggerCalls.OnBeingDamaged.ToString(), self, ex);
            int modifiedValue = ex.GetModifiedValue();
            if (modifiedValue >= self.CurrentHealth) modifiedValue = self.CurrentHealth - 1;
            if (killer != null && !killer.Equals(null))
            {
                CombatManager.Instance.ProcessImmediateAction(new UnitDamagedImmediateAction(modifiedValue, killer.IsUnitCharacter));
            }

            int num3 = Mathf.Max(self.CurrentHealth - modifiedValue, 0);
            int num4 = self.CurrentHealth - num3;
            if (num4 != 0)
            {
                self.CurrentHealth = num3;
                if (specialDamage == "")
                {
                    specialDamage = Tools.Utils.GetBasicDamageIDFromAmount(modifiedValue);
                }

                CombatManager.Instance.AddUIAction(Help.GenerateEnemyDamagedUIAction(self.ID, self.CurrentHealth, self.MaximumHealth, modifiedValue, specialDamage));
                if (addHealthMana)
                {
                    CombatManager.Instance.ProcessImmediateAction(new AddManaToManaBarAction(self.HealthColor, LoadedDBsHandler.CombatData.EnemyPigmentAmount, self.IsUnitCharacter, self.ID));
                }

                IntegerReference args = Help.GenerateDamageIntReference(num4, specialDamage, deathTypeID, directDamage, ignoresShield, num, num2, killer, self);

                CombatManager.Instance.PostNotification(TriggerCalls.OnDamaged.ToString(), self, args);
                string notificationName = (directDamage ? TriggerCalls.OnDirectDamaged.ToString() : TriggerCalls.OnIndirectDamaged.ToString());
                CombatManager.Instance.PostNotification(notificationName, self, args);
            }
            else if (!ex.ShouldIgnoreUI)
            {
                CombatManager.Instance.AddUIAction(new EnemyNotDamagedUIAction(self.ID));
            }

            bool flag = self.CurrentHealth == 0 && num4 != 0;
            if (flag)
            {
                CombatManager.Instance.AddSubAction(new EnemyDeathAction(self.ID, killer, deathTypeID));
            }

            return Help.GenerateDamageInfo(num4, modifiedValue, flag);
        }
        public static DamageInfo NoKillDamage(this IUnit self, int amount, IUnit killer, string deathTypeID, int targetSlotOffset = -1, bool addHealthMana = true, bool directDamage = true, bool ignoresShield = false, string specialDamage = "")
        {
            if (self is CharacterCombat chara) return chara.NoKillDamageCH(amount, killer, deathTypeID, targetSlotOffset, addHealthMana, directDamage, ignoresShield, specialDamage);
            else return (self as EnemyCombat).NoKillDamageEN(amount, killer, deathTypeID, targetSlotOffset, addHealthMana, directDamage, ignoresShield, specialDamage);
        }

    }
}
