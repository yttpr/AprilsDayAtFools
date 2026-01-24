using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Saline
    {
        public static void Add()
        {
            PerformEffectPassiveAbility scary = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            scary._passiveName = "Scary";
            scary.passiveIcon = ResourceLoader.LoadSprite("ScaryPassive.png");
            scary.m_PassiveID = "Scary_PA";
            scary._enemyDescription = "On being directly damaged, Curse the Opposing party member.";
            scary._characterDescription = "On being directly damaged, Curse the Opposing enemy";
            scary.doesPassiveTriggerInformationPanel = true;
            scary.effects = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyCursedEffect>(), 1, Slots.Front).SelfArray();
            scary._triggerOn = new TriggerCalls[1] { TriggerCalls.OnDirectDamaged };
            scary.AddPassiveToGlossary("Scary", "On being directly damaged, Curse the Opposing unit.");
            scary.AddToPassiveDatabase();

            Character saline = new Character("Saline", "Saline_CH");
            saline.HealthColor = Pigments.Purple;
            saline.AddUnitType("FemaleID");
            saline.AddUnitType("Sandwich_Gore");
            saline.AddUnitType("FemaleLooking");
            saline.UsesBasicAbility = true;
            //slap
            saline.UsesAllAbilities = false;
            saline.MovesOnOverworld = true;
            //animator
            saline.FrontSprite = ResourceLoader.LoadSprite("SalineFront.png");
            saline.BackSprite = ResourceLoader.LoadSprite("SalineBack.png");
            saline.OverworldSprite = ResourceLoader.LoadSprite("SalineWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            saline.DamageSound = "event:/Lunacy/SOUNDS/FreudHit";
            saline.DeathSound = "event:/Lunacy/SOUNDS/FreudDie";
            saline.DialogueSound = "event:/Lunacy/SOUNDS/FreudHit";
            saline.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            saline.AddFinalBossAchievementData("Heaven", HeavenACH);
            saline.GenerateMenuCharacter(ResourceLoader.LoadSprite("SalineMenu.png"), ResourceLoader.LoadSprite("SalineLock.png"));
            saline.MenuCharacterIsSecret = false;
            saline.MenuCharacterIgnoreRandom = false;
            saline.SetMenuCharacterAsFullDPS();
            saline.AddPassive(scary);

            Ability nightmare1 = new Ability("Starving Nightmare", "Saline_Nightmare_1_A");
            nightmare1.Description = "Heal the Opposing enemy 6 health and Curse them.\nIf no healing was dealt, inflict 1 Frail on them.";
            nightmare1.AbilitySprite = ResourceLoader.LoadSprite("ability_nightmare.png");
            nightmare1.Cost = [Pigments.RedBlue];
            nightmare1.Effects = new EffectInfo[3];
            nightmare1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), 6, Slots.Front);
            nightmare1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyCursedEffect>(), 1, Slots.Front);
            nightmare1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyFrailEffect>(), 1, Slots.Front, BasicEffects.DidThat(false, 2));
            nightmare1.AddIntentsToTarget(Slots.Front, ["Heal_5_10", "Status_Cursed", "Status_Frail"]);
            nightmare1.Visuals = CustomVisuals.GetVisuals("Salt/Monster");
            nightmare1.AnimationTarget = Slots.Front;

            Ability nightmare2 = new Ability(nightmare1.ability, "Saline_Nightmare_2_A", nightmare1.Cost);
            nightmare2.Name = "Feasting Nightmare";
            nightmare2.Description = "Heal the Opposing enemy 3 health and Curse them.\nIf no healing was dealt, inflict 1 Frail on them.";
            nightmare2.Effects[0].entryVariable = 3;
            nightmare1.EffectIntents[0].intents[0] = "Heal_1_4";

            Ability nightmare3 = new Ability(nightmare2.ability, "Saline_Nightmare_3_A", [Pigments.Grey]);
            nightmare3.Name = "Devouring Nightmare";
            nightmare3.Description = "Heal the Opposing enemy 3 health and Curse them.\nIf no healing was dealt, inflict 1 Frail on them.";

            Ability nightmare4 = new Ability(nightmare3.ability, "Saline_Nightmare_4_A", nightmare3.Cost);
            nightmare4.Name = "Bloated Nightmare";
            nightmare4.Description = "Heal the Opposing enemy 1 health and Curse them.\nIf no healing was dealt, inflict 1 Frail on them.";
            nightmare4.Effects[0].entryVariable = 1;

            DamageEffect returnKill = ScriptableObject.CreateInstance<DamageEffect>();
            returnKill._returnKillAsSuccess = true;
            Ability nails1 = new Ability("Nails in the Feet", "Saline_Nails_1_A");
            nails1.Description = "Deal 10 damage to the Opposing enemy and heal them 6 health.\nIf this ability would have killed, Curse the Opposing enemy.";
            nails1.AbilitySprite = ResourceLoader.LoadSprite("ability_nailing.png");
            nails1.Cost = [Pigments.Yellow, Pigments.Yellow, Pigments.Red];
            nails1.Effects = new EffectInfo[5];
            nails1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<NailingSetupEffect>(), 1, Slots.Front);
            nails1.Effects[1] = Effects.GenerateEffect(returnKill, 10, Slots.Front);
            nails1.Effects[2] = Effects.GenerateEffect(nightmare1.Effects[0].effect, 6, Slots.Front);
            nails1.Effects[3] = Effects.GenerateEffect(ScriptableObject.CreateInstance<NailingTriggerEffect>(), 1, Slots.Front);
            nails1.Effects[4] = Effects.GenerateEffect(nightmare1.Effects[1].effect, 1, Slots.Front, BasicEffects.DidThat(true, 3));
            nails1.AddIntentsToTarget(Slots.Front, ["Damage_7_10", "Heal_5_10", "Status_Cursed"]);
            nails1.Visuals = CustomVisuals.GetVisuals("Salt/Nailing");
            nails1.AnimationTarget = Slots.Front;

            Ability nails2 = new Ability(nails1.ability, "Saline_Nails_2_A", nails1.Cost);
            nails2.Name = "Nails in the Hands";
            nails2.Description = "Deal 14 damage to the Opposing enemy and heal them 9 health.\nIf this ability would have killed, Curse the Opposing enemy.";
            nails2.Effects[1].entryVariable = 14;
            nails2.Effects[2].entryVariable = 9;
            nails2.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability nails3 = new Ability(nails2.ability, "Saline_Nails_3_A", nails1.Cost);
            nails3.Name = "Nails in the Eyes";
            nails3.Description = "Deal 18 damage to the Opposing enemy and heal them 12 health.\nIf this ability would have killed, Curse the Opposing enemy.";
            nails3.Effects[1].entryVariable = 18;
            nails3.Effects[2].entryVariable = 12;
            nails3.EffectIntents[0].intents[0] = "Damage_16_20";
            nails3.EffectIntents[0].intents[1] = "Heal_11_20";

            Ability nails4 = new Ability(nails3.ability, "Saline_Nails_4_A", nails1.Cost);
            nails4.Name = "Nails in the Mind";
            nails4.Description = "Deal 22 damage to the Opposing enemy and heal them 15 health.\nIf this ability would have killed, Curse the Opposing enemy.";
            nails4.Effects[1].entryVariable = 22;
            nails4.Effects[2].entryVariable = 15;
            nails4.EffectIntents[0].intents[0] = "Damage_21";

            Ability agony1 = new Ability("Agony of Flesh", "Saline_Agony_1_A");
            agony1.Description = "Heal the Opposing enemy 3 health.\nDeal 5 damage to them, dealing 50% more if they are above half health.";
            agony1.AbilitySprite = ResourceLoader.LoadSprite("ability_agony.png");
            agony1.Cost = [Pigments.Blue, Pigments.RedBlue];
            agony1.Effects = new EffectInfo[2];
            agony1.Effects[0] = Effects.GenerateEffect(nightmare1.Effects[0].effect, 3, Slots.Front);
            agony1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageIncreaseIfTargetAboveHalfEffect>(), 5, Slots.Front);
            agony1.AddIntentsToTarget(Slots.Front, ["Heal_1_4", "Damage_3_6"]);
            agony1.Visuals = CustomVisuals.GetVisuals("Salt/Needle");
            agony1.AnimationTarget = Slots.Front;

            Ability agony2 = new Ability(agony1.ability, "Saline_Agony_2_A", agony1.Cost);
            agony2.Name = "Agony of Material";
            agony2.Description = "Heal the Opposing enemy 4 health.\nDeal 7 damage to them, dealing 50% more if they are above half health.";
            agony2.Effects[0].entryVariable = 4;
            agony2.Effects[1].entryVariable = 7;
            agony2.EffectIntents[0].intents[1] = "Damage_7_10";

            Ability agony3 = new Ability(agony2.ability, "Saline_Agony_3_A", agony1.Cost);
            agony3.Name = "Agony of Fantasy";
            agony3.Description = "Heal the Opposing enemy 5 health.\nDeal 9 damage to them, dealing 50% more if they are above half health.";
            agony3.Effects[0].entryVariable = 5;
            agony3.Effects[1].entryVariable = 9;
            agony3.EffectIntents[0].intents[0] = "Heal_5_10";

            Ability agony4 = new Ability(agony3.ability, "Saline_Agony_4_A", agony1.Cost);
            agony4.Name = "Agony of Death";
            agony4.Description = "Heal the Opposing enemy 6 health.\nDeal 11 damage to them, dealing 50% more if they are above half health.";
            agony4.Effects[0].entryVariable = 6;
            agony4.Effects[1].entryVariable = 11;
            agony4.EffectIntents[0].intents[1] = "Damage_11_15";

            saline.AddLevelData(20, [agony1, nightmare1, nails1]);
            saline.AddLevelData(21, [agony2, nightmare2, nails2]);
            saline.AddLevelData(22, [agony3, nightmare3, nails3]);
            saline.AddLevelData(25, [agony4, nightmare4, nails4]);
            saline.AddCharacter(true);
        }
        public static void Items()
        {
            PerformEffect_Item schoolshooter = new PerformEffect_Item("Aprils_SchoolShooter_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 7, Targetting.Random(false))]);
            schoolshooter.Name = "School Shooter";
            schoolshooter.Flavour = "\"You're projecting\"";
            schoolshooter.Description = "On getting a kill, deal 7 damage to a random enemy.";
            schoolshooter.Icon = ResourceLoader.LoadSprite("item_schoolshooter.png");
            schoolshooter.EquippedModifiers = [];
            schoolshooter.TriggerOn = TriggerCalls.OnKill;
            schoolshooter.DoesPopUpInfo = true;
            schoolshooter.Conditions = [];
            schoolshooter.DoesActionOnTriggerAttached = false;
            schoolshooter.ConsumeOnTrigger = TriggerCalls.Count;
            schoolshooter.ConsumeOnUse = false;
            schoolshooter.ConsumeConditions = [];
            schoolshooter.ShopPrice = 6;
            schoolshooter.IsShopItem = true;
            schoolshooter.StartsLocked = true;
            schoolshooter.OnUnlockUsesTHE = true;
            schoolshooter.UsesSpecialUnlockText = false;
            schoolshooter.SpecialUnlockID = UILocID.None;
            schoolshooter.Item.AddItem("locked_schoolshooter.png", OsmanACH);

            RandomStatusEffect negative = ScriptableObject.CreateInstance<RandomStatusEffect>();
            negative.CanApply = ["Cursed_ID", "Frail_ID", "Ruptured_ID", "Gutted_ID", "OilSlicked_ID", "Scars_ID", "Remorse_ID", "Salted_ID", "Paranoia_ID", "Left_ID", "Pale_ID", "DivineSacrifice_ID", "Muted_ID", "Salt_Entropy_ID", "Acid_ID", "Terror_ID", "Drowning_ID", "Pimples_ID"];
            PerformEffect_Item polygonpipebomb = new PerformEffect_Item("Aprils_PolygonPipeBomb_TW", [Effects.GenerateEffect(negative, 2, Targeting.Unit_AllOpponents)]);
            polygonpipebomb.Name = "Polygon Pipe-Bomb";
            polygonpipebomb.Flavour = "\"In a videogame, in a videogame.\"";
            polygonpipebomb.Description = "On being directly damaged, inflict 2 random negative Status Effects on all enemies.";
            polygonpipebomb.Icon = ResourceLoader.LoadSprite("item_polygonpipebomb.png");
            polygonpipebomb.EquippedModifiers = [];
            polygonpipebomb.TriggerOn = TriggerCalls.OnDirectDamaged;
            polygonpipebomb.DoesPopUpInfo = true;
            polygonpipebomb.Conditions = [];
            polygonpipebomb.DoesActionOnTriggerAttached = false;
            polygonpipebomb.ConsumeOnTrigger = TriggerCalls.Count;
            polygonpipebomb.ConsumeOnUse = false;
            polygonpipebomb.ConsumeConditions = [];
            polygonpipebomb.ShopPrice = 5;
            polygonpipebomb.IsShopItem = false;
            polygonpipebomb.StartsLocked = true;
            polygonpipebomb.OnUnlockUsesTHE = true;
            polygonpipebomb.UsesSpecialUnlockText = false;
            polygonpipebomb.SpecialUnlockID = UILocID.None;
            polygonpipebomb.item.AddItem("locked_polygonpipebomb.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Saline", "Polygon Pipe-Bomb", "School Shooter", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Saline_CH", "Aprils_PolygonPipeBomb_TW", "Aprils_SchoolShooter_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Saline_Heaven_ACH";
        public static string OsmanACH => "Aprils_Saline_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Saline_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Saline_Osman_Unlock";
    }
}
