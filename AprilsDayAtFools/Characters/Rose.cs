using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Rose
    {
        public static void Add()
        {
            Character rose = new Character("Rose", "Rose_CH");
            rose.HealthColor = Pigments.Red;
            rose.AddUnitType("FemaleID");
            rose.AddUnitType("Sandwich_Gore");
            rose.AddUnitType("FemaleLooking");
            rose.UsesBasicAbility = true;
            //slap
            rose.UsesAllAbilities = false;
            rose.MovesOnOverworld = true;
            rose.Animator = Joyce.Assets.LoadAsset<RuntimeAnimatorController>("Assets/RoseAnim/RoseAnimator.overrideController");
            rose.FrontSprite = ResourceLoader.LoadSprite("RoseFront.png");
            rose.BackSprite = ResourceLoader.LoadSprite("RoseBack.png");
            rose.OverworldSprite = ResourceLoader.LoadSprite("RoseWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            rose.DamageSound = LoadedAssetsHandler.GetEnemy("JumbleGuts_Flummoxing_EN").damageSound;
            rose.DeathSound = LoadedAssetsHandler.GetEnemy("JumbleGuts_Flummoxing_EN").deathSound;
            rose.DialogueSound = LoadedAssetsHandler.GetEnemy("JumbleGuts_Flummoxing_EN").damageSound;
            rose.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            rose.AddFinalBossAchievementData("Heaven", HeavenACH);
            rose.GenerateMenuCharacter(ResourceLoader.LoadSprite("RoseMenu.png"), ResourceLoader.LoadSprite("RoseLock.png"));
            rose.MenuCharacterIsSecret = false;
            rose.MenuCharacterIgnoreRandom = false;
            rose.SetMenuCharacterAsFullDPS();
            rose.AddPassive(Passives.Slippery);

            DamageEffect returnkill = ScriptableObject.CreateInstance<DamageEffect>();
            returnkill._returnKillAsSuccess = true;
            ApplyPowerEffect power = ScriptableObject.CreateInstance<ApplyPowerEffect>();
            Ability cleave1 = new Ability("Careless Cleave", "Rose_Cleave_1_A");
            cleave1.Description = "Deal 5 damage to the Opposing and either the Left or Right enemies.\nGain 2 Power. Gain another 2 if it kills.";
            cleave1.AbilitySprite = ResourceLoader.LoadSprite("ability_cleave.png");
            cleave1.Cost = [Pigments.Red, Pigments.Red, Pigments.Red];
            cleave1.Effects = new EffectInfo[7];
            cleave1.Effects[0] = Effects.GenerateEffect(BasicEffects.GetVisuals("Salt/Decapitate", false, Targeting.Slot_FrontAndRight), 0, Targeting.Slot_FrontAndRight, Effects.ChanceCondition(50));
            cleave1.Effects[1] = Effects.GenerateEffect(returnkill, 5, cleave1.Effects[0].targets, BasicEffects.DidThat(true));
            cleave1.Effects[2] = Effects.GenerateEffect(power, 2, Slots.Self, BasicEffects.DidThat(true));
            cleave1.Effects[3] = Effects.GenerateEffect(BasicEffects.GetVisuals("Salt/Decapitate", false, Targeting.GenerateSlotTarget([-1, 0], false)), 0, Targeting.GenerateSlotTarget([-1, 0], false), BasicEffects.DidThat(false, 3));
            cleave1.Effects[4] = Effects.GenerateEffect(returnkill, 5, cleave1.Effects[3].targets, BasicEffects.DidThat(true));
            cleave1.Effects[5] = cleave1.Effects[2];
            cleave1.Effects[6] = Effects.GenerateEffect(power, 2, Slots.Self);
            cleave1.AddIntentsToTarget(Slots.FrontLeftRight, ["Damage_3_6"]);
            cleave1.AddIntentsToTarget(Slots.Self, [Power.Intent]);

            Ability cleave2 = new Ability(cleave1.ability, "Rose_Cleave_2_A", cleave1.Cost);
            cleave2.Name = "Quick Cleave";
            cleave2.Description = "Deal 7 damage to the Opposing and either the Left or Right enemies.\nGain 2 Power. Gain another 2 if it kills.";
            cleave2.Effects[1].entryVariable = 7;
            cleave2.Effects[4].entryVariable = 7;
            cleave2.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability cleave3 = new Ability(cleave2.ability, "Rose_Cleave_3_A", cleave1.Cost);
            cleave3.Name = "Bloody Cleave";
            cleave3.Description = "Deal 9 damage to the Opposing and either the Left or Right enemies.\nGain 2 Power. Gain another 2 if it kills.";
            cleave3.Effects[1].entryVariable = 9;
            cleave3.Effects[4].entryVariable = 9;

            Ability cleave4 = new Ability(cleave3.ability, "Rose_Cleave_4_A", cleave1.Cost);
            cleave4.Name = "Cruel Cleave";
            cleave4.Description = "Deal 11 damage to the Opposing and either the Left or Right enemies.\nGain 2 Power. Gain another 2 if it kills.";
            cleave4.Effects[1].entryVariable = 11;
            cleave4.Effects[4].entryVariable = 11;
            cleave4.EffectIntents[0].intents[0] = "Damage_11_15";

            RefreshAbilityUseEffect refresh = ScriptableObject.CreateInstance<RefreshAbilityUseEffect>();
            DamageEffect returnkil2 = ScriptableObject.CreateInstance<DamageEffect>();
            returnkil2._returnKillAsSuccess = true;
            DamageEffect returnkil4 = ScriptableObject.CreateInstance<DamageEffect>();
            returnkil4._returnKillAsSuccess = true;
            DoubleTargetting doubleFront = ScriptableObject.CreateInstance<DoubleTargetting>();
            doubleFront.firstTargetting = Slots.Front;
            doubleFront.secondTargetting = Slots.Front;
            Ability muti1 = new Ability("Messy Mutilation", "Rose_Muti_1_A");
            muti1.Description = "Deal 2 damage to the Opposing enemy twice, refreshing this party member's ability usage if this kills.\nGain 2 Power.";
            muti1.AbilitySprite = ResourceLoader.LoadSprite("ability_mutilate.png");
            muti1.Cost = [Pigments.Red, Pigments.Yellow];
            muti1.Effects = new EffectInfo[3];
            muti1.Effects[0] = Effects.GenerateEffect(returnkil2, 2, doubleFront);
            muti1.Effects[1] = Effects.GenerateEffect(refresh, 1, Slots.Self, BasicEffects.DidThat(true));
            muti1.Effects[2] = cleave1.Effects[6];
            muti1.AddIntentsToTarget(Slots.Front, ["Damage_1_2", "Damage_1_2"]);
            muti1.AddIntentsToTarget(Slots.Self, ["Misc_Additional", Power.Intent]);
            muti1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Cacophony_1_A").visuals;
            muti1.AnimationTarget = Slots.Front;

            Ability muti2 = new Ability(muti1.ability, "Rose_Muti_2_A", muti1.Cost);
            muti2.Name = "Violent Mutilation";
            muti2.Description = "Deal 3 damage to the Opposing enemy twice, refreshing this party member's ability usage if this kills.\nGain 2 Power.";
            muti2.Effects[0].entryVariable = 3;
            muti2.EffectIntents[0].intents[0] = "Damage_3_6";
            muti2.EffectIntents[0].intents[1] = "Damage_3_6";

            Ability muti3 = new Ability(muti2.ability, "Rose_Muti_3_A", muti1.Cost);
            muti3.Name = "Deliberate Mutilation";
            muti3.Description = "Deal 4 damage to the Opposing enemy twice, refreshing this party member's ability usage if this kills.\nGain 2 Power.";
            muti3.Effects[0].entryVariable = 4;

            Ability muti4 = new Ability(muti3.ability, "Rose_Muti_4_A", muti1.Cost);
            muti4.Name = "Artistic Mutilation";
            muti4.Description = "Deal 5 damage to the Opposing enemy twice, refreshing this party member's ability usage if this kills.\nGain 2 Power.";
            muti4.Effects[0].entryVariable = 5;

            RemoveStatusEffectEffect remPower = ScriptableObject.CreateInstance<RemoveStatusEffectEffect>();
            remPower._status = Power.Object;
            DamageEffect returnkil3 = ScriptableObject.CreateInstance<DamageEffect>();
            returnkil3._returnKillAsSuccess = true;
            Intents.CreateAndAddCustom_Basic_IntentToPool("Rem_Status_Power", ResourceLoader.LoadSprite("Power"), Intents.GetInGame_IntentInfo(IntentType_GameIDs.Rem_Status_Ruptured)._color);
            Ability bodies1 = new Ability("Snapping Tendons", "Rose_Bodies_1_A");
            bodies1.Description = "Deal 7 damage to the Opposing enemy.\nRemove all Power from this party member if this doesn't kill.";
            bodies1.AbilitySprite = ResourceLoader.LoadSprite("ability_bodies.png");
            bodies1.Cost = [Pigments.Red, Pigments.Red, Pigments.Yellow];
            bodies1.Effects = new EffectInfo[2];
            bodies1.Effects[0] = Effects.GenerateEffect(returnkil3, 7, Slots.Front);
            bodies1.Effects[1] = Effects.GenerateEffect(remPower, 1, Slots.Self, BasicEffects.DidThat(false));
            bodies1.AddIntentsToTarget(Slots.Front, ["Damage_7_10"]);
            bodies1.AddIntentsToTarget(Slots.Self, ["Rem_Status_Power"]);
            bodies1.Visuals = CustomVisuals.GetVisuals("Salt/Piano");
            bodies1.AnimationTarget = Slots.Front;

            Ability bodies2 = new Ability(bodies1.ability, "Rose_Bodies_2_A", bodies1.Cost);
            bodies2.Name = "Snapping Bones";
            bodies2.Description = "Deal 9 damage to the Opposing enemy.\nRemove all Power from this party member if this doesn't kill.";
            bodies2.Effects[0].entryVariable = 9;

            Ability bodies3 = new Ability(bodies2.ability, "Rose_Bodies_3_A", bodies1.Cost);
            bodies3.Name = "Snapping Organs";
            bodies3.Description = "Deal 11 damage to the Opposing enemy.\nRemove all Power from this party member if this doesn't kill.";
            bodies3.Effects[0].entryVariable = 11;
            bodies3.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability bodies4 = new Ability(bodies3.ability, "Rose_Bodies_4_A", bodies1.Cost);
            bodies4.Name = "Snapping Bodies";
            bodies4.Description = "Deal 14 damage to the Opposing enemy.\nRemove all Power from this party member if this doesn't kill.";
            bodies4.Effects[0].entryVariable = 14;

            rose.AddLevelData(12, [muti1, cleave1, bodies1]);
            rose.AddLevelData(15, [muti2, cleave2, bodies2]);
            rose.AddLevelData(17, [muti3, cleave3, bodies3]);
            rose.AddLevelData(19, [muti4, cleave4, bodies4]);
            rose.AddCharacter(true);

        }
        public static void Items()
        {
            PercentCurrentHealthDamageEffect loseHealth = ScriptableObject.CreateInstance<PercentCurrentHealthDamageEffect>();
            loseHealth._indirect = true;
            MultiPerformEffectItem toothgun = new MultiPerformEffectItem("Aprils_ToothGun_TW", [Effects.GenerateEffect(loseHealth, 10, Slots.Self)]);
            toothgun.Name = "Tooth Gun";
            toothgun.Flavour = "\"It hurts.\"";
            toothgun.Description = "Increase damage dealt by this party member's missing health.\nOn dealing damage, take 10% of this party member's current health as indirect damage.";
            toothgun.Icon = ResourceLoader.LoadSprite("item_toothgun.png");
            toothgun.EquippedModifiers = [];
            toothgun.TriggerOn = TriggerCalls.OnDidApplyDamage;
            toothgun.DoesPopUpInfo = true;
            toothgun.Conditions = [];
            toothgun.DoesActionOnTriggerAttached = false;
            toothgun.ConsumeOnTrigger = TriggerCalls.Count;
            toothgun.ConsumeOnUse = false;
            toothgun.ConsumeConditions = [];
            toothgun.ShopPrice = 8;
            toothgun.IsShopItem = false;
            toothgun.StartsLocked = true;
            toothgun.OnUnlockUsesTHE = true;
            toothgun.UsesSpecialUnlockText = false;
            toothgun.SpecialUnlockID = UILocID.None;
            EffectTrigger second = new EffectTrigger([], [TriggerCalls.OnWillApplyDamage], [ScriptableObject.CreateInstance<DamageByMissingHealthCondition>()], false);
            toothgun.AddEffectTrigger(second);
            toothgun.Item.AddItem("locked_toothgun.png", OsmanACH);

            MultiPerformEffectItem wristcutter = new MultiPerformEffectItem("Aprils_WristCutter_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyScarsEffect>(), 1, Slots.Self)]);
            wristcutter.Name = "Wrist Cutter";
            wristcutter.Flavour = "\"Wrist Cuter\"";
            wristcutter.Description = "Increase damage dealt by 25%.\nGain 1 Scar at the start of each turn.";
            wristcutter.Icon = ResourceLoader.LoadSprite("item_wristcutter.png");
            wristcutter.EquippedModifiers = [];
            wristcutter.TriggerOn = TriggerCalls.OnTurnStart;
            wristcutter.DoesPopUpInfo = true;
            wristcutter.Conditions = [];
            wristcutter.DoesActionOnTriggerAttached = false;
            wristcutter.ConsumeOnTrigger = TriggerCalls.Count;
            wristcutter.ConsumeOnUse = false;
            wristcutter.ConsumeConditions = [];
            wristcutter.ShopPrice = 6;
            wristcutter.IsShopItem = true;
            wristcutter.StartsLocked = true;
            wristcutter.OnUnlockUsesTHE = true;
            wristcutter.UsesSpecialUnlockText = false;
            wristcutter.SpecialUnlockID = UILocID.None;
            EffectTrigger other = new EffectTrigger([], [TriggerCalls.OnWillApplyDamage], [IncreaseDamagePercentageCondition.Create(25)], false);
            wristcutter.AddEffectTrigger(other);
            wristcutter.item._ItemTypeIDs = ["Knife"];
            wristcutter.item.AddItem("locked_wristcutter.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Rose", "Wrist Cutter", "Tooth Gun", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Rose_CH", "Aprils_WristCutter_SW", "Aprils_ToothGun_TW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Rose_Heaven_ACH";
        public static string OsmanACH => "Aprils_Rose_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Rose_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Rose_Osman_Unlock";
    }
}
