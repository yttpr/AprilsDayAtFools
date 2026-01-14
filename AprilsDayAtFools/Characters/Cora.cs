using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Cora
    {
        public static void Add()
        {
            PerformEffectPassiveAbility ordinary = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            ordinary._passiveName = "Ordinary";
            ordinary.passiveIcon = ResourceLoader.LoadSprite("OrdinaryPassive.png");
            ordinary.m_PassiveID = "Ordinary_PA";
            ordinary._enemyDescription = "i mean.....";
            ordinary._characterDescription = "On using wrong pigment, move Left or Right.";
            ordinary.doesPassiveTriggerInformationPanel = true;
            ordinary.conditions = [WrongPigmentCondition.Create("")];
            ordinary.effects = new EffectInfo[1];
            ordinary.effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<SwapToSidesEffect>(), 1, Slots.Self);
            ordinary._triggerOn = [TriggerCalls.OnAbilityUsed];

            Character cora = new Character("Cora", "Cora_CH");
            cora.HealthColor = Pigments.Blue;
            cora.AddUnitType("FemaleID");
            cora.UsesBasicAbility = true;
            //slap
            cora.UsesAllAbilities = false;
            cora.MovesOnOverworld = true;
            //animator
            cora.FrontSprite = ResourceLoader.LoadSprite("CoraFront.png");
            cora.BackSprite = ResourceLoader.LoadSprite("CoraBack.png");
            cora.OverworldSprite = ResourceLoader.LoadSprite("CoraWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            cora.DamageSound = "event:/Lunacy/SOUNDS2/CoraHit";
            cora.DeathSound = "event:/Lunacy/SOUNDS2/CoraDie";
            cora.DialogueSound = "event:/Lunacy/SOUNDS2/CoraHit";
            cora.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            cora.AddFinalBossAchievementData("Heaven", HeavenACH);
            cora.GenerateMenuCharacter(ResourceLoader.LoadSprite("CoraMenu.png"), ResourceLoader.LoadSprite("CoraLock.png"));
            cora.MenuCharacterIsSecret = false;
            cora.MenuCharacterIgnoreRandom = false;
            cora.SetMenuCharacterAsFullDPS();
            cora.AddPassive(ordinary);

            DamageEffect damage = ScriptableObject.CreateInstance<DamageEffect>();
            DamageOnDoubleCascadeWithExtraPigmentEffectEffect halve = ScriptableObject.CreateInstance<DamageOnDoubleCascadeWithExtraPigmentEffectEffect>();
            halve._cascadeDecrease = 50;
            halve._decreaseAsPercentage = true;
            halve._cascadeIsIndirect = true;
            halve._pigment = 3;
            Ability ink1 = new Ability("Ink Drop", "Cora_Ink_1_A");
            ink1.Description = "Deal 8 damage to the Opposing enemy and generate 3 additional Pigment of their health color.\nIf this is the first ability used this turn, this damage spreads indirectly to the Left and Right.";
            ink1.AbilitySprite = ResourceLoader.LoadSprite("ability_ink.png");
            ink1.Cost = [Pigments.Red, Pigments.Yellow, Pigments.Yellow];
            ink1.Effects = new EffectInfo[4];
            ink1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ExtraVariableForNextEffect>(), 1, null, ScriptableObject.CreateInstance<FirstAbilityUsedCondition>());
            ink1.Effects[1] = Effects.GenerateEffect(halve, 8, Targeting.GenerateSlotTarget([0, 1, 2, 3, 4, -1, -2, -3, -4], false), BasicEffects.DidThat(true));
            ink1.Effects[2] = Effects.GenerateEffect(damage, 8, Slots.Front, BasicEffects.DidThat(false, 2));
            ink1.Effects[3] = Effects.GenerateEffect(ScriptableObject.CreateInstance<GenerateTargetHealthManaEffect>(), 3, Slots.Front, BasicEffects.DidThat(false, 3));
            ink1.AddIntentsToTarget(Slots.Self, ["Misc"]);
            ink1.AddIntentsToTarget(Slots.Front, ["Damage_7_10", "Mana_Generate"]);
            ink1.AddIntentsToTarget(Slots.Left, ["Swap_Left"]);
            ink1.AddIntentsToTarget(Slots.Right, ["Swap_Right"]);
            ink1.Visuals = CustomVisuals.GetVisuals("Salt/Curse");
            ink1.AnimationTarget = Slots.Front;

            Ability ink2 = new Ability(ink1.ability, "Cora_Ink_2_A", [Pigments.Red, Pigments.YellowRed, Pigments.Yellow]);
            ink2.Name = "Ink Spill";
            ink2.Description = "Deal 11 damage to the Opposing enemy and generate 3 additional Pigment of their health color.\nIf this is the first ability used this turn, this damage spreads indirectly to the Left and Right.";
            ink2.Effects[1].entryVariable = 11;
            ink2.Effects[2].entryVariable = 11;
            ink2.EffectIntents[1].intents[0] = "Damage_11_15";

            Ability ink3 = new Ability(ink2.ability, "Cora_Ink_3_A", [Pigments.Red, Pigments.YellowRed, Pigments.YellowRed]);
            ink3.Name = "Ink Pour";
            ink3.Description = "Deal 13 damage to the Opposing enemy and generate 3 additional Pigment of their health color.\nIf this is the first ability used this turn, this damage spreads indirectly to the Left and Right.";
            ink3.Effects[1].entryVariable = 13;
            ink3.Effects[2].entryVariable = 13;

            Ability toy4 = new Ability(ink3.ability, "Cora_Ink_4_A", ink3.Cost);
            toy4.Name = "Ink Flood";
            toy4.Description = "Deal 15 damage to the Opposing enemy and generate 3 additional Pigment of their health color.\nIf this is the first ability used this turn, this damage spreads indirectly to the Left and Right.";
            toy4.Effects[1].entryVariable = 15;
            toy4.Effects[2].entryVariable = 15;

            TargettingFarthestUnits farthest = ScriptableObject.CreateInstance<TargettingFarthestUnits>();
            farthest.ignoreCastSlot = false;
            farthest.getAllies = false;
            farthest.OnlyOne = true;
            Ability checkmate1 = new Ability("Fools Checkmate", "Cora_Checkmate_1_A");
            checkmate1.Description = "Deal 9 damage to the Farthest enemy(s) from this party member and give them another action.";
            checkmate1.AbilitySprite = ResourceLoader.LoadSprite("ability_checkmate.png");
            checkmate1.Cost = [Pigments.Yellow, Pigments.Blue];
            checkmate1.Effects = new EffectInfo[2];
            checkmate1.Effects[0] = Effects.GenerateEffect(damage, 9, farthest);
            checkmate1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<AddTurnTargetToTimelineEffect>(), 1, farthest);
            checkmate1.AddIntentsToTarget(farthest, ["Damage_7_10", "Misc_Additional"]);
            checkmate1.Visuals = CustomVisuals.GetVisuals("Salt/Class");
            checkmate1.AnimationTarget = farthest;

            Ability checkmate2 = new Ability(checkmate1.ability, "Cora_Checkmate_2_A", checkmate1.Cost);
            checkmate2.Name = "Blind Checkmate";
            checkmate2.Description = "Deal 12 damage to the Farthest enemy(s) from this party member and give them another action.";
            checkmate2.Effects[0].entryVariable = 12;
            checkmate2.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability checkmate3 = new Ability(checkmate2.ability, "Cora_Checkmate_3_A", [Pigments.Blue]);
            checkmate3.Name = "Doll Checkmate";
            checkmate3.Description = "Deal 14 damage to the Farthest enemy(s) from this party member and give them another action.";
            checkmate3.Effects[0].entryVariable = 14;

            Ability checkmate4 = new Ability(checkmate3.ability, "Cora_Checkmate_4_A", checkmate3.Cost);
            checkmate4.Name = "Queen's Checkmate";
            checkmate4.Description = "Deal 17 damage to the Farthest enemy(s) from this party member and give them another action.";
            checkmate4.Effects[0].entryVariable = 17;
            checkmate4.EffectIntents[0].intents[0] = "Damage_16_20";

            Ability needles1 = new Ability("A Hundred Needles", "Cora_Needles_1_A");
            needles1.Description = "Deal 4 damage to the Opposing enemy and reroll one of their actions on the timeline.";
            needles1.AbilitySprite = ResourceLoader.LoadSprite("ability_needles.png");
            needles1.Cost = [Pigments.Red, Pigments.BlueYellow];
            needles1.Effects = new EffectInfo[2];
            needles1.Effects[0] = Effects.GenerateEffect(damage, 4, Slots.Front);
            needles1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ReRollTargetTimelineAbilityEffect>(), 1, Slots.Front);
            needles1.AddIntentsToTarget(Slots.Front, ["Damage_3_6", "Misc"]);
            needles1.Visuals = CustomVisuals.GetVisuals("Salt/Needle");
            needles1.AnimationTarget = Slots.Front;

            Ability needles2 = new Ability(needles1.ability, "Cora_Needles_2_A", needles1.Cost);
            needles2.Name = "A Thousand Needles";
            needles2.Description = "Deal 6 damage to the Opposing enemy and reroll one of their actions on the timeline.";
            needles2.Effects[0].entryVariable = 6;

            Ability needles3 = new Ability(needles2.ability, "Cora_Needles_3_A", needles1.Cost);
            needles3.Name = "A Million Needles";
            needles3.Description = "Deal 7 damage to the Opposing enemy and reroll one of their actions on the timeline.";
            needles3.Effects[0].entryVariable = 7;
            needles3.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability needles4 = new Ability(needles3.ability, "Cora_Needles_4_A", needles1.Cost);
            needles4.Name = "A Trillion Needles";
            needles4.Description = "Deal 8 damage to the Opposing enemy and reroll one of their actions on the timeline.";
            needles4.Effects[0].entryVariable = 8;

            cora.AddLevelData(10, [needles1, checkmate1, ink1]);
            cora.AddLevelData(12, [needles2, checkmate2, ink2]);
            cora.AddLevelData(15, [needles3, checkmate3, ink3]);
            cora.AddLevelData(18, [needles4, checkmate4, toy4]);
            cora.AddCharacter(true);
        }
        public static void Items()
        {
            PigmentDamageByStoredValueEffect rootsDamage = ScriptableObject.CreateInstance<PigmentDamageByStoredValueEffect>();
            rootsDamage._increaseDamage = true;
            rootsDamage.m_unitStoredDataID = "RootsOfGod_TW";
            rootsDamage._indirect = true;
            rootsDamage._DeathTypeID = "ManaCost";

            PerformEffect_Item rootsofgod = new PerformEffect_Item("Aprils_RootsOfGod_TW", [Effects.GenerateEffect(rootsDamage, 0, Targeting.Unit_AllOpponents)]);
            rootsofgod.Name = "Roots Of God";
            rootsofgod.Flavour = "\"Reaching for the stars, they bend back down under their own weight.\"";
            rootsofgod.Description = "Wrong Pigment and Overflow damage taken by this party member is also dealt indirectly to all enemies.";
            rootsofgod.Icon = ResourceLoader.LoadSprite("item_rootsofgod.png");
            rootsofgod.EquippedModifiers = [];
            rootsofgod.TriggerOn = AdvancedDamageTrigger.Received;
            rootsofgod.DoesPopUpInfo = true;
            rootsofgod.Conditions = [DamageTypeCondition.Create("Dmg_Pigment"), IntegerRefStoredValSetterCondition.Create("RootsOfGod_TW")];
            rootsofgod.DoesActionOnTriggerAttached = false;
            rootsofgod.ConsumeOnTrigger = TriggerCalls.Count;
            rootsofgod.ConsumeOnUse = false;
            rootsofgod.ConsumeConditions = [];
            rootsofgod.ShopPrice = 4;
            rootsofgod.IsShopItem = false;
            rootsofgod.StartsLocked = true;
            rootsofgod.OnUnlockUsesTHE = true;
            rootsofgod.UsesSpecialUnlockText = false;
            rootsofgod.SpecialUnlockID = UILocID.None;
            rootsofgod.item._ItemTypeIDs = ["Magic"];
            rootsofgod.Item.AddItem("locked_rootsofgod.png", OsmanACH);

            GenericEffectItem<RestrictorEffectWearable> grimoire = new GenericEffectItem<RestrictorEffectWearable>("Aprils_Grimoire_SW", []);
            grimoire.Name = "Grimoire";
            grimoire.Flavour = "\"My Evil Spells\"";
            grimoire.Description = "This party member is permenantly Ruptured.\nDamage dealt spreads indirectly Left and Right.";
            grimoire.Icon = ResourceLoader.LoadSprite("item_grimoire.png");
            grimoire.EquippedModifiers = [];
            grimoire.TriggerOn = CascadingDamageItemHandler.Call;
            grimoire.DoesPopUpInfo = false;
            grimoire.Conditions = [BooleanSetterCondition.Create(true, true, false)];
            grimoire.DoesActionOnTriggerAttached = true;
            grimoire.ConsumeOnTrigger = TriggerCalls.Count;
            grimoire.ConsumeOnUse = false;
            grimoire.ConsumeConditions = [];
            grimoire.ShopPrice = 4;
            grimoire.IsShopItem = true;
            grimoire.StartsLocked = true;
            grimoire.OnUnlockUsesTHE = true;
            grimoire.UsesSpecialUnlockText = false;
            grimoire.SpecialUnlockID = UILocID.None;
            (grimoire.item as RestrictorEffectWearable).Status = StatusField.Ruptured;
            grimoire.item._ItemTypeIDs = ["Magic"];
            grimoire.Item.AddItem("locked_grimoire.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Cora", "Grimoire", "Roots Of God", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Cora_CH", "Aprils_Grimoire_SW", "Aprils_RootsOfGod_TW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Cora_Heaven_ACH";
        public static string OsmanACH => "Aprils_Cora_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Cora_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Cora_Osman_Unlock";
    }
}
