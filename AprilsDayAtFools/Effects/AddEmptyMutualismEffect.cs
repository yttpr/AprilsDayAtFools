using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class AddEmptyMutualismEffect : EffectSO
    {
        public ParasitePassiveAbility Mutualism;
        public static string Value => UnitStoredValueNames_GameIDs.ParasiteCurrentHealthPA.ToString();
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (Mutualism == null)
            {
                Mutualism = ScriptableObject.CreateInstance<ParasitePassiveAbility>();
                Mutualism.conditions = ((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd.conditions;
                Mutualism._damagePercentage = ((ParasitePassiveAbility)((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd)._damagePercentage;
                Mutualism.connectionImmediateEffect = ((ParasitePassiveAbility)((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd).connectionImmediateEffect;
                Mutualism.disconnectionImmediateEffect = ((ParasitePassiveAbility)((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd).disconnectionImmediateEffect;
                Mutualism.doesPassiveTriggerInformationPanel = ((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd.doesPassiveTriggerInformationPanel;
                Mutualism.effects = ((ParasitePassiveAbility)((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd).effects;
                Mutualism.passiveIcon = ((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd.passiveIcon;
                Mutualism.specialStoredData = ((ParasitePassiveAbility)((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd).specialStoredData;
                Mutualism.m_PassiveID = Passives.ParasiteMutualism.m_PassiveID;
                Mutualism._characterDescription = ((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd._characterDescription;
                Mutualism._damagePercentage = ((ParasitePassiveAbility)((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd)._damagePercentage;
                Mutualism._enemyDescription = ((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd._characterDescription;
                Mutualism._isFriendly = true;
                Mutualism._parasiteShield = ((ParasitePassiveAbility)((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd)._parasiteShield;
                Mutualism._passiveName = ((ParasitePassiveAbility)((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd)._passiveName;
                Mutualism._secondTriggerOn = ((ParasitePassiveAbility)((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd)._secondTriggerOn;
                Mutualism._thirdTriggerOn = ((ParasitePassiveAbility)((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd)._thirdTriggerOn;
                Mutualism._triggerOn = ((AddPassiveEffect)LoadedAssetsHandler.GetCharacterAbility("Symbiosis_1_A").effects[5].effect)._passiveToAdd._triggerOn;
                Mutualism.connectionEffects = [];
                Mutualism.disconnectionEffects = [];
            }

            exitAmount = 0;

            List<int> ids = [];
            List<bool> charas = [];
            List<string> name = [];
            List<Sprite> icon = [];

            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    if (!target.Unit.ContainsPassiveAbility(Mutualism.m_PassiveID))
                    {
                        target.Unit.AddPassiveAbility(Mutualism);
                        exitAmount++;
                    }
                    target.Unit.SimpleSetStoredValue(Value, target.Unit.SimpleGetStoredValue(Value) + entryVariable);

                    ids.Add(target.Unit.ID);
                    charas.Add(target.Unit.IsUnitCharacter);
                    name.Add(Mutualism.GetPassiveLocData().text);
                    icon.Add(Mutualism.passiveIcon);
                }
            }

            if (ids.Count > 0) CombatManager.Instance.AddUIAction(new ShowMultiplePassiveInformationUIAction(ids.ToArray(), charas.ToArray(), name.ToArray(), icon.ToArray()));

            return exitAmount > 0;
        }
    }
}
