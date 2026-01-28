using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class DelayedAttackManager
    {
        public static List<DelayedAttack> Attacks = new List<DelayedAttack>();
        public static AttackVisualsSO CrushAnim => CustomVisuals.GetVisuals("Salt/Cannon");
        public static TargetSlotInfo[] Targets(bool playerTurn)
        {
            List<TargetSlotInfo> targets = new List<TargetSlotInfo>();
            foreach (DelayedAttack attack in Attacks)
            {
                if (attack.caster != null && !attack.caster.IsAlive) continue;
                if (!targets.Contains(attack.Target) && (playerTurn == attack.caster.IsUnitCharacter || attack.caster == null)) targets.Add(attack.Target);
            }
            return targets.ToArray();
        }
        public static IUnit[] Attackers
        {
            get
            {
                List<IUnit> casters = new List<IUnit>();
                foreach (DelayedAttack attack in Attacks)
                {
                    if (attack.caster != null && !casters.Contains(attack.caster)) casters.Add(attack.caster);
                }
                return casters.ToArray();
            }
        }
        public static TargetSlotInfo[] TargetsForUnit(IUnit unit)
        {
            List<TargetSlotInfo> targets = new List<TargetSlotInfo>();
            foreach (DelayedAttack attack in Attacks)
            {
                if (!targets.Contains(attack.Target) && attack.caster != null && attack.caster == unit) targets.Add(attack.Target);
            }
            return targets.ToArray();
        }
        public static DelayedAttack[] AttacksForUnit(IUnit unit)
        {
            List<DelayedAttack> targets = new List<DelayedAttack>();
            foreach (DelayedAttack attack in Attacks)
            {
                if (attack.caster != null && attack.caster == unit) targets.Add(attack);
            }
            return targets.ToArray();
        }
        public static void CleanList(bool playerTurn)
        {
            List<DelayedAttack> ret = new List<DelayedAttack>();
            foreach (DelayedAttack attack in Attacks)
            {
                if (attack.caster != null && attack.caster.IsUnitCharacter != playerTurn) ret.Add(attack);
            }
            Attacks = ret;
        }
        public static void PlayerTurnStart(Action<CombatStats> orig, CombatStats self)
        {
            orig(self);
            CombatManager.Instance.AddRootAction(new PerformDelayedAttacksAction(true));
        }
        public static void PlayerTurnEnd(Action<CombatStats> orig, CombatStats self)
        {
            orig(self);
            CombatManager.Instance.AddRootAction(new PerformDelayedAttacksAction(false));
        }
        public static void FinalizeCombat(Action<CombatStats> orig, CombatStats self)
        {
            orig(self);
            Attacks.Clear();
        }

        public static void UIInitialization(Action<CombatStats> orig, CombatStats self)
        {
            //CombatStarterPastCombatStart.Reset();
            //WaterView.Reset();
            orig(self);
            Attacks.Clear();
        }

        public static void Setup()
        {
            IDetour CombatEnd = new Hook(typeof(CombatStats).GetMethod(nameof(CombatStats.FinalizeCombat), ~BindingFlags.Default), typeof(DelayedAttackManager).GetMethod(nameof(FinalizeCombat), ~BindingFlags.Default));
            IDetour CombatStart = new Hook(typeof(CombatStats).GetMethod(nameof(CombatStats.UIInitialization), ~BindingFlags.Default), typeof(DelayedAttackManager).GetMethod(nameof(UIInitialization), ~BindingFlags.Default));
            IDetour TurnStart = new Hook(typeof(CombatStats).GetMethod(nameof(CombatStats.PlayerTurnStart), ~BindingFlags.Default), typeof(DelayedAttackManager).GetMethod(nameof(PlayerTurnStart), ~BindingFlags.Default));
            IDetour TurnEnd = new Hook(typeof(CombatStats).GetMethod(nameof(CombatStats.PlayerTurnEnd), ~BindingFlags.Default), typeof(DelayedAttackManager).GetMethod(nameof(PlayerTurnEnd), ~BindingFlags.Default));
        }

    }
    public class DelayedAttack
    {
        public int Damage;
        public TargetSlotInfo Target;
        public IUnit caster;

        public DelayedAttack(int damage, TargetSlotInfo target, IUnit caster)
        {
            Damage = damage;
            Target = target;
            this.caster = caster;
        }

        public void Add()
        {
            DelayedAttackManager.Attacks.Add(this);
            DelayedAttackVisualizer.UpdateVisuals();
        }

        public int Perform()
        {
            if (caster != null && caster.IsAlive)
            {
                if (Target.HasUnit)
                {
                    int amount = caster.WillApplyDamage(Damage, Target.Unit);
                    DamageInfo hit = Target.Unit.Damage(amount, caster, DeathType_GameIDs.Basic.ToString(), -1, true, true, false);
                    return hit.damageAmount;
                }

            }
            else
            {
                if (Target.HasUnit)
                {
                    Target.Unit.Damage(Damage, null, DeathType_GameIDs.Basic.ToString(), -1, true, true, false);
                    return 0;
                }
            }
            return 0;
        }
    }
    public class PlayAnimationAnywhereAction : CombatAction
    {
        public AttackVisualsSO _visuals;

        public TargetSlotInfo[] _targets;

        public PlayAnimationAnywhereAction(AttackVisualsSO visuals, TargetSlotInfo[] targets)
        {
            _visuals = visuals;
            _targets = targets;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            if (_targets.Length > 0)
                yield return stats.combatUI.PlayAbilityAnimation(_visuals, _targets, true);
        }
    }
    public class PerformDelayedAttacksAction : CombatAction
    {
        public PerformDelayedAttacksAction(bool playerTurn)
        {
            PlayerTurn = playerTurn;
        }

        public bool PlayerTurn;

        public override IEnumerator Execute(CombatStats stats)
        {
            CombatManager.Instance.AddSubAction(new PlayAnimationAnywhereAction(DelayedAttackManager.CrushAnim, DelayedAttackManager.Targets(PlayerTurn)));
            foreach (IUnit unit in DelayedAttackManager.Attackers)
            {
                if (PlayerTurn == unit.IsUnitCharacter)
                {
                    if (!PlayerTurn && (!unit.IsAlive || unit.HasFled)) continue;
                    ReturnMultiTargets targets = ScriptableObject.CreateInstance<ReturnMultiTargets>();
                    targets.Targets = DelayedAttackManager.TargetsForUnit(unit);
                    PerformDelayedAttackEffect attack = ScriptableObject.CreateInstance<PerformDelayedAttackEffect>();
                    attack.Attacks = DelayedAttackManager.AttacksForUnit(unit);
                    EffectInfo effect = Effects.GenerateEffect(attack, 0, targets);
                    EffectInfo[] info = new EffectInfo[] { effect };
                    CombatManager.Instance.AddSubAction(new EffectAction(info, unit));
                }
            }
            CombatManager.Instance.AddSubAction(new PerformCasterlessDelayedAttacksAction(DelayedAttackManager.Attacks.ToArray()));
            DelayedAttackManager.CleanList(PlayerTurn);
            DelayedAttackVisualizer.UpdateVisuals();
            yield return null;
        }
    }
    public class PerformCasterlessDelayedAttacksAction : CombatAction
    {
        public PerformCasterlessDelayedAttacksAction(DelayedAttack[] attacks)
        {
            List<DelayedAttack> ret = new List<DelayedAttack>();
            foreach (DelayedAttack hit in attacks)
            {
                if (hit.caster == null) ret.Add(hit);
            }
            Attacks = ret.ToArray();
        }
        public DelayedAttack[] Attacks;

        public override IEnumerator Execute(CombatStats stats)
        {
            foreach (DelayedAttack hit in Attacks) hit.Perform();
            yield return null;
        }
    }
    public class ReturnSingleTarget : BaseCombatTargettingSO
    {
        public TargetSlotInfo Target;

        public override bool AreTargetAllies => false;

        public override bool AreTargetSlots => true;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            return new TargetSlotInfo[] { Target };
        }
    }
    public class ReturnMultiTargets : BaseCombatTargettingSO
    {
        public TargetSlotInfo[] Targets;

        public override bool AreTargetAllies => false;

        public override bool AreTargetSlots => true;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            return Targets;
        }
    }
    public class PerformDelayedAttackEffect : EffectSO
    {
        public DelayedAttack[] Attacks;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (DelayedAttack attack in Attacks)
            {
                exitAmount += attack.Perform();
            }
            if (exitAmount > 0)
                caster.DidApplyDamage(exitAmount);
            return exitAmount > 0;
        }
    }
    public class AddDelayedAttackEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                new DelayedAttack(entryVariable, target, caster).Add();
                exitAmount += entryVariable;
            }
            return exitAmount > 0;
        }
    }
    public static class DelayedAttackVisualizer
    {
        public static On_Off_CFE_Layout FoolLayout;
        public static On_Off_EFE_Layout EnemyLayout;
        public static void Add()
        {
            GameObject Fool = Joyce.Assets.LoadAsset<GameObject>("Assets/train/DelayFool.prefab");
            FoolLayout = Fool.AddComponent<On_Off_CFE_Layout>();
            FoolLayout.name = "DelayedAttack_Fool";
            FoolLayout.m_Back = new RectTransform[] { Fool.GetComponent<RectTransform>() };
            FoolLayout.m_Objects = new GameObject[] { Fool };
            FoolLayout._Animators = [Fool.GetComponent<Animator>()];

            GameObject Enemy = Joyce.Assets.LoadAsset<GameObject>("Assets/train/DelayEnemy.prefab");
            EnemyLayout = Enemy.AddComponent<On_Off_EFE_Layout>();
            EnemyLayout.name = "DelayedAttack_Enemy";
            EnemyLayout.m_Objects = new GameObject[] { Enemy };
            EnemyLayout._Animators = [Enemy.GetComponent<Animator>()];

            ResetArrays();
            ResetObjects();

            NotificationHook.AddAction(NotifCheck);
        }

        public static bool[] FoolAttacks;
        public static bool[] EnemyAttacks;

        public static CharacterFieldEffectLayout[] LayoutFools;
        public static EnemyFieldEffectLayout[] LayoutEnemies;
        public static void ResetArrays()
        {
            FoolAttacks = [false, false, false, false, false];
            EnemyAttacks = [false, false, false, false, false];
        }
        public static void ResetObjects()
        {
            LayoutFools = new CharacterFieldEffectLayout[5];
            LayoutEnemies = new EnemyFieldEffectLayout[5];
        }

        public static void UpdateVisuals()
        {
            ResetArrays();

            foreach (DelayedAttack attack in DelayedAttackManager.Attacks)
            {
                if (attack.caster != null && !attack.caster.IsUnitCharacter && (!attack.caster.IsAlive || attack.caster.HasFled)) continue;

                if (attack.Target.IsTargetCharacterSlot) FoolAttacks[attack.Target.SlotID] = true;
                else EnemyAttacks[attack.Target.SlotID] = true;
            }

            CombatManager.Instance.AddUIAction(new UpdatedDelayAttackVisualsAction());
        }

        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnDeath.ToString() || name == TriggerCalls.OnFleetingEnd.ToString())
                UpdateVisuals();
            if (name == TriggerCalls.OnBeforeCombatStart.ToString())
                ResetObjects();
        }
    }

    public class UpdatedDelayAttackVisualsAction : CombatAction
    {
        public bool[] fools;
        public bool[] enemies;
        public UpdatedDelayAttackVisualsAction()
        {
            fools = DelayedAttackVisualizer.FoolAttacks;
            enemies = DelayedAttackVisualizer.EnemyAttacks;
        }
        public static void UpdateFoolLayout(CombatStats stats, bool[] values)
        {
            for (int _slotId = 0; _slotId < values.Length; _slotId++)
            {
                CharacterSlotLayout slot = stats.combatUI._characterZone._slots[stats.combatUI._characterSlots[_slotId].SlotID];

                CharacterFieldEffectLayout layout_fool = DelayedAttackVisualizer.LayoutFools[_slotId];

                if (layout_fool == null)
                {
                    layout_fool = UnityEngine.Object.Instantiate(DelayedAttackVisualizer.FoolLayout, slot.transform);
                    layout_fool.InitializeLayout(slot._frontFieldEffectHolder, slot._backHolder, slot._swapHolder);
                    DelayedAttackVisualizer.LayoutFools[_slotId] = layout_fool;
                }

                if (values[_slotId])
                {
                    layout_fool.AccessLayout(slot._hasUnit);
                }

                layout_fool.EndAccessLayout();
            }
        }
        public static void UpdateEnemyLayout(CombatStats stats, bool[] values)
        {
            for (int _slotId = 0; _slotId < values.Length; _slotId++)
            {
                EnemySlotLayout slot = stats.combatUI._enemyZone._slots[stats.combatUI._enemySlots[_slotId].SlotID];

                EnemyFieldEffectLayout layout_enemy = DelayedAttackVisualizer.LayoutEnemies[_slotId];

                if (layout_enemy == null)
                {
                    layout_enemy = UnityEngine.Object.Instantiate(DelayedAttackVisualizer.EnemyLayout, slot.transform);
                    layout_enemy.transform.localPosition = Vector3.zero;
                    DelayedAttackVisualizer.LayoutEnemies[_slotId] = layout_enemy;
                }

                if (values[_slotId])
                {
                    layout_enemy.AccessLayout();
                }

                layout_enemy.EndAccessLayout();
            }
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            UpdateFoolLayout(stats, fools);
            UpdateEnemyLayout(stats, enemies);

            yield break;
        }
    }

    public class On_Off_CFE_Layout : GameObject_CFE_Layout
    {
        public Animator[] _Animators;
        public override void DisableLayout()
        {
            if (!IsActive) ActuallyDisable();

            if (_Animators != null)
            {
                foreach (Animator anim in _Animators)
                {
                    anim.SetTrigger("Disable");
                }
            }
        }
        public void ActuallyDisable()
        {
            GameObject[] objects = m_Objects;
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(value: false);
            }
        }
    }
    public class On_Off_EFE_Layout : GameObject_EFE_Layout
    {
        public Animator[] _Animators;
        public override void DisableLayout()
        {
            if (!IsActive) ActuallyDisable();

            if (_Animators != null)
            {
                foreach (Animator anim in _Animators)
                {
                    anim.SetTrigger("Disable");
                }
            }
        }
        public void ActuallyDisable()
        {
            GameObject[] objects = m_Objects;
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(value: false);
            }
        }
    }
}
