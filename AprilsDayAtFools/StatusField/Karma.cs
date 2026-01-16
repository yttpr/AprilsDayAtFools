using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Karma
    {
        public static string DamageType => "Dmg_Karma";
        public static string StatusID => "Karma_ID";
        public static string Intent => "Status_Karma";
        public static string Rem => "Rem_Status_Karma";
        public static KarmaSE_SO Object;
        public static void Add()
        {
            TMP_ColorGradient karma_gradient = ScriptableObject.CreateInstance<TMP_ColorGradient>();
            Color32 secondcolor = new Color32(64, 0, 32, 255);
            Color32 firstcolor = new Color32(190, 0, 95, 255);
            karma_gradient.bottomLeft = secondcolor;
            karma_gradient.bottomRight = firstcolor;
            karma_gradient.topLeft = firstcolor;
            karma_gradient.topRight = secondcolor;

            if (!LoadedDBsHandler.CombatDB.m_TxtColorPool.ContainsKey(DamageType)) LoadedDBsHandler.CombatDB.AddNewTextColor(DamageType, karma_gradient);

            if (!LoadedDBsHandler.CombatDB.m_SoundPool.ContainsKey(DamageType)) LoadedDBsHandler.CombatDB.AddNewSound(DamageType, "event:/Lunacy/Misc3/KarmaDamage");

            StatusEffectInfoSO karma_info = ScriptableObject.CreateInstance<StatusEffectInfoSO>();
            karma_info.icon = ResourceLoader.LoadSprite("KarmaIcon.png");
            karma_info._statusName = "Karma";
            karma_info._description = "At the end of combat, take direct damage equal to the amount of Karma.";
            karma_info._applied_SE_Event = "event:/Lunacy/Misc3/KarmaApply";
            karma_info._removed_SE_Event = LoadedDBsHandler.StatusFieldDB._StatusEffects[StatusField_GameIDs.Ruptured_ID.ToString()]._EffectInfo.RemovedSoundEvent;
            karma_info._updated_SE_Event = LoadedDBsHandler.StatusFieldDB._StatusEffects[StatusField_GameIDs.Ruptured_ID.ToString()]._EffectInfo.UpdatedSoundEvent;

            KarmaSE_SO karma_object = ScriptableObject.CreateInstance<KarmaSE_SO>();
            karma_object._StatusID = StatusID;
            karma_object._EffectInfo = karma_info;
            Object = karma_object;
            if (!LoadedDBsHandler.StatusFieldDB._StatusEffects.ContainsKey(StatusID)) LoadedDBsHandler.StatusFieldDB.AddNewStatusEffect(karma_object);

            IntentInfoBasic intentinfo = new IntentInfoBasic();
            intentinfo._color = Color.white;
            intentinfo._sprite = ResourceLoader.LoadSprite("KarmaIcon.png");
            if (!LoadedDBsHandler.IntentDB.m_IntentBasicPool.ContainsKey(Intent)) LoadedDBsHandler.IntentDB.AddNewBasicIntent(Intent, intentinfo);

            IntentInfoBasic reminfo = new IntentInfoBasic();
            reminfo._color = LoadedDBsHandler.IntentDB.m_IntentBasicPool["Rem_Status_Frail"]._color;
            reminfo._sprite = ResourceLoader.LoadSprite("KarmaIcon.png");
            if (!LoadedDBsHandler.IntentDB.m_IntentBasicPool.ContainsKey(Rem)) LoadedDBsHandler.IntentDB.AddNewBasicIntent(Rem, reminfo);
        }
    }
    public class KarmaSE_SO : StatusEffect_SO
    {
        public override bool IsPositive => false;
        public override void OnTriggerAttached(StatusEffect_Holder holder, IStatusEffector caller)
        {
            CombatManager.Instance.AddObserver(holder.OnEventTriggered_01, TriggerCalls.OnCombatEnd.ToString(), caller);
        }

        public override void OnTriggerDettached(StatusEffect_Holder holder, IStatusEffector caller)
        {
            CombatManager.Instance.RemoveObserver(holder.OnEventTriggered_01, TriggerCalls.OnCombatEnd.ToString(), caller);
        }

        public override void OnEventCall_01(StatusEffect_Holder holder, object sender, object args)
        {
            if (sender is IUnit unit) unit.Damage(holder.m_ContentMain + holder.Restrictor, null, "Karma", -1, true, true, false, Karma.DamageType);
        }
    }
    public class ApplyKarmaEffect : StatusEffect_Apply_Effect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            _Status = Karma.Object;
            if (Karma.Object == null || Karma.Object.Equals(null)) Debug.LogError("CALL \"Karma.Add();\" IN YOUR AWAKE");
            return base.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable, out exitAmount);
        }
    }
    public class ReduceKarmaEffect : EffectSO
    {
        public bool _randomBetweenPrevious;
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
                            if (status.StatusID == Karma.StatusID)
                            {
                                int num = -1 * Math.Abs(_randomBetweenPrevious ? UnityEngine.Random.Range(PreviousExitValue, entryVariable + 1) : entryVariable);
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
    public class ApplyKarmaCappedToExitEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    int current = target.Unit.GetStatusAmount(Karma.StatusID);
                    int diff = PreviousExitValue - current;
                    if (diff <= 0) continue;

                    int num = Math.Min(diff, entryVariable);
                    if (target.Unit.ApplyStatusEffect(Karma.Object, num))
                        exitAmount += num;
                }
            }

            return exitAmount > 0;
        }
    }
}
