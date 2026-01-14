using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class DivineProtMod
    {
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(DivineProtectionSE_SO).GetMethod(nameof(DivineProtectionSE_SO.OnEventCall_01), ~BindingFlags.Default), typeof(DivineProtMod).GetMethod(nameof(DivineProtectionSE_SO_OnEventCall_01), ~BindingFlags.Default));
        }
        public static void DivineProtectionSE_SO_OnEventCall_01(Action<DivineProtectionSE_SO, StatusEffect_Holder, object, object> orig, DivineProtectionSE_SO self, StatusEffect_Holder holder, object sender, object args)
        {
            if (args is DamageReceivedValueChangeException reference)
            {
                if (reference.damageTypeID == "Heal")
                {
                    reference.AddModifier(new DivineProtectionHealValueModifier(sender as IUnit, self.StatusID, self.m_DamageID));
                    return;
                }
            }
            orig(self, holder, sender, args);
        }
    }

    public class DivineProtectionHealValueModifier : IntValueModifier
    {
        public readonly IUnit attackedUnit;

        public readonly string m_StatusID;

        public readonly string m_SoundTypeID;

        public DivineProtectionHealValueModifier(IUnit attackedUnit, string statusID, string soundTypeID)
            : base(102)
        {
            this.attackedUnit = attackedUnit;
            m_StatusID = statusID;
            m_SoundTypeID = soundTypeID;
        }

        public override int Modify(int value)
        {
            IntegerReference integerReference = new IntegerReference(value);
            CombatManager.Instance.ProcessImmediateAction(new TriggerDivineProtectionHealImmediateAction(attackedUnit, integerReference, m_StatusID, m_SoundTypeID));
            return integerReference.value;
        }
    }

    public class TriggerDivineProtectionHealImmediateAction : IImmediateAction
    {
        public IUnit _attackedUnit;

        public IntegerReference _damageReference;

        public string _StatusID;

        public string _soundTypeID;

        public TriggerDivineProtectionHealImmediateAction(IUnit attackedUnit, IntegerReference damageReference, string statusID, string soundTypeID)
        {
            _attackedUnit = attackedUnit;
            _damageReference = damageReference;
            _StatusID = statusID;
            _soundTypeID = soundTypeID;
        }

        public void Execute(CombatStats stats)
        {
            List<IUnit> list = new List<IUnit>();
            if (_attackedUnit.IsUnitCharacter)
            {
                foreach (CharacterCombat value in stats.CharactersOnField.Values)
                {
                    if (value.IsAlive && value.ID != _attackedUnit.ID && value.CurrentHealth > 0 && !value.ContainsStatusEffect(_StatusID))
                    {
                        list.Add(value);
                    }
                }
            }
            else
            {
                foreach (EnemyCombat value2 in stats.EnemiesOnField.Values)
                {
                    if (value2.IsAlive && value2.ID != _attackedUnit.ID && !value2.ContainsStatusEffect(_StatusID))
                    {
                        list.Add(value2);
                    }
                }
            }

            if (list.Count > 0)
            {
                float num = _damageReference.value;
                while (list.Count > 0 && num > 0f)
                {
                    int num2 = Mathf.CeilToInt(num / (float)list.Count);
                    int index = UnityEngine.Random.Range(0, list.Count);
                    IUnit unit = list[index];
                    list.RemoveAt(index);
                    unit.Heal(num2, null, false);
                    num -= (float)num2;
                }

                _damageReference.value = 0;
            }
        }
    }
}
