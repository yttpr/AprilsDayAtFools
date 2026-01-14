using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Rhys
    {
        public static void Add()
        {
            ExtraCCSprites_ArraySO rhysExtra = ScriptableObject.CreateInstance<ExtraCCSprites_ArraySO>();
            rhysExtra._DefaultID = "";
            rhysExtra._frontSprite = RhysHandler.Sprites;
            rhysExtra._SpecialID = RhysHandler.Value;
            rhysExtra._backSprite = RhysHandler._backs;
            rhysExtra._doesLoop = true;
            SetCasterExtraSpritesRandomUpToEntryEffect rhysSprites = ScriptableObject.CreateInstance<SetCasterExtraSpritesRandomUpToEntryEffect>();
            rhysSprites._spriteType = rhysExtra._SpecialID;

            Character rhys = new Character("Rhys", "Rhys_CH");
            rhys.HealthColor = Pigments.Blue;
            rhys.AddUnitType("FemaleID");
            rhys.AddUnitType("Sandwich_Silly");
            rhys.UsesBasicAbility = true;
            //slap
            rhys.UsesAllAbilities = false;
            rhys.MovesOnOverworld = true;
            //animator
            rhys.FrontSprite = ResourceLoader.LoadSprite("RhysFront0.png");
            rhys.BackSprite = ResourceLoader.LoadSprite("RhysBack.png");
            rhys.OverworldSprite = ResourceLoader.LoadSprite("RhysWorld.png", new Vector2(0.5f, 0f));
            rhys.ExtraSprites = rhysExtra;
            rhys.DamageSound = "event:/Lunacy/SOUNDS/Bone_Hit";
            rhys.DeathSound = "event:/Lunacy/SOUNDS/Bone_Death";
            rhys.DialogueSound = "event:/Lunacy/SOUNDS/Bone_Hit";
            rhys.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            rhys.AddFinalBossAchievementData("Heaven", HeavenACH);
            rhys.GenerateMenuCharacter(ResourceLoader.LoadSprite("RhysMenu.png"), ResourceLoader.LoadSprite("RhysLock.png"));
            rhys.MenuCharacterIsSecret = false;
            rhys.MenuCharacterIgnoreRandom = false;
            rhys.SetMenuCharacterAsFullSupport();
            rhys.AddPassive(Passives.Unstable);

            CasterStoredValueChangeEffect activate = ScriptableObject.CreateInstance<CasterStoredValueChangeEffect>();
            activate._increase = true;
            activate.m_unitStoredDataID = RhysHandler.Value;

            Ability organ1 = new Ability("Organ Chops", "Rhys_Organ_1_A");
            organ1.Description = "Heal the Left ally 4 health, this healing is treated as Shield-piercing direct damage.\nReduce all negative Status Effects on the Left ally by 1.\nHeal this party member 1 health.";
            organ1.AbilitySprite = ResourceLoader.LoadSprite("ability_organ.png");
            organ1.Cost = [Pigments.Red, Pigments.Yellow, Pigments.Blue];
            organ1.Effects = new EffectInfo[5];
            organ1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DirectHealingEffect>(), 4, Targeting.Slot_AllyLeft);
            organ1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ReduceAllNegativeStatusEffect>(), 1, Targeting.Slot_AllyLeft);
            organ1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), 1, Slots.Self);
            organ1.Effects[3] = Effects.GenerateEffect(activate, 1);
            organ1.Effects[4] = Effects.GenerateEffect(rhysSprites, 1);
            organ1.AddIntentsToTarget(Targeting.Slot_AllyLeft, ["Heal_1_4", "Misc"]);
            organ1.AddIntentsToTarget(Slots.Self, ["Heal_1_4"]);
            organ1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Expire_1_A").visuals;
            organ1.AnimationTarget = Targeting.Slot_AllyLeft;

            Ability organ2 = new Ability(organ1.ability, "Rhys_Organ_2_A", organ1.Cost);
            organ2.Name = "Organ Chunks";
            organ2.Description = "Heal the Left ally 5 health, this healing is treated as Shield-piercing direct damage.\nReduce all negative Status Effects on the Left ally by 2.\nHeal this party member 1 health.";
            organ2.Effects[0].entryVariable = 5;
            organ2.Effects[1].entryVariable = 2;
            organ2.EffectIntents[0].intents[0] = "Heal_5_10";

            Ability organ3 = new Ability(organ2.ability, "Rhys_Organ_3_A", organ1.Cost);
            organ3.Name = "Organ Blend";
            organ3.Description = "Heal the Left ally 7 health, this healing is treated as Shield-piercing direct damage.\nReduce all negative Status Effects on the Left ally by 2.\nHeal this party member 1 health.";
            organ3.Effects[0].entryVariable = 7;

            Ability organ4 = new Ability(organ3.ability, "Rhys_Organ_4_A", organ1.Cost);
            organ4.Name = "Organ Puree";
            organ4.Description = "Heal the Left ally 9 health, this healing is treated as Shield-piercing direct damage.\nReduce all negative Status Effects on the Left ally by 2.\nHeal this party member 1 health.";
            organ4.Effects[0].entryVariable = 9;

            ChangeToRandomHealthColorEffect turnRed = ScriptableObject.CreateInstance<ChangeToRandomHealthColorEffect>();
            turnRed._healthColors = [Pigments.Red];

            Ability cauterize1 = new Ability("Cauterize Salmon", "Rhys_Cauterize_1_A");
            cauterize1.Description = "Heal the Left and Right allies 3 health and change their health colors to Red.\nThis healing is treated as Shield-piercing direct damage.";
            cauterize1.AbilitySprite = ResourceLoader.LoadSprite("ability_steak.png");
            cauterize1.Cost = [Pigments.RedPurple, Pigments.RedPurple];
            cauterize1.Effects = new EffectInfo[4];
            cauterize1.Effects[0] = Effects.GenerateEffect(organ1.Effects[0].effect, 3, Slots.Sides);
            cauterize1.Effects[1] = Effects.GenerateEffect(turnRed, 1, Slots.Sides);
            cauterize1.Effects[2] = organ1.Effects[3];
            cauterize1.Effects[3] = organ1.Effects[4];
            cauterize1.AddIntentsToTarget(Slots.Sides, ["Mana_Modify", "Heal_1_4"]);
            cauterize1.Visuals = CustomVisuals.GetVisuals("Salt/Scorch");
            cauterize1.AnimationTarget = Slots.Sides;

            Ability cauterize2 = new Ability(cauterize1.ability, "Rhys_Cauterize_2_A", cauterize1.Cost);
            cauterize2.Name = "Cauterize Poultry";
            cauterize2.Description = "Heal the Left and Right allies 4 health and change their health colors to Red.\nThis healing is treated as Shield-piercing direct damage.";
            cauterize2.Effects[0].entryVariable = 4;

            Ability cauterize3 = new Ability(cauterize2.ability, "Rhys_Cauterize_3_A", cauterize1.Cost);
            cauterize3.Name = "Cauterize Pork";
            cauterize3.Description = "Heal the Left and Right allies 5 health and change their health colors to Red.\nThis healing is treated as Shield-piercing direct damage.";
            cauterize3.Effects[0].entryVariable = 5;
            cauterize3.EffectIntents[0].intents[1] = "Heal_5_10";

            Ability cauterize4 = new Ability(cauterize3.ability, "Rhys_Cauterize_4_A", cauterize1.Cost);
            cauterize4.Name = "Cauterize Sirloin";
            cauterize4.Description = "Heal the Left and Right allies 6 health and change their health colors to Red.\nThis healing is treated as Shield-piercing direct damage.";
            cauterize4.Effects[0].entryVariable = 6;

            Ability marrow1 = new Ability("Marrow Season", "Rhys_Marrow_1_A");
            marrow1.Description = "Heal this party member 1 health, this healing is treated as Shield-piercing direct damage.\n40% chance to refresh this party member's ability usage.";
            marrow1.AbilitySprite = ResourceLoader.LoadSprite("ability_marrow.png");
            marrow1.Cost = [Pigments.YellowBlue];
            marrow1.Effects = new EffectInfo[4];
            marrow1.Effects[0] = Effects.GenerateEffect(organ1.Effects[0].effect, 1, Slots.Self);
            marrow1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), 1, Slots.Self, Effects.ChanceCondition(40));
            marrow1.Effects[2] = organ1.Effects[3];
            marrow1.Effects[3] = organ1.Effects[4];
            marrow1.AddIntentsToTarget(Slots.Self, ["Heal_1_4", "Misc"]);
            marrow1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Malpractice_1_A").visuals;
            marrow1.AnimationTarget = Slots.Self;

            Ability marrow2 = new Ability(marrow1.ability, "Rhys_Marrow_2_A", marrow1.Cost);
            marrow2.Name = "Marrow Broth";
            marrow2.Description = "Heal this party member 1 health, this healing is treated as Shield-piercing direct damage.\n50% chance to refresh this party member's ability usage.";
            marrow2.Effects[1].condition = Effects.ChanceCondition(50);

            Ability marrow3 = new Ability(marrow2.ability, "Rhys_Marrow_3_A", marrow1.Cost);
            marrow3.Name = "Marrow Gravy";
            marrow3.Description = "Heal this party member 2 health, this healing is treated as Shield-piercing direct damage.\n50% chance to refresh this party member's ability usage.";
            marrow3.Effects[0].entryVariable = 2;

            Ability marrow4 = new Ability(marrow3.ability, "Rhys_Marrow_4_A", marrow1.Cost);
            marrow4.Name = "Marrow Custard";
            marrow4.Description = "Heal this party member 2 health, this healing is treated as Shield-piercing direct damage.\n55% chance to refresh this party member's ability usage.";
            marrow4.Effects[1].condition = Effects.ChanceCondition(55);
            
            rhys.AddLevelData(14, [cauterize1, organ1, marrow1]);
            rhys.AddLevelData(16, [cauterize2, organ2, marrow2]);
            rhys.AddLevelData(16, [cauterize3, organ3, marrow3]);
            rhys.AddLevelData(17, [cauterize4, organ4, marrow4]);
            rhys.AddCharacter(true);
        }
        public static void Items()
        {
            PerformEffect_Item cheapkitchenknife = new PerformEffect_Item("Aprils_CheapKitchenKnife_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 10, Targetting.Random(false))]);
            cheapkitchenknife.Name = "Cheap Kitchen Knife";
            cheapkitchenknife.Flavour = "\"It's a decent blunt force weapon.\"";
            cheapkitchenknife.Description = "On using an ability, deal 10 damage to a random enemy.\nThis item is destroyed on activation.";
            cheapkitchenknife.Icon = ResourceLoader.LoadSprite("item_cheapkitchenknife.png");
            cheapkitchenknife.EquippedModifiers = [];
            cheapkitchenknife.TriggerOn = TriggerCalls.OnAbilityUsed;
            cheapkitchenknife.DoesPopUpInfo = true;
            cheapkitchenknife.Conditions = [];
            cheapkitchenknife.DoesActionOnTriggerAttached = false;
            cheapkitchenknife.ConsumeOnTrigger = TriggerCalls.Count;
            cheapkitchenknife.ConsumeOnUse = true;
            cheapkitchenknife.ConsumeConditions = [];
            cheapkitchenknife.ShopPrice = 2;
            cheapkitchenknife.IsShopItem = true;
            cheapkitchenknife.StartsLocked = true;
            cheapkitchenknife.OnUnlockUsesTHE = true;
            cheapkitchenknife.UsesSpecialUnlockText = false;
            cheapkitchenknife.SpecialUnlockID = UILocID.None;
            cheapkitchenknife.item._ItemTypeIDs = ["Knife"];
            cheapkitchenknife.Item.AddItem("locked_cheapkitchenknife.png", OsmanACH);

            SpawnEnemyAnywhereEffect activator = ScriptableObject.CreateInstance<SpawnEnemyAnywhereEffect>();
            activator.enemy = LoadedAssetsHandler.GetEnemy("NeuronActivator_EN");
            activator._spawnTypeID = "Spawn_Basic";
            PerformEffect_Item braineatingamoeba = new PerformEffect_Item("Aprils_BrainEatingAmoeba_TW", [Effects.GenerateEffect(activator, 1)]);
            braineatingamoeba.Name = "Brain-Eating Amoeba";
            braineatingamoeba.Flavour = "\"I don't know if it really is one and I don't want to find out.\"";
            braineatingamoeba.Description = "On turn start, spawn the Neuron Activator.";
            braineatingamoeba.Icon = ResourceLoader.LoadSprite("item_braineatingamoeba.png");
            braineatingamoeba.EquippedModifiers = [];
            braineatingamoeba.TriggerOn = TriggerCalls.OnTurnStart;
            braineatingamoeba.DoesPopUpInfo = true;
            braineatingamoeba.Conditions = [];
            braineatingamoeba.DoesActionOnTriggerAttached = false;
            braineatingamoeba.ConsumeOnTrigger = TriggerCalls.Count;
            braineatingamoeba.ConsumeOnUse = false;
            braineatingamoeba.ConsumeConditions = [];
            braineatingamoeba.ShopPrice = 5;
            braineatingamoeba.IsShopItem = false;
            braineatingamoeba.StartsLocked = true;
            braineatingamoeba.OnUnlockUsesTHE = true;
            braineatingamoeba.UsesSpecialUnlockText = false;
            braineatingamoeba.SpecialUnlockID = UILocID.None;
            braineatingamoeba.item.AddItem("locked_braineatingamoeba.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Rhys", "Brain-Eating Amoeba", "Cheap Kitchen Knife", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Rhys_CH", "Aprils_BrainEatingAmoeba_TW", "Aprils_CheapKitchenKnife_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Rhys_Heaven_ACH";
        public static string OsmanACH => "Aprils_Rhys_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Rhys_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Rhys_Osman_Unlock";
    }
}
