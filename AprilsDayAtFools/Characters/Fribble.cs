using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Tools;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Fribble
    {
        public static void Add()
        {
            Character fribble = new Character("Fribble", "Fribble_CH");
            fribble.HealthColor = Pigments.Purple;
            fribble.AddUnitType("FemaleID");
            fribble.AddUnitType("Sandwich_NULL");
            fribble.AddUnitType("FemaleLooking");
            fribble.UsesBasicAbility = true;
            //slap
            fribble.UsesAllAbilities = false;
            fribble.MovesOnOverworld = true;
            //animator
            fribble.FrontSprite = ResourceLoader.LoadSprite("FribbleFront.png");
            fribble.BackSprite = ResourceLoader.LoadSprite("FribbleBack.png");
            fribble.OverworldSprite = ResourceLoader.LoadSprite("FribbleWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            fribble.DamageSound = LoadedAssetsHandler.GetEnemy("SkinningHomunculus_EN").damageSound;
            fribble.DeathSound = LoadedAssetsHandler.GetEnemy("SkinningHomunculus_EN").deathSound;
            fribble.DialogueSound = LoadedAssetsHandler.GetEnemy("SkinningHomunculus_EN").damageSound;
            //fribble.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            //fribble.AddFinalBossAchievementData("Heaven", HeavenACH);
            //fribble.GenerateMenuCharacter(ResourceLoader.LoadSprite("FribbleMenu.png"), ResourceLoader.LoadSprite("FribbleLock.png"));
            //fribble.MenuCharacterIsSecret = false;
            //fribble.MenuCharacterIgnoreRandom = false;
            //fribble.SetMenuCharacterAsFullDPS();

            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool("Fribble_Earworm_A", "Earworm +{0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Negative));
            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool("Fribble_Noise_A", "Noise +{0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Negative));

            CasterStoredValueChangeEffect noise_up = BasicEffects.ChangeValue("Fribble_Noise_A", true);
            CasterStoredValueChangeEffect earworm_up = BasicEffects.ChangeValue("Fribble_Earworm_A", true);
            DamageByStoredValueEffect noise_hit = ScriptableObject.CreateInstance<DamageByStoredValueEffect>();
            noise_hit.m_unitStoredDataID = "Fribble_Noise_A";
            noise_hit._increaseDamage = true;
            DamageByStoredValueEffect earworm_hit = ScriptableObject.CreateInstance<DamageByStoredValueEffect>();
            earworm_hit.m_unitStoredDataID = "Fribble_Earworm_A";
            earworm_hit._increaseDamage = true;
            DamageEffect damage = ScriptableObject.CreateInstance<DamageEffect>();

            Ability noise1 = new Ability("Background Noise", "Fribble_Noise_1_A");
            noise1.Description = "Deal 4 damage to the Left, Right, and Opposing enemies.\nTake 0 damage, then increase the self damage of this move by 1.";
            noise1.AbilitySprite = ResourceLoader.LoadSprite("ability_noise.png");
            noise1.Cost = [Pigments.Purple];
            noise1.Effects = new EffectInfo[3];
            noise1.Effects[0] = Effects.GenerateEffect(damage, 4, Slots.FrontLeftRight);
            noise1.Effects[1] = Effects.GenerateEffect(noise_hit, 0, Slots.Self);
            noise1.Effects[2] = Effects.GenerateEffect(noise_up, 1, Slots.Self);
            noise1.AddIntentsToTarget(Slots.FrontLeftRight, ["Damage_3_6"]);
            noise1.AddIntentsToTarget(Slots.Self, ["Damage_1_2"]);
            noise1.UnitStoreData = UnitStoreData.GetCustom_UnitStoreData("Fribble_Noise_A");
            noise1.Visuals = CustomVisuals.GetVisuals("Salt/Class");
            noise1.AnimationTarget = Slots.FrontLeftRight;

            Ability noise2 = new Ability(noise1.ability, "Fribble_Noise_2_A", noise1.Cost);
            noise2.Name = "Violent Noise";
            noise2.Description = "Deal 5 damage to the Left, Right, and Opposing enemies.\nTake 0 damage, then increase the self damage of this move by 1.";
            noise2.Effects[0].entryVariable = 5;

            Ability noise3 = new Ability(noise2.ability, "Fribble_Noise_3_A", noise1.Cost);
            noise3.Name = "Surrounding Noise";
            noise3.Description = "Deal 8 damage to the Left, Right, and Opposing enemies.\nTake 0 damage, then increase the self damage of this move by 1.";
            noise3.Effects[0].entryVariable = 8;
            noise3.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability noise4 = new Ability(noise3.ability, "Fribble_Noise_4_A", noise1.Cost);
            noise4.Name = "Radiant Noise";
            noise4.Description = "Deal 10 damage to the Left, Right, and Opposing enemies.\nTake 0 damage, then increase the self damage of this move by 1.";
            noise4.Effects[0].entryVariable = 10;

            Ability flavor1 = new Ability("Strawberry Flavor", "Fribble_Flavor_1_A");
            flavor1.Description = "Gain 2 Shield.\nIf the correct Pigment was used, gain 2 Anesthetics.";
            flavor1.AbilitySprite = ResourceLoader.LoadSprite("ability_flavor.png");
            flavor1.Cost = [Pigments.Blue, Pigments.Blue];
            flavor1.Effects = new EffectInfo[2];
            flavor1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyShieldSlotEffect>(), 2, Slots.Self);
            flavor1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyAnestheticsEffect>(), 2, Slots.Self, WrongPigmentEffectCondition.Create(false));
            flavor1.AddIntentsToTarget(Slots.Self, ["Field_Shield", Anesthetics.Intent]);
            flavor1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Weave_1_A").visuals;
            flavor1.AnimationTarget = Slots.Self;

            Ability flavor2 = new Ability(flavor1.ability, "Fribble_Flavor_2_A", flavor1.Cost);
            flavor2.Name = "Blueberry Flavor";
            flavor2.Description = "Gain 4 Shield.\nIf the correct Pigment was used, gain 2 Anesthetics.";
            flavor2.Effects[0].entryVariable = 4;

            Ability flavor3 = new Ability(flavor2.ability, "Fribble_Flavor_3_A", flavor1.Cost);
            flavor3.Name = "Raspberry Flavor";
            flavor3.Description = "Gain 6 Shield.\nIf the correct Pigment was used, gain 2 Anesthetics.";
            flavor3.Effects[0].entryVariable = 6;

            Ability flavor4 = new Ability(flavor3.ability, "Fribble_Flavor_4_A", flavor1.Cost);
            flavor4.Name = "Cherry Flavor";
            flavor4.Description = "Gain 8 Shield.\nIf the correct Pigment was used, gain 2 Anesthetics.";
            flavor4.Effects[0].entryVariable = 8;

            Ability earworm1 = new Ability("Earworm Mean", "Fribble_Earworm_1_A");
            earworm1.Description = "Deal 10 damage to the Opposing enemy.\nTake 0 damage, then increase the self damage of this move by 1.";
            earworm1.AbilitySprite = ResourceLoader.LoadSprite("ability_earworm.png");
            earworm1.Cost = [Pigments.Red, Pigments.Red];
            earworm1.Effects = new EffectInfo[3];
            earworm1.Effects[0] = Effects.GenerateEffect(damage, 10, Slots.Front);
            earworm1.Effects[1] = Effects.GenerateEffect(earworm_hit, 0, Slots.Self);
            earworm1.Effects[2] = Effects.GenerateEffect(earworm_up, 1, Slots.Self);
            earworm1.AddIntentsToTarget(Slots.Front, ["Damage_7_10"]);
            earworm1.AddIntentsToTarget(Slots.Self, ["Damage_1_2"]);
            earworm1.UnitStoreData = UnitStoreData.GetCustom_UnitStoreData("Fribble_Earworm_A");
            earworm1.Visuals = Visuals.Poke;
            earworm1.AnimationTarget = Slots.Front;

            Ability earworm2 = new Ability(earworm1.ability, "Fribble_Earworm_2_A", earworm1.Cost);
            earworm2.Name = "Earworm Bully";
            earworm2.Description = "Deal 12 damage to the Opposing enemy.\nTake 0 damage, then increase the self damage of this move by 1.";
            earworm2.Effects[0].entryVariable = 12;
            earworm2.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability earworm3 = new Ability(earworm2.ability, "Fribble_Earworm_3_A", earworm1.Cost);
            earworm3.Name = "Earworm Cruel";
            earworm3.Description = "Deal 14 damage to the Opposing enemy.\nTake 0 damage, then increase the self damage of this move by 1.";
            earworm3.Effects[0].entryVariable = 14;

            Ability earworm4 = new Ability(earworm3.ability, "Fribble_Earworm_4_A", earworm1.Cost);
            earworm4.Name = "Earworm Sadistic";
            earworm4.Description = "Deal 16 damage to the Opposing enemy.\nTake 0 damage, then increase the self damage of this move by 1.";
            earworm4.Effects[0].entryVariable = 16;
            earworm4.EffectIntents[0].intents[0] = "Damage_16_20";

            fribble.AddLevelData(10, [noise1, flavor1, earworm1]);
            fribble.AddLevelData(13, [noise2, flavor2, earworm2]);
            fribble.AddLevelData(16, [noise3, flavor3, earworm3]);
            fribble.AddLevelData(18, [noise4, flavor4, earworm4]);
            fribble.AddCharacter(false, true);
        }
    }
}
