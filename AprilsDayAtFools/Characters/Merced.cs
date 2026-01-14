using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Merced
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
            ordinary.AddToPassiveDatabase();
            ordinary.AddPassiveToGlossary("Ordinary", "On using wrong pigment, move Left or Right");

            Character merced = new Character("Merced", "Merced_CH");
            merced.HealthColor = Pigments.Yellow;
            merced.AddUnitType("FemaleID");
            merced.AddUnitType("Sandwich_NULL");
            merced.UsesBasicAbility = true;
            //slap
            merced.UsesAllAbilities = false;
            merced.MovesOnOverworld = true;
            //animator
            merced.FrontSprite = ResourceLoader.LoadSprite("MercedFront.png");
            merced.BackSprite = ResourceLoader.LoadSprite("MercedBack.png");
            merced.OverworldSprite = ResourceLoader.LoadSprite("MercedWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            merced.DamageSound = "event:/Lunacy/SOUNDS2/MercedHit";
            merced.DeathSound = "event:/Lunacy/SOUNDS2/MercedDie";
            merced.DialogueSound = "event:/Lunacy/SOUNDS2/MercedHit";
            merced.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            merced.AddFinalBossAchievementData("Heaven", HeavenACH);
            merced.GenerateMenuCharacter(ResourceLoader.LoadSprite("MercedMenu.png"), ResourceLoader.LoadSprite("MercedLock.png"));
            merced.MenuCharacterIsSecret = false;
            merced.MenuCharacterIgnoreRandom = false;
            merced.SetMenuCharacterAsFullDPS();
            merced.AddPassive(ordinary);

            DamageEffect damage = ScriptableObject.CreateInstance<DamageEffect>();
            DamageOnDoubleCascadeEffect halve = ScriptableObject.CreateInstance<DamageOnDoubleCascadeEffect>();
            halve._cascadeDecrease = 50;
            halve._decreaseAsPercentage = true;
            halve._cascadeIsIndirect = true;
            Ability toy1 = new Ability("Tick Toy", "Merced_Toy_1_A");
            toy1.Description = "Deal 8 damage to the Opposing enemy.\nIf the Opposing enemy is Ruptured, this damage spreads indirectly to the Left and Right.";
            toy1.AbilitySprite = ResourceLoader.LoadSprite("ability_toy.png");
            toy1.Cost = [Pigments.Red, Pigments.Red, Pigments.Red];
            toy1.Effects = new EffectInfo[3];
            toy1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ExtraVariableForNextEffect>(), 1, null, ScriptableObject.CreateInstance<FrontHasRupturedCondition>());
            toy1.Effects[1] = Effects.GenerateEffect(halve, 8, Targeting.GenerateSlotTarget([0, 1, 2, 3, 4, -1, -2, -3, -4], false), BasicEffects.DidThat(true));
            toy1.Effects[2] = Effects.GenerateEffect(damage, 8, Slots.Front, BasicEffects.DidThat(false, 2));
            toy1.AddIntentsToTarget(Slots.Front, ["Misc", "Damage_7_10"]);
            toy1.AddIntentsToTarget(Slots.Left, ["Swap_Left"]);
            toy1.AddIntentsToTarget(Slots.Right, ["Swap_Right"]);
            toy1.Visuals = CustomVisuals.GetVisuals("Salt/Decapitate");
            toy1.AnimationTarget = Slots.Front;

            Ability toy2 = new Ability(toy1.ability, "Merced_Toy_2_A", toy1.Cost);
            toy2.Name = "Mosquito Toy";
            toy2.Description = "Deal 11 damage to the Opposing enemy.\nIf the Opposing enemy is Ruptured, this damage spreads indirectly to the Left and Right.";
            toy2.Effects[1].entryVariable = 11;
            toy2.Effects[2].entryVariable = 11;
            toy2.EffectIntents[0].intents[1] = "Damage_11_15";

            Ability toy3 = new Ability(toy2.ability, "Merced_Toy_3_A", toy1.Cost);
            toy3.Name = "Horsefly Toy";
            toy3.Description = "Deal 13 damage to the Opposing enemy.\nIf the Opposing enemy is Ruptured, this damage spreads indirectly to the Left and Right.";
            toy3.Effects[1].entryVariable = 13;
            toy3.Effects[2].entryVariable = 13;

            Ability toy4 = new Ability(toy3.ability, "Merced_Toy_4_A", toy1.Cost);
            toy4.Name = "Leech Toy";
            toy4.Description = "Deal 15 damage to the Opposing enemy.\nIf the Opposing enemy is Ruptured, this damage spreads indirectly to the Left and Right.";
            toy4.Effects[1].entryVariable = 15;
            toy4.Effects[2].entryVariable = 15;

            Ability gambit1 = new Ability("Fools Gambit", "Merced_Gambit_1_A");
            gambit1.Description = "If the Opposing enemy is Ruptured, deal 10 damage to them.\nOtherwise, permenantly Rupture the Opposing enemy.";
            gambit1.AbilitySprite = ResourceLoader.LoadSprite("ability_gambit.png");
            gambit1.Cost = [Pigments.Red, Pigments.Blue];
            gambit1.Effects = new EffectInfo[3];
            gambit1.Effects[0] = Effects.GenerateEffect(toy1.Effects[0].effect, 1, null, toy1.Effects[0].condition);
            gambit1.Effects[1] = Effects.GenerateEffect(damage, 10, Slots.Front, BasicEffects.DidThat(true));
            gambit1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyPermanentRupturedEffect>(), 1, Slots.Front, BasicEffects.DidThat(false, 2));
            gambit1.AddIntentsToTarget(Slots.Front, ["Damage_11_15", "Status_Ruptured"]);
            gambit1.Visuals = LoadedAssetsHandler.GetEnemyAbility("Domination_A").visuals;
            gambit1.AnimationTarget = Slots.Front;

            Ability gambit2 = new Ability(gambit1.ability, "Merced_Gambit_2_A", gambit1.Cost);
            gambit2.Name = "Blind Gambit";
            gambit2.Description = "If the Opposing enemy is Ruptured, deal 14 damage to them.\nOtherwise, permenantly Rupture the Opposing enemy.";
            gambit2.Effects[1].entryVariable = 14;

            Ability gambit3 = new Ability(gambit2.ability, "Merced_Gambit_3_A", gambit1.Cost);
            gambit3.Name = "Doll Gambit";
            gambit3.Description = "If the Opposing enemy is Ruptured, deal 16 damage to them.\nOtherwise, permenantly Rupture the Opposing enemy.";
            gambit3.Effects[1].entryVariable = 16;
            gambit3.EffectIntents[0].intents[0] = "Damage_16_20";

            Ability gambit4 = new Ability(gambit3.ability, "Merced_Gambit_4_A", gambit1.Cost);
            gambit4.Name = "Queen's Gambit";
            gambit4.Description = "If the Opposing enemy is Ruptured, deal 20 damage to them.\nOtherwise, permenantly Rupture the Opposing enemy.";
            gambit4.Effects[1].entryVariable = 20;

            Ability cuts1 = new Ability("A Hundred Papercuts", "Merced_Cuts_1_A");
            cuts1.Description = "Deal 4 damage to the Opposing enemy.\nIf damage is dealt, inflict 2 Ruptured on all enemies.";
            cuts1.AbilitySprite = ResourceLoader.LoadSprite("ability_papercuts.png");
            cuts1.Cost = [Pigments.Red, Pigments.Yellow];
            cuts1.Effects = new EffectInfo[2];
            cuts1.Effects[0] = Effects.GenerateEffect(damage, 4, Slots.Front);
            cuts1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyRupturedEffect>(), 2, Targeting.Unit_AllOpponents, BasicEffects.DidThat(true));
            cuts1.AddIntentsToTarget(Slots.Front, ["Damage_3_6"]);
            cuts1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Status_Ruptured"]);
            cuts1.Visuals = CustomVisuals.GetVisuals("Salt/Needle");
            cuts1.AnimationTarget = Slots.Front;

            Ability cuts2 = new Ability(cuts1.ability, "Merced_Cuts_2_A", cuts1.Cost);
            cuts2.Name = "A Thousand Papercuts";
            cuts2.Description = "Deal 6 damage to the Opposing enemy.\nIf damage is dealt, inflict 2 Ruptured on all enemies.";
            cuts2.Effects[0].entryVariable = 6;

            Ability cuts3 = new Ability(cuts2.ability, "Merced_Cuts_3_A", cuts1.Cost);
            cuts3.Name = "A Million Papercuts";
            cuts3.Description = "Deal 7 damage to the Opposing enemy.\nIf damage is dealt, inflict 2 Ruptured on all enemies.";
            cuts3.Effects[0].entryVariable = 7;
            cuts3.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability cuts4 = new Ability(cuts3.ability, "Merced_Cuts_4_A", cuts1.Cost);
            cuts4.Name = "A Trillion Papercuts";
            cuts4.Description = "Deal 8 damage to the Opposing enemy.\nIf damage is dealt, inflict 2 Ruptured on all enemies.";
            cuts4.Effects[0].entryVariable = 8;

            merced.AddLevelData(10, [cuts1, gambit1, toy1]);
            merced.AddLevelData(12, [cuts2, gambit2, toy2]);
            merced.AddLevelData(15, [cuts3, gambit3, toy3]);
            merced.AddLevelData(18, [cuts4, gambit4, toy4]);
            merced.AddCharacter(true);
        }
        public static void Items()
        {
            PerformEffect_Item bloodcutter = new PerformEffect_Item("Aprils_BloodCutter_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyRupturedEffect>(), 2, Slots.Front)]);
            bloodcutter.Name = "Blood Cutter";
            bloodcutter.Flavour = "\"Rip it out.\"";
            bloodcutter.Description = "On taking any damage, inflict 2 Ruptured on the Opposing enemy.";
            bloodcutter.Icon = ResourceLoader.LoadSprite("item_bloodcutter.png");
            bloodcutter.EquippedModifiers = [];
            bloodcutter.TriggerOn = TriggerCalls.OnDamaged;
            bloodcutter.DoesPopUpInfo = true;
            bloodcutter.Conditions = [];
            bloodcutter.DoesActionOnTriggerAttached = false;
            bloodcutter.ConsumeOnTrigger = TriggerCalls.Count;
            bloodcutter.ConsumeOnUse = false;
            bloodcutter.ConsumeConditions = [];
            bloodcutter.ShopPrice = 3;
            bloodcutter.IsShopItem = true;
            bloodcutter.StartsLocked = true;
            bloodcutter.OnUnlockUsesTHE = true;
            bloodcutter.UsesSpecialUnlockText = false;
            bloodcutter.SpecialUnlockID = UILocID.None;
            bloodcutter.item._ItemTypeIDs = ["Knife"];
            bloodcutter.Item.AddItem("locked_bloodcutter.png", OsmanACH);

            GenericEffectItem<RestrictorEffectWearable> bloodclotter = new GenericEffectItem<RestrictorEffectWearable>("Aprils_BloodClotter_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyPowerEffect>(), 2, Slots.Self)]);
            bloodclotter.Name = "Blood Clotter";
            bloodclotter.Flavour = "\"Won't let it flow.\"";
            bloodclotter.Description = "This party member is permenantly Ruptured.\nOn taking any damage, gain 2 Power.";
            bloodclotter.Icon = ResourceLoader.LoadSprite("item_bloodclotter.png");
            bloodclotter.EquippedModifiers = [];
            bloodclotter.TriggerOn = TriggerCalls.OnDamaged;
            bloodclotter.DoesPopUpInfo = true;
            bloodclotter.Conditions = [];
            bloodclotter.DoesActionOnTriggerAttached = true;
            bloodclotter.ConsumeOnTrigger = TriggerCalls.Count;
            bloodclotter.ConsumeOnUse = false;
            bloodclotter.ConsumeConditions = [];
            bloodclotter.ShopPrice = 4;
            bloodclotter.IsShopItem = true;
            bloodclotter.StartsLocked = true;
            bloodclotter.OnUnlockUsesTHE = true;
            bloodclotter.UsesSpecialUnlockText = false;
            bloodclotter.SpecialUnlockID = UILocID.None;
            (bloodclotter.item as RestrictorEffectWearable).Status = StatusField.Ruptured;
            bloodclotter.Item.AddItem("locked_bloodclotter.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Merced", "Blood Clotter", "Blood Cutter", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Merced_CH", "Aprils_BloodClotter_SW", "Aprils_BloodCutter_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Merced_Heaven_ACH";
        public static string OsmanACH => "Aprils_Merced_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Merced_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Merced_Osman_Unlock";
    }
}
