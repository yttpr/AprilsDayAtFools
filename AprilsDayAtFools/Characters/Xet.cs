using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Xet
    {
        public static void Add()
        {
            Ability draw = new Ability("Luck of the Draw", IDs.Luck);
            draw.Description = "On combat start, replace this ability with a random ability from another same level party member.";
            draw.AbilitySprite = ResourceLoader.LoadSprite("ability_draw.png");

            Character xet = new Character("Xet", "Xet_CH");
            xet.HealthColor = Pigments.Yellow;
            xet.AddUnitType("FemaleID");
            xet.AddUnitType("Angel");
            xet.AddUnitType("FemaleLooking");
            xet.UsesBasicAbility = true;
            xet.SetBasicAbility(draw.GenerateCharacterAbility());
            xet.UsesAllAbilities = false;
            xet.MovesOnOverworld = true;
            xet.Animator = Joyce.Assets.LoadAsset<RuntimeAnimatorController>("Assets/AnimationBaseData/NewBigGuy/XetAnimController2.overrideController");
            xet.FrontSprite = ResourceLoader.LoadSprite("XetSmall.png");
            xet.BackSprite = ResourceLoader.LoadSprite("XetBack.png");
            xet.OverworldSprite = ResourceLoader.LoadSprite("XetWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            xet.DamageSound = LoadedAssetsHandler.GetCharacter("Doll_CH").damageSound;
            xet.DeathSound = LoadedAssetsHandler.GetCharacter("Doll_CH").deathSound;
            xet.DialogueSound = LoadedAssetsHandler.GetCharacter("Doll_CH").dxSound;
            xet.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            xet.AddFinalBossAchievementData("Heaven", HeavenACH);
            xet.GenerateMenuCharacter(ResourceLoader.LoadSprite("XetMenu.png"), ResourceLoader.LoadSprite("XetLock.png"));
            xet.MenuCharacterIsSecret = false;
            xet.MenuCharacterIgnoreRandom = false;
            xet.SetMenuCharacterAsFullDPS();

            //construct
            RandomAbilityPassive construct = ScriptableObject.CreateInstance<RandomAbilityPassive>();
            construct._passiveName = LoadedAssetsHandler.GetCharacter("Doll_CH").passiveAbilities[0]._passiveName;
            construct.passiveIcon = LoadedAssetsHandler.GetCharacter("Doll_CH").passiveAbilities[0].passiveIcon;
            construct.m_PassiveID = LoadedAssetsHandler.GetCharacter("Doll_CH").passiveAbilities[0].m_PassiveID;
            construct._enemyDescription = LoadedAssetsHandler.GetCharacter("Doll_CH").passiveAbilities[0]._enemyDescription;
            construct._characterDescription = LoadedAssetsHandler.GetCharacter("Doll_CH").passiveAbilities[0]._characterDescription;
            construct._triggerOn = [(TriggerCalls)889532];
            xet.AddPassive(construct);

            Ability replace1 = new Ability("Replace Wood", "Xet_Replace_1_A");
            replace1.Description = "Deal 5 damage to the Left and Right enemies and Reroll one of their actions on the timeline.\nReroll this party member's Construct ability.";
            replace1.AbilitySprite = ResourceLoader.LoadSprite("ability_core.png");
            replace1.Cost = [Pigments.Red, Pigments.Yellow];
            replace1.Effects = new EffectInfo[3];
            replace1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 5, Slots.LeftRight);
            replace1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ReRollTargetTimelineAbilityEffect>(), 1, Slots.LeftRight);
            replace1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RerollConstructEffect>(), 1, Slots.Self);
            replace1.AddIntentsToTarget(Slots.LeftRight, ["Damage_3_6", "Misc"]);
            replace1.AddIntentsToTarget(Slots.Self, [IntentType_GameIDs.PA_Construct.ToString()]);
            replace1.Visuals = CustomVisuals.GetVisuals("Salt/Coda");
            replace1.AnimationTarget = Slots.LeftRight;

            Ability replace2 = new Ability(replace1.ability, "Xet_Replace_2_A", replace1.Cost);
            replace2.Name = "Replace Brass";
            replace2.Description = "Deal 7 damage to the Left and Right enemies and Reroll one of their actions on the timeline.\nReroll this party member's Construct ability.";
            replace2.Effects[0].entryVariable = 7;
            replace2.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability replace3 = new Ability(replace2.ability, "Xet_Replace_3_A", replace1.Cost);
            replace3.Name = "Replace Fossil";
            replace3.Description = "Deal 9 damage to the Left and Right enemies and Reroll one of their actions on the timeline.\nReroll this party member's Construct ability.";
            replace3.Effects[0].entryVariable = 9;

            Ability replace4 = new Ability(replace3.ability, "Xet_Replace_4_A", [Pigments.Red]);
            replace4.Name = "Replace Core";
            replace4.Description = "Deal 10 damage to the Left or Right enemies and Reroll one of their actions on the timeline.\nReroll this party member's Construct ability.";
            replace4.Effects[0].entryVariable = 10;


            RandomStatusEffect random = ScriptableObject.CreateInstance<RandomStatusEffect>();
            //negative.CanApply = ["Cursed_ID", "Frail_ID", "Ruptured_ID", "Gutted_ID", "OilSlicked_ID", "Scars_ID", "Remorse_ID", "Salted_ID", "Paranoia_ID", "Left_ID", "Pale_ID", "DivineSacrifice_ID", "Muted_ID", "Salt_Entropy_ID", "Acid_ID", "Terror_ID", "Drowning_ID", "Pimples_ID"];
            random.CanApply = ["Cursed_ID", "Frail_ID", "Ruptured_ID", "DivineProtection_ID", "Focused_ID", "Gutted_ID", "Linked_ID", "OilSlicked_ID", "Scars_ID", "Remorse_ID", "WildCard_ID", "Salted_ID", "Paranoia_ID", "Anesthetics_ID", "Inverted_ID", "Left_ID", "Pale_ID", "Power_ID", "Determined_ID", "DivineSacrifice_ID", "Favor_ID", "Muted_ID", "Photo_ID", "Dodge_ID", "Salt_Entropy_ID", "Haste_ID", "Acid_ID", "Terror_ID", "Drowning_ID", "Pimples_ID"];

            Ability rearrange1 = new Ability("Rearrange Carboxyl", "Xet_Rearrange_1_A");
            rearrange1.Description = "Deal 7 damage to the Opposing enemy and inflict 3 random Status Effects on them.";
            rearrange1.Cost = [Pigments.Red, Pigments.Red, Pigments.Yellow];
            rearrange1.AbilitySprite = ResourceLoader.LoadSprite("ability_enzyme.png");
            rearrange1.Effects = new EffectInfo[2];
            rearrange1.Effects[0] = Effects.GenerateEffect(replace1.Effects[0].effect, 7, Slots.Front);
            rearrange1.Effects[1] = Effects.GenerateEffect(random, 3, Slots.Front);
            rearrange1.AddIntentsToTarget(Slots.Front, ["Damage_7_10", "Misc_Hidden"]);
            rearrange1.Visuals = CustomVisuals.GetVisuals("Salt/Gears");
            rearrange1.AnimationTarget = Slots.Front;

            Ability rearrange2 = new Ability(rearrange1.ability, "Xet_Rearrange_2_A", rearrange1.Cost);
            rearrange2.Name = "Rearrange Amino";
            rearrange2.Description = "Deal 9 damage to the Opposing enemy and inflict 5 random Status Effects on them.";
            rearrange2.Effects[0].entryVariable = 9;
            rearrange2.Effects[1].entryVariable = 5;

            Ability rearrange3 = new Ability(rearrange2.ability, "Xet_Rearrange_3_A", rearrange1.Cost);
            rearrange3.Name = "Rearrange Protein";
            rearrange3.Description = "Deal 11 damage to the Opposing enemy and inflict 7 random Status Effects on them.";
            rearrange3.Effects[0].entryVariable = 11;
            rearrange3.Effects[1].entryVariable = 7;
            rearrange3.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability rearrange4 = new Ability(rearrange3.ability, "Xet_Rearrange_4_A", rearrange3.Cost);
            rearrange4.Name = "Rearrange Enzyme";
            rearrange4.Description = "Deal 13 damage to the Opposing enemy and inflict 9 random Status Effects on them.";
            rearrange4.Effects[0].entryVariable = 13;
            rearrange4.Effects[1].entryVariable = 9;

            SpawnEnemyAnywhereEffect summonScrap = ScriptableObject.CreateInstance<SpawnEnemyAnywhereEffect>();
            summonScrap._spawnTypeID = "Spawn_Basic";
            summonScrap.enemy = LoadedAssetsHandler.GetEnemy("ScrapBomb_EN");
            Ability design1 = new Ability("Scrap Design", "Xet_Design_1_A");
            design1.Description = "Deal 4 damage to a random enemy.\nGain 4 Shield and spawn a Scrap Bomb.";
            design1.Cost = [Pigments.Red, Pigments.Blue];
            design1.AbilitySprite = ResourceLoader.LoadSprite("ability_creak");
            design1.Effects = new EffectInfo[3];
            design1.Effects[0] = Effects.GenerateEffect(replace1.Effects[0].effect, 4, ScriptableObject.CreateInstance<TargettingRandomUnit>());
            design1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyShieldSlotEffect>(), 4, Slots.Self);
            design1.Effects[2] = Effects.GenerateEffect(summonScrap, 1);
            design1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Damage_3_6"]);
            design1.AddIntentsToTarget(Slots.Self, ["Field_Shield"]);
            design1.Visuals = CustomVisuals.GetVisuals("Salt/Cube");
            design1.AnimationTarget = Slots.Self;

            Ability design2 = new Ability(design1.ability, "Xet_Design_2_A", design1.Cost);
            design2.Name = "Creak Design";
            design2.Description = "Deal 6 damage to a random enemy.\nGain 5 Shield and spawn a Scrap Bomb.";
            design2.Effects[0].entryVariable = 6;
            design2.Effects[1].entryVariable = 5;

            Ability design3 = new Ability(design2.ability, "Xet_Design_3_A", design1.Cost);
            design3.Name = "Tinker Design";
            design3.Description = "Deal 7 damage to a random enemy.\nGain 6 Shield and spawn a Scrap Bomb.";
            design3.Effects[0].entryVariable = 7;
            design3.Effects[1].entryVariable = 6;
            design3.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability design4 = new Ability(design3.ability, "Xet_Design_4_A", design1.Cost);
            design4.Name = "Homebrew Design";
            design4.Description = "Deal 8 damage to a random enemy.\nGain 7 Shield and spawn a Scrap Bomb.";
            design4.Effects[0].entryVariable = 8;
            design4.Effects[1].entryVariable = 7;


            xet.AddLevelData(10, [design1, replace1, rearrange1]);
            xet.AddLevelData(14, [design2, replace2, rearrange2]);
            xet.AddLevelData(17, [design3, replace3, rearrange3]);
            xet.AddLevelData(19, [design4, replace4, rearrange4]);
            xet.AddCharacter(true);
        }
        public static void Items()
        {
            PerformEffect_Item glasscompass = new PerformEffect_Item("Aprils_GlassCompass_SW", []);
            glasscompass.Name = "Glass Compass";
            glasscompass.Flavour = "\"See beyond destination\"";
            glasscompass.Description = "On dealing damage, move the target to the Left or Right.";
            glasscompass.Icon = ResourceLoader.LoadSprite("item_glasscompass.png");
            glasscompass.EquippedModifiers = [];
            glasscompass.TriggerOn = AdvancedDamageTrigger.Dealt;
            glasscompass.DoesPopUpInfo = false;
            glasscompass.Conditions = [DamageTargetEffectsCondition.Create([Effects.GenerateEffect(ScriptableObject.CreateInstance<SwapToSidesEffect>(), 1, Slots.Self)], true)];
            glasscompass.DoesActionOnTriggerAttached = false;
            glasscompass.ConsumeOnTrigger = TriggerCalls.Count;
            glasscompass.ConsumeOnUse = false;
            glasscompass.ConsumeConditions = [];
            glasscompass.ShopPrice = 3;
            glasscompass.IsShopItem = true;
            glasscompass.StartsLocked = true;
            glasscompass.OnUnlockUsesTHE = true;
            glasscompass.UsesSpecialUnlockText = false;
            glasscompass.SpecialUnlockID = UILocID.None;
            glasscompass.item._ItemTypeIDs = ["Magic"];
            glasscompass.Item.AddItem("locked_glasscompass.png", OsmanACH);

            MultiPerformEffectItem doohikey = new MultiPerformEffectItem("Aprils_Doohikey_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<RandomStatusEffect>(), 9, Targeting.Unit_AllOpponents)]);
            doohikey.Name = "Doohikey";
            doohikey.Flavour = "\"Piece of crap.\"";
            doohikey.Description = "On using an ability, inflict 1 random positive or negative Status Effects on all enemies.\nOn being damaged, inflict 9 random positive or negative Status Effects on all enemies and destroy this item.";
            doohikey.Icon = ResourceLoader.LoadSprite("item_doohikey.png");
            doohikey.EquippedModifiers = [];
            doohikey.TriggerOn = TriggerCalls.OnDirectDamaged;
            doohikey.DoesPopUpInfo = true;
            doohikey.Conditions = [];
            doohikey.DoesActionOnTriggerAttached = false;
            doohikey.ConsumeOnTrigger = TriggerCalls.Count;
            doohikey.ConsumeOnUse = true;
            doohikey.ConsumeConditions = [];
            doohikey.ShopPrice = 4;
            doohikey.IsShopItem = true;
            doohikey.StartsLocked = true;
            doohikey.OnUnlockUsesTHE = true;
            doohikey.UsesSpecialUnlockText = false;
            doohikey.SpecialUnlockID = UILocID.None;
            EffectTrigger second = new EffectTrigger([Effects.GenerateEffect(ScriptableObject.CreateInstance<RandomStatusEffect>(), 1, Targeting.Unit_AllOpponents)], [TriggerCalls.OnAbilityUsed], [], true);
            doohikey.AddEffectTrigger(second);
            doohikey.item.AddItem("locked_doohikey.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Xet", "Doohikey", "Glass Compass", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Xet_CH", "Aprils_Doohikey_SW", "Aprils_GlassCompass_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Xet_Heaven_ACH";
        public static string OsmanACH => "Aprils_Xet_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Xet_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Xet_Osman_Unlock";
    }
}
