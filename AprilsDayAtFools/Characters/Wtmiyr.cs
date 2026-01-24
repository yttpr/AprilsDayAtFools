using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Wtmiyr
    {
        public static void Add()
        {
            Character wtmiyr = new Character("Wtmiyr", "Wtmiyr_CH");
            wtmiyr.HealthColor = Pigments.Purple;
            wtmiyr.AddUnitType("FemaleID");
            wtmiyr.AddUnitType("Sandwich_Silly");
            wtmiyr.AddUnitType("FemaleLooking");
            wtmiyr.UsesBasicAbility = true;
            //slap
            wtmiyr.UsesAllAbilities = false;
            wtmiyr.MovesOnOverworld = true;
            //animator
            wtmiyr.FrontSprite = ResourceLoader.LoadSprite("WtmiyrFront.png");
            wtmiyr.BackSprite = ResourceLoader.LoadSprite("WtmiyrBack.png");
            wtmiyr.OverworldSprite = ResourceLoader.LoadSprite("WtmiyrWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            wtmiyr.DamageSound = LoadedAssetsHandler.GetEnemy("SingingStone_EN").damageSound;
            wtmiyr.DeathSound = LoadedAssetsHandler.GetEnemy("SingingStone_EN").deathSound;
            wtmiyr.DialogueSound = LoadedAssetsHandler.GetEnemy("SingingStone_EN").damageSound;
            wtmiyr.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            wtmiyr.AddFinalBossAchievementData("Heaven", HeavenACH);
            wtmiyr.GenerateMenuCharacter(ResourceLoader.LoadSprite("WtmiyrMenu.png"), ResourceLoader.LoadSprite("WtmiyrLock.png"));
            wtmiyr.MenuCharacterIsSecret = false;
            wtmiyr.MenuCharacterIgnoreRandom = false;
            wtmiyr.SetMenuCharacterAsFullDPS();

            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool(IDs.Clash, "Xkadh -{0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Negative));

            DamageByStoredValueEffect clashdamage = ScriptableObject.CreateInstance<DamageByStoredValueEffect>();
            clashdamage._increaseDamage = false;
            clashdamage.m_unitStoredDataID = IDs.Clash;

            Ability clash1 = new Ability("Ctssd Xkadh", "Wtmiyr_Clash_1_A");
            clash1.Description = "Deal 18 damage to the Opposing enemy.\nReduce this move's damage by the amount of damage dealt.";
            clash1.AbilitySprite = ResourceLoader.LoadSprite("ability_clash.png");
            clash1.Cost = [Pigments.Red, Pigments.Red, Pigments.Red];
            clash1.Effects = new EffectInfo[2];
            clash1.Effects[0] = Effects.GenerateEffect(clashdamage, 18, Slots.Front);
            clash1.Effects[1] = Effects.GenerateEffect(ChanageValueByPreviousEffect.Create(IDs.Clash, true));
            clash1.AddIntentsToTarget(Slots.Front, ["Damage_16_20"]);
            clash1.AddIntentsToTarget(Slots.Self, ["Misc"]);
            clash1.Visuals = CustomVisuals.GetVisuals("Salt/Cannon");
            clash1.AnimationTarget = Slots.Front;
            clash1.UnitStoreData = UnitStoreData.GetCustom_UnitStoreData(IDs.Clash);

            Ability clash2 = new Ability(clash1.ability, "Wtmiyr_Clash_2_A", clash1.Cost);
            clash2.Name = "Nradh Xkadh";
            clash2.Description = "Deal 24 damage to the Opposing enemy.\nReduce this move's damage by the amount of damage dealt.";
            clash2.Effects[0].entryVariable = 24;
            clash2.EffectIntents[0].intents[0] = "Damage_21";

            Ability clash3 = new Ability(clash2.ability, "Wtmiyr_Clash_3_A", clash1.Cost);
            clash3.Name = "Tspif Xkadh";
            clash3.Description = "Deal 32 damage to the Opposing enemy.\nReduce this move's damage by the amount of damage dealt.";
            clash3.Effects[0].entryVariable = 32;

            Ability clash4 = new Ability(clash3.ability, "Wtmiyr_Clash_4_A", clash1.Cost);
            clash4.Name = "Afhressibr Xkadh";
            clash4.Description = "Deal 40 damage to the Opposing enemy.\nReduce this move's damage by the amount of damage dealt.";
            clash4.Effects[0].entryVariable = 40;

            DamageByStoredValueEffect warmupdamage = ScriptableObject.CreateInstance<DamageByStoredValueEffect>();
            warmupdamage._increaseDamage = true;
            warmupdamage.m_unitStoredDataID = IDs.Warmup;
            CasterCapValueEffect warmupeffect = ScriptableObject.CreateInstance<CasterCapValueEffect>();
            warmupeffect.value = IDs.Warmup;

            Ability warmup1 = new Ability("Jliw Qarmuip", "Wtmiyr_Warmup_1_A");
            warmup1.Description = "Deal 2 damage to the Opposing enemy. If successful, lower this move's damage down to 5.\nWhenever any party member deals damage, increase this move's damage by 1.";
            warmup1.AbilitySprite = ResourceLoader.LoadSprite("ability_warmup.png");
            warmup1.Cost = [Pigments.Yellow, Pigments.Red];
            warmup1.Effects = new EffectInfo[2];
            warmup1.Effects[0] = Effects.GenerateEffect(warmupdamage, 2, Slots.Front);
            warmup1.Effects[1] = Effects.GenerateEffect(warmupeffect, 3, Slots.Self, BasicEffects.DidThat(true));
            warmup1.AddIntentsToTarget(Slots.Front, ["Damage_1_2"]);
            warmup1.AddIntentsToTarget(Slots.Self, ["Misc"]);
            warmup1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Parry_1_A").visuals;
            warmup1.AnimationTarget = Slots.Front;
            warmup1.UnitStoreData = UnitStoreData.GetCustom_UnitStoreData(IDs.Warmup);

            Ability warmup2 = new Ability(warmup1.ability, "Wtmiyr_Warmup_2_A", warmup1.Cost);
            warmup2.Name = "Dteadt Qarmuip";
            warmup2.Description = "Deal 3 damage to the Opposing enemy. If successful, lower this move's damage down to 7.\nWhenever any party member deals damage, increase this move's damage by 1.";
            warmup2.Effects[0].entryVariable = 3;
            warmup2.Effects[1].entryVariable = 4;
            warmup2.EffectIntents[0].intents[0] = "Damage_3_6";

            Ability warmup3 = new Ability(warmup2.ability, "Wtmiyr_Warmup_3_A", warmup1.Cost);
            warmup3.Name = "Xsutopus Qarmuip";
            warmup3.Description = "Deal 4 damage to the Opposing enemy. If successful, lower this move's damage down to 8.\nWhenever any party member deals damage, increase this move's damage by 1.";
            warmup3.Effects[0].entryVariable = 4;

            Ability warmup4 = new Ability(warmup3.ability, "Wtmiyr_Warmup_4_A", warmup1.Cost);
            warmup4.Name = "Yavtixsl Qarmuip";
            warmup4.Description = "Deal 5 damage to the Opposing enemy. If successful, lower this move's damage down to 10.\nWhenever any party member deals damage, increase this move's damage by 1.";
            warmup4.Effects[0].entryVariable = 5;
            warmup4.Effects[1].entryVariable = 5;

            ChanageValueByPreviousEffect ewlaueffect = ScriptableObject.CreateInstance<ChanageValueByPreviousEffect>();
            ewlaueffect._increase = true;
            ewlaueffect.m_unitStoredDataID = IDs.Ewlau;

            Ability ewlau1 = new Ability("Ewlau Vjallemhe", "Wtmiyr_Ewlau_1_A");
            ewlau1.Description = "Deal 2 damage to this party member and convert it into a damage boost benefitting the entire party. This ability will not reduce health below 1.\nHeal this party member 3 health if this ability did not deal any damage.";
            ewlau1.AbilitySprite = ResourceLoader.LoadSprite("ability_relay.png");
            ewlau1.Cost = [Pigments.Yellow, Pigments.Red, Pigments.Blue];
            ewlau1.Effects = new EffectInfo[3];
            ewlau1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<NoKillingDamageEffect>(), 2, Slots.Self);
            ewlau1.Effects[1] = Effects.GenerateEffect(ewlaueffect);
            ewlau1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), 3, Slots.Self, BasicEffects.DidThat(false, 2));
            ewlau1.AddIntentsToTarget(Slots.Self, ["Damage_1_2", "Misc", "Heal_1_4"]);
            ewlau1.Visuals = CustomVisuals.GetVisuals("Salt/Think");
            ewlau1.AnimationTarget = Slots.Self;
            ewlau1.UnitStoreData = UnitStoreData.GetCustom_UnitStoreData(IDs.Ewlau);

            Ability ewlau2 = new Ability(ewlau1.ability, "Wtmiyr_Ewlau_2_A", ewlau1.Cost);
            ewlau2.Name = "Ewlau Tsxe";
            ewlau2.Description = "Deal 2 damage to this party member and convert it into a damage boost benefitting the entire party. This ability will not reduce health below 1.\nHeal this party member 5 health if this ability did not deal any damage.";
            ewlau2.Effects[2].entryVariable = 5;
            ewlau2.EffectIntents[0].intents[2] = "Heal_5_10";

            Ability ewlau3 = new Ability(ewlau2.ability, "Wtmiyr_Ewlau_3_A", [Pigments.Red, Pigments.Blue]);
            ewlau3.Name = "Ewlau Yruak";
            ewlau3.Description = "Deal 4 damage to this party member and convert it into a damage boost benefitting the entire party. This ability will not reduce health below 1.\nHeal this party member 6 health if this ability did not deal any damage.";
            ewlau3.Effects[0].entryVariable = 4;
            ewlau3.Effects[2].entryVariable = 6;
            ewlau3.EffectIntents[0].intents[0] = "Damage_3_6";

            Ability ewlau4 = new Ability(ewlau3.ability, "Wtmiyr_Ewlau_4_A", ewlau3.Cost);
            ewlau4.Name = "Ewlau Wst";
            ewlau4.Description = "Deal 5 damage to this party member and convert it into a damage boost benefitting the entire party. This ability will not reduce health below 1.\nHeal this party member 8 health if this ability did not deal any damage.";
            ewlau4.Effects[0].entryVariable = 5;
            ewlau4.Effects[2].entryVariable = 8;


            wtmiyr.AddLevelData(10, [ewlau1, clash1, warmup1]);
            wtmiyr.AddLevelData(12, [ewlau2, clash2, warmup2]);
            wtmiyr.AddLevelData(14, [ewlau3, clash3, warmup3]);
            wtmiyr.AddLevelData(16, [ewlau4, clash4, warmup4]);
            wtmiyr.AddCharacter(true);
        }
        public static void Items()
        {
            PerformEffect_Item toolsofwar = new PerformEffect_Item("Aprils_ToolsOfWar_SW", []);
            toolsofwar.Name = "Tools of War";
            toolsofwar.Flavour = "\"The works of the wicked\"";
            toolsofwar.Description = "On dealing damage, attempt to deal the damage to self then use the value against the target.";
            toolsofwar.Icon = ResourceLoader.LoadSprite("item_toolsofwar.png");
            toolsofwar.EquippedModifiers = [];
            toolsofwar.TriggerOn = TriggerCalls.OnWillApplyDamage;
            toolsofwar.DoesPopUpInfo = false;
            toolsofwar.Conditions = [ScriptableObject.CreateInstance<ToolsOfWarCondition>()];
            toolsofwar.DoesActionOnTriggerAttached = false;
            toolsofwar.ConsumeOnTrigger = TriggerCalls.Count;
            toolsofwar.ConsumeOnUse = false;
            toolsofwar.ConsumeConditions = [];
            toolsofwar.ShopPrice = 10;
            toolsofwar.IsShopItem = true;
            toolsofwar.StartsLocked = true;
            toolsofwar.OnUnlockUsesTHE = true;
            toolsofwar.UsesSpecialUnlockText = false;
            toolsofwar.SpecialUnlockID = UILocID.None;
            toolsofwar.item._ItemTypeIDs = ["Knife", "Face"];
            toolsofwar.Item.AddItem("locked_toolsofwar.png", OsmanACH);

            Ability purity = new Ability("Purity of Mind", "PurityOfMind_A");
            purity.Description = "Reset all stored values on all party members.";
            purity.AbilitySprite = ResourceLoader.LoadSprite("purity_a.png");
            purity.Cost = [Pigments.Purple, Pigments.Purple, Pigments.Purple];
            purity.Effects = Effects.GenerateEffect(ScriptableObject.CreateInstance<TargetResetStoredValueEffect>(), 0, Targeting.Unit_AllAllies).SelfArray();
            purity.AddIntentsToTarget(Targeting.Unit_AllAllies, ["Misc"]);
            purity.Visuals = LoadedAssetsHandler.GetEnemyAbility("Repent_A").visuals;
            purity.AnimationTarget = Slots.Self;
            ExtraAbility_Wearable_SMS ability = ScriptableObject.CreateInstance<ExtraAbility_Wearable_SMS>();
            ability._extraAbility = purity.GenerateCharacterAbility();
            Basic_Item purityofmind = new Basic_Item("Aprils_PurityOfMind_TW");
            purityofmind.Name = "Purity of Mind";
            purityofmind.Flavour = "\"Nothing is real.\"";
            purityofmind.Description = "Adds the extra ability \"Purity of Mind\", which resets the stored values on all allies.";
            purityofmind.Icon = ResourceLoader.LoadSprite("item_purityofmind.png");
            purityofmind.EquippedModifiers = [ability];
            purityofmind.TriggerOn = TriggerCalls.Count;
            purityofmind.DoesPopUpInfo = false;
            purityofmind.Conditions = [];
            purityofmind.ConsumeOnTrigger = TriggerCalls.Count;
            purityofmind.ConsumeOnUse = false;
            purityofmind.ConsumeConditions = [];
            purityofmind.ShopPrice = 10;
            purityofmind.IsShopItem = false;
            purityofmind.StartsLocked = true;
            purityofmind.OnUnlockUsesTHE = false;
            purityofmind.UsesSpecialUnlockText = false;
            purityofmind.SpecialUnlockID = UILocID.None;
            purityofmind.item.AddItem("locked_purityofmind.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Wtmiyr", "Purity Of Mind", "Tools Of War", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Wtmiyr_CH", "Aprils_PurityOfMind_TW", "Aprils_ToolsOfWar_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }


        public static string HeavenACH => "Aprils_Wtmiyr_Heaven_ACH";
        public static string OsmanACH => "Aprils_Wtmiyr_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Wtmiyr_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Wtmiyr_Osman_Unlock";
    }
}
