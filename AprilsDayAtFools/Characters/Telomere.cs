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
            EffectConditionSO life = ScriptableObject.CreateInstance<IsAliveEffectCondition>();

            Ability minds = new Ability("Exchange of Minds", "Telomere_ExchangeOfMinds_A");
            minds.Description = "Use one of the Opposing enemy's abilities.\nForce the Opposing enemy to use one of this party member's abilities.";
            minds.AbilitySprite = ResourceLoader.LoadSprite("ability_minds.png");
            minds.Cost = [Pigments.Grey, Pigments.Grey, Pigments.Grey];
            minds.Effects = new EffectInfo[2];
            minds.Effects[0] = Effects.GenerateEffect(cast, 1, Slots.Front, life);
            minds.Effects[1] = Effects.GenerateEffect(ImmediateActionEffect.Create([Effects.GenerateEffect(cast, 1, Slots.Front, life)]), 1, Slots.Front);
            minds.AddIntentsToTarget(Slots.Self, ["Misc"]);
            minds.AddIntentsToTarget(Slots.Front, ["Misc"]);
            minds.AnimationTarget = selfandfront;
            minds.Visuals = LoadedAssetsHandler.GetCharacterAbility("Entwined_1_A").visuals;

            Character telomere = new Character("Telomere", "Telomere_CH");
            telomere.HealthColor = Pigments.Grey;
            telomere.AddUnitType("FemaleID");
            telomere.AddUnitType("Sandwich_Gambling");
            telomere.AddUnitType("FemaleLooking");
            telomere.UsesBasicAbility = true;
            telomere.SetBasicAbility(minds.GenerateCharacterAbility(true));
            telomere.UsesAllAbilities = false;
            telomere.MovesOnOverworld = true;
            telomere.Animator = LoadedAssetsHandler.GetCharacter("Rotcore_CH").characterAnimator;
            telomere.FrontSprite = ResourceLoader.LoadSprite("TelomereFront.png");
            telomere.BackSprite = ResourceLoader.LoadSprite("TelomereBack.png");
            telomere.OverworldSprite = ResourceLoader.LoadSprite("TelomereWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            telomere.DamageSound = LoadedAssetsHandler.GetEnemy("ChoirBoy_EN").damageSound;
            telomere.DeathSound = LoadedAssetsHandler.GetEnemy("ChoirBoy_EN").deathSound;
            telomere.DialogueSound = LoadedAssetsHandler.GetEnemy("ChoirBoy_EN").damageSound;
            //telomere.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            //telomere.AddFinalBossAchievementData("Heaven", HeavenACH);
            telomere.GenerateMenuCharacter(ResourceLoader.LoadSprite("TelomereMenu.png"), ResourceLoader.LoadSprite("TelomereLock.png"));
            telomere.MenuCharacterIsSecret = false;
            telomere.MenuCharacterIgnoreRandom = false;
            //dps or support
            telomere.AddPassive(Passives.LeakyGenerator(2));


            Ability break1 = new Ability("Breakdown Flesh", "Telomere_Break_1_A");
            break1.Description = "Take 2 damage. Gain 1 Frail.";
            break1.AbilitySprite = ResourceLoader.LoadSprite("ability_breakdown.png");
            break1.Cost = [Pigments.YellowBlue];
            break1.Effects = new EffectInfo[2];
            break1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 2, Slots.Self);
            break1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyFrailEffect>(), 1, Slots.Self);
            break1.AddIntentsToTarget(Slots.Self, ["Damage_1_2", "Status_Frail"]);
            break1.Visuals = CustomVisuals.GetVisuals("Salt/Gears");
            break1.AnimationTarget = Slots.Self;

            Ability break2 = new Ability(break1.ability, "Telomere_Break_2_A", break1.Cost);
            break2.Name = "Breakdown Organ";
            break2.Description = "Take 4 damage. Gain 1 Frail.";
            break2.Effects[0].entryVariable = 4;
            break2.EffectIntents[0].intents[0] = "Damage_3_6";

            Ability break3 = new Ability(break2.ability, "Telomere_Break_3_A", break1.Cost);
            break3.Name = "Breakdown Bone";
            break3.Description = "Take 5 damage. Gain 1 Frail.";
            break3.Effects[0].entryVariable = 5;

            Ability break4 = new Ability(break3.ability, "Telomere_Break_4_A", break1.Cost);
            break4.Name = "Breakdown Body";
            break4.Description = "Take 7 damage. Gain 1 Frail.";
            break4.Effects[0].entryVariable = 7;
            break4.EffectIntents[0].intents[0] = "Damage_7_10";

            ApplyScarsEffect scars = ScriptableObject.CreateInstance<ApplyScarsEffect>();
            scars._RandomBetweenPrevious = true;

            Ability shell1 = new Ability("Crack Shell", "Telomere_Shell_1_A");
            shell1.Description = "Generate 3 Blue Pigment. Gain 0-1 Scars.";
            shell1.AbilitySprite = ResourceLoader.LoadSprite("ability_egg.png");
            shell1.Cost = [Pigments.YellowRed];
            shell1.Effects = new EffectInfo[3];
            shell1.Effects[0] = Effects.GenerateEffect(BasicEffects.GenPigment(Pigments.Blue), 3, Slots.Self);
            shell1.Effects[1] = Effects.GenerateEffect(BasicEffects.Empty, 0);
            shell1.Effects[2] = Effects.GenerateEffect(scars, 1, Slots.Self);
            shell1.AddIntentsToTarget(Slots.Self, ["Mana_Generate", "Status_Scars"]);
            shell1.Visuals = CustomVisuals.GetVisuals("Salt/Think");
            shell1.AnimationTarget = Slots.Self;

            Ability shell2 = new Ability(shell1.ability, "Telomere_Shell_2_A", shell1.Cost);
            shell2.Name = "Crack Shell";
            shell2.Description = "Generate 3 Blue Pigment. Gain 0-2 Scars.";
            shell2.Effects[2].entryVariable = 2;

            Ability shell3 = new Ability(shell2.ability, "Telomere_Shell_3_A", shell1.Cost);
            shell3.Name = "Splatter Shell";
            shell3.Description = "Generate 3 Blue Pigment. Gain 0-3 Scars.";
            shell3.Effects[2].entryVariable = 3;

            Ability shell4 = new Ability(shell3.ability, "Telomere_Shell_4_A", shell1.Cost);
            shell4.Name = "Obliterate Shell";
            shell4.Description = "Generate 3 Blue Pigment. Gain 1-3 Scars.";
            shell4.Effects[1].entryVariable = 1;

            RandomizeTargetHealthColorsEffect red = ScriptableObject.CreateInstance<RandomizeTargetHealthColorsEffect>();
            red.options = [Pigments.Red];

            Ability pain1 = new Ability("Pain Flow", "Telomere_Pain_1_A");
            pain1.Description = "Turn Red. Gain 2 Ruptured.";
            pain1.AbilitySprite = ResourceLoader.LoadSprite("ability_pain.png");
            pain1.Cost = [Pigments.YellowPurple];
            pain1.Effects = new EffectInfo[2];
            pain1.Effects[0] = Effects.GenerateEffect(red, 1, Slots.Self);
            pain1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyRupturedEffect>(), 2, Slots.Self);
            pain1.AddIntentsToTarget(Slots.Self, ["Mana_Modify", "Status_Ruptured"]);
            pain1.Visuals = Visuals.Quills;
            pain1.AnimationTarget = Slots.Self;

            Ability pain2 = new Ability(pain1.ability, "Telomere_Pain_2_A", pain1.Cost);
            pain2.Name = "Pain River";
            pain2.Description = "Turn Red. Gain 3 Ruptured.";
            pain2.Effects[1].entryVariable = 3;

            Ability pain3 = new Ability(pain2.ability, "Telomere_Pain_3_A", pain1.Cost);
            pain3.Name = "Pain Current";
            pain3.Description = "Turn Red. Gain 5 Ruptured.";
            pain3.Effects[1].entryVariable = 5;

            Ability pain4 = new Ability(pain3.ability, "Telomere_Pain_4_A", pain1.Cost);
            pain4.Name = "Pain Flood";
            pain4.Description = "Turn Red. Gain 7 Ruptured.";
            pain4.Effects[1].entryVariable = 7;


            telomere.AddLevelData(20, [shell1, pain1, break1]);
            telomere.AddLevelData(23, [shell2, pain2, break2]);
            telomere.AddLevelData(25, [shell3, pain3, break3]);
            telomere.AddLevelData(27, [shell4, pain4, break4]);
            telomere.AddCharacter(true);
        }
    }
}
