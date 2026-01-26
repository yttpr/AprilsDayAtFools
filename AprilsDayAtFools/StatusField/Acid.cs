using BrutalAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Acid
    {
        public static string DamageType => "Dmg_Acid";
        public static string StatusID => "Acid_ID";
        public static string Intent => "Status_Acid";
        public static string Rem => "Rem_Status_Acid";
        public static AcidSE_SO Object;
        public static void Add()
        {
            TMP_ColorGradient acidGradient = ScriptableObject.CreateInstance<TMP_ColorGradient>();
            Color32 secondcolor = new Color32(81, 209, 81, 255);
            Color32 firstcolor = new Color32(46, 155, 46, 255);
            acidGradient.bottomLeft = secondcolor;
            acidGradient.bottomRight = firstcolor;
            acidGradient.topLeft = firstcolor;
            acidGradient.topRight = secondcolor;
            
            if (!LoadedDBsHandler.CombatDB.m_TxtColorPool.ContainsKey(DamageType)) LoadedDBsHandler.CombatDB.AddNewTextColor(DamageType, acidGradient);

            if (!LoadedDBsHandler.CombatDB.m_SoundPool.ContainsKey(DamageType)) LoadedDBsHandler.CombatDB.AddNewSound(DamageType, LoadedDBsHandler.CombatDB.m_SoundPool[CombatType_GameIDs.Dmg_Ruptured.ToString()]);

            StatusEffectInfoSO AcidInfo = ScriptableObject.CreateInstance<StatusEffectInfoSO>();
            AcidInfo.icon = ResourceLoader.LoadSprite("Acid.png");
            AcidInfo._statusName = "Acid";
            AcidInfo._description = "On using an ability, take 3 indirect damage. Reduce Acid by 1 at the end of each turn.";
            AcidInfo._applied_SE_Event = LoadedDBsHandler.StatusFieldDB._StatusEffects[StatusField_GameIDs.Ruptured_ID.ToString()]._EffectInfo._applied_SE_Event;
            AcidInfo._removed_SE_Event = LoadedDBsHandler.StatusFieldDB._StatusEffects[StatusField_GameIDs.Ruptured_ID.ToString()]._EffectInfo.RemovedSoundEvent;
            AcidInfo._updated_SE_Event = LoadedDBsHandler.StatusFieldDB._StatusEffects[StatusField_GameIDs.Ruptured_ID.ToString()]._EffectInfo.UpdatedSoundEvent;

            AcidSE_SO AcidSO = ScriptableObject.CreateInstance<AcidSE_SO>();
            AcidSO._StatusID = StatusID;
            AcidSO._EffectInfo = AcidInfo;
            Object = AcidSO;
            if (!LoadedDBsHandler.StatusFieldDB._StatusEffects.ContainsKey(StatusID)) LoadedDBsHandler.StatusFieldDB.AddNewStatusEffect(AcidSO);

            IntentInfoBasic intentinfo = new IntentInfoBasic();
            intentinfo._color = Color.white;
            intentinfo._sprite = ResourceLoader.LoadSprite("Acid.png");
            if (!LoadedDBsHandler.IntentDB.m_IntentBasicPool.ContainsKey(Intent)) LoadedDBsHandler.IntentDB.AddNewBasicIntent(Intent, intentinfo);

            IntentInfoBasic reminfo = new IntentInfoBasic();
            reminfo._color = LoadedDBsHandler.IntentDB.m_IntentBasicPool["Rem_Status_Frail"]._color;
            reminfo._sprite = ResourceLoader.LoadSprite("Acid.png");
            if (!LoadedDBsHandler.IntentDB.m_IntentBasicPool.ContainsKey(Rem)) LoadedDBsHandler.IntentDB.AddNewBasicIntent(Rem, reminfo);
        }
    }
    public class AcidSE_SO : StatusEffect_SO
    {
        public override bool IsPositive => false;
        public static string SkipTick => "Acid_SkipTick_SE";
        public override void OnTriggerAttached(StatusEffect_Holder holder, IStatusEffector caller)
        {
            if (CombatManager.Instance._stats.IsPlayerTurn && caller.IsStatusEffectorCharacter)
            {
                (caller as IUnit).SimpleSetStoredValue(SkipTick, 1);
                CombatManager.Instance.AddRootAction(new RootActionAction(new AcidSkipTickAction()));
            }
            CombatManager.Instance.AddObserver(holder.OnEventTriggered_01, TriggerCalls.OnAbilityUsed.ToString(), caller);
            CombatManager.Instance.AddObserver(holder.OnEventTriggered_02, TriggerCalls.OnTurnFinished.ToString(), caller);
        }

        public override void OnTriggerDettached(StatusEffect_Holder holder, IStatusEffector caller)
        {
            CombatManager.Instance.RemoveObserver(holder.OnEventTriggered_01, TriggerCalls.OnAbilityUsed.ToString(), caller);
            CombatManager.Instance.RemoveObserver(holder.OnEventTriggered_02, TriggerCalls.OnTurnFinished.ToString(), caller);
        }

        public override void OnEventCall_01(StatusEffect_Holder holder, object sender, object args)
        {
            if ((sender as IUnit).SimpleGetStoredValue(SkipTick) > 0)
            {
                (sender as IUnit).SimpleSetStoredValue(SkipTick, 0);
                return;
            }
            CombatManager.Instance.AddSubAction(new AcidDamageAction(sender as IUnit));
        }
        public override void OnEventCall_02(StatusEffect_Holder holder, object sender, object args)
        {
            ReduceDuration(holder, sender as IStatusEffector);
        }
    }
    public class AcidDamageAction : CombatAction
    {
        public IUnit victim;
        public AcidDamageAction(IUnit _victim)
        {
            victim = _victim;
        }
        public override IEnumerator Execute(CombatStats stats)
        {
            victim.Damage(3, null, DeathType_GameIDs.Basic.ToString(), -1, false, false, true, Acid.DamageType);
            yield return null;
        }
    }
    public class AcidSkipTickAction : CombatAction
    {
        public override IEnumerator Execute(CombatStats stats)
        {
            foreach (CharacterCombat chara in stats.CharactersOnField.Values) chara.SimpleSetStoredValue(AcidSE_SO.SkipTick, 0);
            foreach (EnemyCombat enemy in stats.EnemiesOnField.Values) enemy.SimpleSetStoredValue(AcidSE_SO.SkipTick, 0);
            yield return null;
        }
    }
    public class ApplyAcidEffect : StatusEffect_Apply_Effect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            _Status = Acid.Object;
            if (Acid.Object == null || Acid.Object.Equals(null)) Debug.LogError("CALL \"Acid.Add();\" IN YOUR AWAKE");
            return base.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable, out exitAmount);
        }
    }
    public class HalveAcidEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach (TargetSlotInfo targetSlotInfo in targets)
            {
                if (targetSlotInfo.HasUnit)
                {
                    if (targetSlotInfo.Unit is IStatusEffector effector)
                    {
                        foreach (IStatusEffect status in new List<IStatusEffect>(effector.StatusEffects))
                        {
                            if (status.StatusID == Acid.StatusID)
                            {
                                int num = -1 * (int)Math.Ceiling((float)status.StatusContent / 2);
                                if (num == 0) continue;

                                if (status.StatusContent > Math.Abs(num))
                                {
                                    if (status.TryAddContent(num, 0))
                                    {
                                        effector.StatusEffectValuesChanged(status.StatusID, num, true);
                                        exitAmount += Math.Abs(num);
                                    }
                                }
                                else
                                {
                                    exitAmount += targetSlotInfo.Unit.TryRemoveStatusEffect(status.StatusID);
                                }
                            }
                        }
                    }
                }
            }
            return exitAmount > 0;
        }
    }
    public class RemoveAllStatusOtherThanAcidEffect : EffectSO
    {
        public static int TryRemoveAllStatusEffectsExceptAcidCH(CharacterCombat chara)
        {
            int num = 0;
            List<int> list = new List<int>();
            List<int> list2 = new List<int>();
            List<string> list3 = new List<string>();
            for (int num2 = chara.StatusEffects.Count - 1; num2 >= 0; num2--)
            {
                if (chara.StatusEffects[num2].StatusID == Acid.StatusID) continue;

                int num3 = chara.StatusEffects[num2].JustRemoveAllContent();
                num += num3;
                if (!chara.StatusEffects[num2].CanBeRemoved)
                {
                    if (num3 > 0)
                    {
                        list2.Add(num2);
                        list3.Add(chara.StatusEffects[num2].DisplayText);
                    }
                }
                else
                {
                    list.Add(num2);
                    chara.StatusEffects[num2].OnTriggerDettached(chara);
                    chara.StatusEffects.RemoveAt(num2);
                }
            }

            CombatManager.Instance.AddUIAction(new CharacterStatusEffectRemovedAndUpdatedAllUIAction(chara.ID, list2.ToArray(), list3.ToArray(), list.ToArray(), showInfo: true));
            chara.SetVolatileUpdateUIAction();
            return num;
        }
        public static int TryRemoveAllStatusEffectsExceptAcidEN(EnemyCombat enemy)
        {
            int num = 0;
            List<int> list = new List<int>();
            List<int> list2 = new List<int>();
            List<string> list3 = new List<string>();
            //if (enemy.StatusEffects.Count > 0 && enemy.StatusEffects[0].EffectInfo != null)
            //{
                //_ = enemy.StatusEffects[0].EffectInfo.RemovedSoundEvent;
            //}

            for (int num2 = enemy.StatusEffects.Count - 1; num2 >= 0; num2--)
            {
                if (enemy.StatusEffects[num2].StatusID == Acid.StatusID) continue;

                int num3 = enemy.StatusEffects[num2].JustRemoveAllContent();
                num += num3;
                if (!enemy.StatusEffects[num2].CanBeRemoved)
                {
                    if (num3 > 0)
                    {
                        list2.Add(num2);
                        list3.Add(enemy.StatusEffects[num2].DisplayText);
                    }
                }
                else
                {
                    list.Add(num2);
                    enemy.StatusEffects[num2].OnTriggerDettached(enemy);
                    enemy.StatusEffects.RemoveAt(num2);
                }
            }

            CombatManager.Instance.AddUIAction(new EnemyStatusEffectRemovedAndUpdatedAllUIAction(enemy.ID, list2.ToArray(), list3.ToArray(), list.ToArray(), showInfo: true));
            return num;
        }

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    if (target.Unit is CharacterCombat chara) exitAmount += TryRemoveAllStatusEffectsExceptAcidCH(chara);
                    if (target.Unit is EnemyCombat enemy) exitAmount += TryRemoveAllStatusEffectsExceptAcidEN(enemy);
                }
            }
            return exitAmount > 0;
        }
    }
}
