using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class RandomAbilityPassive : BasePassiveAbilitySO
    {
        public static ExtraAbilityInfo GetRandomItemAbility()
        {
            //LoadedDBsHandler
            //CasterAddRandomExtraAbilityEffect effect = (LoadedAssetsHandler.GetCharacter("Doll_CH").passiveAbilities[0] as Connection_PerformEffectPassiveAbility).connectionEffects[1].effect as CasterAddRandomExtraAbilityEffect;
            Connection_PerformEffectPassiveAbility connection_PerformEffectPassiveAbility = LoadedAssetsHandler.GetCharacter("Doll_CH").passiveAbilities[0] as Connection_PerformEffectPassiveAbility;
            CasterAddRandomExtraAbilityEffect effect = connection_PerformEffectPassiveAbility.connectionEffects[1].effect as CasterAddRandomExtraAbilityEffect;
            List<BasicAbilityChange_Wearable_SMS> changeWearableSmsList = new List<BasicAbilityChange_Wearable_SMS>(effect._slapData);
            List<ExtraAbility_Wearable_SMS> abilityWearableSmsList = new List<ExtraAbility_Wearable_SMS>(effect._extraData);
            int count1 = changeWearableSmsList.Count;
            int count2 = abilityWearableSmsList.Count;
            int index1 = UnityEngine.Random.Range(0, count1 + count2);
            ExtraAbilityInfo randomItemAbility;
            RaritySO rar = ScriptableObject.CreateInstance<RaritySO>();
            rar.canBeRerolled = true;
            rar.rarityValue = 5;
            if (index1 < changeWearableSmsList.Count)
            {
                BasicAbilityChange_Wearable_SMS changeWearableSms = changeWearableSmsList[index1];
                changeWearableSmsList.RemoveAt(index1);
                int num = count1 - 1;
                randomItemAbility = new ExtraAbilityInfo(changeWearableSms.BasicAbility);
            }
            else
            {
                int index2 = index1 - count1;
                ExtraAbility_Wearable_SMS abilityWearableSms = abilityWearableSmsList[index2];
                abilityWearableSmsList.RemoveAt(index2);
                int num = count2 - 1;
                randomItemAbility = new ExtraAbilityInfo(abilityWearableSms.ExtraAbility);
            }
            randomItemAbility.rarity = rar;
            return randomItemAbility;
        }
        private Dictionary<IUnit, ExtraAbilityInfo> extraAbilities;

        public override bool IsPassiveImmediate => true;

        public override bool DoesPassiveTrigger => true;

        public override void TriggerPassive(object sender, object args)
        {
            IUnit key = sender as IUnit;
            ExtraAbilityInfo extraAbilityInfo;
            if (this.extraAbilities.TryGetValue(key, out extraAbilityInfo))
            {
                key.TryRemoveExtraAbility(extraAbilityInfo);
                this.extraAbilities.Remove(key);
            }
            ExtraAbilityInfo randomItemAbility = GetRandomItemAbility();
            this.extraAbilities.Add(key, randomItemAbility);
            key.AddExtraAbility(this.extraAbilities[key]);
        }

        public override void OnPassiveConnected(IUnit unit)
        {
            if (this.extraAbilities == null)
                this.extraAbilities = new Dictionary<IUnit, ExtraAbilityInfo>();
            ExtraAbilityInfo randomItemAbility = GetRandomItemAbility();
            this.extraAbilities.Add(unit, randomItemAbility);
            unit.AddExtraAbility(this.extraAbilities[unit]); //EnemyCombat e;
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
            ExtraAbilityInfo extraAbilityInfo;
            if (!this.extraAbilities.TryGetValue(unit, out extraAbilityInfo))
                return;
            unit.TryRemoveExtraAbility(extraAbilityInfo);
            this.extraAbilities.Remove(unit);
        }
    }
    public class RerollConstructEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                    CombatManager.Instance.PostNotification(((TriggerCalls)889532).ToString(), target.Unit, null);
            }
            return true;
        }
    }
}
