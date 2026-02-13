using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Telomere
    {
        public static void Add()
        {
            CasterPerformRandomTargetAbilityEffect cast = ScriptableObject.CreateInstance<CasterPerformRandomTargetAbilityEffect>();
            DoubleTargetting selfandfront = ScriptableObject.CreateInstance<DoubleTargetting>();
            selfandfront.firstTargetting = Slots.Self;
            selfandfront.secondTargetting = Slots.Front;

            Ability minds = new Ability("Exchange of Minds", "Telomere_ExchangeOfMinds_A");
            minds.Description = "Use one of the Opposing enemy's abilities.\nForce the Opposing enemy to use one of this party member's abilities.";
            minds.AbilitySprite = ResourceLoader.LoadSprite("ability_minds.png");
            minds.Cost = [Pigments.Grey, Pigments.Grey, Pigments.Grey];
            minds.Effects = new EffectInfo[2];
            minds.Effects[0] = Effects.GenerateEffect(cast, 1, Slots.Front);
            minds.Effects[1] = Effects.GenerateEffect(SubActionEffect.Create([Effects.GenerateEffect(cast, 1, Slots.Front)]), 1, Slots.Front);
            minds.AddIntentsToTarget(Slots.Self, ["Misc"]);
            minds.AddIntentsToTarget(Slots.Front, ["Misc"]);
            minds.AnimationTarget = selfandfront;
            minds.Visuals = Visuals.Connection;

            Character telomere = new Character("Telomere", "Telomere_CH");
            telomere.HealthColor = Pigments.Grey;
            telomere.AddUnitType("FemaleID");
            telomere.AddUnitType("Sandwich_Gambling");
            telomere.AddUnitType("FemaleLooking");
            telomere.UsesBasicAbility = true;
            //slap
            telomere.UsesAllAbilities = false;
            telomere.MovesOnOverworld = true;
            //animator
            telomere.FrontSprite = ResourceLoader.LoadSprite("InfanticideFront.png");
            telomere.BackSprite = ResourceLoader.LoadSprite("InfanticideBack.png");
            telomere.OverworldSprite = ResourceLoader.LoadSprite("InfanticideWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            telomere.DamageSound = LoadedAssetsHandler.GetEnemy("Bronzo_Bananas_Mean_EN").damageSound;
            telomere.DeathSound = LoadedAssetsHandler.GetEnemy("Bronzo_Bananas_Mean_EN").deathSound;
            telomere.DialogueSound = LoadedAssetsHandler.GetEnemy("Bronzo_Bananas_Mean_EN").damageSound;
            //infanticide.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            //infanticide.AddFinalBossAchievementData("Heaven", HeavenACH);
            telomere.GenerateMenuCharacter(ResourceLoader.LoadSprite("InfanticideMenu.png"), ResourceLoader.LoadSprite("InfanticideLock.png"));
            telomere.MenuCharacterIsSecret = false;
            telomere.MenuCharacterIgnoreRandom = false;
            telomere.SetMenuCharacterAsFullDPS();
            telomere.SetBasicAbility(minds.GenerateCharacterAbility(true));


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


            telomere.AddLevelData(10, [away1, violence1, fun1]);
            telomere.AddLevelData(13, [away2, violence2, fun2]);
            telomere.AddLevelData(16, [away3, violence3, fun3]);
            telomere.AddLevelData(18, [away4, violence4, fun4]);
            telomere.AddCharacter(true);
        }
    }
}
