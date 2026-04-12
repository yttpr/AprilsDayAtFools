using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class NN
    {
        public static void Add()
        {
            PerformEffectPassiveAbility apocalyptic = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            apocalyptic._passiveName = "Apocalyptic";
            apocalyptic.name = "Apocalyptic_PA";
            apocalyptic.passiveIcon = ResourceLoader.LoadSprite("Temp_ApocalypticPassive.png");
            apocalyptic.m_PassiveID = "Apocalyptic_PA";
            apocalyptic._enemyDescription = "On being directly damaged, end combat for all party members.";
            apocalyptic._characterDescription = "On being directly damaged, end combat for all enemies.";
            apocalyptic.doesPassiveTriggerInformationPanel = true;
            apocalyptic.effects = Effects.GenerateEffect(ScriptableObject.CreateInstance<EndCombatTargetEffect>(), 1, Targeting.Unit_AllOpponents).SelfArray();
            apocalyptic._triggerOn = new TriggerCalls[1] { TriggerCalls.OnDirectDamaged };
            //apocalyptic.AddPassiveToGlossary("Apocalyptic", "On being directly damaged, end combat for all opponents.");
            //apocalyptic.AddToPassiveDatabase();

            Character nn = new Character("NN3X41-HK5", "NN_CH");
            nn.HealthColor = Pigments.Red;
            nn.AddUnitType("FemaleID");
            nn.AddUnitType("FemaleLooking");
            nn.UsesBasicAbility = true;
            //slap
            nn.UsesAllAbilities = false;
            nn.MovesOnOverworld = true;
            //animator
            nn.FrontSprite = ResourceLoader.LoadSprite("Temp_NNFront.png");
            nn.BackSprite = ResourceLoader.LoadSprite("Temp_NNBack.png");
            nn.OverworldSprite = ResourceLoader.LoadSprite("Temp_NNWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            nn.DamageSound = LoadedAssetsHandler.GetEnemy("Visage_MyOwn_EN").damageSound;
            nn.DeathSound = LoadedAssetsHandler.GetEnemy("Visage_MyOwn_EN").deathSound;
            nn.DialogueSound = LoadedAssetsHandler.GetEnemy("Visage_MyOwn_EN").damageSound;
            //nn.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            //nn.AddFinalBossAchievementData("Heaven", HeavenACH);
            if (April.Me)
            {
                nn.GenerateMenuCharacter(ResourceLoader.LoadSprite("Temp_NNMenu.png"), ResourceLoader.LoadSprite("Temp_NNLock.png"));
                nn.MenuCharacterIsSecret = true;
                nn.MenuCharacterIgnoreRandom = false;
                nn.SetMenuCharacterAsFullDPS();
            }
            nn.AddPassive(apocalyptic);

            Ability basic = new Ability("BX5D02", "NN_B_A");
            basic.Description = "Take 8 damage.";
            basic.AbilitySprite = ResourceLoader.LoadSprite("ability_b.png");
            basic.Cost = [Pigments.Red];
            basic.Effects = [Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 8, Slots.Self)];
            basic.AddIntentsToTarget(Slots.Self, ["Damage_7_10"]);
            basic.AnimationTarget = Slots.Self;
            basic.Visuals = Visuals.Wriggle;

            nn.BasicAbility = basic;

            ApplyKarmaEffect karma = ScriptableObject.CreateInstance<ApplyKarmaEffect>();
            HasStatusEffect hasKarma = ScriptableObject.CreateInstance<HasStatusEffect>();
            hasKarma.StatusID = Karma.StatusID;

            BaseCombatTargettingSO d_slots = Slots.SlotTarget([0, 1, 2], false);

            Ability d1 = new Ability("D-115BD-N", "NN_D_1_A");
            d1.Description = "Inflict 7 Karma on the Opposing enemy.\nIf they didn't already have Karma, inflict 4 more Karma on the Opposing, Right, and Far Right enemies.";
            d1.AbilitySprite = ResourceLoader.LoadSprite("ability_d.png");
            d1.Cost = [Pigments.Yellow, Pigments.Red, Pigments.Red];
            d1.Effects = new EffectInfo[3];
            d1.Effects[0] = Effects.GenerateEffect(hasKarma, 1, Slots.Front);
            d1.Effects[1] = Effects.GenerateEffect(karma, 7, Slots.Front);
            d1.Effects[2] = Effects.GenerateEffect(karma, 4, d_slots, BasicEffects.DidThat(false, 2));
            d1.AddIntentsToTarget(Slots.Front, ["Misc_Hidden"]);
            d1.AddIntentsToTarget(d_slots, [Karma.Intent]);
            d1.Visuals = CustomVisuals.GetVisuals("Salt/Decapitate");
            d1.AnimationTarget = Slots.Front;

            Ability d2 = new Ability(d1.ability, "NN_D_2_A", d1.Cost);
            d2.Name = "D-225BD-N";
            d2.Description = "Inflict 10 Karma on the Opposing enemy.\nIf they didn't already have Karma, inflict 5 more Karma on the Opposing, Right, and Far Right enemies.";
            d2.Effects[1].entryVariable = 10;
            d2.Effects[2].entryVariable = 5;

            Ability d3 = new Ability(d2.ability, "NN_D_3_A", d1.Cost);
            d3.Name = "D-335BD-N";
            d3.Description = "Inflict 13 Karma on the Opposing enemy.\nIf they didn't already have Karma, inflict 6 more Karma on the Opposing, Right, and Far Right enemies.";
            d3.Effects[1].entryVariable = 13;
            d3.Effects[2].entryVariable = 6;

            Ability d4 = new Ability(d3.ability, "NN_D_4_A", d1.Cost);
            d4.Name = "D-445BD-N";
            d4.Description = "Inflict 16 Karma on the Opposing enemy.\nIf they didn't already have Karma, inflict 7 more Karma on the Opposing, Right, and Far Right enemies.";
            d4.Effects[1].entryVariable = 16;
            d4.Effects[2].entryVariable = 7;

            ContinuousTargettingByStatus_Left k_targets = ScriptableObject.CreateInstance<ContinuousTargettingByStatus_Left>();
            k_targets._getAllies = false;
            k_targets._statusID = Karma.StatusID;
            k_targets._getAllSlots = true;
            ContinuousTargettingByStatus_Left k_apply = ScriptableObject.CreateInstance<ContinuousTargettingByStatus_Left>();
            k_apply._getAllSlots = false;
            k_apply._getAllies = false;
            k_apply._statusID = Karma.StatusID;

            Ability k1 = new Ability("K-87-F4J", "NN_K_1_A");
            k1.Description = "Inflict 7 Karma on the Opposing enemy. If they already have Karma, apply to their Left as well.\nRepeat until unable to.";
            k1.AbilitySprite = ResourceLoader.LoadSprite("ability_k.png");
            k1.Cost = [Pigments.Purple, Pigments.Red, Pigments.Red];
            k1.Effects = new EffectInfo[1];
            k1.Effects[0] = Effects.GenerateEffect(karma, 7, k_apply);
            k1.AddIntentsToTarget(Slots.SlotTarget([0, -1, -2, -3, -4], false), ["Misc_Hidden"]);
            k1.AddIntentsToTarget(k_apply, [Karma.Intent]);
            k1.Visuals = Visuals.WrigglingWrath;
            k1.AnimationTarget = k_targets;

            Ability k2 = new Ability(k1.ability, "NN_K_2_A", k1.Cost);
            k2.Name = "K-8787-F4J";
            k2.Description = "Inflict 10 Karma on the Opposing enemy. If they already have Karma, apply to their Left as well.\nRepeat until unable to.";
            k2.Effects[0].entryVariable = 10;

            Ability k3 = new Ability(k2.ability, "NN_K_3_A", k1.Cost);
            k3.Name = "K-878787-F4J";
            k3.Description = "Inflict 13 Karma on the Opposing enemy. If they already have Karma, apply to their Left as well.\nRepeat until unable to.";
            k3.Effects[0].entryVariable = 13;

            Ability k4 = new Ability(k3.ability, "NN_K_4_A", k1.Cost);
            k4.Name = "K-87878789-F4J";
            k4.Description = "Inflict 16 Karma on the Opposing enemy. If they already have Karma, apply to their Left as well.\nRepeat until unable to.";
            k4.Effects[0].entryVariable = 16;

            Targetting_ByUnit_Side_Specific_Status target_karma = ScriptableObject.CreateInstance<Targetting_ByUnit_Side_Specific_Status>();
            target_karma.m_GetBySpecificStatus = true;
            target_karma.m_SpecificStatus = [Karma.Object];

            Ability f1 = new Ability("72S-BP99-0F1", "NN_F_1_A");
            f1.Description = "Inflict 6 Karma on all enemies with Karma.\nIf no Karma is applied, inflict 4 Karma on all enemies.";
            f1.AbilitySprite = ResourceLoader.LoadSprite("ability_f.png");
            f1.Cost = [Pigments.Blue, Pigments.Red, Pigments.Red];
            f1.Effects = new EffectInfo[5];
            f1.Effects[0] = Effects.GenerateEffect(hasKarma, 1, target_karma);
            f1.Effects[1] = Effects.GenerateEffect(BasicEffects.GetVisuals("OfDeath_1_A", true, target_karma), 0, null, BasicEffects.DidThat(true));
            f1.Effects[2] = Effects.GenerateEffect(karma, 6, target_karma);
            f1.Effects[3] = Effects.GenerateEffect(BasicEffects.GetVisuals("OfDeath_1_A", true, Targeting.Unit_AllOpponents), 0, null, BasicEffects.DidThat(false, 3));
            f1.Effects[4] = Effects.GenerateEffect(karma, 4, Targeting.Unit_AllOpponents, BasicEffects.DidThat(false, 2));
            f1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Misc_Hidden", Karma.Intent]);
            f1.AnimationTarget = Slots.Self;
            f1.Visuals = null;

            Ability f2 = new Ability(f1.ability, "NN_F_2_A", f1.Cost);
            f2.Name = "83T-BP99-0F1";
            f2.Description = "Inflict 8 Karma on all enemies with Karma.\nIf no Karma is applied, inflict 5 Karma on all enemies.";
            f2.Effects[2].entryVariable = 8;
            f2.Effects[4].entryVariable = 5;

            Ability f3 = new Ability(f2.ability, "NN_F_3_A", f1.Cost);
            f3.Name = "94U-BP99-0F1";
            f3.Description = "Inflict 10 Karma on all enemies with Karma.\nIf no Karma is applied, inflict 6 Karma on all enemies.";
            f3.Effects[2].entryVariable = 10;
            f3.Effects[4].entryVariable = 6;

            Ability f4 = new Ability(f3.ability, "NN_F_4_A", f1.Cost);
            f4.Name = "05V-BP99-0F1";
            f4.Description = "Inflict 12 Karma on all enemies with Karma.\nIf no Karma is applied, inflict 7 Karma on all enemies.";
            f4.Effects[2].entryVariable = 12;
            f4.Effects[4].entryVariable = 7;

            nn.AddLevelData(20, [f1, k1, d1]);
            nn.AddLevelData(22, [f2, k2, d2]);
            nn.AddLevelData(24, [f3, k3, d3]);
            nn.AddLevelData(25, [f4, k4, d4]);
            nn.AddCharacter(April.Me, !April.Me);
        }
    }
}
