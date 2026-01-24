using BrutalAPI;
using BrutalAPI.Items;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Bodybag
    {
        public static void Add()
        {
            ExtraCCSprites_ArraySO bodybagExtra = ScriptableObject.CreateInstance<ExtraCCSprites_ArraySO>();
            bodybagExtra._DefaultID = "";
            bodybagExtra._frontSprite = [ResourceLoader.LoadSprite("BodybagFront1.png"), ResourceLoader.LoadSprite("BodybagFront1.png")];
            bodybagExtra._SpecialID = IDs.Bodybag;
            bodybagExtra._backSprite = [ResourceLoader.LoadSprite("BodybagBack.png"), ResourceLoader.LoadSprite("BodybagBack.png")];
            bodybagExtra._doesLoop = false;
            SetCasterExtraSpritesRandomUpToEntryEffect bodybagSprites = ScriptableObject.CreateInstance<SetCasterExtraSpritesRandomUpToEntryEffect>();
            bodybagSprites._spriteType = bodybagExtra._SpecialID;

            PerformEffectPassiveAbility backlash = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            backlash._passiveName = "Backlash";
            backlash.m_PassiveID = "Backlash_PA";
            backlash.passiveIcon = ResourceLoader.LoadSprite("BacklashPassive.png");
            backlash._enemyDescription = "On taking direct damage, apply Shield to this enemy's positions for the amount of damage taken.";
            backlash._characterDescription = "On taking direct damage, apply Shield to this character's position for the amount of damage taken.";
            backlash.doesPassiveTriggerInformationPanel = false;
            backlash.conditions = [ScriptableObject.CreateInstance<BacklashCondition>()];
            backlash._triggerOn = [TriggerCalls.OnDirectDamaged];
            backlash.effects = [];
            backlash.AddPassiveToGlossary("Backlash", "On taking direct damage, apply Shield to this unit's position for the amount of damage taken.");
            backlash.AddToPassiveDatabase();

            Character bodybag = new Character("Bodybag", "Bodybag_CH");
            bodybag.HealthColor = Pigments.Purple;
            bodybag.AddUnitType("FemaleID");
            bodybag.AddUnitType("Sandwich_BDSM");
            bodybag.AddUnitType("FemaleLooking");
            bodybag.UsesBasicAbility = true;
            //slap
            bodybag.UsesAllAbilities = false;
            bodybag.MovesOnOverworld = true;
            //animator
            bodybag.FrontSprite = ResourceLoader.LoadSprite("BodybagFront0.png");
            bodybag.BackSprite = ResourceLoader.LoadSprite("BodybagBack.png");
            bodybag.OverworldSprite = ResourceLoader.LoadSprite("BodybagWorld.png", new Vector2(0.5f, 0f));
            bodybag.ExtraSprites = bodybagExtra;
            bodybag.DamageSound = LoadedAssetsHandler.GetEnemy("TaintedYolk_EN").damageSound;
            bodybag.DeathSound = LoadedAssetsHandler.GetEnemy("TaintedYolk_EN").deathSound;
            bodybag.DialogueSound = LoadedAssetsHandler.GetEnemy("TaintedYolk_EN").damageSound;
            bodybag.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            bodybag.AddFinalBossAchievementData("Heaven", HeavenACH);
            bodybag.GenerateMenuCharacter(ResourceLoader.LoadSprite("BodybagMenu.png"), ResourceLoader.LoadSprite("BodybagLock.png"));
            bodybag.MenuCharacterIsSecret = false;
            bodybag.MenuCharacterIgnoreRandom = false;
            bodybag.SetMenuCharacterAsFullDPS();
            bodybag.AddPassive(backlash);


            DamageEffect damage = ScriptableObject.CreateInstance<DamageEffect>();
            Ability stand1 = new Ability("Last Stand", "Bodybag_Stand_1_A");
            stand1.Description = "Deal 5 damage to the Left and Right enemies. Increase this damage by 2 while in Shield.\nGain 5 Shield.";
            stand1.AbilitySprite = ResourceLoader.LoadSprite("ability_stand.png");
            stand1.Cost = [Pigments.Blue, Pigments.Yellow, Pigments.Red];
            stand1.Effects = new EffectInfo[3];
            stand1.Effects[0] = Effects.GenerateEffect(BasicEffects.Empty, 2);
            stand1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageAddExitIfShieldEffect>(), 5, Slots.LeftRight);
            stand1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyShieldSlotEffect>(), 5, Slots.Self);
            stand1.AddIntentsToTarget(Slots.LeftRight, ["Damage_7_10"]);
            stand1.AddIntentsToTarget(Slots.Self, ["Field_Shield"]);
            stand1.Visuals = CustomVisuals.GetVisuals("Salt/Coda");//change the anim maybe
            stand1.AnimationTarget = Slots.LeftRight;

            Ability stand2 = new Ability(stand1.ability, "Bodybag_Stand_2_A", stand1.Cost);
            stand2.Name = "Last Stage";
            stand2.Description = "Deal 7 damage to the Left and Right enemies. Increase this damage by 2 while in Shield.\nGain 7 Shield.";
            stand2.Effects[1].entryVariable = 7;
            stand2.Effects[2].entryVariable = 7;
            stand2.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability stand3 = new Ability(stand2.ability, "Bodybag_Stand_3_A", stand1.Cost);
            stand3.Name = "Last Defense";
            stand3.Description = "Deal 8 damage to the Left and Right enemies. Increase this damage by 3 while in Shield.\nGain 8 Shield.";
            stand3.Effects[0].entryVariable = 3;
            stand3.Effects[1].entryVariable = 8;
            stand3.Effects[2].entryVariable = 8;
            stand3.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability stand4 = new Ability(stand3.ability, "Bodybag_Stand_4_A", stand1.Cost);
            stand4.Name = "Last Chance";
            stand4.Description = "Deal 10 damage to the Left and Right enemies. Increase this damage by 4 while in Shield.\nGain 10 Shield.";
            stand4.Effects[0].entryVariable = 4;
            stand4.Effects[1].entryVariable = 10;
            stand4.Effects[2].entryVariable = 10;
            stand4.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability sticks1 = new Ability("Game of Sticks", "Bodybag_Sticks_1_A");
            sticks1.Description = "Deal 8 damage to a random enemy Opposed by Shield.";
            sticks1.AbilitySprite = ResourceLoader.LoadSprite("ability_sticks.png");
            sticks1.Cost = [Pigments.Red, Pigments.Red];
            sticks1.Effects = new EffectInfo[1];
            sticks1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageTargetRandomEffect>(), 8, TargettingByOpposingFieldEffect.Create(false, "Shield_ID"));
            sticks1.AddIntentsToTarget(TargettingByOpposingFieldEffect.Create(false, "Shield_ID", false), ["Damage_7_10"]);
            sticks1.AddIntentsToTarget(Targetting.Everything(false), ["Misc_Hidden"]);
            sticks1.Visuals = CustomVisuals.GetVisuals("Salt/Needle");
            sticks1.AnimationTarget = Slots.Self;

            Ability sticks2 = new Ability(sticks1.ability, "Bodybag_Sticks_2_A", sticks1.Cost);
            sticks2.Name = "Game of Needles";
            sticks2.Description = "Deal 11 damage to a random enemy Opposed by Shield.";
            sticks2.Effects[0].entryVariable = 11;
            sticks2.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability sticks3 = new Ability(sticks2.ability, "Bodybag_Sticks_3_A", sticks1.Cost);
            sticks3.Name = "Game of Knives";
            sticks3.Description = "Deal 14 damage to a random enemy Opposed by Shield.";
            sticks3.Effects[0].entryVariable = 14;

            Ability sticks4 = new Ability(sticks3.ability, "Bodybag_Sticks_4_A", sticks1.Cost);
            sticks4.Name = "Game of Blades";
            sticks4.Description = "Deal 17 damage to a random enemy Opposed by Shield.";
            sticks4.Effects[0].entryVariable = 17;
            sticks4.EffectIntents[0].intents[0] = "Damage_16_20";

            ApplyShieldSlotEffect shieldExit = ScriptableObject.CreateInstance<ApplyShieldSlotEffect>();
            shieldExit._UsePreviousExitValueAsMultiplier = true;

            Ability auto1 = new Ability("Auto-Trauma", "Bodybag_Auto_1_A");
            auto1.Description = "Deal 4 damage to the Opposing enemy.\nApply Shield to this party member's position and their Left and Right equal to the damage dealt.";
            auto1.AbilitySprite = ResourceLoader.LoadSprite("ability_auto.png");
            auto1.Cost = [Pigments.Red, Pigments.Blue];
            auto1.Effects = new EffectInfo[2];
            auto1.Effects[0] = Effects.GenerateEffect(damage, 4, Slots.Front);
            auto1.Effects[1] = Effects.GenerateEffect(shieldExit, 1, Targeting.Slot_SelfAll_AndSides);
            auto1.AddIntentsToTarget(Slots.Front, ["Damage_3_6"]);
            auto1.AddIntentsToTarget(Targeting.Slot_SelfAll_AndSides, ["Field_Shield"]);
            auto1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Entrenched_1_A").visuals;
            auto1.AnimationTarget = Slots.Front;

            Ability auto2 = new Ability(auto1.ability, "Bodybag_Auto_2_A", auto1.Cost);
            auto2.Name = "Auto-Tomy";
            auto2.Description = "Deal 6 damage to the Opposing enemy.\nApply Shield to this party member's position and their Left and Right equal to the damage dealt.";
            auto2.Effects[0].entryVariable = 6;

            Ability auto3 = new Ability(auto2.ability, "Bodybag_Auto_3_A", auto1.Cost);
            auto3.Name = "Auto-Phagy";
            auto3.Description = "Deal 7 damage to the Opposing enemy.\nApply Shield to this party member's position and their Left and Right equal to the damage dealt.";
            auto3.Effects[0].entryVariable = 7;
            auto3.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability auto4 = new Ability(auto3.ability, "Bodybag_Auto_4_A", auto1.Cost);
            auto4.Name = "Auto-Disembowling";
            auto4.Description = "Deal 8 damage to the Opposing enemy.\nApply Shield to this party member's position and their Left and Right equal to the damage dealt.";
            auto4.Effects[0].entryVariable = 8;

            bodybag.AddLevelData(15, [auto1, sticks1, stand1]);
            bodybag.AddLevelData(20, [auto2, sticks2, stand2]);
            bodybag.AddLevelData(23, [auto3, sticks3, stand3]);
            bodybag.AddLevelData(25, [auto4, sticks4, stand4]);
            bodybag.AddCharacter(true);
        }
        public static void Items()
        {
            PerformEffect_Item restrainingorder = new PerformEffect_Item("Aprils_RestrainingOrder_SW", []);
            restrainingorder.Name = "Restraining Order";
            restrainingorder.Flavour = "\"I feel like the only guy in town\"";
            restrainingorder.Description = "Increase damage dealt to targets by 15% for each side of the target not touching another unit.";
            restrainingorder.Icon = ResourceLoader.LoadSprite("item_restrainingorder.png");
            restrainingorder.EquippedModifiers = [];
            restrainingorder.TriggerOn = TriggerCalls.OnWillApplyDamage;
            restrainingorder.DoesPopUpInfo = false;
            restrainingorder.Conditions = [ScriptableObject.CreateInstance<RestrainingOrderCondition>()];
            restrainingorder.DoesActionOnTriggerAttached = false;
            restrainingorder.ConsumeOnTrigger = TriggerCalls.Count;
            restrainingorder.ConsumeOnUse = false;
            restrainingorder.ConsumeConditions = [];
            restrainingorder.ShopPrice = 10;
            restrainingorder.IsShopItem = true;
            restrainingorder.StartsLocked = true;
            restrainingorder.OnUnlockUsesTHE = true;
            restrainingorder.UsesSpecialUnlockText = false;
            restrainingorder.SpecialUnlockID = UILocID.None;
            restrainingorder.Item.AddItem("locked_restrainingorder.png", OsmanACH);

            CopyAndSpawnCustomCharacterAnywhereEffect spawnDog = ScriptableObject.CreateInstance<CopyAndSpawnCustomCharacterAnywhereEffect>();
            spawnDog._extraModifiers = [];
            spawnDog._characterCopy = "RabidDog_CH";
            PerformEffect_Item rabiddog = new PerformEffect_Item("Aprils_RabidDog_SW", [Effects.GenerateEffect(spawnDog, 1)]);
            rabiddog.Name = "Rabid Dog";
            rabiddog.Flavour = "\"BLOOD!\"";
            rabiddog.Description = "On combat start, spawn the Rabid Dog.";
            rabiddog.Icon = ResourceLoader.LoadSprite("item_rabiddog.png");
            rabiddog.EquippedModifiers = [];
            rabiddog.TriggerOn = TriggerCalls.OnCombatStart;
            rabiddog.DoesPopUpInfo = true;
            rabiddog.Conditions = [];
            rabiddog.DoesActionOnTriggerAttached = false;
            rabiddog.ConsumeOnTrigger = TriggerCalls.Count;
            rabiddog.ConsumeOnUse = false;
            rabiddog.ConsumeConditions = [];
            rabiddog.ShopPrice = 7;
            rabiddog.IsShopItem = true;
            rabiddog.StartsLocked = true;
            rabiddog.OnUnlockUsesTHE = true;
            rabiddog.UsesSpecialUnlockText = false;
            rabiddog.SpecialUnlockID = UILocID.None;
            rabiddog.Item.AddItem("locked_rabiddog.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Bodybag", "Rabid Dog", "Restraining Order", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Bodybag_CH", "Aprils_RabidDog_SW", "Aprils_RestrainingOrder_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Bodybag_Heaven_ACH";
        public static string OsmanACH => "Aprils_Bodybag_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Bodybag_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Bodybag_Osman_Unlock";
    }
}
