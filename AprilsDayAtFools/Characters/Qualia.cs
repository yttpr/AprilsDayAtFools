using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Qualia
    {
        public static void Test()
        {
            ImmediatePerformEffectPassive depiction = ScriptableObject.CreateInstance<ImmediatePerformEffectPassive>();
            depiction._passiveName = "Depiction";
            depiction.passiveIcon = ResourceLoader.LoadSprite("DepictionPassive.png");
            depiction.m_PassiveID = "Depiction_PA";
            depiction._enemyDescription = "This enemy is only temporary";
            depiction._characterDescription = "This party member is only temporary.";
            depiction.doesPassiveTriggerInformationPanel = true;
            depiction.conditions = [];
            depiction.effects = [Effects.GenerateEffect(ScriptableObject.CreateInstance<FleeTargetEffect>(), 1, Slots.Self)];
            depiction._triggerOn = [TimelineEndHandler.Before];
            depiction.AddToPassiveDatabase();
            depiction.AddPassiveToGlossary("Depiction", "This unit is only temporary.");

            ExtraPassiveAbility_Wearable_SMS add_picture = ScriptableObject.CreateInstance<ExtraPassiveAbility_Wearable_SMS>();
            add_picture._extraPassiveAbility = depiction;

            TemporaryReplacementEffect replace_all = ScriptableObject.CreateInstance<TemporaryReplacementEffect>();
            replace_all._extraModifiers = [add_picture];

            TemporaryReplacementEffect replace_unused = ScriptableObject.CreateInstance<TemporaryReplacementEffect>();
            replace_unused._extraModifiers = [add_picture];
        }
    }
}
