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
            //REWORK HER AT SOME POINT
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
            esther.AddUnitType("FemaleLooking");
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

            ApplyLinkedEffect linked = ScriptableObject.CreateInstance<ApplyLinkedEffect>();

            Ability bullet1 = new Ability("First Bullet", "Esther_Bullet_1_A");
            bullet1.Description = "Inflict 3 Linked and deal 7 damage to the Opposing enemy.\nGain 1 Linked.";
            bullet1.AbilitySprite = ResourceLoader.LoadSprite("ability_bullet.png");
            bullet1.Cost = [Pigments.Yellow, Pigments.Purple, Pigments.Red];
            bullet1.Effects = new EffectInfo[3];
            bullet1.Effects[0] = Effects.GenerateEffect(linked, 3, Slots.Front);
            bullet1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 7, Slots.Front);
            bullet1.Effects[2] = Effects.GenerateEffect(CasterRootActionEffect.Create([Effects.GenerateEffect(linked, 1, Slots.Self)]));
            bullet1.AddIntentsToTarget(Slots.Front, ["Status_Linked", "Damage_7_10"]);
            bullet1.AddIntentsToTarget(Slots.Self, ["Status_Linked"]);
            bullet1.Visuals = CustomVisuals.GetVisuals("Salt/Unlock");
            bullet1.AnimationTarget = Slots.Front;

            Ability bullet2 = new Ability(bullet1.ability, "Esther_Bullet_2_A", bullet1.Cost);
            bullet2.Name = "Second Bullet";
            bullet2.Description = "Inflict 4 Linked and deal 10 damage to the Opposing enemy.\nGain 1 Linked.";
            bullet2.Effects[0].entryVariable = 4;
            bullet2.Effects[1].entryVariable = 10;

            Ability bullet3 = new Ability(bullet2.ability, "Esther_Bullet_3_A", bullet1.Cost);
            bullet3.Name = "Third Bullet";
            bullet3.Description = "Inflict 4 Linked and deal 13 damage to the Opposing enemy.\nGain 1 Linked.";
            bullet3.Effects[1].entryVariable = 13;
            bullet3.EffectIntents[0].intents[1] = "Damage_11_15";

            Ability bullet4 = new Ability(bullet3.ability, "Esther_Bullet_4_A", bullet1.Cost);
            bullet4.Name = "Fourth Bullet";
            bullet4.Description = "Inflict 5 Linked and deal 15 damage to the Opposing enemy.\nGain 1 Linked.";
            bullet4.Effects[0].entryVariable = 5;
            bullet4.Effects[1].entryVariable = 15;

            InstantlySpawnEnemyInSlot immortal = ScriptableObject.CreateInstance<InstantlySpawnEnemyInSlot>();
            immortal.enemyName = "ImmortalFigures_EN";
            ApplyStatusByExitEffect ex_link = ScriptableObject.CreateInstance<ApplyStatusByExitEffect>();
            ex_link._Status = StatusField.Linked;

            Ability ellegy1 = new Ability("Deathbed Ellegy", "Esther_Ellegy_1_A");
            ellegy1.Description = "If the Left, Right, or Opposing enemy positions are empty, spawn Immortal Figures there.\nInflict 2 Linked on the Left, Right and Opposing enemies, and 1 Linked on this party member.";
            ellegy1.AbilitySprite = ResourceLoader.LoadSprite("ability_ellegy.png");
            ellegy1.Cost = [Pigments.Blue, Pigments.Purple];
            ellegy1.Effects = new EffectInfo[4];
            ellegy1.Effects[0] = Effects.GenerateEffect(immortal, 1, Slots.FrontLeftRight);
            ellegy1.Effects[1] = Effects.GenerateEffect(linked, 2, Slots.FrontLeftRight);
            ellegy1.Effects[2] = Effects.GenerateEffect(linked, 1, Slots.Self);
            ellegy1.Effects[3] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyDeterminedEffect>(), 0, Slots.Self);
            ellegy1.AddIntentsToTarget(Slots.FrontLeftRight, ["Other_Spawn", "Status_Linked"]);
            ellegy1.AddIntentsToTarget(Slots.Self, ["Status_Linked"]);
            ellegy1.Visuals = Visuals.Melt;
            ellegy1.AnimationTarget = Slots.FrontLeftRight;

            Ability ellegy2 = new Ability(ellegy1.ability, "Esther_Ellegy_2_A", ellegy1.Cost);
            ellegy2.Name = "Coffin Ellegy";
            ellegy2.Description = "If the Left, Right, or Opposing enemy positions are empty, spawn Immortal Figures there.\nInflict 2 Linked on the Left, Right and Opposing enemies, and 1 Linked on this party member.\nGain 1 Determined.";
            ellegy2.Effects[3].entryVariable = 1;
            ellegy2.EffectIntents[1].intents = ["Status_Linked", Determined.Intent];

            Ability ellegy3 = new Ability(ellegy2.ability, "Esther_Ellegy_3_A", ellegy1.Cost);
            ellegy3.Name = "Funeral Ellegy";
            ellegy3.Description = "If the Left, Right, or Opposing enemy positions are empty, spawn Immortal Figures there.\nInflict 3 Linked on the Left, Right and Opposing enemies, and 1 Linked on this party member.\nGain 1 Determined.";
            ellegy3.Effects[1].entryVariable = 3;

            Ability ellegy4 = new Ability(ellegy3.ability, "Esther_Ellegy_4_A", ellegy1.Cost);
            ellegy4.Name = "Burial Ellegy";
            ellegy4.Description = "If the Left, Right, or Opposing enemy positions are empty, spawn Immortal Figures there.\nInflict 3 Linked on the Left, Right and Opposing enemies, and 1 Linked on this party member.\nGain 2 Determined.";
            ellegy4.Effects[3].entryVariable = 2;


            Ability finale1 = new Ability("Sudden Finale", "Esther_Finale_1_A");
            finale1.Description = "Heal this and the Right ally 6 health. If there is no Right ally, revive the most recently deceased applicable party member in the Right position at 6 health.\nInflict 25 Pale on this and the Right ally.";
            finale1.AbilitySprite = ResourceLoader.LoadSprite("ability_finale.png");
            finale1.Cost = [Pigments.Purple];
            finale1.Effects = new EffectInfo[3];
            finale1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), 6, Targeting.Slot_SelfAndRight);
            finale1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ResurrectLastInSlotEffect>(), 6, Targeting.Slot_AllyRight);
            finale1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyPaleEffect>(), 25, Targeting.Slot_SelfAndRight);
            finale1.AddIntentsToTarget(Targeting.Slot_AllyRight, [IntentType_GameIDs.Other_Resurrect.ToString()]);
            finale1.AddIntentsToTarget(Targeting.Slot_SelfAndRight, ["Heal_5_10", Pale.Intent]);
            finale1.Visuals = CustomVisuals.GetVisuals("Salt/Rose");
            finale1.AnimationTarget = Targeting.Slot_AllyRight;

            Ability finale2 = new Ability(finale1.ability, "Esther_Finale_2_A", finale1.Cost);
            finale2.Name = "Announced Finale";
            finale2.Description = "Heal this and the Right ally 8 health. If there is no Right ally, revive the most recently deceased applicable party member in the Right position at 8 health.\nInflict 25 Pale on this and the Right ally.";
            finale2.Effects[0].entryVariable = 8;
            finale2.Effects[1].entryVariable = 8;

            Ability finale3 = new Ability(finale2.ability, "Esther_Finale_3_A", finale1.Cost);
            finale3.Name = "Prophetic Finale";
            finale3.Description = "Heal this and the Right ally 10 health. If there is no Right ally, revive the most recently deceased applicable party member in the Right position at 10 health.\nInflict 25 Pale on this and the Right ally.";
            finale3.Effects[0].entryVariable = 10;
            finale3.Effects[1].entryVariable = 10;

            Ability finale4 = new Ability(finale3.ability, "Esther_Finale_4_A", finale1.Cost);
            finale4.Name = "Deterministic Finale";
            finale4.Description = "Heal this and the Right ally 12 health. If there is no Right ally, revive the most recently deceased applicable party member in the Right position at 12 health.\nInflict 25 Pale on this and the Right ally.";
            finale4.Effects[0].entryVariable = 12;
            finale4.Effects[1].entryVariable = 12;
            finale4.EffectIntents[1].intents[0] = "Heal_11_20";

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
            CasterHealthEffectorCondition above_one = ScriptableObject.CreateInstance<CasterHealthEffectorCondition>();
            above_one.Amount = 4;
            PerformEffect_Item beheading = new PerformEffect_Item("Aprils_Beheading_TW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), 4, Slots.Self)], true);
            beheading.Name = "Beheading";
            beheading.Flavour = "\"Oh, woe is me!\"";
            beheading.Description = "This party member is 1 level lower than usual.\nOn receiving any damage while above 4 health, heal 4 health.\nThis item is destroyed on death.";
            beheading.Icon = ResourceLoader.LoadSprite("item_beheading.png");
            beheading.EquippedModifiers = [down];
            beheading.TriggerOn = TriggerCalls.OnDamaged;
            beheading.DoesPopUpInfo = true;
            beheading.Conditions = [above_one];
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
