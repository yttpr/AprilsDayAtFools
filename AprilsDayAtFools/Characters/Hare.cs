using BrutalAPI;
using BrutalAPI.Items;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Hare
    {
        public static void Add()
        {
            Character hare = new Character("Hare", "Hare_CH");
            hare.HealthColor = Pigments.Grey;
            hare.AddUnitType("FemaleID");
            hare.AddUnitType("Sandwich_Pigment");
            hare.AddUnitType("FemaleLooking");
            hare.UsesBasicAbility = true;
            //slap
            hare.UsesAllAbilities = false;
            hare.MovesOnOverworld = true;
            //animator
            hare.FrontSprite = ResourceLoader.LoadSprite("HareFront.png");
            hare.BackSprite = ResourceLoader.LoadSprite("HareBack.png");
            hare.OverworldSprite = ResourceLoader.LoadSprite("HareWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            hare.DamageSound = "event:/Lunacy/SOUNDS/PawnHit";
            hare.DeathSound = "event:/Lunacy/SOUNDS/PawnDie";
            hare.DialogueSound = "event:/Lunacy/SOUNDS/PawnHit";
            hare.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            hare.AddFinalBossAchievementData("Heaven", HeavenACH);
            hare.GenerateMenuCharacter(ResourceLoader.LoadSprite("HareMenu.png"), ResourceLoader.LoadSprite("HareLock.png"));
            hare.MenuCharacterIsSecret = false;
            hare.MenuCharacterIgnoreRandom = false;
            hare.SetMenuCharacterAsFullDPS();

            Ability art1 = new Ability("Preserved Artillery", "Hare_Art_1_A");
            art1.Description = "Deal damage to the Opposing enemy equal to the amount of Pigment in the Pigment tray + 3 and inflict 3 Pimples on them.\nDouble all Pigment in the tray.";
            art1.AbilitySprite = ResourceLoader.LoadSprite("ability_artillery.png");
            art1.Cost = [Pigments.Red, Pigments.Red];
            art1.Effects = new EffectInfo[3];
            art1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageByPigmentTrayEffect>(), 3, Slots.Front);
            art1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyPimplesEffect>(), 3, Slots.Front);
            art1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<GeneratePigmentTrayEffect>(), 1);
            art1.AddIntentsToTarget(Slots.Front, ["Damage_11_15", Pimples.Intent]);
            art1.AddIntentsToTarget(Slots.Self, ["Mana_Generate"]);
            art1.Visuals = CustomVisuals.GetVisuals("Salt/Cannon");
            art1.AnimationTarget = Slots.Front;

            Ability art2 = new Ability(art1.ability, "Hare_Art_2_A", art1.Cost);
            art2.Name = "Processed Artillery";
            art2.Description = "Deal damage to the Opposing enemy equal to the amount of Pigment in the Pigment tray + 7 and inflict 3 Pimples on them.\nDouble all Pigment in the tray.";
            art2.Effects[0].entryVariable = 7;
            art2.EffectIntents[0].intents[0] = "Damage_16_20";

            Ability art3 = new Ability(art2.ability, "Hare_Art_3_A", art1.Cost);
            art3.Name = "Canned Artillery";
            art3.Description = "Deal damage to the Opposing enemy equal to the amount of Pigment in the Pigment tray + 10 and inflict 3 Pimples on them.\nDouble all Pigment in the tray.";
            art3.Effects[0].entryVariable = 10;

            Ability art4 = new Ability(art3.ability, "Hare_Art_4_A", art1.Cost);
            art4.Name = "Mass-Produced Artillery";
            art4.Description = "Deal damage to the Opposing enemy equal to the amount of Pigment in the Pigment tray + 13 and inflict 3 Pimples on them.\nDouble all Pigment in the tray.";
            art4.Effects[0].entryVariable = 13;
            art4.EffectIntents[0].intents[0] = "Damage_21";

            HasStatusEffect hasPimples = HasStatusEffect.Create("Pimples_ID");
            ApplyScarsEffect scars = ScriptableObject.CreateInstance<ApplyScarsEffect>();
            GenerateRandomManaBetweenEffect random = ScriptableObject.CreateInstance<GenerateRandomManaBetweenEffect>();
            random.possibleMana = [Pigments.Red, Pigments.Blue, Pigments.Yellow, Pigments.Purple];
            scars._RandomBetweenPrevious = true;
            Ability spray1 = new Ability("Septic Spray", "Hare_Spray_1_A");
            spray1.Description = "Deal 6 damage to the Opposing enemy and inflict 4 Pimples on them.\nIf they already had Pimples, generate 3 random Pigment.";
            spray1.AbilitySprite = ResourceLoader.LoadSprite("ability_spray.png");
            spray1.Cost = [Pigments.Grey, Pigments.Grey, Pigments.Grey];
            spray1.Effects = new EffectInfo[4];
            spray1.Effects[0] = Effects.GenerateEffect(hasPimples, 1, Slots.Front);
            spray1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 6, Slots.Front);
            spray1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyPimplesEffect>(), 4, Slots.Front);
            spray1.Effects[3] = Effects.GenerateEffect(random, 3, Slots.Self, BasicEffects.DidThat(true, 3));
            spray1.AddIntentsToTarget(Slots.Front, ["Damage_3_6", Pimples.Intent]);
            spray1.AddIntentsToTarget(Slots.Self, ["Mana_Generate"]);
            spray1.Visuals = CustomVisuals.GetVisuals("Salt/Reload");
            spray1.AnimationTarget = Slots.Front;

            Ability spray2 = new Ability(spray1.ability, "Hare_Spray_2_A", spray1.Cost);
            spray2.Name = "Coagulant Spray";
            spray2.Description = "Deal 8 damage to the Opposing enemy and inflict 4 Pimples on them.\nIf they already had Pimples, generate 3 random Pigment.";
            spray2.Effects[1].entryVariable = 8;
            spray2.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability spray3 = new Ability(spray2.ability, "Hare_Spray_3_A", spray1.Cost);
            spray3.Name = "Toxin Spray";
            spray3.Description = "Deal 10 damage to the Opposing enemy and inflict 4 Pimples on them.\nIf they already had Pimples, generate 3 random Pigment.";
            spray3.Effects[1].entryVariable = 10;

            Ability spray4 = new Ability(spray3.ability, "Hare_Spray_4_A", spray1.Cost);
            spray4.Name = "Biohazard Spray";
            spray4.Description = "Deal 13 damage to the Opposing enemy and inflict 4 Pimples on them.\nIf they already had Pimples, generate 3 random Pigment.";
            spray4.Effects[1].entryVariable = 13;
            spray4.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability pores1 = new Ability("Spin Pores", "Hare_Pores_1_A");
            pores1.Description = "Deal 4 damage to the Opposing enemy.\nIf the Pigment tray has over 5 Pigment, consume 3 Random Pigment.\nOtherwise, produce 3 Pigment of the target's health color.";
            pores1.AbilitySprite = ResourceLoader.LoadSprite("ability_pores.png");
            pores1.Cost = [Pigments.Red];
            pores1.Effects = new EffectInfo[4];
            pores1.Effects[0] = Effects.GenerateEffect(spray1.Effects[1].effect, 4, Slots.Front);
            pores1.Effects[1] = Effects.GenerateEffect(BasicEffects.Empty, 1, null, ManaBarAmountCondition.Create(5, true));
            pores1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ConsumeRandomManaEffect>(), 3, null, BasicEffects.DidThat(true));
            pores1.Effects[3] = Effects.GenerateEffect(ScriptableObject.CreateInstance<GenerateTargetHealthManaEffect>(), 3, Slots.Front, BasicEffects.DidThat(false, 2));
            pores1.AddIntentsToTarget(Slots.Front, ["Damage_3_6", "Mana_Generate"]);
            pores1.AddIntentsToTarget(Slots.Self, ["Mana_Consume"]);
            pores1.Visuals = CustomVisuals.GetVisuals("Salt/Class");
            pores1.AnimationTarget = Slots.Front;

            Ability pores2 = new Ability(pores1.ability, "Hare_Pores_2_A", pores1.Cost);
            pores2.Name = "Flip Pores";
            pores2.Description = "Deal 6 damage to the Opposing enemy.\nIf the Pigment tray has over 5 Pigment, consume 3 Random Pigment.\nOtherwise, produce 3 Pigment of the target's health color.";
            pores2.Effects[0].entryVariable = 6;

            Ability pores3 = new Ability(pores2.ability, "Hare_Pores_3_A", pores1.Cost);
            pores3.Name = "Swap Pores";
            pores3.Description = "Deal 7 damage to the Opposing enemy.\nIf the Pigment tray has over 5 Pigment, consume 3 Random Pigment.\nOtherwise, produce 3 Pigment of the target's health color.";
            pores3.Effects[0].entryVariable = 7;
            pores3.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability pores4 = new Ability(pores3.ability, "Hare_Pores_4_A", pores1.Cost);
            pores4.Name = "Toggle Pores";
            pores4.Description = "Deal 8 damage to the Opposing enemy.\nIf the Pigment tray has over 5 Pigment, consume 3 Random Pigment.\nOtherwise, produce 3 Pigment of the target's health color.";
            pores4.Effects[0].entryVariable = 8;

            hare.AddLevelData(10, [pores1, art1, spray1]);
            hare.AddLevelData(13, [pores2, art2, spray2]);
            hare.AddLevelData(16, [pores3, art3, spray3]);
            hare.AddLevelData(19, [pores4, art4, spray4]);
            hare.AddCharacter(true);
        }
        public static void Items()
        {
            PerformEffect_Item radiationgas = new PerformEffect_Item("Aprils_RadiationGas_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyPimplesEffect>(), 1, Targeting.Unit_AllOpponents)]);
            radiationgas.Name = "Radiation Gas";
            radiationgas.Flavour = "\"Filled with joy and whimsy.\"";
            radiationgas.Description = "On using an ability inflict 1 Pimples on all enemies.";
            radiationgas.Icon = ResourceLoader.LoadSprite("item_radiationgas.png");
            radiationgas.EquippedModifiers = [];
            radiationgas.TriggerOn = TriggerCalls.OnAbilityUsed;
            radiationgas.DoesPopUpInfo = true;
            radiationgas.Conditions = [];
            radiationgas.DoesActionOnTriggerAttached = false;
            radiationgas.ConsumeOnTrigger = TriggerCalls.Count;
            radiationgas.ConsumeOnUse = false;
            radiationgas.ConsumeConditions = [];
            radiationgas.ShopPrice = 3;
            radiationgas.IsShopItem = true;
            radiationgas.StartsLocked = true;
            radiationgas.OnUnlockUsesTHE = true;
            radiationgas.UsesSpecialUnlockText = false;
            radiationgas.SpecialUnlockID = UILocID.None;
            radiationgas.Item.AddItem("locked_radiationgas.png", OsmanACH);

            ChangeToRandomHealthColorEffect turnPurple = ScriptableObject.CreateInstance<ChangeToRandomHealthColorEffect>();
            turnPurple._healthColors = [Pigments.Purple];
            MultiPerformEffectItem leadchunk = new MultiPerformEffectItem("Aprils_LeadChunk_SW", [Effects.GenerateEffect(turnPurple, 1, Slots.Front)]);
            leadchunk.Name = "Lead Chunk";
            leadchunk.Flavour = "\"Industry Poison\"";
            leadchunk.Description = "On dealing damage, turn the Opposing enemy Purple.\nDeal 50% more damage to Purple enemies.";
            leadchunk.Icon = ResourceLoader.LoadSprite("item_leadchunk.png");
            leadchunk.EquippedModifiers = [];
            leadchunk.TriggerOn = TriggerCalls.OnDidApplyDamage;
            leadchunk.DoesPopUpInfo = true;
            leadchunk.Conditions = [];
            leadchunk.DoesActionOnTriggerAttached = false;
            leadchunk.ConsumeOnTrigger = TriggerCalls.Count;
            leadchunk.ConsumeOnUse = false;
            leadchunk.ConsumeConditions = [];
            leadchunk.ShopPrice = 6;
            leadchunk.IsShopItem = true;
            leadchunk.StartsLocked = true;
            leadchunk.OnUnlockUsesTHE = true;
            leadchunk.UsesSpecialUnlockText = false;
            leadchunk.SpecialUnlockID = UILocID.None;
            EffectTrigger second = new EffectTrigger([], [TriggerCalls.OnWillApplyDamage], [IncreaseDamageByColorCondition.Create(Pigments.Purple, 50)], false);
            leadchunk.AddEffectTrigger(second);
            leadchunk.item.AddItem("locked_leadchunk.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Hare", "Lead Chunk", "Radiation Gas", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Hare_CH", "Aprils_LeadChunk_SW", "Aprils_RadiationGas_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Hare_Heaven_ACH";
        public static string OsmanACH => "Aprils_Hare_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Hare_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Hare_Osman_Unlock";
    }
}
