using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class DirectHealingEffect : EffectSO
    {
        public bool usePreviousExitValue;

        public bool entryAsPercentage;

        [SerializeField]
        public bool _directHeal = true;

        [SerializeField]
        public bool _onlyIfHasHealthOver0;

        [SerializeField]
        public bool _ignoreShield = true;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (usePreviousExitValue)
            {
                entryVariable *= base.PreviousExitValue;
            }

            exitAmount = 0;
            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit && (!_onlyIfHasHealthOver0 || targetSlotInfo.Unit.CurrentHealth > 0))
                {
                    int num = caster.WillApplyDamage(entryVariable, targetSlotInfo.Unit);
                    if (entryAsPercentage)
                    {
                        num = targetSlotInfo.Unit.CalculatePercentualAmount(num);
                    }
                    DamageReceivedValueChangeException ex = new DamageReceivedValueChangeException(num, "Heal", _directHeal, _ignoreShield, targetSlotInfo.Unit.SlotID, targetSlotInfo.Unit.SlotID + targetSlotInfo.Unit.Size - 1, caster, targetSlotInfo.Unit);
                    CombatManager.Instance.PostNotification(TriggerCalls.OnBeingDamaged.ToString(), targetSlotInfo.Unit, ex);
                    int modifiedValue = ex.GetModifiedValue();
                    int amount = 0;
                    if (targetSlotInfo.Unit.CanHeal(_directHeal, modifiedValue))
                    {
                        if (_directHeal)
                        {
                            num = caster.WillApplyHeal(modifiedValue, targetSlotInfo.Unit);
                        }
                        ExtraHealReturnInfo check = new ExtraHealReturnInfo();
                        amount = targetSlotInfo.Unit.Heal(num, caster, _directHeal, "", check);

                        IntegerReference post = Help.GenerateDamageIntReference(Math.Max(amount, check.attemptedHealAmount), "Heal", _directHeal, _ignoreShield, targetSlotInfo.Unit.SlotID, targetSlotInfo.Unit.SlotID + targetSlotInfo.Unit.Size - 1, caster, targetSlotInfo.Unit);

                        if (post.value > 0)
                        {
                            if (targetSlotInfo.Unit.IsUnitCharacter)
                            {
                                CombatManager.Instance.AddUIAction(new CharacterDirectHealedUIAction(targetSlotInfo.Unit.ID, targetSlotInfo.Unit.CurrentHealth, targetSlotInfo.Unit.MaximumHealth, modifiedValue, ""));
                                if (targetSlotInfo.Unit.IsAlive)
                                {
                                    CombatManager.Instance.ProcessImmediateAction(new AddManaToManaBarAction(targetSlotInfo.Unit.HealthColor, LoadedDBsHandler.CombatData.CharacterPigmentAmount, targetSlotInfo.Unit.IsUnitCharacter, targetSlotInfo.Unit.ID));
                                }
                            }
                            else
                            {
                                CombatManager.Instance.AddUIAction(new EnemyDirectHealedUIAction(targetSlotInfo.Unit.ID, targetSlotInfo.Unit.CurrentHealth, targetSlotInfo.Unit.MaximumHealth, modifiedValue, ""));
                                if (targetSlotInfo.Unit.IsAlive)
                                {
                                    CombatManager.Instance.ProcessImmediateAction(new AddManaToManaBarAction(targetSlotInfo.Unit.HealthColor, LoadedDBsHandler.CombatData.EnemyPigmentAmount, targetSlotInfo.Unit.IsUnitCharacter, targetSlotInfo.Unit.ID));
                                }
                            }
                            targetSlotInfo.Unit.SimpleSetStoredValue(IDs.DirectHealAttempt, check.attemptedHealAmount);
                            targetSlotInfo.Unit.SimpleSetStoredValue(IDs.DirectHealAmount, amount);
                            targetSlotInfo.Unit.SimpleSetStoredValue(IDs.DirectHealing, 1);
                            CombatManager.Instance.PostNotification(TriggerCalls.OnDamaged.ToString(), targetSlotInfo.Unit, post);
                            CombatManager.Instance.PostNotification(_directHeal ? TriggerCalls.OnDirectDamaged.ToString() : TriggerCalls.OnIndirectDamaged.ToString(), targetSlotInfo.Unit, post);
                            targetSlotInfo.Unit.SimpleSetStoredValue(IDs.DirectHealing, 0);
                            targetSlotInfo.Unit.SimpleSetStoredValue(IDs.DirectHealAmount, 0);
                            targetSlotInfo.Unit.SimpleSetStoredValue(IDs.DirectHealAttempt, 0);
                        }

                        exitAmount += post.value;
                    }
                }
            }
            CombatManager.Instance.PostNotification(TriggerCalls.OnDidApplyDamage.ToString(), caster, new IntegerReference(exitAmount));
            return exitAmount > 0;
        }
    }
    public static class DirectHealingHandler
    {
        public static IEnumerator TryDirectHealCharacter(this CombatVisualizationController self, int id, int currentHealth, int maxHealth, string damageType, int totalDamage, bool triggerAnimations)
        {
            if (self._charactersInCombat.TryGetValue(id, out var value))
            {
                yield return self._characterZone.DirectHealCharacter(value, damageType, triggerAnimations);
            }
        }
        public static IEnumerator DirectHealCharacter(this CharacterZoneHandler self, CharacterCombatUIInfo character, string damageType, bool triggerAnimations)
        {
            CharacterInfoLayout characterInfoLayout = self._characters[character.FieldID];
            if (triggerAnimations)
            {
                characterInfoLayout.FieldEntity.DamageCharacter();
                yield return self.PlayDamageSoundsSequentially(damageType, character.CharacterBase.damageSound, characterInfoLayout.FieldPosition);
                yield return self._waitForMiscTimer;
            }
        }
        public static IEnumerator TryDirectHealEnemy(this CombatVisualizationController self, int id, int currentHealth, int maxHealth, string damageType, int totalDamage)
        {
            if (self._enemiesInCombat.TryGetValue(id, out var value))
            {
                yield return self._enemyZone.DirectHealEnemy(value, damageType);
            }
        }
        public static IEnumerator DirectHealEnemy(this EnemyZoneHandler self, EnemyCombatUIInfo enemy, string damageType)
        {
            EnemyInfoLayout enemyInfoLayout = self._enemies[enemy.FieldID];
            enemyInfoLayout.FieldEntity.DamageEnemy();
            yield return self.PlayDamageSoundsSequentially(damageType, enemy.EnemyBase.damageSound, enemyInfoLayout.FieldPosition);
            yield return self._waitForMiscTimer;
        }
    }

    public class CharacterDirectHealedUIAction : CombatAction
    {
        public int _ID;

        public int _currentHealth;

        public int _maxHealth;

        public int _totalAmount;

        public string _dmgType;

        public bool _triggerAnimations;

        public CharacterDirectHealedUIAction(int id, int currentHealth, int maxHealth, int totalAmount, string dmgType, bool triggerAnimations = true)
        {
            _ID = id;
            _currentHealth = currentHealth;
            _maxHealth = maxHealth;
            _totalAmount = totalAmount;
            _dmgType = dmgType;
            _triggerAnimations = triggerAnimations;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            yield return stats.combatUI.TryDirectHealCharacter(_ID, _currentHealth, _maxHealth, _dmgType, _totalAmount, _triggerAnimations);
        }
    }
    public class EnemyDirectHealedUIAction : CombatAction
    {
        public int _ID;

        public int _currentHealth;

        public int _maxHealth;

        public int _totalAmount;

        public string _dmgType;

        public EnemyDirectHealedUIAction(int id, int currentHealth, int maxHealth, int totalAmount, string dmgType)
        {
            _ID = id;
            _currentHealth = currentHealth;
            _maxHealth = maxHealth;
            _totalAmount = totalAmount;
            _dmgType = dmgType;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            yield return stats.combatUI.TryDirectHealEnemy(_ID, _currentHealth, _maxHealth, _dmgType, _totalAmount);
        }
    }
}
