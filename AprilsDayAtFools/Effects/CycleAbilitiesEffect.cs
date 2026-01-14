using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class CycleAbilitiesEffect : EffectSO
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
        public static CombatAbility FromExtraAbility(ExtraAbilityInfo info)
        {
            CombatAbility ret = new CombatAbility(info.ability, info.cost);
            ret.rarity = info.rarity;
            return ret;
        }
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit && target.Unit is CharacterCombat chara)
                {
                    if (!chara.Character.HasRankedData)
                    {
                        bool addbasic = chara.Character.usesBasicAbility;
                        if (addbasic)
                        {
                            foreach (CombatAbility ability in chara.CombatAbilities)
                            {
                                if (ability.ability == chara.Character.basicCharAbility.ability) addbasic = false;
                            }
                        }

                        chara.CombatAbilities.RemoveAt(0);
                        
                        if (addbasic) chara.CombatAbilities.Add(new CombatAbility(chara.Character.basicCharAbility));
                        else chara.CombatAbilities.Add(FromExtraAbility(GetRandomItemAbility()));
                    }
                    else
                    {
                        CharacterRankedData rank = chara.Character.rankedData[chara.Character.ClampRank(chara.Rank)];

                        List<CharacterAbility> grab = [.. rank.rankAbilities];
                        if (chara.Character.usesBasicAbility) grab.Add(chara.Character.basicCharAbility);

                        foreach (CombatAbility ability in chara.CombatAbilities)
                        {
                            for (int i = 0; i < grab.Count; i++)
                            {
                                if (grab[i].ability == ability.ability)
                                {
                                    grab.RemoveAt(i);
                                    break;
                                }
                            }
                        }

                        chara.CombatAbilities.RemoveAt(0);

                        if (grab.Count > 0) chara.CombatAbilities.Add(new CombatAbility(grab.GetRandom()));
                        else chara.CombatAbilities.Add(FromExtraAbility(GetRandomItemAbility()));
                    }

                    chara.TriggerNotification(IDs.Initialize, null);

                    CombatManager.Instance.AddUIAction(new CharacterUpdateAllAttacksUIAction(chara.ID, chara.CombatAbilities.ToArray()));

                    CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(chara.ID, chara.IsUnitCharacter, chara.CombatAbilities[chara.CombatAbilities.Count - 1].ability._abilityName + " Added", chara.CombatAbilities[chara.CombatAbilities.Count - 1].ability.abilitySprite));

                    exitAmount++;
                }
            }

            return exitAmount > 0;
        }
    }
}
