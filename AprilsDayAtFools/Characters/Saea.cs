using BrutalAPI;
using BrutalAPI.Items;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Saea
    {
        public static void Add()
        {
            Character saea = new Character("Saea", "Saea_CH");
            saea.HealthColor = Pigments.Blue;
            saea.AddUnitType("FemaleID");
            saea.UsesBasicAbility = true;
            //slap
            saea.UsesAllAbilities = false;
            saea.MovesOnOverworld = false;
            saea.Animator = LoadedAssetsHandler.GetCharacter("Rotcore_CH").characterAnimator;
            saea.FrontSprite = ResourceLoader.LoadSprite("SaeaFront.png");
            saea.BackSprite = ResourceLoader.LoadSprite("SaeaBack.png");
            saea.OverworldSprite = ResourceLoader.LoadSprite("SaeaWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            saea.DamageSound = "event:/Lunacy/SOUNDS/StarlessHit";
            saea.DeathSound = "event:/Lunacy/SOUNDS/StarlessDie";
            saea.DialogueSound = "event:/Lunacy/SOUNDS/StarlessHit";
            saea.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            saea.AddFinalBossAchievementData("Heaven", HeavenACH);
            saea.GenerateMenuCharacter(ResourceLoader.LoadSprite("SaeaMenu.png"), ResourceLoader.LoadSprite("SaeaLock.png"));
            saea.MenuCharacterIsSecret = true;
            saea.MenuCharacterIgnoreRandom = true;

            DamageEffect damage = ScriptableObject.CreateInstance<DamageEffect>();
            Ability cat1 = new Ability("Great Cataclysm", "Saea_Cat_1_A");
            cat1.Description = "Deal 12 damage to all enemies.";
            cat1.AbilitySprite = ResourceLoader.LoadSprite("ability_cataclysm.png");
            cat1.Cost = [Pigments.Purple, Pigments.Purple, Pigments.Purple];
            cat1.Effects = new EffectInfo[1];
            cat1.Effects[0] = Effects.GenerateEffect(damage, 12, Targeting.Unit_AllOpponents);
            cat1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Damage_11_15"]);
            cat1.Visuals = CustomVisuals.GetVisuals("Salt/StarBomb");
            cat1.AnimationTarget = Targetting.Everything(false);

            Ability cat2 = new Ability(cat1.ability, "Saea_Cat_2_A", cat1.Cost);
            cat2.Name = "Cruel Cataclysm";
            cat2.Description = "Deal 16 damage to all enemies.";
            cat2.Effects[0].entryVariable = 16;
            cat2.EffectIntents[0].intents[0] = "Damage_16_20";

            Ability cat3 = new Ability(cat2.ability, "Saea_Cat_3_A", cat1.Cost);
            cat3.Name = "Morbid Cataclysm";
            cat3.Description = "Deal 24 damage to all enemies.";
            cat3.Effects[0].entryVariable = 24;
            cat3.EffectIntents[0].intents[0] = "Damage_21";

            Ability cat4 = new Ability(cat3.ability, "Saea_Cat_4_A", cat1.Cost);
            cat4.Name = "Destined Cataclysm";
            cat4.Description = "Deal 30 damage to all enemies.";
            cat4.Effects[0].entryVariable = 30;

            Ability ori1 = new Ability("Placated Origin", "Saea_Ori_1_A");
            ori1.Description = "Resurrect as many party members as possible.\nRandomize the health of all party members between 1-8.";
            ori1.AbilitySprite = ResourceLoader.LoadSprite("ability_origin.png");
            ori1.Cost = [Pigments.Blue, Pigments.Blue, Pigments.Blue];
            ori1.Effects = new EffectInfo[2];
            ori1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ResurrectEffect>(), 1, Targetting.Everything(true));
            ori1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RandomizeHealthUpToEntryEffect>(), 8, Targetting.Everything(true));
            ori1.AddIntentsToTarget(Targetting.Everything(true), ["Other_Resurrect", "Other_MaxHealth_Alt"]);
            ori1.Visuals = CustomVisuals.GetVisuals("Salt/Insta/Shatter");
            ori1.AnimationTarget = Slots.Self;

            Ability ori2 = new Ability(ori1.ability, "Saea_Ori_2_A", ori1.Cost);
            ori2.Name = "Cordial Origin";
            ori2.Description = "Resurrect as many party members as possible.\nRandomize the health of all party members between 1-14.";
            ori2.Effects[1].entryVariable = 14;

            Ability ori3 = new Ability(ori2.ability, "Saea_Ori_3_A", ori1.Cost);
            ori3.Name = "Amiable Origin";
            ori3.Description = "Resurrect as many party members as possible.\nRandomize the health of all party members between 1-20.";
            ori3.Effects[1].entryVariable = 20;

            Ability ori4 = new Ability(ori3.ability, "Saea_Ori_4_A", ori1.Cost);
            ori4.Name = "Hospitable Origin";
            ori4.Description = "Resurrect as many party members as possible.\nRandomize the health of all party members between 1-28.";
            ori4.Effects[1].entryVariable = 28;

            Ability onset1 = new Ability("Onset of Shadows", "Saea_Onset_1_A");
            onset1.Description = "Deal 5 indirect damage to all enemies.\nShuffle all enemy positions.\nReroll the entire timeline.";
            onset1.AbilitySprite = ResourceLoader.LoadSprite("ability_onset.png");
            onset1.Cost = [Pigments.Red, Pigments.Red, Pigments.Red];
            onset1.Effects = new EffectInfo[3];
            onset1.Effects[0] = Effects.GenerateEffect(BasicEffects.Indirect, 5, Targeting.Unit_AllOpponents);
            onset1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<MassSwapZoneEffect>(), 1, Targetting.Everything(false));
            onset1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RerollTimelineEffect>());
            onset1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Damage_3_6", "Misc"]);
            onset1.AddIntentsToTarget(Targetting.Everything(false), ["Swap_Mass"]);
            onset1.Visuals = CustomVisuals.GetVisuals("Salt/Claws");
            onset1.AnimationTarget = Targetting.Everything(false);

            Ability onset2 = new Ability(onset1.ability, "Saea_Onset_2_A", onset1.Cost);
            onset2.Name = "Onset of Darkness";
            onset2.Description = "Deal 7 indirect damage to all enemies.\nShuffle all enemy positions.\nReroll the entire timeline.";
            onset2.Effects[0].entryVariable = 7;
            onset2.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability onset3 = new Ability(onset2.ability, "Saea_Onset_3_A", onset1.Cost);
            onset3.Name = "Onset of Death";
            onset3.Description = "Deal 9 indirect damage to all enemies.\nShuffle all enemy positions.\nReroll the entire timeline.";
            onset3.Effects[0].entryVariable = 9;

            Ability onset4 = new Ability(onset3.ability, "Saea_Onset_4_A", onset1.Cost);
            onset4.Name = "Onset of Hell";
            onset4.Description = "Deal 11 indirect damage to all enemies.\nShuffle all enemy positions.\nReroll the entire timeline.";
            onset4.Effects[0].entryVariable = 11;
            onset4.EffectIntents[0].intents[0] = "Damage_11_15";

            saea.AddLevelData(1, [onset1, ori1, cat1]);
            saea.AddLevelData(1, [onset2, ori2, cat2]);
            saea.AddLevelData(1, [onset3, ori3, cat3]);
            saea.AddLevelData(1, [onset4, ori4, cat4]);
            saea.IgnoredAbilitiesForSupportBuilds = [2];
            saea.IgnoredAbilitiesForDPSBuilds = [0, 1];
            saea.AddCharacter(false, true);
        }
        public static void Items()
        {
            ApplyPaleEffect range = ScriptableObject.CreateInstance<ApplyPaleEffect>();
            range._RandomBetweenPrevious = true;
            PerformEffect_Item constellationgrin = new PerformEffect_Item("Aprils_ConstellationGrin_TW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<ExtraVariableForNextEffect>(), 70), Effects.GenerateEffect(range, 90, Targeting.Unit_AllOpponents)]);
            constellationgrin.Name = "Constellation Grin";
            constellationgrin.Flavour = "\"Sky is a bed for my sleep\"";
            constellationgrin.Description = "On death, inflict 70-90 Pale to all enemies.";
            constellationgrin.Icon = ResourceLoader.LoadSprite("item_constellationgrin.png");
            constellationgrin.EquippedModifiers = [];
            constellationgrin.TriggerOn = TriggerCalls.OnDeath;
            constellationgrin.DoesPopUpInfo = true;
            constellationgrin.Conditions = [];
            constellationgrin.DoesActionOnTriggerAttached = false;
            constellationgrin.ConsumeOnTrigger = TriggerCalls.Count;
            constellationgrin.ConsumeOnUse = false;
            constellationgrin.ConsumeConditions = [];
            constellationgrin.ShopPrice = 6;
            constellationgrin.IsShopItem = false;
            constellationgrin.StartsLocked = true;
            constellationgrin.OnUnlockUsesTHE = true;
            constellationgrin.UsesSpecialUnlockText = false;
            constellationgrin.SpecialUnlockID = UILocID.None;
            constellationgrin.item._ItemTypeIDs = ["Magic", "Heart", "Face"];
            constellationgrin.Item.AddItem("locked_constellationgrin.png", OsmanACH);

            PerformEffect_Item starsteardrop = new PerformEffect_Item("Aprils_StarsTeardrop_TW", [Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 5, Targeting.Unit_AllOpponents)]);
            starsteardrop.Name = "Star's Teardrop";
            starsteardrop.Flavour = "\"The Sky Is Falling\"";
            starsteardrop.Description = "On receiving any damage, deal 5 damage to all enemies.";
            starsteardrop.Icon = ResourceLoader.LoadSprite("item_starsteardrop.png");
            starsteardrop.EquippedModifiers = [];
            starsteardrop.TriggerOn = TriggerCalls.OnDamaged;
            starsteardrop.DoesPopUpInfo = true;
            starsteardrop.Conditions = [];
            starsteardrop.DoesActionOnTriggerAttached = false;
            starsteardrop.ConsumeOnTrigger = TriggerCalls.Count;
            starsteardrop.ConsumeOnUse = false;
            starsteardrop.ConsumeConditions = [];
            starsteardrop.ShopPrice = 6;
            starsteardrop.IsShopItem = false;
            starsteardrop.StartsLocked = true;
            starsteardrop.OnUnlockUsesTHE = true;
            starsteardrop.UsesSpecialUnlockText = false;
            starsteardrop.SpecialUnlockID = UILocID.None;
            starsteardrop.item._ItemTypeIDs = ["Magic", "Heart"];
            starsteardrop.item.AddItem("locked_starsteardrop.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Saea", "Star's Teardrop", "Constellation Grin", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Saea_CH", "Aprils_StarsTeardrop_TW", "Aprils_ConstellationGrin_TW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Saea_Heaven_ACH";
        public static string OsmanACH => "Aprils_Saea_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Saea_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Saea_Osman_Unlock";
    }
}
