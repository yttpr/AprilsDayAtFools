using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Esther
    {
        public static void Add()
        {
            PerformEffectPassiveAbility eternal = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            eternal._passiveName = "Eternal";
            eternal.passiveIcon = ResourceLoader.LoadSprite("EternityPassive.png");
            eternal.m_PassiveID = IDs.Eternal;
            eternal.name = IDs.Eternal;
            eternal._enemyDescription = "won't do anything.";
            eternal._characterDescription = "At the end of combat, attempt to resurrect this character at 1 health.";
            eternal.doesPassiveTriggerInformationPanel = false;
            eternal.effects = [];
            eternal._triggerOn = [TriggerCalls.Count];
            eternal.AddPassiveToGlossary("Eternal", "At the end of combat, attempt to resurrect this character at 1 health.");
            eternal.AddToPassiveDatabase();

            Character esther = new Character("Esther", "Esther_CH");
            esther.HealthColor = Pigments.Purple;
            esther.AddUnitType("FemaleID");
            esther.AddUnitType("Sandwich_Sprit");
            esther.UsesBasicAbility = true;
            //slap
            esther.UsesAllAbilities = false;
            esther.MovesOnOverworld = true;
            //animator
            esther.FrontSprite = ResourceLoader.LoadSprite("EstherFront.png");
            esther.BackSprite = ResourceLoader.LoadSprite("EstherBack.png");
            esther.OverworldSprite = ResourceLoader.LoadSprite("EstherWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            esther.DamageSound = LoadedAssetsHandler.GetEnemy("Spoggle_Resonant_EN").damageSound;
            esther.DeathSound = LoadedAssetsHandler.GetEnemy("Spoggle_Resonant_EN").deathSound;
            esther.DialogueSound = LoadedAssetsHandler.GetEnemy("Spoggle_Resonant_EN").damageSound;
            esther.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            esther.AddFinalBossAchievementData("Heaven", HeavenACH);
            esther.GenerateMenuCharacter(ResourceLoader.LoadSprite("EstherMenu.png"), ResourceLoader.LoadSprite("EstherLock.png"));
            esther.MenuCharacterIsSecret = false;
            esther.MenuCharacterIgnoreRandom = false;
            //set full dps or support
            esther.AddPassive(eternal);

            AnimationVisualsEffect instance2 = ScriptableObject.CreateInstance<AnimationVisualsEffect>();
            instance2._visuals = LoadedAssetsHandler.GetEnemy("TriggerFingers_BOSS").abilities[3].ability.visuals;
            instance2._animationTarget = Slots.Self;
            Ability bullet1 = new Ability("First Bullet", "Esther_Bullet_1_A");
            bullet1.Description = "Deal 8 damage to the Opposing enemy.\n1/6 chance to instantly kill this party member.";
            bullet1.AbilitySprite = ResourceLoader.LoadSprite("ability_bullet.png");
            bullet1.Cost = [Pigments.Red, Pigments.Red];
            bullet1.Effects = new EffectInfo[3];
            bullet1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 8, Slots.Front);
            bullet1.Effects[1] = Effects.GenerateEffect(instance2, 1, Slots.Self, Effects.ChanceCondition(16));
            bullet1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DirectDeathEffect>(), 1, Slots.Self, BasicEffects.DidThat(true));
            bullet1.AddIntentsToTarget(Slots.Front, ["Damage_7_10"]);
            bullet1.AddIntentsToTarget(Slots.Self, ["Damage_Death"]);
            bullet1.Visuals = CustomVisuals.GetVisuals("Salt/Unlock");
            bullet1.AnimationTarget = Slots.Front;

            Ability bullet2 = new Ability(bullet1.ability, "Esther_Bullet_2_A", bullet1.Cost);
            bullet2.Name = "Second Bullet";
            bullet2.Description = "Deal 10 damage to the Opposing enemy.\n1/6 chance to instantly kill this party member.";
            bullet2.Effects[0].entryVariable = 10;

            Ability bullet3 = new Ability(bullet2.ability, "Esther_Bullet_3_A", bullet1.Cost);
            bullet3.Name = "Third Bullet";
            bullet3.Description = "Deal 13 damage to the Opposing enemy.\n1/6 chance to instantly kill this party member.";
            bullet3.Effects[0].entryVariable = 13;
            bullet3.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability bullet4 = new Ability(bullet3.ability, "Esther_Bullet_4_A", bullet1.Cost);
            bullet4.Name = "Fourth Bullet";
            bullet4.Description = "Deal 16 damage to the Opposing enemy.\n1/6 chance to instantly kill this party member.";
            bullet4.Effects[0].entryVariable = 16;
            bullet4.EffectIntents[0].intents[0] = "Damage_16_20";

            Ability ellegy1 = new Ability("Deathbed Ellegy", "Esther_Ellegy_1_A");
            ellegy1.Description = "Heal the Left ally 5 health.\nInflict 25 Pale on this party member and the Left ally.";
            ellegy1.AbilitySprite = ResourceLoader.LoadSprite("ability_ellegy.png");
            ellegy1.Cost = [Pigments.Blue];
            ellegy1.Effects = new EffectInfo[2];
            ellegy1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), 5, Targeting.Slot_AllyLeft);
            ellegy1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyPaleEffect>(), 25, Targeting.Slot_SelfAndLeft);
            ellegy1.AddIntentsToTarget(Targeting.Slot_AllyLeft, ["Heal_5_10", Pale.Intent]);
            ellegy1.AddIntentsToTarget(Slots.Self, [Pale.Intent]);
            ellegy1.Visuals = CustomVisuals.GetVisuals("Salt/Claws");
            ellegy1.AnimationTarget = Targeting.Slot_AllyLeft;

            Ability ellegy2 = new Ability(ellegy1.ability, "Esther_Ellegy_2_A", ellegy1.Cost);
            ellegy2.Name = "Coffin Ellegy";
            ellegy2.Description = "Heal the Left ally 7 health.\nInflict 25 Pale on this party member and the Left ally.";
            ellegy2.Effects[0].entryVariable = 7;

            Ability ellegy3 = new Ability(ellegy2.ability, "Esther_Ellegy_3_A", ellegy1.Cost);
            ellegy3.Name = "Funeral Ellegy";
            ellegy3.Description = "Heal the Left ally 10 health.\nInflict 25 Pale on this party member and the Left ally.";
            ellegy3.Effects[0].entryVariable = 10;

            Ability ellegy4 = new Ability(ellegy3.ability, "Esther_Ellegy_4_A", ellegy1.Cost);
            ellegy4.Name = "Burial Ellegy";
            ellegy4.Description = "Heal the Left ally 12 health.\nInflict 25 Pale on this party member and the Left ally.";
            ellegy4.Effects[0].entryVariable = 12;
            ellegy4.EffectIntents[0].intents[0] = "Heal_11_20";

            ResurrectLastEffect revive = ScriptableObject.CreateInstance<ResurrectLastEffect>();
            revive.PassiveToBlock = IDs.Eternal;
            revive.SelfSlot = true;
            Ability finale1 = new Ability("Sudden Finale", "Esther_Finale_1_A");
            finale1.Description = "Instantly kill this party member.\nRevive the most recently killed party member other than this one if possible at 5 health.";
            finale1.AbilitySprite = ResourceLoader.LoadSprite("ability_finale.png");
            finale1.Cost = [Pigments.Purple, Pigments.Purple, Pigments.Purple];
            finale1.Effects = new EffectInfo[2];
            finale1.Effects[0] = Effects.GenerateEffect(bullet1.Effects[2].effect, 1, Slots.Self);
            finale1.Effects[1] = Effects.GenerateEffect(CasterPriorityRootActionEffect.Create(Effects.GenerateEffect(revive, 5).SelfArray()));
            finale1.AddIntentsToTarget(Slots.Self, ["Damage_Death", "Other_Resurrect"]);
            finale1.Visuals = CustomVisuals.GetVisuals("Salt/Hung");
            finale1.AnimationTarget = Slots.Self;

            Ability finale2 = new Ability(finale1.ability, "Esther_Finale_2_A", finale1.Cost);
            finale2.Name = "Announced Finale";
            finale2.Description = "Instantly kill this party member.\nRevive the most recently killed party member other than this one if possible at 6 health.";
            finale2.Effects[1].effect = CasterRootActionEffect.Create(Effects.GenerateEffect(revive, 6).SelfArray());

            Ability finale3 = new Ability(finale2.ability, "Esther_Finale_3_A", finale1.Cost);
            finale3.Name = "Prophetic Finale";
            finale3.Description = "Instantly kill this party member.\nRevive the most recently killed party member other than this one if possible at 7 health.";
            finale3.Effects[1].effect = CasterRootActionEffect.Create(Effects.GenerateEffect(revive, 7).SelfArray());

            Ability finale4 = new Ability(finale3.ability, "Esther_Finale_4_A", finale1.Cost);
            finale4.Name = "Deterministic Finale";
            finale4.Description = "Instantly kill this party member.\nRevive the most recently killed party member other than this one if possible at 8 health.";
            finale4.Effects[1].effect = CasterRootActionEffect.Create(Effects.GenerateEffect(revive, 8).SelfArray());

            esther.AddLevelData(8, [finale1, ellegy1, bullet1]);
            esther.AddLevelData(10, [finale2, ellegy2, bullet2]);
            esther.AddLevelData(11, [finale3, ellegy3, bullet3]);
            esther.AddLevelData(14, [finale4, ellegy4, bullet4]);
            esther.IgnoredAbilitiesForSupportBuilds = [2];
            esther.IgnoredAbilitiesForDPSBuilds = [0, 1];
            esther.AddCharacter(true);
        }
        public static void Items()
        {
            MultiPerformEffectItem artificialadrenalinerush = new MultiPerformEffectItem("Aprils_ArtificialAdrenalineRush_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<DirectDeathEffect>(), 1, Slots.Self)]);
            artificialadrenalinerush.Name = "Artificial Adrenaline Rush";
            artificialadrenalinerush.Flavour = "\"Life only has value because of death.\"";
            artificialadrenalinerush.Description = "Increase damage dealt by 50%.\nDie on the 6th turn.";
            artificialadrenalinerush.Icon = ResourceLoader.LoadSprite("item_artificialadrenalinerush.png");
            artificialadrenalinerush.EquippedModifiers = [];
            artificialadrenalinerush.TriggerOn = TriggerCalls.OnTurnStart;
            artificialadrenalinerush.DoesPopUpInfo = true;
            artificialadrenalinerush.Conditions = [TurnPassedCondition.Create(6)];
            artificialadrenalinerush.DoesActionOnTriggerAttached = false;
            artificialadrenalinerush.ConsumeOnTrigger = TriggerCalls.Count;
            artificialadrenalinerush.ConsumeOnUse = false;
            artificialadrenalinerush.ConsumeConditions = [];
            artificialadrenalinerush.ShopPrice = 10;
            artificialadrenalinerush.IsShopItem = true;
            artificialadrenalinerush.StartsLocked = true;
            artificialadrenalinerush.OnUnlockUsesTHE = true;
            artificialadrenalinerush.UsesSpecialUnlockText = false;
            artificialadrenalinerush.SpecialUnlockID = UILocID.None;
            EffectTrigger secnd = new EffectTrigger([], [TriggerCalls.OnWillApplyDamage], [IncreaseDamagePercentageCondition.Create(50)], false);
            artificialadrenalinerush.AddEffectTrigger(secnd);
            artificialadrenalinerush.Item.AddItem("locked_artificialadrenalinerush.png", OsmanACH);

            RankChange_Wearable_SMS down = ScriptableObject.CreateInstance<RankChange_Wearable_SMS>();
            down._rankAdditive = -1;
            IntegerReferenceOverEqualValueEffectorCondition above1 = ScriptableObject.CreateInstance<IntegerReferenceOverEqualValueEffectorCondition>();
            above1.compareValue = 2;
            PerformEffect_Item beheading = new PerformEffect_Item("Aprils_Beheading_TW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), 1, Slots.Self)], true);
            beheading.Name = "Beheading";
            beheading.Flavour = "\"Oh, woe is me!\"";
            beheading.Description = "This party member is 1 level lower than usual.\nOn taking direct damage above 1, heal 1 health.\nThis item is destroyed on death.";
            beheading.Icon = ResourceLoader.LoadSprite("item_beheading.png");
            beheading.EquippedModifiers = [down];
            beheading.TriggerOn = TriggerCalls.OnDirectDamaged;
            beheading.DoesPopUpInfo = true;
            beheading.Conditions = [above1];
            beheading.DoesActionOnTriggerAttached = false;
            beheading.ConsumeOnTrigger = TriggerCalls.OnDeath;
            beheading.ConsumeOnUse = false;
            beheading.ConsumeConditions = [];
            beheading.ShopPrice = 8;
            beheading.IsShopItem = false;
            beheading.StartsLocked = true;
            beheading.OnUnlockUsesTHE = true;
            beheading.UsesSpecialUnlockText = false;
            beheading.SpecialUnlockID = UILocID.None;
            beheading.item.AddItem("locked_beheading.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Esther", "Beheading", "Artificial Adrenaline Rush", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Esther_CH", "Aprils_Beheading_TW", "Aprils_ArtificialAdrenalineRush_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Esther_Heaven_ACH";
        public static string OsmanACH => "Aprils_Esther_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Esther_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Esther_Osman_Unlock";
    }
}
