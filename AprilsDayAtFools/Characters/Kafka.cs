using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Kafka
    {
        public static void Add()
        {
            ExtraCCSprites_ArraySO kafkaExtra = ScriptableObject.CreateInstance<ExtraCCSprites_ArraySO>();
            kafkaExtra._DefaultID = "";
            kafkaExtra._SpecialID = IDs.Kafka;
            kafkaExtra._frontSprite = [ResourceLoader.LoadSprite("KafkaDetermined.png"),
                ResourceLoader.LoadSprite("KafkaDet2.png"),
                ResourceLoader.LoadSprite("KafkaDet3.png"),
                ResourceLoader.LoadSprite("KafkaDet4.png"),
                ResourceLoader.LoadSprite("KafkaDet5.png")];
            kafkaExtra._backSprite = [ResourceLoader.LoadSprite("KafkaBack.png"),
                ResourceLoader.LoadSprite("KafkaBack.png"),
                ResourceLoader.LoadSprite("KafkaBack.png"),
                ResourceLoader.LoadSprite("KafkaBack4.png"),
                ResourceLoader.LoadSprite("KafkaBack5.png")];
            kafkaExtra._doesLoop = false;

            Character kafka = new Character("Kafka", "Kafka_CH");
            kafka.HealthColor = Pigments.Purple;
            kafka.AddUnitType("FemaleID");
            kafka.AddUnitType("FemaleLooking");
            kafka.UsesBasicAbility = true;
            //slap
            kafka.UsesAllAbilities = false;
            kafka.MovesOnOverworld = true;
            //animator
            kafka.FrontSprite = ResourceLoader.LoadSprite("KafkaFront.png");
            kafka.BackSprite = ResourceLoader.LoadSprite("KafkaBack.png");
            kafka.OverworldSprite = ResourceLoader.LoadSprite("KafkaWorld.png", new Vector2(0.5f, 0f));
            kafka.ExtraSprites = kafkaExtra;
            kafka.DamageSound = "event:/Lunacy/SOUNDS/FoxtrotHit";
            kafka.DeathSound = "event:/Lunacy/SOUNDS/FoxtrotDie";
            kafka.DialogueSound = "event:/Lunacy/SOUNDS/FoxtrotHit";
            kafka.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            kafka.AddFinalBossAchievementData("Heaven", HeavenACH);
            kafka.GenerateMenuCharacter(ResourceLoader.LoadSprite("KafkaMenu.png"), ResourceLoader.LoadSprite("KafkaLocked.png"));
            kafka.MenuCharacterIsSecret = false;
            kafka.MenuCharacterIgnoreRandom = false;
            kafka.SetMenuCharacterAsFullSupport();

            ApplyDeterminedEffect determined = ScriptableObject.CreateInstance<ApplyDeterminedEffect>();
            HealEffect heal = ScriptableObject.CreateInstance<HealEffect>();

            Ability hope1 = new Ability("Miniscule Hope", "Kafka_Hope_1_A");
            hope1.Description = "Heal the Left ally 7 health and apply 2 Determined to them.\nApply 2 Determined to this party member.";
            hope1.AbilitySprite = ResourceLoader.LoadSprite("ability_hope.png");
            hope1.Cost = [Pigments.Blue, Pigments.Purple, Pigments.Yellow];
            hope1.Effects = new EffectInfo[3];
            hope1.Effects[0] = Effects.GenerateEffect(heal, 7, Targeting.Slot_AllyLeft);
            hope1.Effects[1] = Effects.GenerateEffect(determined, 2, Targeting.Slot_AllyLeft);
            hope1.Effects[2] = Effects.GenerateEffect(determined, 2, Slots.Self);
            hope1.AddIntentsToTarget(Targeting.Slot_AllyLeft, ["Heal_5_10", Determined.Intent]);
            hope1.AddIntentsToTarget(Slots.Self, [Determined.Intent]);
            hope1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Mend_1_A").visuals;
            hope1.AnimationTarget = Targeting.Slot_AllyLeft;

            Ability hope2 = new Ability(hope1.ability, "Kafka_Hope_2_A", hope1.Cost);
            hope2.Name = "Tiny Hope";
            hope2.Description = "Heal the Left ally 9 health and apply 3 Determined to them.\nApply 2 Determined to this party member.";
            hope2.Effects[0].entryVariable = 9;
            hope2.Effects[1].entryVariable = 3;

            Ability hope3 = new Ability(hope2.ability, "Kafka_Hope_3_A", hope1.Cost);
            hope3.Name = "Small Hope";
            hope3.Description = "Heal the Left ally 10 health and apply 3 Determined to them.\nApply 3 Determined to this party member.";
            hope3.Effects[0].entryVariable = 10;
            hope3.Effects[2].entryVariable = 3;

            Ability hope4 = new Ability(hope3.ability, "Kafka_Hope_4_A", hope1.Cost);
            hope4.Name = "Lesser Hope";
            hope4.Description = "Heal the Left ally 12 health and apply 4 Determined to them.\nApply 3 Determined to this party member.";
            hope4.Effects[0].entryVariable = 12;
            hope4.Effects[1].entryVariable = 4;
            hope4.EffectIntents[0].intents[0] = "Heal_11_20";

            Ability desperate1 = new Ability("Leaking Desperation", "Kafka_Desperation_1_A");
            desperate1.Description = "Heal the Left and Right allies 3 health.\nIf neither was healed, apply 4 Determined to both.";
            desperate1.AbilitySprite = ResourceLoader.LoadSprite("ability_desperation.png");
            desperate1.Cost = [Pigments.Yellow, Pigments.Blue, Pigments.Blue];
            desperate1.Effects = new EffectInfo[2];
            desperate1.Effects[0] = Effects.GenerateEffect(heal, 3, Slots.Sides);
            desperate1.Effects[1] = Effects.GenerateEffect(determined, 4, Slots.Sides, BasicEffects.DidThat(false));
            desperate1.AddIntentsToTarget(Slots.Sides, ["Heal_1_4", Determined.Intent]);
            desperate1.Visuals = CustomVisuals.GetVisuals("Salt/Shush");
            desperate1.AnimationTarget = Slots.Sides;

            Ability desperate2 = new Ability(desperate1.ability, "Kafka_Desperation_2_A", desperate1.Cost);
            desperate2.Name = "Flowing Desperation";
            desperate2.Description = "Heal the Left and Right allies 5 health.\nIf neither was healed, apply 5 Determined to both.";
            desperate2.Effects[0].entryVariable = 5;
            desperate2.Effects[1].entryVariable = 5;
            desperate2.EffectIntents[0].intents[0] = "Heal_5_10";

            Ability desperate3 = new Ability(desperate2.ability, "Kafka_Desperation_3_A", desperate1.Cost);
            desperate3.Name = "Streaming Desperation";
            desperate3.Description = "Heal the Left and Right allies 7 health.\nIf neither was healed, apply 5 Determined to both.";
            desperate3.Effects[0].entryVariable = 7;

            Ability desperate4 = new Ability(desperate3.ability, "Kafka_Desperation_4_A", desperate1.Cost);
            desperate4.Name = "Down-Pouring Desperation";
            desperate4.Description = "Heal the Left and Right allies 9 health.\nIf neither was healed, apply 6 Determined to both.";
            desperate4.Effects[0].entryVariable = 9;
            desperate4.Effects[1].entryVariable = 6;

            Ability envy1 = new Ability("Mild Envy", "Kafka_Envy_1_A");
            envy1.Description = "Apply 4 Determined to the Right ally.\nHeal this party member 1 health.";
            envy1.AbilitySprite = ResourceLoader.LoadSprite("ability_envy.png");
            envy1.Cost = [Pigments.Blue];
            envy1.Effects = new EffectInfo[2];
            envy1.Effects[0] = Effects.GenerateEffect(determined, 4, Targeting.Slot_AllyRight);
            envy1.Effects[1] = Effects.GenerateEffect(heal, 1, Slots.Self);
            envy1.AddIntentsToTarget(Targeting.Slot_AllyRight, [Determined.Intent]);
            envy1.AddIntentsToTarget(Slots.Self, ["Heal_1_4"]);
            envy1.Visuals = CustomVisuals.GetVisuals("Salt/Call");
            envy1.AnimationTarget = Targeting.Slot_AllyRight;

            Ability envy2 = new Ability(envy1.ability, "Kafka_Envy_2_A", envy1.Cost);
            envy2.Name = "Festering Envy";
            envy2.Description = "Apply 5 Determined to the Right ally.\nHeal this party member 1 health.";
            envy2.Effects[0].entryVariable = 5;

            Ability envy3 = new Ability(envy2.ability, "Kafka_Envy_3_A", envy1.Cost);
            envy3.Name = "Furious Envy";
            envy3.Description = "Apply 6 Determined to the Right ally.\nHeal this party member 2 health.";
            envy3.Effects[0].entryVariable = 6;
            envy3.Effects[1].entryVariable = 2;

            Ability envy4 = new Ability(envy3.ability, "Kafka_Envy_4_A", envy1.Cost);
            envy4.Name = "Maddening Envy";
            envy4.Description = "Apply 8 Determined to the Right ally.\nHeal this party member 2 health.";
            envy4.Effects[0].entryVariable = 8;
            envy4.Effects[1].entryVariable = 2;

            kafka.AddLevelData(7, [envy1, hope1, desperate1]);
            kafka.AddLevelData(8, [envy2, hope2, desperate2]);
            kafka.AddLevelData(9, [envy3, hope3, desperate3]);
            kafka.AddLevelData(10, [envy4, hope4, desperate4]);
            kafka.AddCharacter(true);
        }
        public static void Items()
        {
            PerformEffect_Item handhelddiorama = new PerformEffect_Item("Aprils_HandHeldDiorama_TW", []);
            handhelddiorama.Name = "Hand-Held Diorama";
            handhelddiorama.Flavour = "\"The world for your pleasure\"";
            handhelddiorama.Description = "On using an ability, change the ability's Pigment costs to the Pigments used.";
            handhelddiorama.Icon = ResourceLoader.LoadSprite("item_handhelddiorama.png");
            handhelddiorama.EquippedModifiers = [];
            handhelddiorama.TriggerOn = TriggerCalls.OnAbilityWillBeUsed;
            handhelddiorama.DoesPopUpInfo = false;
            handhelddiorama.Conditions = [ScriptableObject.CreateInstance<HandHeldDioramaCondition>()];
            handhelddiorama.DoesActionOnTriggerAttached = false;
            handhelddiorama.ConsumeOnTrigger = TriggerCalls.Count;
            handhelddiorama.ConsumeOnUse = false;
            handhelddiorama.ConsumeConditions = [];
            handhelddiorama.ShopPrice = 6;
            handhelddiorama.IsShopItem = false;
            handhelddiorama.StartsLocked = true;
            handhelddiorama.OnUnlockUsesTHE = true;
            handhelddiorama.UsesSpecialUnlockText = false;
            handhelddiorama.SpecialUnlockID = UILocID.None;
            handhelddiorama.item._ItemTypeIDs = ["Magic", "Heart"];
            handhelddiorama.Item.AddItem("locked_handhelddiorama.png", OsmanACH);

            PerformEffect_Item page11 = new PerformEffect_Item("Aprils_Page11_TW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), 5, Targeting.Unit_AllAllies), Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyCursedEffect>(), 1, Targeting.AllUnits)]);
            page11.Name = "Page 11";
            page11.Flavour = "\"#!&?%8#\"";
            page11.Description = "At the start of combat, heal all party members 5 health then Curse all enemies and party members.";
            page11.Icon = ResourceLoader.LoadSprite("item_page11.png");
            page11.EquippedModifiers = [];
            page11.TriggerOn = TriggerCalls.OnCombatStart;
            page11.DoesPopUpInfo = true;
            page11.Conditions = [];
            page11.DoesActionOnTriggerAttached = false;
            page11.ConsumeOnTrigger = TriggerCalls.Count;
            page11.ConsumeOnUse = false;
            page11.ConsumeConditions = [];
            page11.ShopPrice = 5;
            page11.IsShopItem = false;
            page11.StartsLocked = true;
            page11.OnUnlockUsesTHE = false;
            page11.UsesSpecialUnlockText = false;
            page11.SpecialUnlockID = UILocID.None;
            page11.item._ItemTypeIDs = ["Magic"];
            page11.item.AddItem("locked_page11.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Kafka", "Page 11", "Hand-Held Diorama", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Kafka_CH", "Aprils_Page11_TW", "Aprils_HandHeldDiorama_TW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Kafka_Heaven_ACH";
        public static string OsmanACH => "Aprils_Kafka_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Kafka_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Kafka_Osman_Unlock";
    }
}
