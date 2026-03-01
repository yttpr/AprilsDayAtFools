using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Lifesteal
    {
        public static string HealType => "Heal_Lifesteal";
        public static string FieldID => "Lifesteal_ID";
        public static string Intent => "Field_Lifesteal";
        public static FieldEffect_SO Object;
        public static void Add()
        {
            TMP_ColorGradient lifesteal_gradient = ScriptableObject.CreateInstance<TMP_ColorGradient>();
            Color32 secondcolor = new Color32(127, 0, 63, 255);
            Color32 firstcolor = new Color32(190, 0, 95, 255);
            lifesteal_gradient.bottomLeft = firstcolor;
            lifesteal_gradient.bottomRight = secondcolor;
            lifesteal_gradient.topLeft = Color.white;
            lifesteal_gradient.topRight = firstcolor;

            if (!LoadedDBsHandler.CombatDB.m_TxtColorPool.ContainsKey(HealType)) LoadedDBsHandler.CombatDB.AddNewTextColor(HealType, lifesteal_gradient);

            if (!LoadedDBsHandler.CombatDB.m_SoundPool.ContainsKey(HealType)) LoadedDBsHandler.CombatDB.AddNewSound(HealType, LoadedDBsHandler.CombatDB.m_SoundPool[CombatType_GameIDs.Heal_Basic.ToString()]);

            SlotStatusEffectInfoSO LifestealInfo = ScriptableObject.CreateInstance<SlotStatusEffectInfoSO>();
            LifestealInfo.icon = ResourceLoader.LoadSprite("LifestealIcon.png");
            LifestealInfo._fieldName = "Lifesteal";
            LifestealInfo._description = "On dealing damage in Lifesteal, heal for half the damage dealt.\nLifesteal reduces by 1 on turn end and on taking direct damage in Lifesteal.";
            LifestealInfo._applied_SE_Event = LoadedDBsHandler.StatusFieldDB._StatusEffects[StatusField_GameIDs.Cursed_ID.ToString()]._EffectInfo._applied_SE_Event;
            LifestealInfo._removed_SE_Event = LoadedDBsHandler.StatusFieldDB._StatusEffects[StatusField_GameIDs.Cursed_ID.ToString()]._EffectInfo.RemovedSoundEvent;
            LifestealInfo._updated_SE_Event = LoadedDBsHandler.StatusFieldDB._StatusEffects[StatusField_GameIDs.Cursed_ID.ToString()]._EffectInfo.UpdatedSoundEvent;

            GameObject Fool = Joyce.Assets.LoadAsset<GameObject>("Assets/Lifesteal/LifestealHolder.prefab");
            Animator_CFE_Layout LayoutFool = Fool.AddComponent<Animator_CFE_Layout>();
            LayoutFool.m_Front = new RectTransform[] { Fool.transform.GetChild(0).GetComponent<RectTransform>() };
            LayoutFool.m_Back = new RectTransform[] { Fool.transform.GetChild(1).GetComponent<RectTransform>() };
            LayoutFool.m_Animators = new Animator[] { Fool.transform.GetChild(0).GetComponent<Animator>(), Fool.transform.GetChild(1).GetComponent<Animator>() };
            LifestealInfo.m_CharacterLayoutTemplate = LayoutFool;

            GameObject Enemy = Joyce.Assets.LoadAsset<GameObject>("Assets/Lifesteal/LifestealEnemy.prefab");
            GameObject_EFE_Layout LayoutEnemy = Enemy.AddComponent<GameObject_EFE_Layout>();
            LayoutEnemy.m_Objects = [Enemy];
            LifestealInfo.m_EnemyLayoutTemplate = LayoutEnemy;

            LifestealFE_SO lifestealSO = ScriptableObject.CreateInstance<LifestealFE_SO>();
            lifestealSO._FieldID = FieldID;
            lifestealSO._EffectInfo = LifestealInfo;
            Object = lifestealSO;
            if (!LoadedDBsHandler.StatusFieldDB.FieldEffects.ContainsKey(FieldID))
            {
                LoadedDBsHandler.StatusFieldDB.AddNewFieldEffect(lifestealSO);
            }

            IntentInfoBasic intentinfo = new IntentInfoBasic();
            intentinfo._color = Color.white;
            intentinfo._sprite = ResourceLoader.LoadSprite("LifestealIcon.png");
            if (!LoadedDBsHandler.IntentDB.m_IntentBasicPool.ContainsKey(Intent)) LoadedDBsHandler.IntentDB.AddNewBasicIntent(Intent, intentinfo);
        }
    }
    public class LifestealFE_SO : FieldEffect_SO
    {
        public override bool IsPositive => true;
        public override void OnSlotEffectorTriggerAttached(FieldEffect_Holder holder)
        {
            CombatManager.Instance.AddObserver(holder.OnEventTriggered_01, TriggerCalls.OnTurnFinished.ToString(), holder.Effector);
        }
        public override void OnSlotEffectorTriggerDettached(FieldEffect_Holder holder)
        {
            CombatManager.Instance.RemoveObserver(holder.OnEventTriggered_01, TriggerCalls.OnTurnFinished.ToString(), holder.Effector);
        }
        public override void OnTriggerAttached(FieldEffect_Holder holder, IUnit caller)
        {
            CombatManager.Instance.AddObserver(holder.OnEventTriggered_02, TriggerCalls.OnDidApplyDamage.ToString(), caller);
            CombatManager.Instance.AddObserver(holder.OnEventTriggered_01, TriggerCalls.OnDirectDamaged.ToString(), caller);
        }
        public override void OnTriggerDettached(FieldEffect_Holder holder, IUnit caller)
        {
            CombatManager.Instance.RemoveObserver(holder.OnEventTriggered_02, TriggerCalls.OnDidApplyDamage.ToString(), caller);
            CombatManager.Instance.RemoveObserver(holder.OnEventTriggered_01, TriggerCalls.OnDirectDamaged.ToString(), caller);
        }
        public override void OnEventCall_01(FieldEffect_Holder holder, object sender, object args)
        {
            ReduceDuration(holder);
        }
        public override void OnEventCall_02(FieldEffect_Holder holder, object sender, object args)
        {
            (sender as IUnit).Heal((int)Math.Ceiling((args as IntegerReference).value / 2f), sender as IUnit, true, Lifesteal.HealType);
        }
    }
    public class ApplyLifestealSlotEffect : FieldEffect_Apply_Effect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            _Field = Lifesteal.Object;
            if (Lifesteal.Object == null || Lifesteal.Object.Equals(null)) Lifesteal.Add();
            return base.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable, out exitAmount);
        }
    }
}
