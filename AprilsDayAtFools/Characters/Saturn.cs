using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Saturn
    {
        public static void Add()
        {
            ExtraCCSprites_BasicSO saturnExtra = ScriptableObject.CreateInstance<ExtraCCSprites_BasicSO>();
            saturnExtra._DefaultID = IDs.SaturnDefault;
            saturnExtra._frontSprite = ResourceLoader.LoadSprite("SaturnAlt.png");
            saturnExtra._SpecialID = IDs.Saturn;
            saturnExtra._backSprite = ResourceLoader.LoadSprite("SaturnBack.png");

            SetCasterExtraSpritesRandomUpToEntryEffect saturnSprites = ScriptableObject.CreateInstance<SetCasterExtraSpritesRandomUpToEntryEffect>();
            saturnSprites._spriteType = saturnExtra._SpecialID;
            SetCasterExtraSpritesRandomUpToEntryEffect saturnDefault = ScriptableObject.CreateInstance<SetCasterExtraSpritesRandomUpToEntryEffect>();
            saturnDefault._spriteType = saturnExtra._DefaultID;

            PerformEffectPassiveAbility ambush = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            ambush._passiveName = "Ambush (2)";
            ambush.passiveIcon = ResourceLoader.LoadSprite("AmbushPassive.png");
            ambush.m_PassiveID = "Ambush_PA";
            ambush.name = "Ambush_2_PA";
            ambush._enemyDescription = "Whenever an enemy moves in front of this enemy, deal a Little damage to the Opposing party member and move Left, Right, or stay in place.";
            ambush._characterDescription = "Whenever an enemy moves in front of this party member, deal 2 damage to the Opposing enemy and move Left, Right, or stay in place.";
            ambush.doesPassiveTriggerInformationPanel = true;
            DamageByStoredValueEffect ambushDmg = ScriptableObject.CreateInstance<DamageByStoredValueEffect>();
            ambushDmg._increaseDamage = true;
            ambushDmg.m_unitStoredDataID = IDs.Ambush;
            ambush.effects = new EffectInfo[2];
            ambush.effects[0] = Effects.GenerateEffect(ambushDmg, 2, Slots.Front);
            ambush.effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<SwapToSidesEffect>(), 1, Slots.Self, Effects.ChanceCondition(60));
            ambush._triggerOn = [IDs.AmbushCall];
            ambush.specialStoredData = UnitStoreData.GetCustom_UnitStoreData(IDs.Ambush);
            ambush.AddToPassiveDatabase();
            ambush.AddPassiveToGlossary("Ambush", "Whenever an opponent moves in front of this unit, deal a certain amount of damage to the Opposing unit and move Left, Right, or stay in place.");

            Character saturn = new Character("Saturn", "Saturn_CH");
            saturn.HealthColor = Pigments.Grey;
            saturn.AddUnitType("FemaleID");
            saturn.AddUnitType("Sandwich_War");
            saturn.AddUnitType("FemaleLooking");
            saturn.UsesBasicAbility = true;
            //slap
            saturn.UsesAllAbilities = false;
            saturn.MovesOnOverworld = true;
            //animator
            saturn.FrontSprite = ResourceLoader.LoadSprite("SaturnFront.png");
            saturn.BackSprite = ResourceLoader.LoadSprite("SaturnBack.png");
            saturn.OverworldSprite = ResourceLoader.LoadSprite("SaturnWorld.png", new Vector2(0.5f, 0f));
            saturn.ExtraSprites = saturnExtra;
            saturn.DamageSound = "event:/Lunacy/SOUNDS/PhoneHit";
            saturn.DeathSound = "event:/Lunacy/SOUNDS/PhoneDie";
            saturn.DialogueSound = "event:/Lunacy/SOUNDS/PhoneHit";
            saturn.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            saturn.AddFinalBossAchievementData("Heaven", HeavenACH);
            saturn.GenerateMenuCharacter(ResourceLoader.LoadSprite("SaturnMenu.png"), ResourceLoader.LoadSprite("SaturnLock.png"));
            saturn.MenuCharacterIsSecret = false;
            saturn.MenuCharacterIgnoreRandom = false;
            saturn.SetMenuCharacterAsFullDPS();
            saturn.AddPassive(ambush);

            StatusEffectCheckerEffect hasCursed = ScriptableObject.CreateInstance<StatusEffectCheckerEffect>();
            hasCursed._allTargetsHaveStatusEffect = false;
            hasCursed._status = StatusField.Cursed;

            Ability gaze1 = new Ability("Blind Gazer", "Saturn_Gaze_1_A");
            gaze1.Description = "If the Far Far Left and Far Far Right enemies are Cursed, deal 9 damage to them.\nOtherwise, Curse them.";
            gaze1.AbilitySprite = ResourceLoader.LoadSprite("ability_gazer");
            gaze1.Cost = [Pigments.PurpleRed];
            gaze1.Effects = new EffectInfo[3];
            gaze1.Effects[0] = Effects.GenerateEffect(hasCursed, 1, Targeting.GenerateSlotTarget([-3, 3], false));
            gaze1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 9, gaze1.Effects[0].targets, BasicEffects.DidThat(true));
            gaze1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyCursedEffect>(), 1, gaze1.Effects[0].targets, BasicEffects.DidThat(false, 2));
            gaze1.AddIntentsToTarget(gaze1.Effects[0].targets, ["Damage_7_10", "Status_Cursed"]);
            gaze1.Visuals = CustomVisuals.GetVisuals("Salt/Gunshot");
            gaze1.AnimationTarget = gaze1.Effects[0].targets;

            Ability gaze2 = new Ability(gaze1.ability, "Saturn_Gaze_2_A", gaze1.Cost);
            gaze2.Name = "Longshot Gazer";
            gaze2.Description = "If the Far Far Left and Far Far Right enemies are Cursed, deal 11 damage to them.\nOtherwise, Curse them.";
            gaze2.Effects[1].entryVariable = 11;
            gaze2.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability gaze3 = new Ability(gaze2.ability, "Saturn_Gaze_3_A", gaze1.Cost);
            gaze3.Name = "Trispectacled Gazer";
            gaze3.Description = "If the Far Far Left and Far Far Right enemies are Cursed, deal 14 damage to them.\nOtherwise, Curse them.";
            gaze3.Effects[1].entryVariable = 14;
            gaze3.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability gaze4 = new Ability(gaze3.ability, "Saturn_Gaze_4_A", gaze1.Cost);
            gaze4.Name = "Radiostatic Gazer";
            gaze4.Description = "If the Far Far Left and Far Far Right enemies are Cursed, deal 17 damage to them.\nOtherwise, Curse them.";
            gaze4.Effects[1].entryVariable = 17;
            gaze4.EffectIntents[0].intents[0] = "Damage_16_20";


            CasterStoredValueChangeEffect ambushUp = ScriptableObject.CreateInstance<CasterStoredValueChangeEffect>();
            ambushUp._increase = true;
            ambushUp.m_unitStoredDataID = IDs.Ambush;
            RemoveStatusEffectEffect remCursed = ScriptableObject.CreateInstance<RemoveStatusEffectEffect>();
            remCursed._status = StatusField.Cursed;
            Ability number1 = new Ability("Silent Number", "Saturn_Number_1_A");
            number1.Description = "If the Opposing enemy is Cursed, deal 12 damage and remove Cursed from them.\nOtherwise, increase Ambush by 6 until the start of the next turn.";
            number1.AbilitySprite = ResourceLoader.LoadSprite("ability_number.png");
            number1.Cost = [Pigments.Red, Pigments.Red, Pigments.Red];
            number1.Effects = new EffectInfo[4];
            number1.Effects[0] = Effects.GenerateEffect(hasCursed, 1, Slots.Front);
            number1.Effects[1] = Effects.GenerateEffect(gaze1.Effects[1].effect, 12, Slots.Front, BasicEffects.DidThat(true));
            number1.Effects[2] = Effects.GenerateEffect(remCursed, 1, Slots.Front, BasicEffects.DidThat(true, 2));
            number1.Effects[3] = Effects.GenerateEffect(ambushUp, 6, Slots.Self, BasicEffects.DidThat(false, 3));
            number1.AddIntentsToTarget(Slots.Front, ["Damage_11_15", "Rem_Status_Cursed"]);
            number1.AddIntentsToTarget(Slots.Self, ["Misc"]);
            number1.Visuals = CustomVisuals.GetVisuals("Salt/Class");
            number1.AnimationTarget = Slots.Front;

            Ability number2 = new Ability(number1.ability, "Saturn_Number_2_A", number1.Cost);
            number2.Name = "Missense Number";
            number2.Description = "If the Opposing enemy is Cursed, deal 16 damage and remove Cursed from them.\nOtherwise, increase Ambush by 8 until the start of the next turn.";
            number2.Effects[1].entryVariable = 16;
            number2.Effects[3].entryVariable = 8;
            number2.EffectIntents[0].intents[0] = "Damage_16_20";

            Ability number3 = new Ability(number2.ability, "Saturn_Number_3_A", number1.Cost);
            number3.Name = "Nonsense Number";
            number3.Description = "If the Opposing enemy is Cursed, deal 20 damage and remove Cursed from them.\nOtherwise, increase Ambush by 10 until the start of the next turn.";
            number3.Effects[1].entryVariable = 20;
            number3.Effects[3].entryVariable = 10;

            Ability number4 = new Ability(number3.ability, "Saturn_Number_4_A", number1.Cost);
            number4.Name = "Frameshift Number";
            number4.Description = "If the Opposing enemy is Cursed, deal 24 damage and remove Cursed from them.\nOtherwise, increase Ambush by 12 until the start of the next turn.";
            number4.Effects[1].entryVariable = 24;
            number4.Effects[3].entryVariable = 12;
            number4.EffectIntents[0].intents[0] = "Damage_21";

            TargettingByHealthUnitsStatus lowest = ScriptableObject.CreateInstance<TargettingByHealthUnitsStatus>();
            lowest.getAllies = false;
            lowest.Lowest = true;
            lowest.Status = "Cursed_ID";
            lowest.shouldHave = false;
            Ability reap1 = new Ability("Missed Reap", "Saturn_Reap_1_A");
            reap1.Description = "Curse and deal 6 damage to the Lowest health non-Cursed enemy(s).";
            reap1.AbilitySprite = ResourceLoader.LoadSprite("ability_reap.png");
            reap1.Cost = [Pigments.Red, Pigments.Yellow, Pigments.Blue];
            reap1.Effects = new EffectInfo[2];
            reap1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageAndCurseEffect>(), 6, lowest);
            reap1.Effects[1] = Effects.GenerateEffect(saturnSprites, 0, null, BasicEffects.DidThat(false));
            reap1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Status_Cursed", "Damage_3_6"]);
            reap1.Visuals = CustomVisuals.GetVisuals("Salt/Reload");
            reap1.AnimationTarget = lowest;

            Ability reap2 = new Ability(reap1.ability, "Saturn_Reap_2_A", reap1.Cost);
            reap2.Name = "Unseen Reap";
            reap2.Description = "Curse and deal 8 damage to the Lowest health non-Cursed enemy(s).";
            reap2.Effects[0].entryVariable = 8;
            reap2.EffectIntents[0].intents[1] = "Damage_7_10";

            Ability reap3 = new Ability(reap2.ability, "Saturn_Reap_3_A", reap1.Cost);
            reap3.Name = "Pinpoint Reap";
            reap3.Description = "Curse and deal 10 damage to the Lowest health non-Cursed enemy(s).";
            reap3.Effects[0].entryVariable = 10;

            Ability reap4 = new Ability(reap3.ability, "Saturn_Reap_4_A", reap1.Cost);
            reap4.Name = "Telescopic Reap";
            reap4.Description = "Curse and deal 12 damage to the Lowest health non-Cursed enemy(s).";
            reap4.Effects[0].entryVariable = 12;
            reap4.EffectIntents[0].intents[1] = "Damage_11_15";

            saturn.AddLevelData(11, [number1, gaze1, reap1]);
            saturn.AddLevelData(13, [number2, gaze2, reap2]);
            saturn.AddLevelData(16, [number3, gaze3, reap3]);
            saturn.AddLevelData(19, [number4, gaze4, reap4]);
            saturn.AddCharacter(true);
        }
        public static void Items()
        {
            MultiPerformEffectItem dial = new MultiPerformEffectItem("Aprils_Dial_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyPaleEffect>(), 15, Targetting.Random(false))]);
            dial.Name = "Dial";
            dial.Flavour = "\"Don't pick up, just listen.\"";
            dial.Description = "Inflict 15 Pale to a random enemy on combat start.\nDamage dealt by this party member to enemies with Pale spreads indirectly to the Left and Right.";
            dial.Icon = ResourceLoader.LoadSprite("item_dial.png");
            dial.EquippedModifiers = [];
            dial.TriggerOn = TriggerCalls.OnCombatStart;
            dial.DoesPopUpInfo = false;
            dial.Conditions = [];
            dial.DoesActionOnTriggerAttached = false;
            dial.ConsumeOnTrigger = TriggerCalls.Count;
            dial.ConsumeOnUse = false;
            dial.ConsumeConditions = [];
            dial.ShopPrice = 8;
            dial.IsShopItem = true;
            dial.StartsLocked = true;
            dial.OnUnlockUsesTHE = true;
            dial.UsesSpecialUnlockText = false;
            dial.SpecialUnlockID = UILocID.None;
            EffectTrigger second = new EffectTrigger([], [CascadingDamageItemHandler.Call], [ScriptableObject.CreateInstance<DialCondition>()], false);
            dial.AddEffectTrigger(second);
            dial.item._ItemTypeIDs = ["Magic"];
            dial.Item.AddItem("locked_dial.png", OsmanACH);

            ApplyPaleEffect range = ScriptableObject.CreateInstance<ApplyPaleEffect>();
            range._RandomBetweenPrevious = true;
            PerformEffect_Item telescope = new PerformEffect_Item("Aprils_Telescope_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<ExtraVariableForNextEffect>(), 40), Effects.GenerateEffect(range, 60, Targeting.Unit_AllOpponents)]);
            telescope.Name = "Telescope";
            telescope.Flavour = "\"The stars devoured all despair, thus they become clearer flare.\"";
            telescope.Description = "At the start of the third turn, inflict 40-60 Pale to all enemies.";
            telescope.Icon = ResourceLoader.LoadSprite("item_telescope.png");
            telescope.EquippedModifiers = [];
            telescope.TriggerOn = TriggerCalls.OnTurnStart;
            telescope.DoesPopUpInfo = true;
            telescope.Conditions = [TurnPassedCondition.Create(3)];
            telescope.DoesActionOnTriggerAttached = false;
            telescope.ConsumeOnTrigger = TriggerCalls.Count;
            telescope.ConsumeOnUse = false;
            telescope.ConsumeConditions = [];
            telescope.ShopPrice = 6;
            telescope.IsShopItem = true;
            telescope.StartsLocked = true;
            telescope.OnUnlockUsesTHE = true;
            telescope.UsesSpecialUnlockText = false;
            telescope.SpecialUnlockID = UILocID.None;
            telescope.item.AddItem("locked_telescope.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Saturn", "Telescope", "Dial", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Saturn_CH", "Aprils_Telescope_SW", "Aprils_Dial_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Saturn_Heaven_ACH";
        public static string OsmanACH => "Aprils_Saturn_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Saturn_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Saturn_Osman_Unlock";
    }
}
