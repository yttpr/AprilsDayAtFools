using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Defacer
    {
        public static void Add()
        {
            if (!April.Me) return;

            PerformEffectPassiveAbility ripper = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            ripper.name = "Ripper_PA";
            ripper._passiveName = "Ripper";
            ripper.m_PassiveID = "Ripper_PA";
            ripper.passiveIcon = ResourceLoader.LoadSprite("RipperPassive.png");
            ripper._characterDescription = "Inflict Terror on damaged targets.";
            ripper._enemyDescription = ripper._characterDescription;
            ripper.doesPassiveTriggerInformationPanel = false;
            ripper._triggerOn = [AdvancedDamageTrigger.Dealt];
            ripper.conditions = [DamageTargetEffectsCondition.Create([Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyTerrorEffect>(), 1, Slots.Self)], false)];
            ripper.effects = [];
            ripper.AddToPassiveDatabase();
            ripper.AddPassiveToGlossary("Ripper", "Inflict Terror on damaged targets.");

            Character defacer = new Character("Defacer", "Secret_CH");
            defacer.HealthColor = Pigments.Purple;
            defacer.AddUnitType("FemaleID");
            defacer.AddUnitType("Sandwich_Gore");
            defacer.AddUnitType("FemaleLooking");
            defacer.UsesBasicAbility = true;
            //slap
            defacer.UsesAllAbilities = false;
            defacer.MovesOnOverworld = true;
            //animator
            defacer.FrontSprite = ResourceLoader.LoadSprite("DefacerFront.png");
            defacer.BackSprite = ResourceLoader.LoadSprite("DefacerBack.png");
            defacer.OverworldSprite = ResourceLoader.LoadSprite("DefacerWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            defacer.DamageSound = "event:/Lunacy/SOUNDS3/DefacerHit";
            defacer.DeathSound = "event:/Lunacy/SOUNDS3/DefacerDie";
            defacer.DialogueSound = "event:/Lunacy/SOUNDS3/DefacerHit";
            defacer.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            defacer.AddFinalBossAchievementData("Heaven", HeavenACH);
            defacer.GenerateMenuCharacter(ResourceLoader.LoadSprite("DefacerMenu.png"), ResourceLoader.LoadSprite("DefacerLock.png"));
            defacer.MenuCharacterIsSecret = false;
            defacer.MenuCharacterIgnoreRandom = false;
            defacer.SetMenuCharacterAsFullSupport();
            defacer.AddPassive(Passives.Delicate);

            TargettingUnitsWithStatusEffectSide hasTerror = ScriptableObject.CreateInstance<TargettingUnitsWithStatusEffectSide>();
            hasTerror.getAllies = false;
            hasTerror.ignoreCastSlot = false;
            hasTerror.targetStatus = Terror.StatusID;

            Ability skin1 = new Ability("Skin Tear", "Secret_Skin_1_A");
            skin1.Description = "Deal 4-6 damage to all enemies with Terror.\nInflict Terror on the Opposing enemy,";
            skin1.AbilitySprite = ResourceLoader.LoadSprite("ability_mask.png");
            skin1.Cost = [Pigments.Blue, Pigments.Red];
            skin1.Effects = new EffectInfo[3];
            skin1.Effects[0] = Effects.GenerateEffect(BasicEffects.Empty, 4);
            skin1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RandomDamageBetweenPreviousAndEntryEffect>(), 6, hasTerror);
            skin1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyTerrorEffect>(), 1, Slots.Front);
            skin1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Misc_Hidden", "Damage_3_6"]);
            skin1.AddIntentsToTarget(Slots.Front, [Terror.Intent]);
            skin1.Visuals = LoadedAssetsHandler.GetEnemyAbility("Boil_A").visuals;
            skin1.AnimationTarget = hasTerror;

            Ability skin2 = new Ability(skin1.ability, "Secret_Skin_2_A", skin1.Cost);
            skin2.Name = "Skin Slice";
            skin2.Description = "Deal 6-8 damage to all enemies with Terror.\nInflict Terror on the Opposing enemy,";
            skin2.Effects[0].entryVariable = 6;
            skin2.Effects[1].entryVariable = 8;
            skin2.EffectIntents[0].intents[1] = "Damage_7_10";

            Ability skin3 = new Ability(skin2.ability, "Secret_Skin_3_A", skin1.Cost);
            skin3.Name = "Skin Shear";
            skin3.Description = "Deal 8-10 damage to all enemies with Terror.\nInflict Terror on the Opposing enemy,";
            skin3.Effects[0].entryVariable = 8;
            skin3.Effects[1].entryVariable = 10;

            Ability skin4 = new Ability(skin3.ability, "Secret_Skin_4_A", skin1.Cost);
            skin4.Name = "Skin Layer";
            skin4.Description = "Deal 10-12 damage to all enemies with Terror.\nInflict Terror on the Opposing enemy,";
            skin4.Effects[0].entryVariable = 10;
            skin4.Effects[1].entryVariable = 12;
            skin4.EffectIntents[0].intents[1] = "Damage_11_15";

            HealEffect heal = ScriptableObject.CreateInstance<HealEffect>();
            RandomHealBetweenPreviousAndEntryEffect range = ScriptableObject.CreateInstance<RandomHealBetweenPreviousAndEntryEffect>();

            Ability face1 = new Ability("Face Cutter", "Secret_Face_1_A");
            face1.Description = "Remove the Left party member's Leftmost ability and give them a random one of their abilities that they didn't have. If they had all of their abilities give them a random item ability.\nHeal this and the Left ally 3 health.";
            face1.AbilitySprite = ResourceLoader.LoadSprite("ability_shear.png");
            face1.Cost = [Pigments.Yellow, Pigments.Blue, Pigments.Blue];
            face1.Effects = new EffectInfo[2];
            face1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<CycleAbilitiesEffect>(), 1, Targeting.Slot_AllyLeft);
            face1.Effects[1] = Effects.GenerateEffect(heal, 3, Targeting.Slot_SelfAll_AndLeft);
            face1.AddIntentsToTarget(Targeting.Slot_AllyLeft, ["Misc_Hidden"]);
            face1.AddIntentsToTarget(Targeting.Slot_SelfAll_AndLeft, ["Heal_1_4"]);
            face1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Purify_1_A").visuals;
            face1.AnimationTarget = Targeting.Slot_AllyLeft;

            Ability face2 = new Ability(face1.ability, "Secret_Face_2_A", face1.Cost);
            face2.Name = "Face Bleeder";
            face2.Description = "Remove the Left party member's Leftmost ability and give them a random one of their abilities that they didn't have. If they had all of their abilities give them a random item ability.\nHeal this and the Left ally 5 health.";
            face2.Effects[1].entryVariable = 5;
            face2.EffectIntents[1].intents[0] = "Heal_5_10";

            Ability face3 = new Ability(face2.ability, "Secret_Face_3_A", [Pigments.Blue, Pigments.Blue]);
            face3.Name = "Face Ripper";
            face3.Description = "Remove the Left party member's Leftmost ability and give them a random one of their abilities that they didn't have. If they had all of their abilities give them a random item ability.\nHeal this and the Left ally 6 health.";
            face3.Effects[1].entryVariable = 6;

            Ability face4 = new Ability(face3.ability, "Secret_Face_4_A", face3.Cost);
            face4.Name = "Face Remover";
            face4.Description = "Remove the Left party member's Leftmost ability and give them a random one of their abilities that they didn't have. If they had all of their abilities give them a random item ability.\nHeal this and the Left ally 8 health.";
            face4.Effects[1].entryVariable = 8;

            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool(IDs.Level, "Level +{0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Positive));

            Ability mask1 = new Ability("Mask of Fear", "Secret_Mask_1_A");
            mask1.Description = "Temporarily level up the Right ally.\nHeal this and the right ally 0-4 health. If the Right ally was not leveled up, heal 4 health instead.";
            mask1.AbilitySprite = ResourceLoader.LoadSprite("ability_face.png");
            mask1.Cost = [Pigments.Yellow, Pigments.Purple, Pigments.Purple, Pigments.Blue];
            mask1.Effects = new EffectInfo[4];
            mask1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<TempRankUpEffect>(), 1, Targeting.Slot_AllyRight);
            mask1.Effects[1] = Effects.GenerateEffect(BasicEffects.Empty, 0, null);
            mask1.Effects[2] = Effects.GenerateEffect(range, 4, Targeting.Slot_SelfAndRight, BasicEffects.DidThat(true, 2));
            mask1.Effects[3] = Effects.GenerateEffect(heal, 4, Targeting.Slot_SelfAndRight, BasicEffects.DidThat(false, 3));
            mask1.AddIntentsToTarget(Targeting.Slot_AllyRight, ["Misc"]);
            mask1.AddIntentsToTarget(Targeting.Slot_SelfAndRight, ["Heal_1_4"]);
            mask1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Conversion_1_A").visuals;
            mask1.AnimationTarget = Targeting.Slot_AllyRight;

            Ability mask2 = new Ability(mask1.ability, "Secret_Mask_2_A", mask1.Cost);
            mask2.Name = "Mask of Terror";
            mask2.Description = "Temporarily level up the Right ally.\nHeal this and the right ally 0-6 health. If the Right ally was not leveled up, heal 6 health instead.";
            mask2.Effects[2].entryVariable = 6;
            mask2.Effects[3].entryVariable = 6;
            mask2.EffectIntents[1].intents[0] = "Heal_5_10";

            Ability mask3 = new Ability(mask2.ability, "Secret_Mask_3_A", [Pigments.Purple, Pigments.Purple, Pigments.Blue]);
            mask3.Name = "Mask of Horror";
            mask3.Description = "Temporarily level up the Right ally.\nHeal this and the right ally 1-7 health. If the Right ally was not leveled up, heal 7 health instead.";
            mask3.Effects[1].entryVariable = 1;
            mask3.Effects[2].entryVariable = 7;
            mask3.Effects[3].entryVariable = 7;

            Ability mask4 = new Ability(mask3.ability, "Secret_Mask_4_A", [Pigments.Purple, Pigments.BluePurple]);
            mask4.Name = "Mask of Despair";
            mask4.Description = "Temporarily level up the Right ally.\nHeal this and the right ally 1-10 health. If the Right ally was not leveled up, heal 10 health instead.";
            mask4.Effects[2].entryVariable = 10;
            mask4.Effects[3].entryVariable = 10;

            defacer.AddLevelData(10, [skin1, mask1, face1]);
            defacer.AddLevelData(14, [skin2, mask2, face2]);
            defacer.AddLevelData(17, [skin3, mask3, face3]);
            defacer.AddLevelData(19, [skin4, mask4, face4]);
            defacer.AddCharacter(true);
        }
        public static void AddDialogueEmote()
        {
            if (!April.Me) return;

            SpeakerBundle mad = new SpeakerBundle();
            mad.bundleTextColor = new Color32(241, 235, 232, 255);
            mad.dialogueSound = "";
            mad.portrait = ResourceLoader.LoadSprite("DefacerMad.png");

            SpeakerEmote emotion = new SpeakerEmote();
            emotion.emotion = "Mad";
            emotion.bundle = mad;

            LoadedAssetsHandler.GetSpeakerData("Secret_SpeakerData")._emotionBundles = [emotion];
        }


        public static void Items()
        {
            if (!April.Me) return;

            MultiPerformEffectItem dagger = new MultiPerformEffectItem("Aprils_EtherealDagger_SW", []);
            dagger.Name = "Ethereal Dagger";
            dagger.Flavour = "\"Hurts something deep inside.\"";
            dagger.Description = "Curse damaged targets.\nDeal 25% more damage to Cursed targets.";
            dagger.Icon = ResourceLoader.LoadSprite("item_etherealdagger.png");
            dagger.EquippedModifiers = [];
            dagger.TriggerOn = AdvancedDamageTrigger.Dealt;
            dagger.DoesPopUpInfo = false;
            dagger.Conditions = [DamageTargetEffectsCondition.Create([Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyCursedEffect>(), 1, Slots.Self)], true)];
            dagger.DoesActionOnTriggerAttached = false;
            dagger.ConsumeOnTrigger = TriggerCalls.Count;
            dagger.ConsumeOnUse = false;
            dagger.ConsumeConditions = [];
            dagger.ShopPrice = 8;
            dagger.IsShopItem = true;
            dagger.StartsLocked = true;
            dagger.OnUnlockUsesTHE = true;
            dagger.UsesSpecialUnlockText = false;
            dagger.SpecialUnlockID = UILocID.None;
            dagger.item._ItemTypeIDs = ["Knife"];
            dagger.AddEffectTrigger(new EffectTrigger([], [TriggerCalls.OnWillApplyDamage], [ScriptableObject.CreateInstance<EtherealDaggerCondition>()], false));
            dagger.item.AddItem("locked_etherealdagger.png", HeavenACH);

            Ability growth = new Ability("Growth Solution", "GrowthSolution_A");
            growth.Description = "If the Right position is empty, attempt to spawn a level 1 permenant copy of the Left ally with 1 health and Cursed.";
            growth.Cost = [Pigments.Yellow, Pigments.Blue, Pigments.Purple];
            growth.AbilitySprite = ResourceLoader.LoadSprite("ability_growthsolution.png");
            growth.Effects = [
                Effects.GenerateEffect(ScriptableObject.CreateInstance<GrowthSolutionEffect>()),
                Effects.GenerateEffect(CasterSubActionEffect.Create([Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyCursedEffect>(), 1, Targeting.Slot_AllyRight)]), 0, null, BasicEffects.DidThat(true))
                ];
            growth.AddIntentsToTarget(Targeting.Slot_AllyLeft, ["Misc_Hidden"]);
            growth.AddIntentsToTarget(Targeting.Slot_AllyRight, [IntentType_GameIDs.Other_Spawn.ToString()]);
            growth.Visuals = null;

            ExtraAbility_Wearable_SMS add_growth = ScriptableObject.CreateInstance<ExtraAbility_Wearable_SMS>();
            add_growth._extraAbility = growth.GenerateCharacterAbility();

            //((Passives.Construct as Connection_PerformEffectPassiveAbility).connectionEffects[1].effect as CasterAddRandomExtraAbilityEffect)._extraData.Add(add_growth);

            PerformEffect_Item testtube = new PerformEffect_Item("Aprils_TestTube_SW", []);
            testtube.Name = "Test Tube";
            testtube.Flavour = "\"Grow a new family\"";
            testtube.Description = "Adds the extra ability \"Growth Solution,\" which can be used to duplicate party members.\nThis item is destroyed at the end of combat.";
            testtube.Icon = ResourceLoader.LoadSprite("item_testtube.png");
            testtube.EquippedModifiers = [add_growth];
            testtube.TriggerOn = TriggerCalls.Count;
            testtube.DoesPopUpInfo = true;
            testtube.Conditions = [];
            testtube.DoesActionOnTriggerAttached = false;
            testtube.ConsumeOnTrigger = TriggerCalls.OnCombatEnd;
            testtube.ConsumeOnUse = false;
            testtube.ConsumeConditions = [];
            testtube.ShopPrice = 10;
            testtube.IsShopItem = true;
            testtube.StartsLocked = true;
            testtube.OnUnlockUsesTHE = true;
            testtube.UsesSpecialUnlockText = false;
            testtube.SpecialUnlockID = UILocID.None;
            testtube.item._ItemTypeIDs = [];
            testtube.item.AddItem("locked_testtube.png", OsmanACH);
        }
        public static void Unlocks()
        {
            if (!April.Me) return;

            Unlocking.GenerateAchievements("Defacer", "Ethereal Dagger", "Test Tube", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Secret_CH", "Aprils_EtherealDagger_SW", "Aprils_TestTube_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Secret_Heaven_ACH";
        public static string OsmanACH => "Aprils_Secret_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Secret_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Secret_Osman_Unlock";
    }
}
