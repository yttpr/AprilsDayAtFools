using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Prayer
    {
        public static void Add()
        {
            Character prayer = new Character("Prayer", "Prayer_CH");
            prayer.HealthColor = Pigments.Purple;
            prayer.AddUnitType("FemaleID");
            prayer.AddUnitType("Sandwich_War");
            prayer.AddUnitType("FemaleLooking");
            prayer.UsesBasicAbility = true;
            //slap
            prayer.UsesAllAbilities = false;
            prayer.MovesOnOverworld = true;
            //animator
            prayer.FrontSprite = ResourceLoader.LoadSprite("PrayerFront.png");
            prayer.BackSprite = ResourceLoader.LoadSprite("PrayerBack.png");
            prayer.OverworldSprite = ResourceLoader.LoadSprite("PrayerWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            prayer.DamageSound = "event:/Lunacy/SOUNDS/PA_Hit";
            prayer.DeathSound = "event:/Lunacy/SOUNDS/PA_Die";
            prayer.DialogueSound = "event:/Lunacy/SOUNDS/PA_Hit";
            prayer.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            prayer.AddFinalBossAchievementData("Heaven", HeavenACH);
            prayer.GenerateMenuCharacter(ResourceLoader.LoadSprite("PrayerMenu.png"), ResourceLoader.LoadSprite("PrayerLock.png"));
            prayer.MenuCharacterIsSecret = false;
            prayer.MenuCharacterIgnoreRandom = false;
            prayer.SetMenuCharacterAsFullSupport();

            HealEffect heal = ScriptableObject.CreateInstance<HealEffect>();

            Ability complex1 = new Ability("Industrial Complex", "Prayer_Complex_1_A");
            complex1.Description = "If any Purple Pigment was used, inflict 2 Gutted on the Left and Right allies. Otherwise, generate 2 Purple pigment.\nHeal the Left and Right allies 4 health and heal this party member 2 health.";
            complex1.AbilitySprite = ResourceLoader.LoadSprite("ability_complex.png");
            complex1.Cost = [Pigments.Yellow, Pigments.Blue, Pigments.PurpleBlue];
            complex1.Effects = new EffectInfo[4];
            complex1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyGuttedEffect>(), 2, Slots.Sides, UsedColorCondition.Create(Pigments.Purple, true));
            complex1.Effects[1] = Effects.GenerateEffect(BasicEffects.GenPigment(Pigments.Purple), 2, Slots.Self, UsedColorCondition.Create(Pigments.Purple, false));
            complex1.Effects[2] = Effects.GenerateEffect(heal, 4, Slots.Sides);
            complex1.Effects[3] = Effects.GenerateEffect(heal, 2, Slots.Self);
            complex1.AddIntentsToTarget(Slots.Sides, ["Status_Gutted", "Heal_1_4"]);
            complex1.AddIntentsToTarget(Slots.Self, ["Mana_Generate", "Heal_1_4"]);
            complex1.Visuals = CustomVisuals.GetVisuals("Salt/Unlock");
            complex1.AnimationTarget = Slots.Sides;

            Ability complex2 = new Ability(complex1.ability, "Prayer_Complex_2_A", complex1.Cost);
            complex2.Name = "Brutalist Complex";
            complex2.Description = "If any Purple Pigment was used, inflict 2 Gutted on the Left and Right allies. Otherwise, generate 2 Purple pigment.\nHeal the Left and Right allies 5 health and heal this party member 2 health.";
            complex2.Effects[2].entryVariable = 5;
            complex2.EffectIntents[0].intents[1] = "Heal_5_10";

            Ability complex3 = new Ability(complex2.ability, "Prayer_Complex_3_A", [Pigments.Blue, Pigments.BluePurple]);
            complex3.Name = "Colonialist Complex";
            complex3.Description = "If any Purple Pigment was used, inflict 2 Gutted on the Left and Right allies. Otherwise, generate 2 Purple pigment.\nHeal the Left and Right allies 5 health and heal this party member 3 health.";
            complex3.Effects[3].entryVariable = 3;

            Ability complex4 = new Ability(complex3.ability, "Prayer_Complex_4_A", complex3.Cost);
            complex4.Name = "God Complex";
            complex4.Description = "If any Purple Pigment was used, inflict 2 Gutted on the Left and Right allies. Otherwise, generate 2 Purple pigment.\nHeal the Left and Right allies 7 health and heal this party member 3 health.";
            complex4.Effects[2].entryVariable = 7;

            Ability hypo1 = new Ability("Psychological Hypothetical", "Prayer_Hypo_1_A");
            hypo1.Description = "If any Blue Pigment was used, heal the Right ally 3 health.\nIf any Purple Pigment was used, apply 2 Power to the Right ally and heal this party member 1 health.";
            hypo1.AbilitySprite = ResourceLoader.LoadSprite("ability_hypothetical.png");
            hypo1.Cost = [Pigments.PurpleBlue, Pigments.Yellow];
            hypo1.Effects = new EffectInfo[3];
            hypo1.Effects[0] = Effects.GenerateEffect(heal, 3, Targeting.Slot_AllyRight, UsedColorCondition.Create(Pigments.Blue, true));
            hypo1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyPowerEffect>(), 2, Targeting.Slot_AllyRight, complex1.Effects[0].condition);
            hypo1.Effects[2] = Effects.GenerateEffect(heal, 1, Slots.Self, complex1.Effects[0].condition);
            hypo1.AddIntentsToTarget(Targeting.Slot_AllyRight, ["Heal_1_4", Power.Intent]);
            hypo1.AddIntentsToTarget(Slots.Self, ["Heal_1_4"]);
            hypo1.Visuals = CustomVisuals.GetVisuals("Salt/Gaze");
            hypo1.AnimationTarget = Targeting.Slot_AllyRight;

            Ability hypo2 = new Ability(hypo1.ability, "Prayer_Hypo_2_A", hypo1.Cost);
            hypo2.Name = "Spiritual Hypothetical";
            hypo2.Description = "If any Blue Pigment was used, heal the Right ally 4 health.\nIf any Purple Pigment was used, apply 3 Power to the Right ally and heal this party member 2 health.";
            hypo2.Effects[0].entryVariable = 4;
            hypo2.Effects[1].entryVariable = 3;
            hypo2.Effects[2].entryVariable = 2;

            Ability hypo3 = new Ability(hypo2.ability, "Prayer_Hypo_3_A", hypo1.Cost);
            hypo3.Name = "Existential Hypothetical";
            hypo3.Description = "If any Blue Pigment was used, heal the Right ally 5 health.\nIf any Purple Pigment was used, apply 3 Power to the Right ally and heal this party member 2 health.";
            hypo3.Effects[0].entryVariable = 5;
            hypo3.EffectIntents[0].intents[0] = "Heal_5_10";

            Ability hypo4 = new Ability(hypo3.ability, "Prayer_Hypo_4_A", hypo1.Cost);
            hypo4.Name = "Transcendental Hypothetical";
            hypo4.Description = "If any Blue Pigment was used, heal the Right ally 5 health.\nIf any Purple Pigment was used, apply 4 Power to the Right ally and heal this party member 3 health.";
            hypo4.Effects[1].entryVariable = 4;
            hypo4.Effects[2].entryVariable = 3;

            Ability soul1 = new Ability("Strike the Soul", "Prayer_Soul_1_A");
            soul1.Description = "Deal 4 damage to the Opposing enemy.\nDamage generates Pigment of this party member's health color instead of the target's.";
            soul1.AbilitySprite = ResourceLoader.LoadSprite("ability_soul.png");
            soul1.Cost = [Pigments.Red, Pigments.Red];
            soul1.Effects = new EffectInfo[1];
            soul1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<GenerateSelfHealthDamageEffect>(), 4, Slots.Front);
            soul1.AddIntentsToTarget(Slots.Front, ["Damage_3_6", "Mana_Generate"]);
            soul1.Visuals = LoadedAssetsHandler.GetEnemyAbility("RingABell_A").visuals;
            soul1.AnimationTarget = Slots.Front;

            Ability soul2 = new Ability(soul1.ability, "Prayer_Soul_2_A", soul1.Cost);
            soul2.Name = "Split the Soul";
            soul2.Description = "Deal 6 damage to the Opposing enemy.\nDamage generates Pigment of this party member's health color instead of the target's.";
            soul2.Effects[0].entryVariable = 6;

            Ability soul3 = new Ability(soul2.ability, "Prayer_Soul_3_A", [Pigments.RedPurple, Pigments.Red]);
            soul3.Name = "Shatter the Soul";
            soul3.Description = "Deal 7 damage to the Opposing enemy.\nDamage generates Pigment of this party member's health color instead of the target's.";
            soul3.Effects[0].entryVariable = 7;
            soul3.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability soul4 = new Ability(soul3.ability, "Prayer_Soul_4_A", soul3.Cost);
            soul4.Name = "Disintegrate the Soul";
            soul4.Description = "Deal 8 damage to the Opposing enemy.\nDamage generates Pigment of this party member's health color instead of the target's.";
            soul4.Effects[0].entryVariable = 8;

            prayer.AddLevelData(11, [soul1, hypo1, complex1]);
            prayer.AddLevelData(13, [soul2, hypo2, complex2]);
            prayer.AddLevelData(14, [soul3, hypo3, complex3]);
            prayer.AddLevelData(16, [soul4, hypo4, complex4]);
            prayer.AddCharacter(true);
        }
        public static void Items()
        {
            RandomizeAmountPigmentEffect purple = ScriptableObject.CreateInstance<RandomizeAmountPigmentEffect>();
            purple._randomBetweenPrev = true;
            purple.Options = [Pigments.Purple];
            PerformEffect_Item vowkeeper = new PerformEffect_Item("Aprils_Vowkeeper_SW", [Effects.GenerateEffect(purple, 10)]);
            vowkeeper.Name = "Vowkeeper";
            vowkeeper.Flavour = "\"Purble.\"";
            vowkeeper.Description = "At the end of each turn, randomize 0-10 Pigment into Purple.";
            vowkeeper.Icon = ResourceLoader.LoadSprite("item_vowkeeper.png");
            vowkeeper.EquippedModifiers = [];
            vowkeeper.TriggerOn = TriggerCalls.OnTurnFinished;
            vowkeeper.DoesPopUpInfo = true;
            vowkeeper.Conditions = [];
            vowkeeper.DoesActionOnTriggerAttached = false;
            vowkeeper.ConsumeOnTrigger = TriggerCalls.Count;
            vowkeeper.ConsumeOnUse = false;
            vowkeeper.ConsumeConditions = [];
            vowkeeper.ShopPrice = 0;
            vowkeeper.IsShopItem = true;
            vowkeeper.StartsLocked = true;
            vowkeeper.OnUnlockUsesTHE = true;
            vowkeeper.UsesSpecialUnlockText = false;
            vowkeeper.SpecialUnlockID = UILocID.None;
            vowkeeper.Item.AddItem("locked_vowkeeper.png", OsmanACH);

            ExtraPassiveAbility_Wearable_SMS mimic = ScriptableObject.CreateInstance<ExtraPassiveAbility_Wearable_SMS>();
            mimic._extraPassiveAbility = Passives.GetCustomPassive("Mimicry_PA");
            CopyAndSpawnCustomCharacterAnywhereEffect anatomy = ScriptableObject.CreateInstance<CopyAndSpawnCustomCharacterAnywhereEffect>();
            anatomy._extraModifiers = [];
            anatomy._characterCopy = "AnatomyModel_CH";
            PerformEffect_Item anatomymodel = new PerformEffect_Item("Aprils_AnatomyModel_SW", [Effects.GenerateEffect(anatomy, 1)]);
            anatomymodel.Name = "Anatomy Model";
            anatomymodel.Flavour = "\"Plastic\"";
            anatomymodel.Description = "This party member has \"Mimicry\" as a passive.\nOn combat start, spawn the Anatomy Model.";
            anatomymodel.Icon = ResourceLoader.LoadSprite("item_anatomymodel.png");
            anatomymodel.EquippedModifiers = [mimic];
            anatomymodel.TriggerOn = TriggerCalls.OnCombatStart;
            anatomymodel.DoesPopUpInfo = true;
            anatomymodel.Conditions = [];
            anatomymodel.DoesActionOnTriggerAttached = false;
            anatomymodel.ConsumeOnTrigger = TriggerCalls.Count;
            anatomymodel.ConsumeOnUse = false;
            anatomymodel.ConsumeConditions = [];
            anatomymodel.ShopPrice = 7;
            anatomymodel.IsShopItem = true;
            anatomymodel.StartsLocked = true;
            anatomymodel.OnUnlockUsesTHE = true;
            anatomymodel.UsesSpecialUnlockText = false;
            anatomymodel.SpecialUnlockID = UILocID.None;
            anatomymodel.item._ItemTypeIDs = ["Face", "Heart"];
            anatomymodel.item.AddItem("locked_anatomymodel.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Prayer", "Anatomy Model", "Vowkeeper", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Prayer_CH", "Aprils_AnatomyModel_SW", "Aprils_Vowkeeper_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Prayer_Heaven_ACH";
        public static string OsmanACH => "Aprils_Prayer_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Prayer_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Prayer_Osman_Unlock";
    }
}
