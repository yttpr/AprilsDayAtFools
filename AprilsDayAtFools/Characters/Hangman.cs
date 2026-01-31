using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Hangman
    {
        public static void Add()
        {
            PerformEffectPassiveAbility flowers = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            flowers._passiveName = "Flowers";
            flowers.passiveIcon = ResourceLoader.LoadSprite("FlowersPassive.png");
            flowers.m_PassiveID = IDs.Flowers;
            flowers.name = IDs.Flowers;
            flowers._enemyDescription = "On Fleeing, return at the end of the timeline.";
            flowers._characterDescription = "On Fleeing, return at the end of the timeline.";
            flowers.doesPassiveTriggerInformationPanel = true;
            flowers.effects = [];
            flowers._triggerOn = [FlowersUnboxer.Trigger];
            flowers.AddPassiveToGlossary("Flowers", "On Fleeing, return at the end of the timeline.");
            flowers.AddToPassiveDatabase();

            Character hangman = new Character("Hangman", "Hangman_CH");
            hangman.HealthColor = Pigments.Yellow;
            hangman.AddUnitType("FemaleID");
            //hangman.AddUnitType("Sandwich_Spirit");
            hangman.AddUnitType("FemaleLooking");
            hangman.UsesBasicAbility = true;
            //slap
            hangman.UsesAllAbilities = false;
            hangman.MovesOnOverworld = true;
            //animator
            hangman.FrontSprite = ResourceLoader.LoadSprite("HangmanFront0.png");
            hangman.BackSprite = ResourceLoader.LoadSprite("HangmanBack.png");
            hangman.OverworldSprite = ResourceLoader.LoadSprite("HangmanWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            hangman.DamageSound = "event:/Lunacy/SOUNDS/UFO_Hit";
            hangman.DeathSound = "event:/Lunacy/SOUNDS/YA_Death";
            hangman.DialogueSound = "event:/Lunacy/SOUNDS/UFO_Hit";
            hangman.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            hangman.AddFinalBossAchievementData("Heaven", HeavenACH);
            hangman.GenerateMenuCharacter(ResourceLoader.LoadSprite("HangmanMenu.png"), ResourceLoader.LoadSprite("HangmanLock.png"));
            hangman.MenuCharacterIsSecret = false;
            hangman.MenuCharacterIgnoreRandom = false;
            hangman.SetMenuCharacterAsFullSupport();
            hangman.AddPassive(flowers);

            Ability year1 = new Ability("New Today", "Hangman_Year_1_A");
            year1.Description = "Deal 7 damage to the Opposing enemy and apply 1 Divine Protection to them.\nThis ability cannot reduce the Opposing enemy's health below 1.\nInstantly flee.";
            year1.AbilitySprite = ResourceLoader.LoadSprite("ability_year.png");
            year1.Cost = [Pigments.Red, Pigments.Blue, Pigments.Yellow];
            year1.Effects = new EffectInfo[4];
            year1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<NoKillingDamageEffect>(), 7, Slots.Front);
            year1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyDivineProtectionEffect>(), 1, Slots.Front);
            year1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageByCostEffect>(), 1, Slots.Self);
            year1.Effects[3] = Effects.GenerateEffect(ScriptableObject.CreateInstance<CasterFleetingIfAliveEffect>(), 1, Slots.Self);
            year1.AddIntentsToTarget(Slots.Front, ["Damage_7_10", "Status_DivineProtection"]);
            year1.AddIntentsToTarget(Slots.Self, ["PA_Fleeting"]);
            year1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Shank_1_A").visuals;
            year1.AnimationTarget = Slots.Front;

            Ability year2 = new Ability(year1.ability, "Hangman_Year_2_A", year1.Cost);
            year2.Name = "New Tomorrow";
            year2.Description = "Deal 9 damage to the Opposing enemy and apply 1 Divine Protection to them.\nThis ability cannot reduce the Opposing enemy's health below 1.\nInstantly flee.";
            year2.Effects[0].entryVariable = 9;

            Ability year3 = new Ability(year2.ability, "Hangman_Year_3_A", year1.Cost);
            year3.Name = "New Year";
            year3.Description = "Deal 11 damage to the Opposing enemy and apply 1 Divine Protection to them.\nThis ability cannot reduce the Opposing enemy's health below 1.\nInstantly flee.";
            year3.Effects[0].entryVariable = 11;
            year3.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability year4 = new Ability(year3.ability, "Hangman_Year_4_A", year1.Cost);
            year4.Name = "New Era";
            year4.Description = "Deal 13 damage to the Opposing enemy and apply 1 Divine Protection to them.\nThis ability cannot reduce the Opposing enemy's health below 1.\nInstantly flee.";
            year4.Effects[0].entryVariable = 13;

            TargettingByDamagedPreviously lastDamaged = ScriptableObject.CreateInstance<TargettingByDamagedPreviously>();
            lastDamaged.getAllies = true;
            lastDamaged.getAllUnitSlots = false;
            lastDamaged.ignoreCastSlot = false;
            Ability coma1 = new Ability("Ephemeral Break", "Hangman_Coma_1_A");
            coma1.Description = "Heal all allies that took any damage last turn 3 health.\nInstantly flee if this party member is opposing an enemy.";
            coma1.AbilitySprite = ResourceLoader.LoadSprite("ability_coma.png");
            coma1.Cost = [Pigments.Yellow, Pigments.Blue];
            coma1.Effects = new EffectInfo[4];
            coma1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), 3, lastDamaged);
            coma1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ExtraVariableForNextEffect>(), 1, null, ScriptableObject.CreateInstance<FrontSlotEmptyCondition>());
            coma1.Effects[2] = Effects.GenerateEffect(year1.Effects[2].effect, 1, Slots.Self, BasicEffects.DidThat(false));
            coma1.Effects[3] = Effects.GenerateEffect(year1.Effects[3].effect, 1, Slots.Self, BasicEffects.DidThat(false, 2));
            coma1.AddIntentsToTarget(lastDamaged, ["Heal_1_4"]);
            coma1.AddIntentsToTarget(Targeting.Unit_AllAllies, ["Misc_Hidden"]);
            coma1.AddIntentsToTarget(Slots.Self, ["PA_Fleeting"]);
            coma1.Visuals = CustomVisuals.GetVisuals("Salt/Door");
            coma1.AnimationTarget = lastDamaged;

            Ability coma2 = new Ability(coma1.ability, "Hangman_Coma_2_A", coma1.Cost);
            coma2.Name = "Ephemeral Rest";
            coma2.Description = "Heal all allies that took any damage last turn 4 health.\nInstantly flee if this party member is opposing an enemy.";
            coma2.Effects[0].entryVariable = 4;

            Ability coma3 = new Ability(coma2.ability, "Hangman_Coma_3_A", coma1.Cost);
            coma3.Name = "Ephemeral Sleep";
            coma3.Description = "Heal all allies that took any damage last turn 5 health.\nInstantly flee if this party member is opposing an enemy.";
            coma3.Effects[0].entryVariable = 5;
            coma3.EffectIntents[0].intents[0] = "Heal_5_10";

            Ability coma4 = new Ability(coma3.ability, "Hangman_Coma_4_A", coma1.Cost);
            coma4.Name = "Ephemeral Coma";
            coma4.Description = "Heal all allies that took any damage last turn 6 health.\nInstantly flee if this party member is opposing an enemy.";
            coma4.Effects[0].entryVariable = 6;

            HealRemoveStatusEffect byType = ScriptableObject.CreateInstance<HealRemoveStatusEffect>();
            byType._negativeOnly = false;
            byType._useTypes = true;
            Ability plastic1 = new Ability("Plastic Model", "Hangman_Plastic_1_A");
            plastic1.Description = "Remove all Status Effects from the Left ally and heal them 5 health reduced by the amount of unique Status Effects removed.\nHeal this party member 2 health.";
            plastic1.AbilitySprite = ResourceLoader.LoadSprite("ability_plastic.png");
            plastic1.Cost = [Pigments.Yellow, Pigments.Blue, Pigments.Blue];
            plastic1.Effects = new EffectInfo[2];
            plastic1.Effects[0] = Effects.GenerateEffect(byType, 5, Targeting.Slot_AllyLeft);
            plastic1.Effects[1] = Effects.GenerateEffect(coma1.Effects[0].effect, 2, Slots.Self);
            plastic1.AddIntentsToTarget(Targeting.Slot_AllyLeft, ["Misc", "Heal_5_10"]);
            plastic1.AddIntentsToTarget(Slots.Self, ["Heal_1_4"]);
            plastic1.Visuals = CustomVisuals.GetVisuals("Salt/Cube");
            plastic1.AnimationTarget = Targeting.Slot_AllyLeft;

            Ability plastic2 = new Ability(plastic1.ability, "Hangman_Plastic_2_A", [Pigments.Blue, Pigments.Blue]);
            plastic2.Name = "Plastic Display";
            plastic2.Description = "Remove all Status Effects from the Left ally and heal them 6 health reduced by the amount of unique Status Effects removed.\nHeal this party member 2 health.";
            plastic2.Effects[0].entryVariable = 6;

            Ability plastic3 = new Ability(plastic2.ability, "Hangman_Plastic_3_A", plastic2.Cost);
            plastic3.Name = "Plastic Replica";
            plastic3.Description = "Remove all Status Effects from the Left ally and heal them 8 health reduced by the amount of unique Status Effects removed.\nHeal this party member 2 health.";
            plastic3.Effects[0].entryVariable = 8;

            Ability plastic4 = new Ability(plastic3.ability, "Hangman_Plastic_4_A", plastic2.Cost);
            plastic4.Name = "Plastic Construction";
            plastic4.Description = "Remove all Status Effects from the Left ally and heal them 10 health reduced by the amount of unique Status Effects removed.\nHeal this party member 2 health.";
            plastic4.Effects[0].entryVariable = 10;

            hangman.AddLevelData(7, [coma1, year1, plastic1]);
            hangman.AddLevelData(8, [coma2, year2, plastic2]);
            hangman.AddLevelData(9, [coma3, year3, plastic3]);
            hangman.AddLevelData(11, [coma4, year4, plastic4]);
            hangman.AddCharacter(true);
        }
        public static void Items()
        {
            ExtraPassiveAbility_Wearable_SMS flowers = ScriptableObject.CreateInstance<ExtraPassiveAbility_Wearable_SMS>();
            flowers._extraPassiveAbility = Passives.GetCustomPassive("Flowers_PA");
            PerformEffect_Item formsplitter = new PerformEffect_Item("Aprils_FormSplitter_TW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 7, Slots.Front), Effects.GenerateEffect(ScriptableObject.CreateInstance<FleeTargetEffect>(), 1, Slots.Self)]);
            formsplitter.Name = "Form-Splitter";
            formsplitter.Flavour = "\"It splits forms.\"";
            formsplitter.Description = "When an enemy moves in front of this party member, deal 7 damage to them and instantly flee.\nThis party member has \"Flowers\" as a passive.";
            formsplitter.Icon = ResourceLoader.LoadSprite("item_formsplitter.png");
            formsplitter.EquippedModifiers = [flowers];
            formsplitter.TriggerOn = AmbushManager.Trigger;
            formsplitter.DoesPopUpInfo = true;
            formsplitter.Conditions = [];
            formsplitter.DoesActionOnTriggerAttached = false;
            formsplitter.ConsumeOnTrigger = TriggerCalls.Count;
            formsplitter.ConsumeOnUse = false;
            formsplitter.ConsumeConditions = [];
            formsplitter.ShopPrice = 5;
            formsplitter.IsShopItem = false;
            formsplitter.StartsLocked = true;
            formsplitter.OnUnlockUsesTHE = true;
            formsplitter.UsesSpecialUnlockText = false;
            formsplitter.SpecialUnlockID = UILocID.None;
            formsplitter.item._ItemTypeIDs = ["Magic"];
            formsplitter.Item.AddItem("locked_formsplitter.png", OsmanACH);

            PerformEffect_Item braintumor = new PerformEffect_Item("Aprils_BrainTumor_TW", []);
            braintumor.Name = "Brain Tumor";
            braintumor.Flavour = "\"Trolling Organ\"";
            braintumor.Description = "On dealing damage, generate 2 additional Pigment of the target's health color.";
            braintumor.Icon = ResourceLoader.LoadSprite("item_braintumor.png");
            braintumor.EquippedModifiers = [];
            braintumor.TriggerOn = AdvancedDamageTrigger.Dealt;
            braintumor.DoesPopUpInfo = false;
            braintumor.Conditions = [DamageTargetEffectsCondition.Create([Effects.GenerateEffect(ScriptableObject.CreateInstance<GenerateCasterHealthManaEffect>(), 2, Slots.Self)], true)];
            braintumor.DoesActionOnTriggerAttached = false;
            braintumor.ConsumeOnTrigger = TriggerCalls.Count;
            braintumor.ConsumeOnUse = false;
            braintumor.ConsumeConditions = [];
            braintumor.ShopPrice = 4;
            braintumor.IsShopItem = false;
            braintumor.StartsLocked = true;
            braintumor.OnUnlockUsesTHE = true;
            braintumor.UsesSpecialUnlockText = false;
            braintumor.SpecialUnlockID = UILocID.None;
            braintumor.item._ItemTypeIDs = ["Meat"];
            braintumor.Item.AddItem("locked_braintumor.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Hangman", "Brain Tumor", "Form-Splitter", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Hangman_CH", "Aprils_BrainTumor_TW", "Aprils_FormSplitter_TW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Hangman_Heaven_ACH";
        public static string OsmanACH => "Aprils_Hangman_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Hangman_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Hangman_Osman_Unlock";
    }
}
