using BrutalAPI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Infanticide
    {
        public static void Add()
        {
            ExtraCCSprites_BasicSO focus = ScriptableObject.CreateInstance<ExtraCCSprites_BasicSO>();
            focus._DefaultID = CharacterExtraSprite_GameIDs.Unfocused.ToString();
            focus._SpecialID = CharacterExtraSprite_GameIDs.Focused.ToString();
            focus._backSprite = ResourceLoader.LoadSprite("InfanticideBack.png");
            focus._frontSprite = ResourceLoader.LoadSprite("InfanticideFocus.png");

            BasePassiveAbilitySO passive = ScriptableObject.Instantiate(Passives.Focus);
            passive.name = "ADAF_Focus_Generic";
            passive._characterDescription = "Upon killing an enemy, gain Focused.";

            Character infanticide = new Character("Infanticide", "Infanticide_CH");
            infanticide.HealthColor = Pigments.Purple;
            infanticide.AddUnitType("FemaleID");
            infanticide.AddUnitType("Sandwich_Silly");
            infanticide.AddUnitType("FemaleLooking");
            infanticide.UsesBasicAbility = true;
            //slap
            infanticide.UsesAllAbilities = false;
            infanticide.MovesOnOverworld = true;
            //animator
            infanticide.FrontSprite = ResourceLoader.LoadSprite("InfanticideFront.png");
            infanticide.BackSprite = ResourceLoader.LoadSprite("InfanticideBack.png");
            infanticide.OverworldSprite = ResourceLoader.LoadSprite("InfanticideWorld.png", new Vector2(0.5f, 0f));
            infanticide.ExtraSprites = focus;
            infanticide.DamageSound = LoadedAssetsHandler.GetEnemy("Bronzo_Bananas_Mean_EN").damageSound;
            infanticide.DeathSound = LoadedAssetsHandler.GetEnemy("Bronzo_Bananas_Mean_EN").deathSound;
            infanticide.DialogueSound = LoadedAssetsHandler.GetEnemy("Bronzo_Bananas_Mean_EN").damageSound;
            //infanticide.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            //infanticide.AddFinalBossAchievementData("Heaven", HeavenACH);
            infanticide.GenerateMenuCharacter(ResourceLoader.LoadSprite("InfanticideMenu.png"), ResourceLoader.LoadSprite("InfanticideLock.png"));
            infanticide.MenuCharacterIsSecret = false;
            infanticide.MenuCharacterIgnoreRandom = false;
            infanticide.SetMenuCharacterAsFullDPS();
            infanticide.AddPassive(passive);

            CheckMultiplePassivesEffect moving = ScriptableObject.CreateInstance<CheckMultiplePassivesEffect>();
            moving.m_PassiveIDs = [
                Passives.Skittish.m_PassiveID, Passives.Slippery.m_PassiveID, Passives.Constricting.m_PassiveID, "Anchored",
                "Jumpy_PA", "Lightweight_PA", "Scramble_PA", "Evasive_PA", "Turbulent_PA", "CCTV_PA", "Jittery_PA", "Fluttery_PA", "Warping_PA",
                "Lonely_PA", "Melancholy_PA", "Gluttony_PA", "Rotary_PA", "Marching_PA", "Hiding_PA", "Seeking_PA",
                "Lockstep_ID", "Cadence_ID", "RightStrafe_ID", "LeftStrafe_ID"
                ];

            CheckPassiveAbilityEffect infant = ScriptableObject.CreateInstance<CheckPassiveAbilityEffect>();
            infant.m_PassiveID = "Infantile";

            CheckPassiveAbilityEffect wither = ScriptableObject.CreateInstance<CheckPassiveAbilityEffect>();
            wither.m_PassiveID = "Withering";

            DamageEffect damage = ScriptableObject.CreateInstance<DamageEffect>();

            Ability violence1 = new Ability("Violence Against Youth", "Girl_Abort_1_A");
            violence1.Description = "Deal 5 damage to the Opposing enemy, deal 13 damage instead of they have Infantile as a passive.";
            violence1.AbilitySprite = ResourceLoader.LoadSprite("ability_violence.png");
            violence1.Cost = [Pigments.Red, Pigments.Red];
            violence1.Effects = new EffectInfo[3];
            violence1.Effects[0] = Effects.GenerateEffect(infant, 1, Slots.Front);
            violence1.Effects[1] = Effects.GenerateEffect(damage, 13, Slots.Front, BasicEffects.DidThat(true));
            violence1.Effects[2] = Effects.GenerateEffect(damage, 5, Slots.Front, BasicEffects.DidThat(false, 2));
            violence1.AddIntentsToTarget(Slots.Front, ["Misc_Hidden", "Damage_3_6", "Damage_11_15"]);
            violence1.Visuals = Visuals.Crush;
            violence1.AnimationTarget = Slots.Front;

            Ability violence2 = new Ability(violence1.ability, "Girl_Abort_2_A", violence1.Cost);
            violence2.Name = "Violence Against Young";
            violence2.Description = "Deal 7 damage to the Opposing enemy, deal 17 damage instead of they have Infantile as a passive.";
            violence2.Effects[1].entryVariable = 17;
            violence2.Effects[2].entryVariable = 7;
            violence2.EffectIntents[0].intents[1] = "Damage_7_10";
            violence2.EffectIntents[0].intents[2] = "Damage_16_20";

            Ability violence3 = new Ability(violence2.ability, "Girl_Abort_3_A", violence1.Cost);
            violence3.Name = "Violence Against Infants";
            violence3.Description = "Deal 9 damage to the Opposing enemy, deal 21 damage instead of they have Infantile as a passive.";
            violence3.Effects[1].entryVariable = 21;
            violence3.Effects[2].entryVariable = 9;
            violence3.EffectIntents[0].intents[2] = "Damage_21";

            Ability violence4 = new Ability(violence3.ability, "Girl_Abort_4_A", violence1.Cost);
            violence4.Name = "Violence Against Fetus";
            violence4.Description = "Deal 11 damage to the Opposing enemy, deal 26 damage instead of they have Infantile as a passive.";
            violence4.Effects[1].entryVariable = 26;
            violence4.Effects[2].entryVariable = 11;
            violence4.EffectIntents[0].intents[1] = "Damage_11_15";

            Ability fun1 = new Ability("No Room for Fun", "Girl_Fun_1_A");
            fun1.Description = "Deal 4 damage to the Opposing enemy.\nIf they have a common movement passive, inflict 4 Ruptured on all enemies.";
            fun1.AbilitySprite = ResourceLoader.LoadSprite("ability_fun.png");
            fun1.Cost = [Pigments.Grey];
            fun1.Effects = new EffectInfo[3];
            fun1.Effects[0] = Effects.GenerateEffect(damage, 4, Slots.Front);
            fun1.Effects[1] = Effects.GenerateEffect(moving, 1, Slots.Front);
            fun1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyRupturedEffect>(), 4, Targeting.Unit_AllOpponents, BasicEffects.DidThat(true));
            fun1.AddIntentsToTarget(Slots.Front, ["Damage_3_6", "Misc_Hidden"]);
            fun1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Status_Ruptured"]);
            fun1.Visuals = Visuals.Melt;
            fun1.AnimationTarget = Slots.Front;

            Ability fun2 = new Ability(fun1.ability, "Girl_Fun_2_A", fun1.Cost);
            fun2.Name = "No Leeway for Fun";
            fun2.Description = "Deal 6 damage to the Opposing enemy.\nIf they have a common movement passive, inflict 4 Ruptured on all enemies.";
            fun2.Effects[0].entryVariable = 6;

            Ability fun3 = new Ability(fun2.ability, "Girl_Fun_3_A", fun1.Cost);
            fun3.Name = "No Opportunity for Fun";
            fun3.Description = "Deal 7 damage to the Opposing enemy.\nIf they have a common movement passive, inflict 5 Ruptured on all enemies.";
            fun3.Effects[0].entryVariable = 7;
            fun3.Effects[2].entryVariable = 5;
            fun3.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability fun4 = new Ability(fun3.ability, "Girl_Fun_4_A", fun1.Cost);
            fun4.Name = "No Freedom for Fun";
            fun4.Description = "Deal 8 damage to the Opposing enemy.\nIf they have a common movement passive, inflict 6 Ruptured on all enemies.";
            fun4.Effects[0].entryVariable = 8;
            fun4.Effects[2].entryVariable = 6;

            Ability away1 = new Ability("Go Away", "Girl_Away_1_A");
            away1.Description = "Deal 7 damage to the Opposing enemy.\nIf the Opposing enemy is Withering, instantly kill them.";
            away1.AbilitySprite = ResourceLoader.LoadSprite("ability_away.png");
            away1.Cost = [Pigments.Blue, Pigments.Red, Pigments.Red];
            away1.Effects = new EffectInfo[3];
            away1.Effects[0] = Effects.GenerateEffect(damage, 7, Slots.Front);
            away1.Effects[1] = Effects.GenerateEffect(wither, 1, Slots.Front);
            away1.Effects[2] = Effects.GenerateEffect(BasicEffects.Die(), 1, Slots.Front, BasicEffects.DidThat(true));
            away1.AddIntentsToTarget(Slots.Front, ["Damage_7_10", "Misc_Hidden", "Damage_Death"]);
            away1.AnimationTarget = Slots.Front;
            away1.Visuals = Visuals.Parry;

            Ability away2 = new Ability(away1.ability, "Girl_Away_2_A", away1.Cost);
            away2.Name = "Begone Away";
            away2.Description = "Deal 9 damage to the Opposing enemy.\nIf the Opposing enemy is Withering, instantly kill them.";
            away2.Effects[0].entryVariable = 9;

            Ability away3 = new Ability(away2.ability, "Girl_Away_3_A", away1.Cost);
            away3.Name = "Vanish Away";
            away3.Description = "Deal 11 damage to the Opposing enemy.\nIf the Opposing enemy is Withering, instantly kill them.";
            away3.Effects[0].entryVariable = 11;
            away3.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability away4 = new Ability(away3.ability, "Girl_Away_4_A", away1.Cost);
            away4.Name = "Disappear Away";
            away4.Description = "Deal 13 damage to the Opposing enemy.\nIf the Opposing enemy is Withering, instantly kill them.";
            away4.Effects[0].entryVariable = 13;

            infanticide.AddLevelData(10, [away1, violence1, fun1]);
            infanticide.AddLevelData(13, [away2, violence2, fun2]);
            infanticide.AddLevelData(16, [away3, violence3, fun3]);
            infanticide.AddLevelData(18, [away4, violence4, fun4]);
            infanticide.AddCharacter(true);
        }
    }
}
