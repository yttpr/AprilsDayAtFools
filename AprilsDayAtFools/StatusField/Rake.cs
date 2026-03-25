using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace AprilsDayAtFools
{

    public static class Rake
    {
        public static string FieldID => "PvZRake_ID";
        public static string Intent => "Field_PvZRake";
        public static FieldEffect_SO Object;
        public static void Add()
        {
            SlotStatusEffectInfoSO rakeInfo = ScriptableObject.CreateInstance<SlotStatusEffectInfoSO>();
            rakeInfo.icon = ResourceLoader.LoadSprite("RakeIcon.png");
            rakeInfo._fieldName = "The Rake";
            rakeInfo._description = "On moving into The Rake, reduce by 1 and take 9 damage.";
            rakeInfo._applied_SE_Event = LoadedDBsHandler.StatusFieldDB.FieldEffects[StatusField_GameIDs.Shield_ID.ToString()]._EffectInfo._applied_SE_Event;
            rakeInfo._removed_SE_Event = LoadedDBsHandler.StatusFieldDB.FieldEffects[StatusField_GameIDs.Shield_ID.ToString()]._EffectInfo.RemovedSoundEvent;
            rakeInfo._updated_SE_Event = LoadedDBsHandler.StatusFieldDB.FieldEffects[StatusField_GameIDs.Shield_ID.ToString()]._EffectInfo.UpdatedSoundEvent;

            GameObject Fool = Joyce.Assets.LoadAsset<GameObject>("Assets/Rake/RakeCharacter.prefab");
            GameObject_CFE_Layout LayoutFool = Fool.AddComponent<GameObject_CFE_Layout>();
            LayoutFool.m_Front = new RectTransform[] { Fool.GetComponent<RectTransform>() };
            LayoutFool.m_Objects = [Fool];
            rakeInfo.m_CharacterLayoutTemplate = LayoutFool;

            GameObject Enemy = Joyce.Assets.LoadAsset<GameObject>("Assets/Rake/RakeEnemy.prefab");
            GameObject_EFE_Layout LayoutEnemy = Enemy.AddComponent<GameObject_EFE_Layout>();
            LayoutEnemy.m_Objects = [Enemy];
            rakeInfo.m_EnemyLayoutTemplate = LayoutEnemy;

            RakeFE_SO rakeSO = ScriptableObject.CreateInstance<RakeFE_SO>();
            rakeSO._FieldID = FieldID;
            rakeSO._EffectInfo = rakeInfo;
            Object = rakeSO;
            //disable after testing
            if (!LoadedDBsHandler.StatusFieldDB.FieldEffects.ContainsKey(FieldID))
            {
                LoadedDBsHandler.StatusFieldDB.AddNewFieldEffect(rakeSO);
            }

            IntentInfoBasic intentinfo = new IntentInfoBasic();
            intentinfo._color = Color.white;
            intentinfo._sprite = ResourceLoader.LoadSprite("RakeIcon.png");
            if (!LoadedDBsHandler.IntentDB.m_IntentBasicPool.ContainsKey(Intent)) LoadedDBsHandler.IntentDB.AddNewBasicIntent(Intent, intentinfo);
        }
    }
    public class RakeFE_SO : FieldEffect_SO
    {
        public override bool IsPositive => false;
        public override void OnSlotEffectorTriggerAttached(FieldEffect_Holder holder)
        {
        }
        public override void OnSlotEffectorTriggerDettached(FieldEffect_Holder holder)
        {
        }
        public override void OnTriggerAttached(FieldEffect_Holder holder, IUnit caller)
        {
            CombatManager.Instance.AddObserver(holder.OnEventTriggered_01, TriggerCalls.OnMoved.ToString(), caller);
        }
        public override void OnTriggerDettached(FieldEffect_Holder holder, IUnit caller)
        {
            CombatManager.Instance.RemoveObserver(holder.OnEventTriggered_01, TriggerCalls.OnMoved.ToString(), caller);
        }
        public override void OnEventCall_01(FieldEffect_Holder holder, object sender, object args)
        {
            ReduceDuration(holder);

            //audio
            Vector3 loc = default(Vector3);
            string sound = "event:/Lunacy/Misc3/Rake";
            if (sender is IUnit caster)
            {
                try
                {
                    if (caster.UnitTypes == null || !caster.UnitTypes.Contains("FemaleID"))
                    {
                        if (UnityEngine.Random.Range(0, 100) < 30) sound = "event:/Lunacy/Misc3/RakeScream";
                    }


                    if (!caster.IsUnitCharacter)
                    {
                        loc = CombatManager.Instance._stats.combatUI._enemyZone._enemies[caster.FieldID].FieldEntity.Position;
                    }
                    else
                    {
                        loc = CombatManager.Instance._stats.combatUI._characterZone._characters[caster.FieldID].FieldEntity.Position;
                    }
                }
                catch { }
            }
            CombatManager.Instance.AddUIAction(new PlaySoundUIAction(sound, loc));


            //damage
            if (sender is IUnit unit) unit.Damage(9, null, "Rake", holder.SlotID - unit.SlotID, true, true, false, "");
            
        }
    }
    public class ApplyRakeEffect : FieldEffect_Apply_Effect
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            _Field = Rake.Object;
            return base.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable, out exitAmount);
        }
    }
}
