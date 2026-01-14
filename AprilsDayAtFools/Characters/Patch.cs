using BrutalAPI;
using BrutalAPI.Items;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Patch
    {
        public static void Add()
        {
            Character patch = new Character("Patch", "Patch_CH");
            patch.HealthColor = Pigments.Purple;
            patch.AddUnitType("FemaleID");
            patch.AddUnitType("Sandwich_Gore");
            patch.UsesBasicAbility = true;
            //slap
            patch.UsesAllAbilities = false;
            patch.MovesOnOverworld = true;
            //animator
            patch.FrontSprite = ResourceLoader.LoadSprite("PatchFront.png");
            patch.BackSprite = ResourceLoader.LoadSprite("PatchBack.png");
            patch.OverworldSprite = ResourceLoader.LoadSprite("PatchWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            patch.DamageSound = "";
            patch.DeathSound = "";
            patch.DialogueSound = "";
            patch.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            patch.AddFinalBossAchievementData("Heaven", HeavenACH);
            patch.GenerateMenuCharacter(ResourceLoader.LoadSprite("PatchMenu.png"), ResourceLoader.LoadSprite("PatchLock.png"));
            patch.MenuCharacterIsSecret = false;
            patch.MenuCharacterIgnoreRandom = false;
            patch.SetMenuCharacterAsFullDPS();
            patch.AddPassive(Passives.Infestation1);

            DamageEffect returnKill = ScriptableObject.CreateInstance<DamageEffect>();
            returnKill._returnKillAsSuccess = true;
            Ability leather1 = new Ability("Punishment of Skin", "Patch_Leather_1_A");
            leather1.Description = "Deal 6 damage to the Opposing enemy.\nIf this ability kills, spawn an Assistant Incarnate of the target's health color.";
            leather1.AbilitySprite = ResourceLoader.LoadSprite("ability_leather.png");
            leather1.Cost = [Pigments.Red, Pigments.Red, Pigments.Blue];
            leather1.Effects = new EffectInfo[2];
            leather1.Effects[0] = Effects.GenerateEffect(returnKill, 6, Slots.Front);
            leather1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<SpawnAsssitantByTargetEffect>(), 1, Slots.Front, BasicEffects.DidThat(true));
            leather1.AddIntentsToTarget(Slots.Front, ["Damage_3_6", "Other_Spawn"]);
            leather1.Visuals = CustomVisuals.GetVisuals("Salt/Piano");
            leather1.AnimationTarget = Slots.Front;

            Ability leather2 = new Ability(leather1.ability, "Patch_Leather_2_A", leather1.Cost);
            leather2.Name = "Punishment of Hide";
            leather2.Description = "Deal 8 damage to the Opposing enemy.\nIf this ability kills, spawn an Assistant Incarnate of the target's health color.";
            leather2.Effects[0].entryVariable = 8;
            leather2.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability leather3 = new Ability(leather2.ability, "Patch_Leather_3_A", leather1.Cost);
            leather3.Name = "Punishment of Leather";
            leather3.Description = "Deal 10 damage to the Opposing enemy.\nIf this ability kills, spawn an Assistant Incarnate of the target's health color.";
            leather3.Effects[0].entryVariable = 10;

            Ability leather4 = new Ability(leather3.ability, "Patch_Leather_4_A", leather1.Cost);
            leather4.Name = "Punishment of Rind";
            leather4.Description = "Deal 12 damage to the Opposing enemy.\nIf this ability kills, spawn an Assistant Incarnate of the target's health color.";
            leather4.Effects[0].entryVariable = 12;
            leather4.EffectIntents[0].intents[0] = "Damage_11_15";

            InfestationRandomSetEffect infest1 = ScriptableObject.CreateInstance<InfestationRandomSetEffect>();
            infest1._infestationPassive = Passives.Infestation1 as InfestationPassiveAbility;
            Ability script1 = new Ability("Tending Scripture", "Patch_Script_1_A");
            script1.Description = "Deal 3 damage to the Opposing enemy.\nReroll one of their actions and increase their Infestation by 1.";
            script1.AbilitySprite = ResourceLoader.LoadSprite("ability_agriculture.png");
            script1.Cost = [Pigments.Red, Pigments.Yellow];
            script1.Effects = new EffectInfo[3];
            script1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 3, Slots.Front);
            script1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ReRollTargetTimelineAbilityEffect>(), 1, Slots.Front);
            script1.Effects[2] = Effects.GenerateEffect(infest1, 1, Slots.Front);
            script1.AddIntentsToTarget(Slots.Front, ["Damage_3_6", "Misc", "PA_Infestation"]);
            script1.Visuals = CustomVisuals.GetVisuals("Salt/Cube");
            script1.AnimationTarget = Slots.Front;

            Ability script2 = new Ability(script1.ability, "Patch_Script_2_A", script1.Cost);
            script2.Name = "Agriculture Scripture";
            script2.Description = "Deal 4 damage to the Opposing enemy.\nReroll one of their actions and increase their Infestation by 2.";
            script2.Effects[0].entryVariable = 4;
            script2.Effects[2].entryVariable = 2;

            Ability script3 = new Ability(script2.ability, "Patch_Script_3_A", script1.Cost);
            script3.Name = "Harvesting Scripture";
            script3.Description = "Deal 5 damage to the Opposing enemy.\nReroll one of their actions and increase their Infestation by 2.";
            script3.Effects[0].entryVariable = 5;

            Ability script4 = new Ability(script3.ability, "Patch_Script_4_A", script1.Cost);
            script4.Name = "Processing Scripture";
            script4.Description = "Deal 6 damage to the Opposing enemy.\nReroll one of their actions and increase their Infestation by 2.";
            script4.Effects[0].entryVariable = 6;

            Ability demo1 = new Ability("Sewing Demonstration", "Patch_Demo_1_A");
            demo1.Description = "If the Opposing position is empty, spawn an Assistant Incarnate of the Pigment Color used.\nOtherwise, reroll 1 action from the Opposing enemy.\n40% chance to refresh this party member's ability usage.";
            demo1.AbilitySprite = ResourceLoader.LoadSprite("ability_demonstration.png");
            demo1.Cost = [Pigments.Grey];
            demo1.Effects = new EffectInfo[4];
            demo1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ExtraVariableForNextEffect>(), 1, null, ScriptableObject.CreateInstance<FrontSlotEmptyCondition>());
            demo1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<SpawnAssistantByPigmentUsedEffect>(), 1, null, BasicEffects.DidThat(true));
            demo1.Effects[2] = Effects.GenerateEffect(script1.Effects[1].effect, 1, Slots.Front, BasicEffects.DidThat(false, 2));
            demo1.Effects[3] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), 1, Slots.Self, Effects.ChanceCondition(40));
            demo1.AddIntentsToTarget(Slots.Front, ["Other_Spawn", "Misc"]);
            demo1.AddIntentsToTarget(Slots.Self, ["Misc"]);
            demo1.Visuals = CustomVisuals.GetVisuals("Salt/Needle");
            demo1.AnimationTarget = Slots.Front;

            Ability demo2 = new Ability(demo1.ability, "Patch_Demo_2_A", demo1.Cost);
            demo2.Name = "Seaming Demonstration";
            demo2.Description = "If the Opposing position is empty, spawn an Assistant Incarnate of the Pigment Color used.\nOtherwise, reroll 1 action from the Opposing enemy.\n50% chance to refresh this party member's ability usage.";
            demo2.Effects[3].condition = Effects.ChanceCondition(50);

            Ability demo3 = new Ability(demo2.ability, "Patch_Demo_3_A", demo1.Cost);
            demo3.Name = "Needlework Demonstration";
            demo3.Description = "If the Opposing position is empty, spawn an Assistant Incarnate of the Pigment Color used.\nOtherwise, reroll 1 action from the Opposing enemy.\n55% chance to refresh this party member's ability usage.";
            demo3.Effects[3].condition = Effects.ChanceCondition(55);

            Ability demo4 = new Ability(demo3.ability, "Patch_Demo_4_A", demo1.Cost);
            demo4.Name = "Embroidery Demonstration";
            demo4.Description = "If the Opposing position is empty, spawn an Assistant Incarnate of the Pigment Color used.\nOtherwise, reroll 1 action from the Opposing enemy.\n60% chance to refresh this party member's ability usage.";
            demo4.Effects[3].condition = Effects.ChanceCondition(60);

            patch.AddLevelData(10, [demo1, leather1, script1]);
            patch.AddLevelData(14, [demo2, leather2, script2]);
            patch.AddLevelData(17, [demo3, leather3, script3]);
            patch.AddLevelData(19, [demo4, leather4, script4]);
            patch.AddCharacter(true);
        }
        public static void Items()
        {
            PerformEffect_Item teddy = new PerformEffect_Item("Aprils_Teddy_SW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyDeterminedEffect>(), 20, Slots.Self)], true);
            teddy.Name = "Teddy";
            teddy.Flavour = "\"Look inside!\"";
            teddy.Description = "On taking any damage, gain 20 Determined and destroy this item.";
            teddy.Icon = ResourceLoader.LoadSprite("item_teddy.png");
            teddy.EquippedModifiers = [];
            teddy.TriggerOn = TriggerCalls.OnDamaged;
            teddy.DoesPopUpInfo = true;
            teddy.Conditions = [];
            teddy.DoesActionOnTriggerAttached = false;
            teddy.ConsumeOnTrigger = TriggerCalls.Count;
            teddy.ConsumeOnUse = true;
            teddy.ConsumeConditions = [];
            teddy.ShopPrice = 7;
            teddy.IsShopItem = true;
            teddy.StartsLocked = true;
            teddy.OnUnlockUsesTHE = false;
            teddy.UsesSpecialUnlockText = false;
            teddy.SpecialUnlockID = UILocID.None;
            teddy.item._ItemTypeIDs = ["Heart"];
            teddy.Item.AddItem("locked_teddy.png", OsmanACH);

            PerformEffect_Item backupbodies = new PerformEffect_Item("Aprils_BackupBodies_SW", []);
            backupbodies.Name = "Backup Bodies";
            backupbodies.Flavour = "\"We need more.\"";
            backupbodies.Description = "Whenever an ally dies, 50% chance to spawn a nonpermenant clone of them.";
            backupbodies.Icon = ResourceLoader.LoadSprite("item_backupbodies.png");
            backupbodies.EquippedModifiers = [];
            backupbodies.TriggerOn = TriggerCalls.OnAllyHasDied;
            backupbodies.DoesPopUpInfo = true;
            backupbodies.Conditions = [ScriptableObject.CreateInstance<BackupBodiesCondition>()];
            backupbodies.DoesActionOnTriggerAttached = false;
            backupbodies.ConsumeOnTrigger = TriggerCalls.Count;
            backupbodies.ConsumeOnUse = false;
            backupbodies.ConsumeConditions = [];
            backupbodies.ShopPrice = 6;
            backupbodies.IsShopItem = true;
            backupbodies.StartsLocked = true;
            backupbodies.OnUnlockUsesTHE = true;
            backupbodies.UsesSpecialUnlockText = false;
            backupbodies.SpecialUnlockID = UILocID.None;
            backupbodies.item._ItemTypeIDs = ["Magic"];
            backupbodies.item.AddItem("locked_backupbodies.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Patch", "Backup Bodies", "Teddy", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Patch_CH", "Aprils_BackupBodies_SW", "Aprils_Teddy_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Patch_Heaven_ACH";
        public static string OsmanACH => "Aprils_Patch_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Patch_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Patch_Osman_Unlock";
    }
}
