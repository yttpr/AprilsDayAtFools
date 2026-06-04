using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace YourNamespace
{
    public static class TargettedAlreadyHandler_ByPassive
    {
        public static void Setup(string passiveid)
        {
            _passiveID = passiveid;

            IDetour hook1 = new Hook(typeof(EffectInfo).GetMethod(nameof(EffectInfo.StartEffect), ~BindingFlags.Default), typeof(TargettedAlreadyHandler_ByPassive).GetMethod(nameof(TargettedAlreadyHandler_StartEffect), ~BindingFlags.Default));

            Fools = new Dictionary<IUnit, List<int>>();
            Enemies = new Dictionary<IUnit, List<int>>();

            Past_Fools = new Dictionary<IUnit, List<int>>();
            Past_Enemies = new Dictionary<IUnit, List<int>>();

            IDetour hook2 = new Hook(typeof(CombatManager).GetMethod(nameof(CombatManager.InitializeCombat), ~BindingFlags.Default), typeof(TargettedAlreadyHandler_ByPassive).GetMethod(nameof(CombatManager_InitializeCombat), ~BindingFlags.Default));
            IDetour hook3 = new Hook(typeof(CombatStats).GetMethod(nameof(CombatStats.PlayerTurnStart), ~BindingFlags.Default), typeof(TargettedAlreadyHandler_ByPassive).GetMethod(nameof(Stats_PlayerTurnStart), ~BindingFlags.Default));

            IDetour hook4 = new Hook(typeof(PlayAbilityAnimationAction).GetMethod(nameof(PlayAbilityAnimationAction.Execute), ~BindingFlags.Default), typeof(TargettedAlreadyHandler_ByPassive).GetMethod(nameof(PlayAbilityAnimationAction_Execute), ~BindingFlags.Default));

        }


        public static string _passiveID = "Temp";
        public static string PassiveID => _passiveID;

        public static Dictionary<IUnit, List<int>> Fools;
        public static Dictionary<IUnit, List<int>> Enemies;

        public static Dictionary<IUnit, List<int>> Past_Fools;
        public static Dictionary<IUnit, List<int>> Past_Enemies;
        public static int TargettedAlreadyHandler_StartEffect(Func<EffectInfo, CombatStats, IUnit, TargetSlotInfo[], bool, int, int> orig, EffectInfo self, CombatStats stats, IUnit caster, TargetSlotInfo[] possibleTargets, bool areTargetSlots, int previousExitValue)
        {
            if (caster != null && possibleTargets != null)
            {
                List<int> fools = [];
                List<int> enemies = [];

                foreach (TargetSlotInfo target in possibleTargets)
                {
                    if (target.IsTargetCharacterSlot) fools.Add(target.SlotID);
                    else enemies.Add(target.SlotID);

                    if (target.HasUnit && !areTargetSlots)
                    {
                        for (int i = 0; i < target.Unit.Size; i++)
                        {
                            if (target.IsTargetCharacterSlot) fools.Add(target.Unit.SlotID + i);
                            else enemies.Add(target.Unit.SlotID + i);
                        }
                    }
                }

                CombatManager.Instance.AddSubAction(new AddQueuedSlotsAction(caster, fools.ToArray(), enemies.ToArray()));

                if (caster.ContainsPassiveAbility(PassiveID) && areTargetSlots)
                {
                    ShowPassive(caster, PassiveID);

                    List<TargetSlotInfo> ret = [..possibleTargets];

                    if (fools.Count > 0)
                    {
                        foreach (CombatSlot slot in stats.combatSlots.CharacterSlots)
                        {
                            if (CheckSlot(caster, slot.SlotID, true) && !ret.Contains(slot.TargetSlotInformation))
                                ret.Add(slot.TargetSlotInformation);
                        }
                    }
                    if (enemies.Count > 0)
                    {
                        foreach (CombatSlot slot in stats.combatSlots.EnemySlots)
                        {
                            if (CheckSlot(caster, slot.SlotID, false) && !ret.Contains(slot.TargetSlotInformation))
                                ret.Add(slot.TargetSlotInformation);
                        }
                    }

                    return orig(self, stats, caster, ret.ToArray(), areTargetSlots, previousExitValue);
                }
            }

            return orig(self, stats, caster, possibleTargets, areTargetSlots, previousExitValue);
        }
        public static void AddSlot(IUnit caster, int slot, bool party_side)
        {
            if (party_side)
            {
                if (Fools == null) Fools = new Dictionary<IUnit, List<int>>();
                if (!Fools.ContainsKey(caster) || Fools[caster] == null)
                    Fools[caster] = [];
                if (!Fools[caster].Contains(slot))
                    Fools[caster].Add(slot);
            }
            else
            {
                if (Enemies == null) Enemies = new Dictionary<IUnit, List<int>>();
                if (!Enemies.ContainsKey(caster) || Enemies[caster] == null)
                    Enemies[caster] = [];
                if (!Enemies[caster].Contains(slot))
                    Enemies[caster].Add(slot);
            }
        }
        public static bool CheckSlot(IUnit caster, int slot, bool party_side)
        {
            if (party_side)
            {
                if (Past_Fools != null && Past_Fools.ContainsKey(caster) && Past_Fools[caster] != null && Past_Fools[caster].Contains(slot))
                    return true;

                return Fools != null && Fools.ContainsKey(caster) && Fools[caster] != null && Fools[caster].Contains(slot);
            }
            else
            {
                if (Past_Enemies != null && Past_Enemies.ContainsKey(caster) && Past_Enemies[caster] != null && Past_Enemies[caster].Contains(slot))
                    return true;

                return Enemies != null && Enemies.ContainsKey(caster) && Enemies[caster] != null && Enemies[caster].Contains(slot);
            }
        }
        public static bool CheckContainsCaster(IUnit caster)
        {
            if (Past_Fools != null && Past_Fools.ContainsKey(caster) && Past_Fools[caster] != null && Past_Fools[caster].Count > 0)
                return true;
            if (Past_Enemies != null && Past_Enemies.ContainsKey(caster) && Past_Enemies[caster] != null && Past_Enemies[caster].Count > 0)
                return true;
            if (Fools != null && Fools.ContainsKey(caster) && Fools[caster] != null && Fools[caster].Count > 0)
                return true;
            if (Enemies != null && Enemies.ContainsKey(caster) && Enemies[caster] != null && Enemies[caster].Count > 0)
                return true;
            return false;
        }
        public static void CombatManager_InitializeCombat(Action<CombatManager> orig, CombatManager self)
        {
            orig(self);
            Fools.Clear();
            Enemies.Clear();
            Past_Fools.Clear();
            Past_Enemies.Clear();
        }
        public static void Stats_PlayerTurnStart(Action<CombatStats> orig, CombatStats self)
        {
            Past_Fools = Fools;
            Past_Enemies = Enemies;

            Fools = new Dictionary<IUnit, List<int>>();
            Enemies = new Dictionary<IUnit, List<int>>();

            orig(self);
        }
        public static void ShowPassive(IUnit unit, string id)
        {
            if (unit.ContainsPassiveAbility(id) && unit is IPassiveEffector effector)
            {
                foreach (BasePassiveAbilitySO passive in effector.PassiveAbilities)
                {
                    if (passive != null && passive.m_PassiveID == id && passive.doesPassiveTriggerInformationPanel)
                    {
                        CombatManager.Instance.AddUIAction(new NonAggregate_ShowPassiveInformationUIAction(unit.ID, unit.IsUnitCharacter, passive.GetPassiveLocData().text, passive.passiveIcon));
                        return;
                    }
                }
            }
        }

        public class AddQueuedSlotsAction : CombatAction
        {
            public IUnit _caster;
            public int[] _fools;
            public int[] _enemies;

            public AddQueuedSlotsAction(IUnit caster, int[] fools, int[] enemies)
            {
                _caster = caster;
                _fools = fools;
                _enemies = enemies;
            }

            public override IEnumerator Execute(CombatStats stats)
            {
                foreach (int num in _fools)
                {
                    AddSlot(_caster, num, true);
                }
                foreach (int num in _enemies)
                {
                    AddSlot(_caster, num, false);
                }
                yield break;
            }
        }
        public class NonAggregate_ShowPassiveInformationUIAction : ShowPassiveInformationUIAction
        {
            public bool _skip;
            public static List<NonAggregate_ShowPassiveInformationUIAction> instances;

            public NonAggregate_ShowPassiveInformationUIAction(int unitID, bool isUnitCharacter, string passiveName, Sprite passiveIcon) : base(unitID, isUnitCharacter, passiveName, passiveIcon)
            {
                if (instances == null) instances = [];

                foreach (NonAggregate_ShowPassiveInformationUIAction inst in instances)
                {
                    if (inst._id == unitID && inst._isUnitCharacter == isUnitCharacter && inst._passiveName == passiveName)
                    {
                        _skip = true;
                        return;
                    }
                }

                _skip = false;
                instances.Add(this);
            }

            public override IEnumerator Execute(CombatStats stats)
            {
                if (_skip)
                {
                    yield break;
                }
                else
                {
                    instances.Remove(this);
                    yield return base.Execute(stats);
                }
            }
        }
        public static IEnumerator PlayAbilityAnimationAction_Execute(Func<PlayAbilityAnimationAction, CombatStats, IEnumerator> orig, PlayAbilityAnimationAction self, CombatStats stats)
        {
            if (!self._caster.ContainsPassiveAbility(PassiveID) || self._targetting == null || self._targetting.Equals(null) || (self._targetting != null && !self._targetting.AreTargetSlots))
            {
                yield return orig(self, stats);
            }
            else
            {
                TargetSlotInfo[] targets = null;
                bool areTargetSlots = true;
                if (self._targetting != null)
                {
                    targets = self._targetting.GetTargets(stats.combatSlots, self._caster.SlotID, self._caster.IsUnitCharacter);
                    areTargetSlots = self._targetting.AreTargetSlots;
                }

                bool addFool = false;
                bool addEnemy = false;

                List<TargetSlotInfo> ret = [];

                foreach (TargetSlotInfo target in targets)
                {
                    if (target.IsTargetCharacterSlot) addFool = true;
                    else addEnemy = true;
                    ret.Add(target);
                }

                if (addFool)
                {
                    foreach (CombatSlot slot in stats.combatSlots.CharacterSlots)
                    {
                        if (CheckSlot(self._caster, slot.SlotID, true) && !ret.Contains(slot.TargetSlotInformation))
                            ret.Add(slot.TargetSlotInformation);
                    }
                }
                if (addEnemy)
                {
                    foreach (CombatSlot slot in stats.combatSlots.EnemySlots)
                    {
                        if (CheckSlot(self._caster, slot.SlotID, false) && !ret.Contains(slot.TargetSlotInformation))
                            ret.Add(slot.TargetSlotInformation);
                    }
                }

                yield return stats.combatUI.PlayAbilityAnimation(self._visuals, targets, areTargetSlots);
            }
        }
    }
}
