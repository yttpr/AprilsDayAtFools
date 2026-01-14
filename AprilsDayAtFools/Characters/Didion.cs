using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Didion
    {
        public static void Add()
        {
            Character didion = new Character("Didion", "Didion_CH");
            didion.HealthColor = Pigments.Blue;
            didion.AddUnitType("FemaleID");
            didion.UsesBasicAbility = true;
            //slap
            didion.UsesAllAbilities = false;
            didion.MovesOnOverworld = true;
            didion.Animator = Joyce.Assets.LoadAsset<RuntimeAnimatorController>("Assets/DidionAnim/DidionAnimator.overrideController");
            didion.FrontSprite = ResourceLoader.LoadSprite("DidionFront.png");
            didion.BackSprite = ResourceLoader.LoadSprite("DidionBack.png");
            didion.OverworldSprite = ResourceLoader.LoadSprite("DidionWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            didion.DamageSound = LoadedAssetsHandler.GetEnemy("Sepulchre_EN").damageSound;
            didion.DeathSound = LoadedAssetsHandler.GetEnemy("Sepulchre_EN").deathSound;
            didion.DialogueSound = LoadedAssetsHandler.GetEnemy("Sepulchre_EN").damageSound;
            didion.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            didion.AddFinalBossAchievementData("Heaven", HeavenACH);
            didion.GenerateMenuCharacter(ResourceLoader.LoadSprite("DidionMenu.png"), ResourceLoader.LoadSprite("DidionLock.png"));
            didion.MenuCharacterIsSecret = false;
            didion.MenuCharacterIgnoreRandom = false;
            didion.SetMenuCharacterAsFullSupport();
            didion.AddPassive(Passives.Skittish);

            HasStatusEffect hasAnesthetics = ScriptableObject.CreateInstance<HasStatusEffect>();
            ApplyAnestheticsEffect anesthetics = ScriptableObject.CreateInstance<ApplyAnestheticsEffect>();
            HealEffect heal = ScriptableObject.CreateInstance<HealEffect>();
            ApplyShieldSlotEffect shield = ScriptableObject.CreateInstance<ApplyShieldSlotEffect>();
            hasAnesthetics.StatusID = "Anesthetics_ID";
            Ability weakness1 = new Ability("Social Weakness", "Didion_Weakness_1_A");
            weakness1.Description = "Heal the Left and Right allies 3 health and apply 2 Anesthetics to them.\nIf they already had Anesthetics, apply 4 Shield to them instead.";
            weakness1.AbilitySprite = ResourceLoader.LoadSprite("ability_weakness.png");
            weakness1.Cost = [Pigments.Blue, Pigments.Blue];
            weakness1.Effects = new EffectInfo[8];
            weakness1.Effects[0] = Effects.GenerateEffect(hasAnesthetics, 1, Targeting.Slot_AllyLeft);
            weakness1.Effects[1] = Effects.GenerateEffect(heal, 3, Targeting.Slot_AllyLeft, BasicEffects.DidThat(false));
            weakness1.Effects[2] = Effects.GenerateEffect(anesthetics, 2, Targeting.Slot_AllyLeft, BasicEffects.DidThat(false, 2));
            weakness1.Effects[3] = Effects.GenerateEffect(shield, 4, Targeting.Slot_AllyLeft, BasicEffects.DidThat(true, 3));
            weakness1.Effects[4] = Effects.GenerateEffect(hasAnesthetics, 1, Targeting.Slot_AllyRight);
            weakness1.Effects[5] = Effects.GenerateEffect(heal, 3, Targeting.Slot_AllyRight, BasicEffects.DidThat(false));
            weakness1.Effects[6] = Effects.GenerateEffect(anesthetics, 2, Targeting.Slot_AllyRight, BasicEffects.DidThat(false, 2));
            weakness1.Effects[7] = Effects.GenerateEffect(shield, 4, Targeting.Slot_AllyRight, BasicEffects.DidThat(true, 3));
            weakness1.AddIntentsToTarget(Slots.Sides, [Anesthetics.Intent, "Heal_1_4", "Field_Shield"]);
            weakness1.Visuals = CustomVisuals.GetVisuals("Salt/Keyhole");
            weakness1.AnimationTarget = Slots.Sides;

            Ability weakness2 = new Ability(weakness1.ability, "Didion_Weakness_2_A", weakness1.Cost);
            weakness2.Name = "Hidden Weakness";
            weakness2.Description = "Heal the Left and Right allies 4 health and apply 2 Anesthetics to them.\nIf they already had Anesthetics, apply 5 Shield to them instead.";
            weakness2.Effects[1].entryVariable = 4;
            weakness2.Effects[3].entryVariable = 5;
            weakness2.Effects[5].entryVariable = 4;
            weakness2.Effects[7].entryVariable = 5;

            Ability weakness3 = new Ability(weakness2.ability, "Didion_Weakness_3_A", weakness1.Cost);
            weakness3.Name = "Nervous Weakness";
            weakness3.Description = "Heal the Left and Right allies 5 health and apply 2 Anesthetics to them.\nIf they already had Anesthetics, apply 7 Shield to them instead.";
            weakness3.Effects[1].entryVariable = 5;
            weakness3.Effects[3].entryVariable = 7;
            weakness3.Effects[5].entryVariable = 5;
            weakness3.Effects[7].entryVariable = 7;
            weakness3.EffectIntents[0].intents[1] = "Heal_5_10";

            Ability weakness4 = new Ability(weakness3.ability, "Didion_Weakness_4_A", weakness3.Cost);
            weakness4.Name = "Paranoid Weakness";
            weakness4.Description = "Heal the Left and Right allies 6 health and apply 2 Anesthetics to them.\nIf they already had Anesthetics, apply 8 Shield to them instead.";
            weakness4.Effects[1].entryVariable = 6;
            weakness4.Effects[3].entryVariable = 8;
            weakness4.Effects[5].entryVariable = 6;
            weakness4.Effects[7].entryVariable = 8;

            Ability where1 = new Ability("Somewhere Better", "Didion_Where_1_A");
            where1.Description = "Apply 4 Anesthetics to the Left ally. If they already have Anesthetics, heal them 6 health instead.";
            where1.AbilitySprite = ResourceLoader.LoadSprite("ability_where.png");
            where1.Cost = [Pigments.Blue, Pigments.Blue, Pigments.Blue];
            where1.Effects = new EffectInfo[3];
            where1.Effects[0] = Effects.GenerateEffect(hasAnesthetics, 1, Targeting.Slot_AllyLeft);
            where1.Effects[1] = Effects.GenerateEffect(anesthetics, 4, Targeting.Slot_AllyLeft, BasicEffects.DidThat(false));
            where1.Effects[2] = Effects.GenerateEffect(heal, 6, Targeting.Slot_AllyLeft, BasicEffects.DidThat(true, 2));
            where1.AddIntentsToTarget(Targeting.Slot_AllyLeft, [Anesthetics.Intent, "Heal_5_10"]);
            where1.Visuals = LoadedAssetsHandler.GetEnemyAbility("Weep_A").visuals;
            where1.AnimationTarget = Targeting.Slot_AllyLeft;

            Ability where2 = new Ability(where1.ability, "Didion_Where_2_A", where1.Cost);
            where2.Name = "Elsewhere Better";
            where2.Description = "Apply 5 Anesthetics to the Left ally. If they already have Anesthetics, heal them 7 health instead.";
            where2.Effects[1].entryVariable = 5;
            where2.Effects[2].entryVariable = 7;

            Ability where3 = new Ability(where2.ability, "Didion_Where_3_A", where1.Cost);
            where3.Name = "Anywhere Better";
            where3.Description = "Apply 5 Anesthetics to the Left ally. If they already have Anesthetics, heal them 9 health instead.";
            where3.Effects[2].entryVariable = 9;

            Ability where4 = new Ability(where3.ability, "Didion_Where_4_A", where1.Cost);
            where4.Name = "Nowhere Better";
            where4.Description = "Apply 6 Anesthetics to the Left ally. If they already have Anesthetics, heal them 9 health instead.";
            where4.Effects[1].entryVariable = 6;

            Ability escape1 = new Ability("Hopeful Escape", "Didion_Escape_1_A");
            escape1.Description = "Heal the Right ally 4 health and apply 1 Anesthetics to them.";
            escape1.AbilitySprite = ResourceLoader.LoadSprite("ability_escape.png");
            escape1.Cost = [Pigments.Blue, Pigments.Yellow];
            escape1.Effects = new EffectInfo[2];
            escape1.Effects[0] = Effects.GenerateEffect(heal, 4, Targeting.Slot_AllyRight);
            escape1.Effects[1] = Effects.GenerateEffect(anesthetics, 1, Targeting.Slot_AllyRight);
            escape1.AddIntentsToTarget(Targeting.Slot_AllyRight, ["Heal_1_4", Anesthetics.Intent]);
            escape1.Visuals = CustomVisuals.GetVisuals("Salt/Door");
            escape1.AnimationTarget = Targeting.Slot_AllyRight;

            Ability escape2 = new Ability(escape1.ability, "Didion_Escape_2_A", escape1.Cost);
            escape2.Name = "Tearful Escape";
            escape2.Description = "Heal the Right ally 5 health and apply 1 Anesthetics to them.";
            escape2.Effects[0].entryVariable = 5;
            escape2.EffectIntents[0].intents[0] = "Heal_5_10";

            Ability escape3 = new Ability(escape2.ability, "Didion_Escape_3_A", escape1.Cost);
            escape3.Name = "Desperate Escape";
            escape3.Description = "Heal the Right ally 6 health and apply 1 Anesthetics to them.";
            escape3.Effects[0].entryVariable = 6;

            Ability escape4 = new Ability(escape3.ability, "Didion_Escape_4_A", escape1.Cost);
            escape4.Name = "Frantic Escape";
            escape4.Description = "Heal the Right ally 7 health and apply 1 Anesthetics to them.";
            escape4.Effects[0].entryVariable = 7;

            didion.AddLevelData(8, [weakness1, where1, escape1]);
            didion.AddLevelData(9, [weakness2, where2, escape2]);
            didion.AddLevelData(11, [weakness3, where3, escape3]);
            didion.AddLevelData(14, [weakness4, where4, escape4]);
            didion.AddCharacter(true);
        }
        public static void Items()
        {
            PerformEffect_Item martyr = new PerformEffect_Item("Aprils_Martyr_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyDeterminedEffect>(), 2, Targeting.Unit_OtherAllies)]);
            martyr.Name = "Martyr";
            martyr.Flavour = "\"Don't interrupt.\"";
            martyr.Description = "On death, apply 2 Determined to all other allies.";
            martyr.Icon = ResourceLoader.LoadSprite("item_martyr.png");
            martyr.EquippedModifiers = [];
            martyr.TriggerOn = TriggerCalls.OnDeath;
            martyr.DoesPopUpInfo = true;
            martyr.Conditions = [];
            martyr.DoesActionOnTriggerAttached = false;
            martyr.ConsumeOnTrigger = TriggerCalls.Count;
            martyr.ConsumeOnUse = false;
            martyr.ConsumeConditions = [];
            martyr.ShopPrice = 3;
            martyr.IsShopItem = true;
            martyr.StartsLocked = true;
            martyr.OnUnlockUsesTHE = true;
            martyr.UsesSpecialUnlockText = false;
            martyr.SpecialUnlockID = UILocID.None;
            martyr.item._ItemTypeIDs = ["Magic"];
            martyr.Item.AddItem("locked_martyr.png", OsmanACH);

            PerformEffect_Item betablockers = new PerformEffect_Item("Aprils_BetaBlockers_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyAnestheticsEffect>(), 4, Slots.Self)]);
            betablockers.Name = "Beta-Blockers";
            betablockers.Flavour = "\"This won't hurt a bit.\"";
            betablockers.Description = "Apply 4 Anesthetics to this party member on combat start.";
            betablockers.Icon = ResourceLoader.LoadSprite("item_betablockers.png");
            betablockers.EquippedModifiers = [];
            betablockers.TriggerOn = TriggerCalls.OnFirstTurnStart;
            betablockers.DoesPopUpInfo = true;
            betablockers.Conditions = [];
            betablockers.DoesActionOnTriggerAttached = false;
            betablockers.ConsumeOnTrigger = TriggerCalls.Count;
            betablockers.ConsumeOnUse = false;
            betablockers.ConsumeConditions = [];
            betablockers.ShopPrice = 4;
            betablockers.IsShopItem = true;
            betablockers.StartsLocked = true;
            betablockers.OnUnlockUsesTHE = true;
            betablockers.UsesSpecialUnlockText = false;
            betablockers.SpecialUnlockID = UILocID.None;
            betablockers.item.AddItem("locked_betablockers.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Didion", "Beta-Blockers", "Martyr", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Didion_CH", "Aprils_BetaBlockers_SW", "Aprils_Martyr_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Didion_Heaven_ACH";
        public static string OsmanACH => "Aprils_Didion_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Didion_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Didion_Osman_Unlock";
    }
}
