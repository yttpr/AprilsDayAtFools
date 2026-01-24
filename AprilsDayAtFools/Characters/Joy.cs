using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Joy
    {
        public static void Add()
        {
            ExtraCCSprites_ArraySO joyExtra = ScriptableObject.CreateInstance<ExtraCCSprites_ArraySO>();
            joyExtra._DefaultID = "";
            joyExtra._frontSprite = [ResourceLoader.LoadSprite("JoyFront_Blue.png"),
                ResourceLoader.LoadSprite("JoyFront_Red.png"),
                ResourceLoader.LoadSprite("JoyFront_Yellow.png"),
                ResourceLoader.LoadSprite("JoyFront_Purple.png")];
            joyExtra._SpecialID = IDs.Joy;
            joyExtra._backSprite = [ResourceLoader.LoadSprite("JoyBack.png"),
                ResourceLoader.LoadSprite("JoyBack.png"),
                ResourceLoader.LoadSprite("JoyBack.png"),
                ResourceLoader.LoadSprite("JoyBack.png")];
            joyExtra._doesLoop = true;
            SetCasterExtraSpritesRandomUpToEntryEffect joySprites = ScriptableObject.CreateInstance<SetCasterExtraSpritesRandomUpToEntryEffect>();
            joySprites._spriteType = joyExtra._SpecialID;

            RandomStatusEffect status = ScriptableObject.CreateInstance<RandomStatusEffect>();
            status._usePreviousExit = true;

            PerformEffectPassiveAbility random = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            random._passiveName = "Unpredictable";
            random.passiveIcon = ResourceLoader.LoadSprite("UnpredictablePassive.png");
            random.m_PassiveID = "Unpredictable_PA";
            random._enemyDescription = "i mean.....";
            random._characterDescription = "On using wrong pigment, gain 1 random Positive or Negative status effect for each wrong pigment used.";
            random.doesPassiveTriggerInformationPanel = true;
            random.conditions = [WrongPigmentCondition.Create(IDs.Unpredictable)];
            random.effects = new EffectInfo[2];
            random.effects[0] = Effects.GenerateEffect(SetExitByStoredValueEffect.Create(IDs.Unpredictable));
            random.effects[1] = Effects.GenerateEffect(status, 1, Slots.Self);
            random._triggerOn = [TriggerCalls.OnAbilityUsed];
            random.AddToPassiveDatabase();
            random.AddPassiveToGlossary("Unpredictable", "On using wrong pigment, gain 1 random Positive or Negative status effect for each wrong pigment used.");

            Character joy = new Character("Joy", "Joy_CH");
            joy.HealthColor = Pigments.Blue;
            joy.AddUnitType("FemaleID");
            joy.AddUnitType("Sandwich_Robot");
            joy.AddUnitType("FemaleLooking");
            joy.UsesBasicAbility = true;
            //slap
            joy.UsesAllAbilities = false;
            joy.MovesOnOverworld = true;
            //animator
            joy.FrontSprite = ResourceLoader.LoadSprite("JoyFront_Blue.png");
            joy.BackSprite = ResourceLoader.LoadSprite("JoyBack.png");
            joy.OverworldSprite = ResourceLoader.LoadSprite("JoyWorld.png", new Vector2(0.5f, 0f));
            joy.ExtraSprites = joyExtra;
            joy.DamageSound = "event:/Lunacy/SOUNDS/SolitaireHit";
            joy.DeathSound = "event:/Lunacy/SOUNDS/SolitaireDie";
            joy.DialogueSound = "event:/Lunacy/SOUNDS/SolitaireHit";
            joy.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            joy.AddFinalBossAchievementData("Heaven", HeavenACH);
            joy.GenerateMenuCharacter(ResourceLoader.LoadSprite("JoyMenu.png"), ResourceLoader.LoadSprite("JoyLock.png"));
            joy.MenuCharacterIsSecret = false;
            joy.MenuCharacterIgnoreRandom = false;
            joy.SetMenuCharacterAsFullSupport();
            joy.AddPassive(random);

            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool(IDs.Contract, "Contractual -{0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Negative));

            HealDecreaseByStoredValueEffect contractHeal = ScriptableObject.CreateInstance<HealDecreaseByStoredValueEffect>();
            contractHeal.m_unitStoredDataID = IDs.Contract;
            ChanageValueByPreviousEffect contractDec = ScriptableObject.CreateInstance<ChanageValueByPreviousEffect>();
            contractDec._increase = true;
            contractDec.m_unitStoredDataID = IDs.Contract;
            
            Ability contract1 = new Ability("Contractual Obligation", "Joy_Contract_1_A");
            contract1.Description = "Heal this party member and the Left ally 11 health and decrease this ability's healing for the amount healed.";
            contract1.AbilitySprite = ResourceLoader.LoadSprite("ability_contract.png");
            contract1.Cost = [Pigments.Purple, Pigments.Blue];
            contract1.Effects = new EffectInfo[3];
            contract1.Effects[0] = Effects.GenerateEffect(contractHeal, 11, Targeting.Slot_SelfAndLeft);
            contract1.Effects[1] = Effects.GenerateEffect(contractDec);
            contract1.Effects[2] = Effects.GenerateEffect(joySprites, 4, null, BasicEffects.DidThat(true, 2));
            contract1.AddIntentsToTarget(Targeting.Slot_SelfAndLeft, ["Heal_11_20"]);
            contract1.AddIntentsToTarget(Slots.Self, ["Misc"]);
            contract1.Visuals = LoadedAssetsHandler.GetEnemyAbility("Boil_A").visuals;
            contract1.AnimationTarget = Targeting.Slot_AllyLeft;
            contract1.UnitStoreData = UnitStoreData.GetCustom_UnitStoreData(IDs.Contract);

            Ability contract2 = new Ability(contract1.ability, "Joy_Contract_2_A", contract1.Cost);
            contract2.Name = "Contractual Assistance";
            contract2.Description = "Heal this party member and the Left ally 14 health and decrease this ability's healing for the amount healed.";
            contract2.Effects[0].entryVariable = 14;

            Ability contract3 = new Ability(contract2.ability, "Joy_Contract_3_A", contract1.Cost);
            contract3.Name = "Contractual Cooperation";
            contract3.Description = "Heal this party member and the Left ally 17 health and decrease this ability's healing for the amount healed.";
            contract3.Effects[0].entryVariable = 17;

            Ability contract4 = new Ability(contract3.ability, "Joy_Contract_4_A", contract1.Cost);
            contract4.Name = "Contractual Compensation";
            contract4.Description = "Heal this party member and the Left ally 21 health and decrease this ability's healing for the amount healed.";
            contract4.Effects[0].entryVariable = 21;
            contract4.EffectIntents[0].intents[0] = "Heal_21";

            HealRemoveStatusEffect scamHeal = ScriptableObject.CreateInstance<HealRemoveStatusEffect>();
            scamHeal._negativeOnly = true;

            Ability scam1 = new Ability("Loan Scam", "Joy_Scam_1_A");
            scam1.Description = "Lose 2 Coins and heal the Left and Right allies 6 health.\nIf the targets have negative Status Effects, reduce the amount healed for each negative Status Effect and remove all negative Status Effects from them.";
            scam1.AbilitySprite = ResourceLoader.LoadSprite("ability_scam.png");
            scam1.Cost = [Pigments.YellowBlue];
            scam1.Effects = new EffectInfo[5];
            scam1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ShowPlayerCurrencyEffect>());
            scam1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<LosePlayerCurrencyEffect>(), 2);
            scam1.Effects[2] = Effects.GenerateEffect(BasicEffects.GetVisuals("Salt/Snap", false, Slots.Sides));
            scam1.Effects[3] = Effects.GenerateEffect(scamHeal, 6, Slots.Sides);
            scam1.Effects[4] = Effects.GenerateEffect(joySprites, 4);
            scam1.AddIntentsToTarget(Slots.Self, ["Misc_Currency"]);
            scam1.AddIntentsToTarget(Slots.Sides, ["Heal_5_10", "Misc"]);

            Ability scam2 = new Ability(scam1.ability, "Joy_Scam_2_A", scam1.Cost);
            scam2.Name = "Spreadsheet Scam";
            scam2.Description = "Lose 2 Coins and heal the Left and Right allies 8 health.\nIf the targets have negative Status Effects, reduce the amount healed for each negative Status Effect and remove all negative Status Effects from them.";
            scam2.Effects[3].entryVariable = 8;

            Ability scam3 = new Ability(scam2.ability, "Joy_Scam_3_A", scam1.Cost);
            scam3.Name = "Investment Scam";
            scam3.Description = "Lose 2 Coins and heal the Left and Right allies 10 health.\nIf the targets have negative Status Effects, reduce the amount healed for each negative Status Effect and remove all negative Status Effects from them.";
            scam3.Effects[3].entryVariable = 10;

            Ability scam4 = new Ability(scam3.ability, "Joy_Scam_4_A", scam1.Cost);
            scam4.Name = "Embezzlement Scam";
            scam4.Description = "Lose 2 Coins and heal the Left and Right allies 12 health.\nIf the targets have negative Status Effects, reduce the amount healed for each negative Status Effect and remove all negative Status Effects from them.";
            scam4.Effects[3].entryVariable = 12;
            scam4.EffectIntents[1].intents[0] = "Heal_11_20";

            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool("Scramble_A", "Scramble -{0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Negative));
            CasterStoredValueChangeEffect scrambleVal = ScriptableObject.CreateInstance<CasterStoredValueChangeEffect>();
            scrambleVal._increase = true;
            scrambleVal.m_unitStoredDataID = "Scramble_A";
            StoredValueReachedCondition scrambleCheck = StoredValueReachedCondition.Create("Scramble_A", 2);
            Ability scramble1 = new Ability("Quick Scramble", "Joy_Scramble_1_A");
            scramble1.Description = "Heal all party members 3-5 health.\nRefresh this party member's ability usage.\nUsing this ability 2 times will make this party member instantly flee.";
            scramble1.AbilitySprite = ResourceLoader.LoadSprite("ability_scramble.png");
            scramble1.Cost = [Pigments.Blue, Pigments.Yellow];
            scramble1.Effects = new EffectInfo[7];
            scramble1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), 1, Slots.Self);
            scramble1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ExtraVariableForNextEffect>(), 3, Slots.Self);
            scramble1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RandomHealBetweenPreviousAndEntryEffect>(), 5, Targeting.Unit_AllAllies);
            scramble1.Effects[3] = Effects.GenerateEffect(joySprites, 4);
            scramble1.Effects[4] = Effects.GenerateEffect(scrambleVal, 1);
            scramble1.Effects[5] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageByCostEffect>(), 1, Slots.Self, scrambleCheck);
            scramble1.Effects[6] = Effects.GenerateEffect(ScriptableObject.CreateInstance<FleeTargetEffect>(), 1, Slots.Self, scrambleCheck);
            scramble1.AddIntentsToTarget(Slots.Self, ["Misc_Additional", "PA_Fleeting"]);
            scramble1.AddIntentsToTarget(Targeting.Unit_AllAllies, ["Heal_5_10"]);
            scramble1.Visuals = LoadedAssetsHandler.GetEnemyAbility("Wriggle_A").visuals;
            scramble1.AnimationTarget = Targeting.Unit_AllAllies;
            scramble1.UnitStoreData = UnitStoreData.GetCustom_UnitStoreData("Scramble_A");

            Ability scramble2 = new Ability(scramble1.ability, "Joy_Scramble_2_A", scramble1.Cost);
            scramble2.Name = "Mad Scramble";
            scramble2.Description = "Heal all party members 3-6 health.\nRefresh this party member's ability usage.\nUsing this ability 2 times will make this party member instantly flee.";
            scramble2.Effects[2].entryVariable = 6;

            Ability scramble3 = new Ability(scramble2.ability, "Joy_Scramble_3_A", scramble1.Cost);
            scramble3.Name = "Deranged Scramble";
            scramble3.Description = "Heal all party members 3-8 health.\nRefresh this party member's ability usage.\nUsing this ability 2 times will make this party member instantly flee.";
            scramble3.Effects[2].entryVariable = 8;

            Ability scramble4 = new Ability(scramble3.ability, "Joy_Scramble_4_A", scramble1.Cost);
            scramble4.Name = "Psychotic Scramble";
            scramble4.Description = "Heal all party members 4-8 health.\nRefresh this party member's ability usage.\nUsing this ability 2 times will make this party member instantly flee.";
            scramble4.Effects[1].entryVariable = 4;

            joy.AddLevelData(8, [scramble1, contract1, scam1]);
            joy.AddLevelData(10, [scramble2, contract2, scam2]);
            joy.AddLevelData(12, [scramble3, contract3, scam3]);
            joy.AddLevelData(14, [scramble4, contract4, scam4]);
            joy.AddCharacter(true);
        }
        public static void Items()
        {
            PerformEffect_Item rustedsurgeonstools = new PerformEffect_Item("Aprils_RustedSurgeonsTools_SW", []);
            rustedsurgeonstools.Name = "Rusted Surgeon's Tools";
            rustedsurgeonstools.Flavour = "\"I mean, they work still.\"";
            rustedsurgeonstools.Description = "Double healing dealt by this party member.\nInflict 2 Ruptured on healed targets.";
            rustedsurgeonstools.Icon = ResourceLoader.LoadSprite("item_rustedsurgeonstools.png");
            rustedsurgeonstools.EquippedModifiers = [];
            rustedsurgeonstools.TriggerOn = TriggerCalls.OnWillApplyHeal;
            rustedsurgeonstools.DoesPopUpInfo = false;
            rustedsurgeonstools.Conditions = [ScriptableObject.CreateInstance<SurgeonsToolsCondition>()];
            rustedsurgeonstools.DoesActionOnTriggerAttached = false;
            rustedsurgeonstools.ConsumeOnTrigger = TriggerCalls.Count;
            rustedsurgeonstools.ConsumeOnUse = false;
            rustedsurgeonstools.ConsumeConditions = [];
            rustedsurgeonstools.ShopPrice = 8;
            rustedsurgeonstools.IsShopItem = true;
            rustedsurgeonstools.StartsLocked = true;
            rustedsurgeonstools.OnUnlockUsesTHE = true;
            rustedsurgeonstools.UsesSpecialUnlockText = false;
            rustedsurgeonstools.SpecialUnlockID = UILocID.None;
            rustedsurgeonstools.item._ItemTypeIDs = ["Knife"];
            rustedsurgeonstools.Item.AddItem("locked_rustedsurgeonstools.png", OsmanACH);

            ApplyFireSlotEffect range = ScriptableObject.CreateInstance<ApplyFireSlotEffect>();
            range._UseRandomBetweenPrevious = true;
            PerformEffect_Item digestiblefireworks = new PerformEffect_Item("Aprils_DigestibleFireworks_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<ExtraVariableForNextEffect>(), 3), Effects.GenerateEffect(range, 5, Targetting.Everything(false))]);
            digestiblefireworks.Name = "Digestible Firworks";
            digestiblefireworks.Flavour = "\"One last show...\"";
            digestiblefireworks.Description = "On death, inflict 3-5 Fire to all enemy positions.\nThis item is consumed on use.";
            digestiblefireworks.Icon = ResourceLoader.LoadSprite("item_digestiblefireworks.png");
            digestiblefireworks.EquippedModifiers = [];
            digestiblefireworks.TriggerOn = TriggerCalls.OnDeath;
            digestiblefireworks.DoesPopUpInfo = true;
            digestiblefireworks.Conditions = [];
            digestiblefireworks.DoesActionOnTriggerAttached = false;
            digestiblefireworks.ConsumeOnTrigger = TriggerCalls.Count;
            digestiblefireworks.ConsumeOnUse = true;
            digestiblefireworks.ConsumeConditions = [];
            digestiblefireworks.ShopPrice = 3;
            digestiblefireworks.IsShopItem = true;
            digestiblefireworks.StartsLocked = true;
            digestiblefireworks.OnUnlockUsesTHE = true;
            digestiblefireworks.UsesSpecialUnlockText = false;
            digestiblefireworks.SpecialUnlockID = UILocID.None;
            digestiblefireworks.item.AddItem("locked_digestiblefireworks.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Joy", "Digestible Fireworks", "Rusted Surgeon's Tools", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Joy_CH", "Aprils_DigestibleFireworks_SW", "Aprils_RustedSurgeonsTools_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Joy_Heaven_ACH";
        public static string OsmanACH => "Aprils_Joy_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Joy_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Joy_Osman_Unlock";
    }
}
