using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Six
    {
        public static void Add()
        {
            ExtraCCSprites_BasicSO sixExtra = ScriptableObject.CreateInstance<ExtraCCSprites_BasicSO>();
            sixExtra._DefaultID = "Unfocused";
            sixExtra._SpecialID = "Focused";
            sixExtra._frontSprite = ResourceLoader.LoadSprite("SixFocused.png");
            sixExtra._backSprite = ResourceLoader.LoadSprite("SixBack.png");

            Character six = new Character("Six", "Six_CH");
            six.HealthColor = Pigments.Red;
            six.AddUnitType("FemaleID");
            six.AddUnitType("Sandwich_Robot");
            six.UsesBasicAbility = true;
            //slap
            six.UsesAllAbilities = false;
            six.MovesOnOverworld = true;
            //animator
            six.FrontSprite = ResourceLoader.LoadSprite("SixFront.png");
            six.BackSprite = ResourceLoader.LoadSprite("SixBack.png");
            six.OverworldSprite = ResourceLoader.LoadSprite("SixWorld.png", new Vector2(0.5f, 0f));
            six.ExtraSprites = sixExtra;
            six.DamageSound = LoadedAssetsHandler.GetCharacter("Mung_CH").damageSound;
            six.DeathSound = LoadedAssetsHandler.GetCharacter("Mung_CH").deathSound;
            six.DialogueSound = LoadedAssetsHandler.GetCharacter("Mung_CH").dxSound;
            six.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            six.AddFinalBossAchievementData("Heaven", HeavenACH);
            six.GenerateMenuCharacter(ResourceLoader.LoadSprite("SixMenu.png"), ResourceLoader.LoadSprite("SixLock.png"));
            six.MenuCharacterIsSecret = true;
            six.MenuCharacterIgnoreRandom = true;
            six.SetMenuCharacterAsFullDPS();

            DamageEffect returnkill = ScriptableObject.CreateInstance<DamageEffect>();
            returnkill._returnKillAsSuccess = true;
            Ability days1 = new Ability("Six Days", "Six_Days_1_A");
            days1.Description = "Deal 3 damage to the Opposing enemy.\nIf this ability kills, instantly flee and spawn a permenant copy of this character.";
            days1.AbilitySprite = ResourceLoader.LoadSprite("ability_days.png");
            days1.Cost = [Pigments.Red, Pigments.Red];
            days1.Effects = new EffectInfo[4];
            days1.Effects[0] = Effects.GenerateEffect(returnkill, 3, Slots.Front);
            days1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageByCostEffect>(), 1, Slots.Self, BasicEffects.DidThat(true));
            days1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<FleeTargetEffect>(), 1, Slots.Self, BasicEffects.DidThat(true, 2));
            days1.Effects[3] = Effects.GenerateEffect(ScriptableObject.CreateInstance<CopyAndSpawnPermenantCharacterExhaustedInSelfSlotEffect>(), 1, Slots.Self, BasicEffects.DidThat(true, 3));
            days1.AddIntentsToTarget(Slots.Front, ["Damage_3_6"]);
            days1.AddIntentsToTarget(Slots.Self, ["PA_Fleeting", "Other_Spawn"]);
            days1.Visuals = LoadedAssetsHandler.GetEnemyAbility("Chomp_A").visuals;
            days1.AnimationTarget = Slots.Front;

            Ability days2 = new Ability(days1.ability, "Six_Days_2_A", days1.Cost);
            days2.Name = "Six Hours";
            days2.Description = "Deal 4 damage to the Opposing enemy.\nIf this ability kills, instantly flee and spawn a permenant copy of this character.";
            days2.Effects[0].entryVariable = 4;

            Ability days3 = new Ability(days2.ability, "Six_Days_3_A", days1.Cost);
            days3.Name = "Six Minutes";
            days3.Description = "Deal 5 damage to the Opposing enemy.\nIf this ability kills, instantly flee and spawn a permenant copy of this character.";
            days3.Effects[0].entryVariable = 5;

            Ability days4 = new Ability(days3.ability, "Six_Days_4_A", days1.Cost);
            days4.Name = "Six Seconds";
            days4.Description = "Deal 6 damage to the Opposing enemy.\nIf this ability kills, instantly flee and spawn a permenant copy of this character.";
            days4.Effects[0].entryVariable = 6;

            Ability fingers1 = new Ability("Six Fingers", "Six_Fingers_1_A");
            fingers1.Description = "Deal 4 damage to the Opposing enemy.";
            fingers1.AbilitySprite = ResourceLoader.LoadSprite("ability_fingers.png");
            fingers1.Cost = [Pigments.Red, Pigments.Yellow];
            fingers1.Effects = new EffectInfo[1];
            fingers1.Effects[0] = Effects.GenerateEffect(returnkill, 4, Slots.Front);
            fingers1.AddIntentsToTarget(Slots.Front, ["Damage_3_6"]);
            fingers1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Shank_1_A").visuals;
            fingers1.AnimationTarget = Slots.Front;

            Ability fingers2 = new Ability(fingers1.ability, "Six_Fingers_2_A", fingers1.Cost);
            fingers2.Name = "Six Hands";
            fingers2.Description = "Deal 6 damage to the Opposing enemy.";
            fingers2.Effects[0].entryVariable = 6;

            Ability fingers3 = new Ability(fingers2.ability, "Six_Fingers_3_A", fingers1.Cost);
            fingers3.Name = "Six Arms";
            fingers3.Description = "Deal 7 damage to the Opposing enemy.";
            fingers3.Effects[0].entryVariable = 7;
            fingers3.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability fingers4 = new Ability(fingers3.ability, "Six_Fingers_4_A", fingers1.Cost);
            fingers4.Name = "Six Bodies";
            fingers4.Description = "Deal 8 damage to the Opposing enemy.";
            fingers4.Effects[0].entryVariable = 8;

            Ability hearts1 = new Ability("Six Thoughts", "Six_Hearts_1_A");
            hearts1.Description = "Instantly flee and spawn a permenant copy of this party member with Focused.";
            hearts1.AbilitySprite = ResourceLoader.LoadSprite("ability_hearts.png");
            hearts1.Cost = [Pigments.Purple, Pigments.Purple];
            hearts1.Effects = new EffectInfo[4];
            hearts1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageByCostEffect>(), 1, Slots.Self);
            hearts1.Effects[1] = Effects.GenerateEffect(days1.Effects[2].effect, 1, Slots.Self);
            hearts1.Effects[2] = Effects.GenerateEffect(days1.Effects[3].effect, 1, Slots.Self);
            hearts1.Effects[3] = Effects.GenerateEffect(CasterRootActionEffect.Create(Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyFocusedEffect>(), 1, Slots.Self).SelfArray()));
            hearts1.AddIntentsToTarget(Slots.Self, ["PA_Fleeting", "Other_Spawn", "Status_Focused"]);
            hearts1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Wrath_1_A").visuals;
            hearts1.AnimationTarget = Slots.Self;

            Ability hearts2 = new Ability(hearts1.ability, "Six_Hearts_2_A", hearts1.Cost);
            hearts2.Name = "Six Hearts";

            Ability hearts3 = new Ability(hearts2.ability, "Six_Hearts_3_A", hearts1.Cost);
            hearts3.Name = "Six Minds";

            Ability hearts4 = new Ability(hearts3.ability, "Six_Hearts_4_A", hearts1.Cost);
            hearts4.Name = "Six Souls";

            six.AddLevelData(6, [hearts1, days1, fingers1]);
            six.AddLevelData(6, [hearts2, days2, fingers2]);
            six.AddLevelData(6, [hearts3, days3, fingers3]);
            six.AddLevelData(6, [hearts4, days4, fingers4]);
            six.AddCharacter(true);
            BlockFromShops.Add(six.character);
        }
        public static void Items()
        {
            PercentageEffectorCondition chance = ScriptableObject.CreateInstance<PercentageEffectorCondition>();
            chance.triggerPercentage = 18;
            MultiPerformEffectItem goldfish = new MultiPerformEffectItem("Aprils_Goldfish_EW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), 2, Slots.Self)]);
            goldfish.Name = "Goldfish";
            goldfish.Flavour = "\"You Caught a... Goldfish! 15cm.\"";
            goldfish.Description = "On any ability being used, heal 2 health. 18% chance to be destroyed upon activation.\nProduce 1 coin at the end of combat.";
            goldfish.Icon = ResourceLoader.LoadSprite("item_goldfish.png");
            goldfish.EquippedModifiers = [];
            goldfish.TriggerOn = TriggerCalls.OnAnyAbilityUsed;
            goldfish.DoesPopUpInfo = true;
            goldfish.Conditions = [];
            goldfish.DoesActionOnTriggerAttached = false;
            goldfish.ConsumeOnTrigger = TriggerCalls.Count;
            goldfish.ConsumeOnUse = true;
            goldfish.ConsumeConditions = [chance];
            goldfish.ShopPrice = 4;
            goldfish.IsShopItem = false;
            goldfish.StartsLocked = true;
            goldfish.OnUnlockUsesTHE = true;
            goldfish.UsesSpecialUnlockText = true;
            goldfish.SpecialUnlockID = UILocID.ItemFishLocationLabel;
            EffectTrigger second = new EffectTrigger([Effects.GenerateEffect(ScriptableObject.CreateInstance<GainPlayerCurrencyEffect>(), 1)], [TriggerCalls.OnCombatEnd], []);
            second.SetConsumeInfo(true, []);
            goldfish.AddEffectTrigger(second);
            goldfish.Item.AddFishItem(2, "locked_goldfish.png", OsmanACH);

            PerformEffect_Item devilspurse = new PerformEffect_Item("Aprils_DevilsPurse_EW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), 15, Slots.Self)]);
            devilspurse.Name = "Devil's Purse";
            devilspurse.Flavour = "\'You Caught a... Shark Egg Sac! 15 cm.\"";
            devilspurse.Description = "At the start of combat, heal 15 health and destroy this item.";
            devilspurse.Icon = ResourceLoader.LoadSprite("item_devilspurse.png");
            devilspurse.EquippedModifiers = [];
            devilspurse.TriggerOn = TriggerCalls.OnCombatStart;
            devilspurse.DoesPopUpInfo = true;
            devilspurse.Conditions = [];
            devilspurse.DoesActionOnTriggerAttached = false;
            devilspurse.ConsumeOnTrigger = TriggerCalls.Count;
            devilspurse.ConsumeOnUse = true;
            devilspurse.ConsumeConditions = [];
            devilspurse.ShopPrice = 3;
            devilspurse.IsShopItem = false;
            devilspurse.StartsLocked = true;
            devilspurse.OnUnlockUsesTHE = true;
            devilspurse.UsesSpecialUnlockText = true;
            devilspurse.SpecialUnlockID = UILocID.ItemFishLocationLabel;
            devilspurse.item.AddFishItem(3, "locked_devilspurse.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Six", "Devil's Purse", "Goldfish", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Six_CH", "Aprils_DevilsPurse_EW", "Aprils_Goldfish_EW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Six_Heaven_ACH";
        public static string OsmanACH => "Aprils_Six_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Six_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Six_Osman_Unlock";
    }
}
