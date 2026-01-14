using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Rotcore
    {
        public static void Add()
        {
            Character rotcore = new Character("Rotcore_I", "Rotcore_CH");
            rotcore.HealthColor = Pigments.Yellow;
            rotcore.AddUnitType("FemaleID");
            rotcore.AddUnitType("Sandwich_War");
            rotcore.UsesBasicAbility = true;
            //slap
            rotcore.UsesAllAbilities = false;
            rotcore.MovesOnOverworld = true;
            rotcore.Animator = Joyce.Assets.LoadAsset<RuntimeAnimatorController>("Assets/AnimationBaseData/NewBigGuy/MedAnimController.overrideController");
            rotcore.FrontSprite = ResourceLoader.LoadSprite("RotcoreFront0.png");
            rotcore.BackSprite = ResourceLoader.LoadSprite("RotcoreBack0.png");
            rotcore.OverworldSprite = ResourceLoader.LoadSprite("RotcoreWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            rotcore.DamageSound = "event:/Lunacy/SOUNDS3/HauntlingHit";
            rotcore.DeathSound = "event:/Lunacy/SOUNDS3/HauntlingDie";
            rotcore.DialogueSound = "event:/Lunacy/SOUNDS3/DamoclesHit";
            rotcore.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            rotcore.AddFinalBossAchievementData("Heaven", HeavenACH);
            rotcore.GenerateMenuCharacter(ResourceLoader.LoadSprite("RotcoreMenu.png"), ResourceLoader.LoadSprite("RotcoreLock.png"));
            rotcore.MenuCharacterIsSecret = false;
            rotcore.MenuCharacterIgnoreRandom = false;
            rotcore.SetMenuCharacterAsFullDPS();

            ApplyDivineProtectionEffect divine = ScriptableObject.CreateInstance<ApplyDivineProtectionEffect>();
            RemoveStatusEffectEffect remove = ScriptableObject.CreateInstance<RemoveStatusEffectEffect>();
            remove._status = StatusField.DivineProtection;
            DamageEffect damage = ScriptableObject.CreateInstance<DamageEffect>();

            Ability karma1 = new Ability("Readjustment of Faith", "Rot_Karma_1_A");
            karma1.Description = "Apply 1 Divine Protection to all enemies. Remove Divine Protection from the Opposing enemy.\nDeal 3 damage to all enemies.";
            karma1.AbilitySprite = ResourceLoader.LoadSprite("ability_karma.png");
            karma1.Cost = [Pigments.Red, Pigments.Red, Pigments.Red, Pigments.Yellow];
            karma1.Effects = new EffectInfo[3];
            karma1.Effects[0] = Effects.GenerateEffect(divine, 1, Targeting.Unit_AllOpponents);
            karma1.Effects[1] = Effects.GenerateEffect(remove, 1, Slots.Front);
            karma1.Effects[2] = Effects.GenerateEffect(damage, 3, Targeting.Unit_AllOpponents);
            karma1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Status_DivineProtection"]);
            karma1.AddIntentsToTarget(Slots.Front, ["Rem_Status_DivineProtection"]);
            karma1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Damage_3_6"]);
            karma1.AnimationTarget = Slots.Front;
            karma1.Visuals = Visuals.Excommunicate;

            Ability karma2 = new Ability(karma1.ability, "Rot_Karma_2_A", karma1.Cost);
            karma2.Name = "Readjustment of Karma";
            karma2.Description = "Apply 1 Divine protection to all enemies. Remove Divine Protection from the Opposing enemy.\nDeal 4 damage to all enemies.";
            karma2.Effects[2].entryVariable = 4;

            Ability karma3 = new Ability(karma2.ability, "Rot_Karma_3_A", karma1.Cost);
            karma3.Name = "Readjustment of Suffering";
            karma3.Description = "Apply 1 Divine protection to all enemies. Remove Divine Protection from the Opposing enemy.\nDeal 5 damage to all enemies.";
            karma3.Effects[2].entryVariable = 5;

            Ability karma4 = new Ability(karma3.ability, "Rot_Karma_4_A", karma1.Cost);
            karma4.Name = "Readjustment of Salvation";
            karma4.Description = "Apply 1 Divine protection to all enemies. Remove Divine Protection from the Opposing enemy.\nDeal 6 damage to all enemies.";
            karma4.Effects[2].entryVariable = 6;

            Ability goat1 = new Ability("Another Scapegoat", "Rot_Goat_1_A");
            goat1.Description = "Apply 1 Divine Protection to the Opposing enemy and deal 10 damage to them.";
            goat1.AbilitySprite = ResourceLoader.LoadSprite("ability_goat.png");
            goat1.Cost = [Pigments.Yellow, Pigments.Yellow, Pigments.Red];
            goat1.Effects = new EffectInfo[2];
            goat1.Effects[0] = Effects.GenerateEffect(divine, 1, Slots.Front);
            goat1.Effects[1] = Effects.GenerateEffect(damage, 10, Slots.Front);
            goat1.AddIntentsToTarget(Slots.Front, ["Status_DivineProtection", "Damage_7_10"]);
            goat1.AnimationTarget = Slots.Front;
            goat1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Conversion_1_A").visuals;

            Ability goat2 = new Ability(goat1.ability, "Rot_Goat_2_A", goat1.Cost);
            goat2.Name = "Shameless Scapegoat";
            goat2.Description = "Apply 1 Divine Protection to the Opposing enemy and deal 14 damage to them.";
            goat2.Effects[1].entryVariable = 14;
            goat2.EffectIntents[0].intents[1] = "Damage_11_15";

            Ability goat3 = new Ability(goat2.ability, "Rot_Goat_3_A", goat1.Cost);
            goat3.Name = "Criminal Scapegoat";
            goat3.Description = "Apply 1 Divine Protection to the Opposing enemy and deal 18 damage to them.";
            goat3.Effects[1].entryVariable = 18;
            goat3.EffectIntents[0].intents[1] = "Damage_16_20";

            Ability goat4 = new Ability(goat3.ability, "Rot_Goat_4_A", goat1.Cost);
            goat4.Name = "Remorseless Scapegoat";
            goat4.Description = "Apply 1 Divine Protection to the Opposing enemy and deal 21 damage to them.";
            goat4.Effects[1].entryVariable = 21;
            goat4.EffectIntents[0].intents[1] = "Damage_21";

            Ability suffer1 = new Ability("Suffer Now, Pay Later", "Rot_Suffer_1_A");
            suffer1.Description = "Deal 6 damage to the Opposing enemy and apply 3 Divine Protection to them.";
            suffer1.AbilitySprite = ResourceLoader.LoadSprite("ability_suffer.png");
            suffer1.Cost = [Pigments.Red, Pigments.Yellow];
            suffer1.Effects = new EffectInfo[2];
            suffer1.Effects[0] = Effects.GenerateEffect(damage, 6, Slots.Front);
            suffer1.Effects[1] = Effects.GenerateEffect(divine, 3, Slots.Front);
            suffer1.AddIntentsToTarget(Slots.Front, ["Damage_3_6", "Status_DivineProtection"]);
            suffer1.AnimationTarget = Slots.Front;
            suffer1.Visuals = Visuals.Talons;

            Ability suffer2 = new Ability(suffer1.ability, "Rot_Suffer_2_A", suffer1.Cost);
            suffer2.Name = "Suffer Now, Punished Later";
            suffer2.Description = "Deal 8 damage to the Opposing enemy and apply 3 Divine Protection to them.";
            suffer2.Effects[0].entryVariable = 8;
            suffer2.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability suffer3 = new Ability(suffer2.ability, "Rot_Suffer_3_A", suffer1.Cost);
            suffer3.Name = "Suffer Now, Repent Later";
            suffer3.Description = "Deal 10 damage to the Opposing enemy and apply 3 Divine Protection to them.";
            suffer3.Effects[0].entryVariable = 10;

            Ability suffer4 = new Ability(suffer3.ability, "Rot_Suffer_4_A", suffer1.Cost);
            suffer4.Name = "Suffer Now, Die Later";
            suffer4.Description = "Deal 12 damage to the Opposing enemy and apply 3 Divine Protection to them.";
            suffer4.Effects[0].entryVariable = 12;
            suffer4.EffectIntents[0].intents[0] = "Damage_11_15";

            rotcore.AddLevelData(12, [suffer1, goat1, karma1]);
            rotcore.AddLevelData(14, [suffer2, goat2, karma2]);
            rotcore.AddLevelData(18, [suffer3, goat3, karma3]);
            rotcore.AddLevelData(22, [suffer4, goat4, karma4]);
            rotcore.AddCharacter(true);
        }
        public static void Items()
        {
            ApplyStatusIfPassivesEffect dp = ScriptableObject.CreateInstance<ApplyStatusIfPassivesEffect>();
            dp._Status = StatusField.DivineProtection;
            dp.Passives = [Passives.Withering.m_PassiveID, Passives.Fleeting1.m_PassiveID, "Salt_Flithering_PA"];

            PerformEffect_Item radio = new PerformEffect_Item("Aprils_FrequencyIsolator_SW", [Effects.GenerateEffect(dp, 3, Targeting.Unit_AllOpponents)]);
            radio.Name = "Frequency Isolator";
            radio.Flavour = "\"Somewhere in the universe\"";
            radio.Description = "At the start of combat, apply 3 Divine Protection to all enemies with \"Withering,\" \"Flithering,\" and \"Fleeting\" as passives.";
            radio.Icon = ResourceLoader.LoadSprite("item_frequencyisolator.png");
            radio.EquippedModifiers = [];
            radio.TriggerOn = TriggerCalls.OnCombatStart;
            radio.DoesPopUpInfo = true;
            radio.Conditions = [];
            radio.DoesActionOnTriggerAttached = false;
            radio.ConsumeOnTrigger = TriggerCalls.Count;
            radio.ConsumeOnUse = false;
            radio.ConsumeConditions = [];
            radio.ShopPrice = 6;
            radio.IsShopItem = true;
            radio.StartsLocked = true;
            radio.OnUnlockUsesTHE = true;
            radio.UsesSpecialUnlockText = false;
            radio.SpecialUnlockID = UILocID.None;
            radio.item._ItemTypeIDs = [];
            radio.item.AddItem("locked_frequencyisolator.png", HeavenACH);

            PercentageEffectorCondition p80 = ScriptableObject.CreateInstance<PercentageEffectorCondition>();
            p80.triggerPercentage = 80;

            PerformEffect_Item springlocks = new PerformEffect_Item("Aprils_Springlocks_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 2, Slots.Self), Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 7, Slots.Front)]);
            springlocks.Name = "Springlocks";
            springlocks.Flavour = "\"I Always Come Back\"";
            springlocks.Description = "On taking any damage, 80% chance to take 2 damage and deal 7 damage to the Opposing enemy.";
            springlocks.Icon = ResourceLoader.LoadSprite("item_springlocks.png");
            springlocks.EquippedModifiers = [];
            springlocks.TriggerOn = TriggerCalls.OnDamaged;
            springlocks.DoesPopUpInfo = true;
            springlocks.Conditions = [p80];
            springlocks.DoesActionOnTriggerAttached = false;
            springlocks.ConsumeOnTrigger = TriggerCalls.Count;
            springlocks.ConsumeOnUse = false;
            springlocks.ConsumeConditions = [];
            springlocks.ShopPrice = 3;
            springlocks.IsShopItem = true;
            springlocks.StartsLocked = true;
            springlocks.OnUnlockUsesTHE = true;
            springlocks.UsesSpecialUnlockText = false;
            springlocks.SpecialUnlockID = UILocID.None;
            springlocks.item._ItemTypeIDs = [];
            springlocks.item.AddItem("locked_springlocks.png", OsmanACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Rotcore", "Frequency Isolator", "Springlocks", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Rotcore_CH", "Aprils_FrequencyIsolator_SW", "Aprils_Springlocks_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Rotcore_Heaven_ACH";
        public static string OsmanACH => "Aprils_Rotcore_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Rotcore_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Rotcore_Osman_Unlock";
    }
}
