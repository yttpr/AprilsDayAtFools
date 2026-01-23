using System;
using System.Reflection;
using UnityEngine;
using System.Collections;

namespace AprilsDayAtFools
{
    public static class Help
    {
        public static DamageInfo GenerateDamageInfo(int exit, int entry, bool killed)
        {
            DamageInfo ret = new DamageInfo();
            ret.damageAmount = exit;
            ret.beenKilled = killed;

            if (typeof(DamageInfo).GetField("attemptedDamageAmount") != null)
            {
                typeof(DamageInfo).GetField("attemptedDamageAmount").SetValue(ret, entry);
            }

            return ret;
        }

        public static IntegerReference GenerateDamageIntReference(int amount, string damageTypeID, string deathTypeID, bool directDamage, bool ignoreShield, int affectedStartSlot, int affectedEndSlot, IUnit possibleSourceUnit, IUnit damagedUnit)
        {
            if (typeof(IntegerReference).Assembly.GetType("IntegerReference_Damage", false) != null)
            {
                object ret = typeof(IntegerReference).Assembly.GetType("IntegerReference_Damage").GetConstructor([typeof(int), typeof(string), typeof(string), typeof(bool), typeof(bool), typeof(int), typeof(int), typeof(IUnit), typeof(IUnit)]).Invoke([amount, damageTypeID, deathTypeID, directDamage, ignoreShield, affectedStartSlot, affectedEndSlot, possibleSourceUnit, damagedUnit]);

                return ret as IntegerReference;
            }

            return new IntegerReference(amount);
        }

        public static DamageReceivedValueChangeException GenerateDamageTakenException(int amount, string damageTypeID, string deathTypeID, bool directDamage, bool ignoreShield, int affectedStartSlot, int affectedEndSlot, IUnit possibleSourceUnit, IUnit damagedUnit)
        {
            ConstructorInfo[] constructors = typeof(DamageReceivedValueChangeException).GetConstructors();

            foreach (ConstructorInfo constructor in constructors)
            {
                if (constructor.GetParameters().Length == 9)
                {
                    return constructor.Invoke([amount, damageTypeID, deathTypeID, directDamage, ignoreShield, affectedStartSlot, affectedEndSlot, possibleSourceUnit, damagedUnit]) as DamageReceivedValueChangeException;
                }
                if (constructor.GetParameters().Length == 8)
                {
                    return constructor.Invoke([amount, damageTypeID, directDamage, ignoreShield, affectedStartSlot, affectedEndSlot, possibleSourceUnit, damagedUnit]) as DamageReceivedValueChangeException;
                }
            }
            Debug.LogError("versioncompatability:damagereceivedexception. tldr, WHAT THE FUCK");
            return null;
        }

        public static void AnyAbilityHasFinished(IUnit unit, int caster, bool isChara, AbilitySO ability)
        {
            if (unit is CharacterCombat chara)
            {
                MethodInfo method = typeof(CharacterCombat).GetMethod(nameof(CharacterCombat.AnyAbilityHasFinished));
                if (method.GetParameters().Length == 0)
                    method.Invoke(unit, []);
                else if (method.GetParameters().Length == 1)
                {
                    try
                    {
                        Helper._subHelper_AnyAbilityHasFinished(unit, caster, isChara, ability);
                    }
                    catch
                    {
                        Debug.LogWarning("idk");
                    }
                }
            }
            if (unit is EnemyCombat enemy)
            {
                MethodInfo method = typeof(EnemyCombat).GetMethod(nameof(EnemyCombat.AnyAbilityHasFinished));
                if (method.GetParameters().Length == 0)
                    method.Invoke(unit, []);
                else if (method.GetParameters().Length == 1)
                {
                    try
                    {
                        Helper._subHelper_AnyAbilityHasFinished(unit, caster, isChara, ability);
                    }
                    catch
                    {
                        Debug.LogWarning("idk");
                    }
                }
            }
        }

        public static bool GenericDirectDeath(this IUnit self, IUnit killer, bool obliteration = false)
        {
            MethodInfo method = typeof(IUnit).GetMethod(nameof(IUnit.DirectDeath));
            if (method.GetParameters().Length == 2)
                return (bool)method.Invoke(self, [killer, obliteration]);
            else
            {
                try
                {
                    return Helper._subHelper_DirectDeath(self, killer, obliteration);
                }
                catch
                {
                    Debug.LogWarning("idk");
                }
            }
            return false;
        }

        public static EnemyDamagedUIAction GenerateEnemyDamagedUIAction(int id, int currentHealth, int maxHealth, int totalAmount, string dmgTypeID, bool triggerPopUp = true, bool triggerAnim = true)
        {
            ConstructorInfo[] constructors = typeof(EnemyDamagedUIAction).GetConstructors();

            foreach (ConstructorInfo constructor in constructors)
            {
                if (constructor.GetParameters().Length == 7)
                {
                    return constructor.Invoke([id, currentHealth, maxHealth, totalAmount, dmgTypeID, triggerPopUp, triggerAnim]) as EnemyDamagedUIAction;
                }
                if (constructor.GetParameters().Length == 5)
                {
                    return constructor.Invoke([id, currentHealth, maxHealth, totalAmount, dmgTypeID]) as EnemyDamagedUIAction;
                }
            }
            Debug.LogError("versioncompatability:EnemyDamagedUIAction. tldr, WHAT THE FUCK");
            return null;
        }


        public class CompatibleEndAbilityAction : CombatAction
        {
            public int _unitID;

            public bool _isUnitCharacter;

            public AbilitySO _ability;

            public CompatibleEndAbilityAction(int unitID, bool isUnitCharacter, AbilitySO ability)
            {
                _unitID = unitID;
                _isUnitCharacter = isUnitCharacter;
                _ability = ability;
            }

            public override IEnumerator Execute(CombatStats stats)
            {
                if (_isUnitCharacter)
                {
                    CharacterCombat characterCombat = stats.TryGetCharacterOnField(_unitID);
                    if (characterCombat != null)
                    {
                        if (characterCombat.IsAlive)
                        {
                            characterCombat.AbilityHasFinished();
                        }

                        characterCombat.FinalizeAbilityActions();
                    }
                }
                else
                {
                    EnemyCombat enemyCombat = stats.TryGetEnemyOnField(_unitID);
                    if (enemyCombat != null && enemyCombat.IsAlive)
                    {
                        enemyCombat.AbilityHasFinished();
                    }
                    else
                    {
                        CombatManager.Instance.AddRootAction(new NextTurnAction());
                    }
                }

                foreach (CharacterCombat value in stats.CharactersOnField.Values)
                {
                    Help.AnyAbilityHasFinished(value, _unitID, _isUnitCharacter, _ability);
                }

                foreach (EnemyCombat value2 in stats.EnemiesOnField.Values)
                {
                    Help.AnyAbilityHasFinished(value2, _unitID, _isUnitCharacter, _ability);
                }

                yield return null;
            }
        }
    }
    public static class Helper
    {
        public static bool _subHelper_DirectDeath(IUnit target, IUnit killer, bool obliteration)
        {
            return target.DirectDeath(killer, obliteration, out int num);
        }
        public static void _subHelper_AnyAbilityHasFinished(IUnit unit, int caster, bool isChara, AbilitySO ability)
        {
            if (unit is CharacterCombat chara)
            {
                chara.AnyAbilityHasFinished(new AbilityUsageReference(caster, isChara, ability));
            }
            if (unit is EnemyCombat enemy)
            {
                enemy.AnyAbilityHasFinished(new AbilityUsageReference(caster, isChara, ability));
            }
        }
    }
}
