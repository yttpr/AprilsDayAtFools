using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Clerk
    {
        public static void Add()
        {
            Character clerk = new Character("Clerk", "Clerk_CH");
            clerk.HealthColor = Pigments.Purple;
            clerk.AddUnitType("FemaleID");
            clerk.AddUnitType("Sandwich_Gambling");
            clerk.AddUnitType("FemaleLooking");
            clerk.UsesBasicAbility = true;
            //slap
            clerk.UsesAllAbilities = false;
            clerk.MovesOnOverworld = true;
            //animator
            clerk.FrontSprite = ResourceLoader.LoadSprite("ClerkFront.png");
            clerk.BackSprite = ResourceLoader.LoadSprite("ClerkBack.png");
            clerk.OverworldSprite = ResourceLoader.LoadSprite("ClerkWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            clerk.DamageSound = "event:/Lunacy/SOUNDS/ClockHit";
            clerk.DeathSound = "event:/Lunacy/SOUNDS/ClockDie";
            clerk.DialogueSound = "event:/Lunacy/SOUNDS/ClockHit";
            clerk.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            clerk.AddFinalBossAchievementData("Heaven", HeavenACH);
            clerk.GenerateMenuCharacter(ResourceLoader.LoadSprite("ClerkMenu.png"), ResourceLoader.LoadSprite("ClerkLock.png"));
            clerk.MenuCharacterIsSecret = false;
            clerk.MenuCharacterIgnoreRandom = false;
            clerk.SetMenuCharacterAsFullDPS();

            Ability solo1 = new Ability("Opening Reading", "Clerk_Solo_1_A");
            solo1.Description = "Deal 9 damage to the Opposing enemy and gain 1 Determined.\nForce the Opposing enemy to prematurely perform their next action.";
            solo1.AbilitySprite = ResourceLoader.LoadSprite("ability_soliloquy.png");
            solo1.Cost = [Pigments.Red, Pigments.Red, Pigments.Blue];
            solo1.Effects = new EffectInfo[3];
            solo1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 9, Slots.Front);
            solo1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyDeterminedEffect>(), 1, Slots.Self);
            solo1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<TargetForceFirstActionEffect>(), 1, Slots.Front);
            solo1.AddIntentsToTarget(Slots.Self, [Determined.Intent]);
            solo1.AddIntentsToTarget(Slots.Front, ["Damage_7_10", "Misc"]);
            solo1.Visuals = CustomVisuals.GetVisuals("Salt/Gaze");
            solo1.AnimationTarget = Slots.Front;

            Ability solo2 = new Ability(solo1.ability, "Clerk_Solo_2_A", solo1.Cost);
            solo2.Name = "Opening Session";
            solo2.Description = "Deal 13 damage to the Opposing enemy and gain 1 Determined.\nForce the Opposing enemy to prematurely perform their next action.";
            solo2.Effects[0].entryVariable = 13;
            solo2.EffectIntents[1].intents[0] = "Damage_11_15";

            Ability solo3 = new Ability(solo2.ability, "Clerk_Solo_3_A", solo1.Cost);
            solo3.Name = "Opening Act";
            solo3.Description = "Deal 17 damage to the Opposing enemy and gain 1 Determined.\nForce the Opposing enemy to prematurely perform their next action.";
            solo3.Effects[0].entryVariable = 17;
            solo3.EffectIntents[1].intents[0] = "Damage_16_20";

            Ability solo4 = new Ability(solo3.ability, "Clerk_Solo_4_A", solo1.Cost);
            solo4.Name = "Opening Soliloquy";
            solo4.Description = "Deal 20 damage to the Opposing enemy and gain 1 Determined.\nForce the Opposing enemy to prematurely perform their next action.";
            solo4.Effects[0].entryVariable = 20;

            Ability script1 = new Ability("Redo Script", "Clerk_Script_1_A");
            script1.Description = "Deal 7 damage and apply 2 Divine Protection to the Opposing enemy.\nIf no damage is dealt, Reroll the whole timeline.";
            script1.AbilitySprite = ResourceLoader.LoadSprite("ability_script.png");
            script1.Cost = [Pigments.Red, Pigments.Red];
            script1.Effects = new EffectInfo[3];
            script1.Effects[0] = Effects.GenerateEffect(solo1.Effects[0].effect, 7, Slots.Front);
            script1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyDivineProtectionEffect>(), 2, Slots.Front);
            script1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RerollTimelineEffect>(), 1, Slots.Self, BasicEffects.DidThat(false, 2));
            script1.AddIntentsToTarget(Slots.Front, ["Damage_7_10", "Status_DivineProtection"]);
            script1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Misc"]);
            script1.Visuals = CustomVisuals.GetVisuals("Salt/Ribbon");
            script1.AnimationTarget = Slots.Front;

            Ability script2 = new Ability(script1.ability, "Clerk_Script_2_A", script1.Cost);
            script2.Name = "Rewrite Script";
            script2.Description = "Deal 9 damage and apply 2 Divine Protection to the Opposing enemy.\nIf no damage is dealt, Reroll the whole timeline.";
            script2.Effects[0].entryVariable = 9;

            Ability script3 = new Ability(script2.ability, "Clerk_Script_3_A", script1.Cost);
            script3.Name = "Refocus Script";
            script3.Description = "Deal 11 damage and apply 2 Divine Protection to the Opposing enemy.\nIf no damage is dealt, Reroll the whole timeline.";
            script3.Effects[0].entryVariable = 11;
            script3.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability script4 = new Ability(script3.ability, "Clerk_Script_4_A", script1.Cost);
            script4.Name = "Retcon Script";
            script4.Description = "Deal 13 damage and apply 2 Divine Protection to the Opposing enemy.\nIf no damage is dealt, Reroll the whole timeline.";
            script4.Effects[0].entryVariable = 13;

            Ability dance1 = new Ability("Initial Dance", "Clerk_Dance_1_A");
            dance1.Description = "Deal 5 damage to the Opposing enemy.\nForce them to prematurely perform their next action, then give them another one.\nRefresh this party member's ability usage.";
            dance1.AbilitySprite = ResourceLoader.LoadSprite("ability_dance.png");
            dance1.Cost = [Pigments.Purple, Pigments.Red];
            dance1.Effects = new EffectInfo[4];
            dance1.Effects[0] = Effects.GenerateEffect(solo1.Effects[0].effect, 5, Slots.Front);
            dance1.Effects[1] = Effects.GenerateEffect(solo1.Effects[2].effect, 1, Slots.Front);
            dance1.Effects[2] = Effects.GenerateEffect(RootActionEffect.Create(Effects.GenerateEffect(ScriptableObject.CreateInstance<AddTurnCasterToTimelineEffect>(), 1).SelfArray()), 1, Slots.Front);
            dance1.Effects[3] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), 1, Slots.Self);
            dance1.AddIntentsToTarget(Slots.Front, ["Damage_3_6", "Misc", "Misc_Additional"]);
            dance1.AddIntentsToTarget(Slots.Self, ["Misc_Additional"]);
            dance1.Visuals = LoadedAssetsHandler.GetEnemyAbility("Fandango_A").visuals;
            dance1.AnimationTarget = Slots.Front;

            Ability dance2 = new Ability(dance1.ability, "Clerk_Dance_2_A", dance1.Cost);
            dance2.Name = "Intermediary Dance";
            dance2.Description = "Deal 6 damage to the Opposing enemy.\nForce them to prematurely perform their next action, then give them another one.\nRefresh this party member's ability usage.";
            dance2.Effects[0].entryVariable = 6;

            Ability dance3 = new Ability(dance2.ability, "Clerk_Dance_3_A", dance1.Cost);
            dance3.Name = "Penultimate Dance";
            dance3.Description = "Deal 8 damage to the Opposing enemy.\nForce them to prematurely perform their next action, then give them another one.\nRefresh this party member's ability usage.";
            dance3.Effects[0].entryVariable = 8;
            dance3.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability dance4 = new Ability(dance3.ability, "Clerk_Dance_4_A", dance1.Cost);
            dance4.Name = "Final Dance";
            dance4.Description = "Deal 9 damage to the Opposing enemy.\nForce them to prematurely perform their next action, then give them another one.\nRefresh this party member's ability usage.";
            dance4.Effects[0].entryVariable = 9;

            clerk.AddLevelData(17, [dance1, solo1, script1]);
            clerk.AddLevelData(20, [dance2, solo2, script2]);
            clerk.AddLevelData(21, [dance3, solo3, script3]);
            clerk.AddLevelData(22, [dance4, solo4, script4]);
            clerk.AddCharacter(true);
        }
        public static void Items()
        {
            PerformEffect_Item eraser = new PerformEffect_Item("Aprils_Eraser_TW", []);
            eraser.Name = "Eraser";
            eraser.Flavour = "\"Rewrite it\"";
            eraser.Description = "On dealing damage to an enemy, either remove one of their actions, reroll one of their actions, or give them another action on the timeline.";
            eraser.Icon = ResourceLoader.LoadSprite("item_eraser.png");
            eraser.EquippedModifiers = [];
            eraser.TriggerOn = AdvancedDamageTrigger.Dealt;
            eraser.DoesPopUpInfo = false;
            eraser.Conditions = [ScriptableObject.CreateInstance<EraserCondition>()];
            eraser.DoesActionOnTriggerAttached = false;
            eraser.ConsumeOnTrigger = TriggerCalls.Count;
            eraser.ConsumeOnUse = false;
            eraser.ConsumeConditions = [];
            eraser.ShopPrice = 5;
            eraser.IsShopItem = false;
            eraser.StartsLocked = true;
            eraser.OnUnlockUsesTHE = true;
            eraser.UsesSpecialUnlockText = false;
            eraser.SpecialUnlockID = UILocID.None;
            eraser.item._ItemTypeIDs = ["Magic"];
            eraser.Item.AddItem("locked_eraser.png", OsmanACH);

            PerformEffect_Item puppeteer = new PerformEffect_Item("Aprils_Puppeteer_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<PerformRandomAbilityEffect>(), 1, Slots.Self)]);
            puppeteer.Name = "Puppeteer";
            puppeteer.Flavour = "\"I'm in control now.\"";
            puppeteer.Description = "On taking any damage, force this party member to perform a random one of their abilities.";
            puppeteer.Icon = ResourceLoader.LoadSprite("item_puppeteer.png");
            puppeteer.EquippedModifiers = [];
            puppeteer.TriggerOn = TriggerCalls.OnDamaged;
            puppeteer.DoesPopUpInfo = true;
            puppeteer.Conditions = [];
            puppeteer.DoesActionOnTriggerAttached = false;
            puppeteer.ConsumeOnTrigger = TriggerCalls.Count;
            puppeteer.ConsumeOnUse = false;
            puppeteer.ConsumeConditions = [];
            puppeteer.ShopPrice = 8;
            puppeteer.IsShopItem = true;
            puppeteer.StartsLocked = true;
            puppeteer.OnUnlockUsesTHE = true;
            puppeteer.UsesSpecialUnlockText = false;
            puppeteer.SpecialUnlockID = UILocID.None;
            puppeteer.item._ItemTypeIDs = ["Heart"];
            puppeteer.item.AddItem("locked_puppeteer.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Clerk", "Puppeteer", "Eraser", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Clerk_CH", "Aprils_Puppeteer_SW", "Aprils_Eraser_TW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Clerk_Heaven_ACH";
        public static string OsmanACH => "Aprils_Clerk_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Clerk_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Clerk_Osman_Unlock";
    }
}
