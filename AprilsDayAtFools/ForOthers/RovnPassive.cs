using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace YourNamespace
{
    public class RepeatAbilityNextTurnCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is AbilityUsageReference reference && reference.m_UnitID == effector.ID && reference.m_IsUnitCharacter == effector.IsUnitCharacter)
            {
                Setup();

                if (effector.IsUnitCharacter)
                {
                    PlayerTurnStartList.Add(new AbilitySlotCaster(reference.m_AbilityUsed, effector as IUnit, effector.SlotID));
                }
                else
                {
                    EnemyTurnStartList.Add(new AbilitySlotCaster(reference.m_AbilityUsed, effector as IUnit, effector.SlotID));
                }
                return true;
            }
            return false;
        }

        public struct AbilitySlotCaster
        {
            public AbilitySO _abilty;
            public IUnit _caster;
            public int _slot;

            public AbilitySlotCaster(AbilitySO ability, IUnit caster, int slot)
            {
                _abilty = ability;
                _caster = caster;
                _slot = slot;
            }
        }
        public static List<AbilitySlotCaster> PlayerTurnStartList;
        public static List<AbilitySlotCaster> EnemyTurnStartList;

        public class SlotEffectAction : CombatAction
        {
            public EffectInfo[] _effects;

            public IUnit _caster;

            public int _startResult;

            public int _slot;

            public SlotEffectAction(EffectInfo[] effects, IUnit caster, int slot, int startResult = 0)
            {
                _effects = effects;
                _caster = caster;
                _slot = slot;
                _startResult = startResult;
            }

            public override IEnumerator Execute(CombatStats stats)
            {
                bool isCasterCharacter = _caster.IsUnitCharacter;
                int resultValue = _startResult;
                for (int i = 0; i < _effects.Length; i++)
                {
                    EffectConditionSO condition = _effects[i].condition;
                    if (condition == null || condition.Equals(null) || condition.MeetCondition(_caster, _effects, i))
                    {
                        TargetSlotInfo[] possibleTargets = ((_effects[i].targets != null) ? _effects[i].targets.GetTargets(stats.combatSlots, _slot, isCasterCharacter) : new TargetSlotInfo[0]);
                        bool areTargetSlots = !(_effects[i].targets != null) || _effects[i].targets.AreTargetSlots;
                        resultValue = _effects[i].StartEffect(stats, _caster, possibleTargets, areTargetSlots, resultValue);
                    }
                    else
                    {
                        resultValue = _effects[i].FailEffect(resultValue);
                    }

                    yield return null;
                }
            }
        }
        public class SlotPlayAbilityAnimationAction : CombatAction
        {
            public BaseCombatTargettingSO _targetting;

            public AttackVisualsSO _visuals;

            public IUnit _caster;

            public int _slot;

            public SlotPlayAbilityAnimationAction(AttackVisualsSO visuals, BaseCombatTargettingSO targetting, IUnit caster, int slot)
            {
                _visuals = visuals;
                _targetting = targetting;
                _caster = caster;
                _slot = slot;
            }

            public override IEnumerator Execute(CombatStats stats)
            {
                TargetSlotInfo[] targets = null;
                bool areTargetSlots = true;
                if (_targetting != null)
                {
                    targets = _targetting.GetTargets(stats.combatSlots, _slot, _caster.IsUnitCharacter);
                    areTargetSlots = _targetting.AreTargetSlots;
                }

                yield return stats.combatUI.PlayAbilityAnimation(_visuals, targets, areTargetSlots);
            }
        }
        public class SlotShowAttackInformationUIAction : CombatAction
        {
            public int _id;

            public bool _isUnitCharacter;

            public string _attackName;

            public Vector3 _pos;

            public SlotShowAttackInformationUIAction(int id, bool isUnitCharacter, string attackName, Vector3 pos)
            {
                _id = id;
                _isUnitCharacter = isUnitCharacter;
                _attackName = attackName;
                _pos = pos;
            }

            public override IEnumerator Execute(CombatStats stats)
            {
                Vector3 vector = _pos;
                stats.combatUI.PlaySoundOnPosition(stats.audioController.panelPopUpSound, vector);
                yield return stats.combatUI._popUpHandler3D.StartAttackShowcase(!_isUnitCharacter, vector, _attackName);
            }
        }

        public static void PerformAbility(AbilitySlotCaster obj)
        {
            IUnit unit = obj._caster;

            Vector3 currentPos = ((!unit.IsUnitCharacter) ? CombatManager.Instance._stats.combatUI.TryGetEnemyFieldPosition(unit.ID) : CombatManager.Instance._stats.combatUI.TryGetCharacterFieldPosition(unit.ID));

            CombatManager.Instance.AddSubAction(new SlotShowAttackInformationUIAction(unit.ID, unit.IsUnitCharacter, obj._abilty.GetAbilityLocData().text, currentPos));
            CombatManager.Instance.AddSubAction(new SlotPlayAbilityAnimationAction(obj._abilty.visuals, obj._abilty.animationTarget, unit, obj._slot));
            CombatManager.Instance.AddSubAction(new SlotEffectAction(obj._abilty.effects, unit, obj._slot));
            unit.SetVolatileUpdateUIAction();
        }

        public static void Stats_PlayerTurnStart(Action<CombatStats> orig, CombatStats self)
        {
            orig(self);

            foreach (AbilitySlotCaster caster in PlayerTurnStartList)
            {
                PerformAbility(caster);
            }

            PlayerTurnStartList.Clear();
        }
        public static void Stats_PlayerTurnEnd(Action<CombatStats> orig, CombatStats self)
        {
            foreach (AbilitySlotCaster caster in EnemyTurnStartList)
            {
                PerformAbility(caster);
            }

            EnemyTurnStartList.Clear();

            orig(self);
        }
        public static void Stats_CombatStart(Action<CombatStats> orig, CombatStats self)
        {
            PlayerTurnStartList.Clear();
            EnemyTurnStartList.Clear();
            orig(self);
        }

        public static void Setup()
        {
            if (PlayerTurnStartList == null) PlayerTurnStartList = new List<AbilitySlotCaster>();
            if (EnemyTurnStartList == null) EnemyTurnStartList = new List<AbilitySlotCaster>();

            if (_set) return;
            Hooks();
            _set = true;
        }
        static bool _set;
        public static void Hooks()
        {
            IDetour hook1 = new Hook(typeof(CombatStats).GetMethod(nameof(CombatStats.PlayerTurnStart), ~BindingFlags.Default), typeof(RepeatAbilityNextTurnCondition).GetMethod(nameof(Stats_PlayerTurnStart), ~BindingFlags.Default));
            IDetour hook2 = new Hook(typeof(CombatStats).GetMethod(nameof(CombatStats.PlayerTurnEnd), ~BindingFlags.Default), typeof(RepeatAbilityNextTurnCondition).GetMethod(nameof(Stats_PlayerTurnEnd), ~BindingFlags.Default));
            IDetour hook3 = new Hook(typeof(CombatStats).GetMethod(nameof(CombatStats.FinalizeInitialization), ~BindingFlags.Default), typeof(RepeatAbilityNextTurnCondition).GetMethod(nameof(Stats_CombatStart), ~BindingFlags.Default));
        }
    }
}
