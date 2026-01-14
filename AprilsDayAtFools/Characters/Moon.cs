using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Playables;

namespace AprilsDayAtFools
{
    public static class Moon
    {
        public static void Add()
        {
            ExtraCCSprites_BasicSO moonExtra = ScriptableObject.CreateInstance<ExtraCCSprites_BasicSO>();
            moonExtra._DefaultID = "";
            moonExtra._frontSprite = ResourceLoader.LoadSprite("MoonAlt.png");
            moonExtra._SpecialID = IDs.Moon;
            moonExtra._backSprite = ResourceLoader.LoadSprite("MoonBack.png");

            Character moon = new Character("Moon", "Moon_CH");
            moon.HealthColor = Pigments.Purple;
            moon.AddUnitType("FemaleID");
            moon.AddUnitType("Sandwich_Fire");
            moon.UsesBasicAbility = true;
            //slap
            moon.UsesAllAbilities = false;
            moon.MovesOnOverworld = true;
            //animator
            moon.FrontSprite = ResourceLoader.LoadSprite("MoonFront.png");
            moon.BackSprite = ResourceLoader.LoadSprite("MoonBack.png");
            moon.OverworldSprite = ResourceLoader.LoadSprite("MoonWorld.png", new Vector2(0.5f, 0f));
            moon.ExtraSprites = moonExtra;
            moon.DamageSound = "event:/Combat/StatusEffects/SE_Fire_Apl";
            moon.DeathSound = LoadedAssetsHandler.GetCharacter("Dimitri_CH").deathSound;
            moon.DialogueSound = "event:/Combat/StatusEffects/SE_Fire_Apl";
            moon.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            moon.AddFinalBossAchievementData("Heaven", HeavenACH);
            moon.GenerateMenuCharacter(ResourceLoader.LoadSprite("MoonMenu.png"), ResourceLoader.LoadSprite("MoonLock.png"));
            moon.MenuCharacterIsSecret = false;
            moon.MenuCharacterIgnoreRandom = false;
            moon.SetMenuCharacterAsFullDPS();

            PerformEffectPassiveAbility solitude = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            solitude._passiveName = "Solitude";
            solitude.passiveIcon = ResourceLoader.LoadSprite("SolitudePassive.png");
            solitude.m_PassiveID = IDs.Solitude;
            solitude._enemyDescription = "Fire does not decrease on this enemy's position.\nThis enemy is immune to Fire damage.";
            solitude._characterDescription = "Fire does not decrease on this party member's position.\nThis party member is immune to Fire damage.";
            solitude._triggerOn = [TriggerCalls.OnBeingDamaged];
            solitude.conditions = [ScriptableObject.CreateInstance<SolitudeCondition>()];
            solitude.effects = [];
            solitude.AddToPassiveDatabase();
            solitude.AddPassiveToGlossary("Solitude", "Fire does not decrease on this unit's position.\nThis unit is immune to Fire damage.");
            moon.AddPassive(solitude);

            FireDamageByStoredValueEffect isolDmg = ScriptableObject.CreateInstance<FireDamageByStoredValueEffect>();
            isolDmg._increaseDamage = false;
            isolDmg.m_unitStoredDataID = IDs.Isolation;
            CasterStoredValueChangeEffect isolDown = ScriptableObject.CreateInstance<CasterStoredValueChangeEffect>();
            isolDown._increase = true;
            isolDown.m_unitStoredDataID = IDs.Isolation;
            CasterCapValueEffect isolCap = ScriptableObject.CreateInstance<CasterCapValueEffect>();
            isolCap.value = IDs.Isolation;
            CasterStoreValueSetterEffect isolSet = ScriptableObject.CreateInstance<CasterStoreValueSetterEffect>();
            isolSet.m_unitStoredDataID = IDs.Isolation;

            FireDamageByStoredValueEffect wallowDmg = ScriptableObject.CreateInstance<FireDamageByStoredValueEffect>();
            wallowDmg._increaseDamage = false;
            wallowDmg.m_unitStoredDataID = IDs.Wallowing;
            CasterStoredValueChangeEffect wallowDown = ScriptableObject.CreateInstance<CasterStoredValueChangeEffect>();
            wallowDown._increase = true;
            wallowDown.m_unitStoredDataID = IDs.Wallowing;
            CasterCapValueEffect wallowCap = ScriptableObject.CreateInstance<CasterCapValueEffect>();
            wallowCap.value = IDs.Wallowing;
            CasterStoreValueSetterEffect wallowSet = ScriptableObject.CreateInstance<CasterStoreValueSetterEffect>();
            wallowSet.m_unitStoredDataID = IDs.Wallowing;

            CasterStoreValueSetterEffect lighterEffect = ScriptableObject.CreateInstance<CasterStoreValueSetterEffect>();
            lighterEffect.m_unitStoredDataID = IDs.Lighter;

            Ability isol1 = new Ability("Lingering Isolation", "Moon_Isolation_1_A");
            isol1.Description = "Deal 10 direct Fire damage to the Opposing position and decrease this move's damage by 2, resetting if it would reach 0.\nThis move's damage decreases by 2 on turn end and on taking any damage.\nApply 1 Fire to this party member.";
            isol1.AbilitySprite = ResourceLoader.LoadSprite("ability_isolation.png");
            isol1.Cost = [Pigments.Red, Pigments.Yellow, Pigments.Yellow];
            isol1.Effects = new EffectInfo[5];
            isol1.Effects[0] = Effects.GenerateEffect(isolDmg, 10, Slots.Front);
            isol1.Effects[1] = Effects.GenerateEffect(isolDown, 2, null, ScriptableObject.CreateInstance<MoonLighterCondition>());
            isol1.Effects[2] = Effects.GenerateEffect(isolCap, 10);
            isol1.Effects[3] = Effects.GenerateEffect(isolSet, 0, null, BasicEffects.DidThat(true));
            isol1.Effects[4] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyFireSlotEffect>(), 1, Slots.Self);
            isol1.AddIntentsToTarget(Slots.Front, ["Damage_7_10"]);
            isol1.AddIntentsToTarget(Slots.Self, ["Misc", "Field_Fire"]);
            isol1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Sear_1_A").visuals;
            isol1.AnimationTarget = Slots.Front;
            isol1.UnitStoreData = UnitStoreData.GetCustom_UnitStoreData(IDs.Isolation);

            Ability isol2 = new Ability(isol1.ability, "Moon_Isolation_2_A", isol1.Cost);
            isol2.Name = "Embering Isolation";
            isol2.Description = "Deal 13 direct Fire damage to the Opposing position and decrease this move's damage by 3, resetting if it would reach 0.\nThis move's damage decreases by 2 on turn end and on taking any damage.\nApply 1 Fire to this party member.";
            isol2.Effects[0].entryVariable = 13;
            isol2.Effects[1].entryVariable = 3;
            isol2.Effects[2].entryVariable = 13;
            isol2.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability isol3 = new Ability(isol2.ability, "Moon_Isolation_3_A", isol1.Cost);
            isol3.Name = "Burning Isolation";
            isol3.Description = "Deal 16 direct Fire damage to the Opposing position and decrease this move's damage by 4, resetting if it would reach 0.\nThis move's damage decreases by 2 on turn end and on taking any damage.\nApply 1 Fire to this party member.";
            isol3.Effects[0].entryVariable = 16;
            isol3.Effects[1].entryVariable = 4;
            isol3.Effects[2].entryVariable = 16;
            isol3.EffectIntents[0].intents[0] = "Damage_16_20";

            Ability isol4 = new Ability(isol3.ability, "Moon_Isolation_4_A", isol1.Cost);
            isol4.Name = "Scorched Isolation";
            isol4.Description = "Deal 20 direct Fire damage to the Opposing position and decrease this move's damage by 5, resetting if it would reach 0.\nThis move's damage decreases by 2 on turn end and on taking any damage.\nApply 1 Fire to this party member.";
            isol4.Effects[0].entryVariable = 20;
            isol3.Effects[1].entryVariable = 5;
            isol4.Effects[2].entryVariable = 20;

            Ability light1 = new Ability("Little Lighter", "Moon_Light_1_A");
            light1.Description = "Apply 1 Fire to this party member and 2 Fire to the Opposing enemy.\nRestore the damage of \"Isolation\" and \"Wallowing\" to their bases; their damages won't decrease at the end of this turn.";
            light1.AbilitySprite = ResourceLoader.LoadSprite("ability_lighter.png");
            light1.Cost = [Pigments.Red, Pigments.Yellow];
            light1.Effects = new EffectInfo[5];
            light1.Effects[0] = Effects.GenerateEffect(isol1.Effects[4].effect, 1, Slots.Self);
            light1.Effects[1] = Effects.GenerateEffect(isol1.Effects[4].effect, 2, Slots.Front);
            light1.Effects[2] = Effects.GenerateEffect(isolSet, 0);
            light1.Effects[3] = Effects.GenerateEffect(wallowSet, 0);
            light1.Effects[4] = Effects.GenerateEffect(lighterEffect, 1);
            light1.AddIntentsToTarget(Slots.Front, ["Field_Fire"]);
            light1.AddIntentsToTarget(Slots.Self, ["Field_Fire", "Misc"]);
            light1.Visuals = LoadedAssetsHandler.GetCharacterAbility("WholeAgain_1_A").visuals;
            light1.AnimationTarget = Slots.Self;
            light1.UnitStoreData = UnitStoreData.GetCustom_UnitStoreData(IDs.Lighter);

            Ability light2 = new Ability(light1.ability, "Moon_Light_2_A", light1.Cost);
            light2.Name = "Loose Lighter";
            light2.Description = "Apply 1 Fire to this party member and 3 Fire to the Opposing enemy.\nRestore the damage of \"Isolation\" and \"Wallowing\" to their bases; their damages won't decrease at the end of this turn.";
            light2.Effects[1].entryVariable = 3;

            Ability light3 = new Ability(light2.ability, "Moon_Light_3_A", light1.Cost);
            light3.Name = "Risky Lighter";
            light3.Description = "Apply 1 Fire to this party member and 3 Fire to the Opposing enemy.\nRestore the damage of \"Isolation\" and \"Wallowing\" to their bases; their damages won't decrease at the end of this turn.";
            light3.Effects[1].entryVariable = 3;

            Ability light4 = new Ability(light3.ability, "Moon_Light_4_A", light1.Cost);
            light4.Name = "Reckless Lighter";
            light4.Description = "Apply 1 Fire to this party member and 4 Fire to the Opposing enemy.\nRestore the damage of \"Isolation\" and \"Wallowing\" to their bases; their damages won't decrease at the end of this turn.";
            light4.Effects[1].entryVariable = 4;

            DoubleTargetting selfFront = ScriptableObject.CreateInstance<DoubleTargetting>();
            selfFront.firstTargetting = Slots.Front;
            selfFront.secondTargetting = Slots.Self;
            Ability wallow1 = new Ability("Wallowing in Tinders", "Moon_Wallow_1_A");
            wallow1.Description = "Inflict 2 Fire to the Opposing enemy.\nDeal 8 direct Fire damage to all enemies in Fire and decrease this move's damage by 1, resetting if it would reach 0.\nThis move's damage decreases by 1 on turn end and on taking any damage.";
            wallow1.AbilitySprite = ResourceLoader.LoadSprite("ability_wallowing.png");
            wallow1.Cost = [Pigments.Red, Pigments.Red, Pigments.Red];
            wallow1.Effects = new EffectInfo[5];
            wallow1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyFireSlotEffect>(), 2, Slots.Front);
            wallow1.Effects[1] = Effects.GenerateEffect(wallowDmg, 8, TargettingUnitsWithFieldEffectSide.Create("OnFire_ID", false));
            wallow1.Effects[2] = Effects.GenerateEffect(wallowDown, 1, null, ScriptableObject.CreateInstance<MoonLighterCondition>());
            wallow1.Effects[3] = Effects.GenerateEffect(wallowCap, 8);
            wallow1.Effects[4] = Effects.GenerateEffect(wallowSet, 0, null, BasicEffects.DidThat(true));
            wallow1.AddIntentsToTarget(Slots.Front, ["Field_Fire"]);
            wallow1.AddIntentsToTarget(Slots.Self, ["Field_Fire", "Misc"]);
            wallow1.AddIntentsToTarget(wallow1.Effects[1].targets, ["Damage_7_10"]);
            wallow1.AddIntentsToTarget(TargettingTargettingWithoutFieldEffect.Create("OnFire_ID", Slots.Front), ["Damage_7_10"]);
            DoubleTargetting wallowAnim = ScriptableObject.CreateInstance<DoubleTargetting>();
            wallowAnim.firstTargetting = wallow1.Effects[1].targets;
            wallowAnim.secondTargetting = Slots.Front;
            wallow1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Sear_1_A").visuals;
            wallow1.AnimationTarget = wallowAnim;
            wallow1.UnitStoreData = UnitStoreData.GetCustom_UnitStoreData(IDs.Wallowing);

            Ability wallow2 = new Ability(wallow1.ability, "Moon_Wallow_2_A", wallow1.Cost);
            wallow2.Name = "Wallowing in Flames";
            wallow2.Description = "Inflict 2 Fire to the Opposing enemy.\nDeal 10 direct Fire damage to all enemies in Fire and decrease this move's damage by 2, resetting if it would reach 0.\nThis move's damage decreases by 1 on turn end and on taking any damage.";
            wallow2.Effects[1].entryVariable = 10;
            wallow2.Effects[2].entryVariable = 2;
            wallow2.Effects[3].entryVariable = 10;

            Ability wallow3 = new Ability(wallow2.ability, "Moon_Wallow_3_A", wallow1.Cost);
            wallow3.Name = "Wallowing in Wildfires";
            wallow3.Description = "Inflict 2 Fire to the Opposing enemy.\nDeal 12 direct Fire damage to all enemies in Fire and decrease this move's damage by 3, resetting if it would reach 0.\nThis move's damage decreases by 1 on turn end and on taking any damage.";
            wallow3.Effects[1].entryVariable = 12;
            wallow3.Effects[2].entryVariable = 3;
            wallow3.Effects[3].entryVariable = 12;
            wallow3.EffectIntents[2].intents[0] = "Damage_11_15";
            wallow3.EffectIntents[3].intents[0] = "Damage_11_15";

            Ability wallow4 = new Ability(wallow3.ability, "Moon_Wallow_4_A", wallow1.Cost);
            wallow4.Name = "Wallowing in Firebombings";
            wallow4.Description = "Inflict 2 Fire to the Opposing enemy.\nDeal 14 direct Fire damage to all enemies in Fire and decrease this move's damage by 4, resetting if it would reach 0.\nThis move's damage decreases by 1 on turn end and on taking any damage.";
            wallow4.Effects[1].entryVariable = 14;
            wallow4.Effects[2].entryVariable = 4;
            wallow4.Effects[3].entryVariable = 14;

            moon.AddLevelData(11, [wallow1, isol1, light1]);
            moon.AddLevelData(14, [wallow2, isol2, light2]);
            moon.AddLevelData(16, [wallow3, isol3, light3]);
            moon.AddLevelData(17, [wallow4, isol4, light4]);
            moon.AddCharacter(true);
        }
        public static void Items()
        {
            PerformEffect_Item comfortlighter = new PerformEffect_Item("Aprils_ComfortLighter_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyFireSlotEffect>(), 1, Slots.Front)]);
            comfortlighter.Name = "Comfort Lighter";
            comfortlighter.Flavour = "\"Self-destructive coping mechanisms\"";
            comfortlighter.Description = "Whenever an enemy moves in front of this party member, inflict 1 Fire on the Opposing position.";
            comfortlighter.Icon = ResourceLoader.LoadSprite("item_comfortlighter.png");
            comfortlighter.EquippedModifiers = [];
            comfortlighter.TriggerOn = AmbushManager.Trigger;
            comfortlighter.DoesPopUpInfo = true;
            comfortlighter.Conditions = [];
            comfortlighter.DoesActionOnTriggerAttached = false;
            comfortlighter.ConsumeOnTrigger = TriggerCalls.Count;
            comfortlighter.ConsumeOnUse = false;
            comfortlighter.ConsumeConditions = [];
            comfortlighter.ShopPrice = 4;
            comfortlighter.IsShopItem = true;
            comfortlighter.StartsLocked = true;
            comfortlighter.OnUnlockUsesTHE = true;
            comfortlighter.UsesSpecialUnlockText = false;
            comfortlighter.SpecialUnlockID = UILocID.None;
            comfortlighter.Item.AddItem("locked_comfortlighter.png", OsmanACH);

            PerformEffect_Item weightedscales = new PerformEffect_Item("Aprils_WeightedScales_TW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyPaleEffect>(), 15, Slots.Front)]);
            weightedscales.Name = "Weighted Scales";
            weightedscales.Flavour = "\"Always in our favor.\"";
            weightedscales.Description = "Whenever an enemy moves in front of this party member, inflict 15 Pale on the Opposing enemy.";
            weightedscales.Icon = ResourceLoader.LoadSprite("item_weightedscales.png");
            weightedscales.EquippedModifiers = [];
            weightedscales.TriggerOn = AmbushManager.Trigger;
            weightedscales.DoesPopUpInfo = true;
            weightedscales.Conditions = [];
            weightedscales.DoesActionOnTriggerAttached = false;
            weightedscales.ConsumeOnTrigger = TriggerCalls.Count;
            weightedscales.ConsumeOnUse = false;
            weightedscales.ConsumeConditions = [];
            weightedscales.ShopPrice = 4;
            weightedscales.IsShopItem = false;
            weightedscales.StartsLocked = true;
            weightedscales.OnUnlockUsesTHE = true;
            weightedscales.UsesSpecialUnlockText = false;
            weightedscales.SpecialUnlockID = UILocID.None;
            weightedscales.item._ItemTypeIDs = ["Magic"];
            weightedscales.item.AddItem("locked_weightedscales.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Moon", "Weighted Scales", "Comfort Lighter", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Moon_CH", "Aprils_WeightedScales_TW", "Aprils_ComfortLighter_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Moon_Heaven_ACH";
        public static string OsmanACH => "Aprils_Moon_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Moon_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Moon_Osman_Unlock";
    }
}
