using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Catten
    {
        public static void Add()
        {
            ApplyEntropyEffect entropy = ScriptableObject.CreateInstance<ApplyEntropyEffect>();

            PerformEffectPassiveAbility liminal = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            liminal.name = "Liminal_1_PA";
            liminal._passiveName = "Liminal (1)";
            liminal.m_PassiveID = "Liminal_PA";
            liminal.passiveIcon = ResourceLoader.LoadSprite("LiminalPassive.png");
            liminal._characterDescription = "On pausing, gain 1 Entropy.";
            liminal._enemyDescription = liminal._characterDescription;
            liminal._triggerOn = [PauseHook.Trigger];
            liminal.conditions = [];
            liminal.doesPassiveTriggerInformationPanel = true;
            liminal.effects = [Effects.GenerateEffect(entropy, 1, Slots.Self)];
            liminal.AddToPassiveDatabase();
            liminal.AddPassiveToGlossary("Liminal", "On pausing, gain a certain amount of Entropy.");

            Character catten = new Character("Catten", "Catten_CH");
            catten.HealthColor = Pigments.Purple;
            catten.AddUnitType("FemaleID");
            catten.AddUnitType("Sandwich_Robot");
            catten.UsesBasicAbility = true;
            //slap
            catten.UsesAllAbilities = false;
            catten.MovesOnOverworld = false;
            catten.Animator = Joyce.Assets.LoadAsset<RuntimeAnimatorController>("Assets/CattenAnim/CattenAnimator.overrideController");
            catten.FrontSprite = ResourceLoader.LoadSprite("CattenFront.png");
            catten.BackSprite = ResourceLoader.LoadSprite("CattenBack.png");
            catten.OverworldSprite = ResourceLoader.LoadSprite("CattenWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            catten.DeathSound = "event:/Lunacy/SOUNDS3/BlackStarDie";
            catten.DamageSound = "event:/Lunacy/SOUNDS3/BlackStarHit";
            catten.DialogueSound = "event:/Lunacy/SOUNDS3/BlackStarDie";
            catten.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            catten.AddFinalBossAchievementData("Heaven", HeavenACH);
            catten.GenerateMenuCharacter(ResourceLoader.LoadSprite("CattenMenu.png"), ResourceLoader.LoadSprite("CattenLock.png"));
            catten.MenuCharacterIsSecret = false;
            catten.MenuCharacterIgnoreRandom = false;
            catten.SetMenuCharacterAsFullDPS();
            catten.AddPassive(liminal);

            DamageEffect damage = ScriptableObject.CreateInstance<DamageEffect>();

            Ability interest1 = new Ability("Unit Interest", "Cat_Interest_1_A");
            interest1.Description = "Deal 4 damage to the Opposing enemy and inflict 2 Entropy on the Left, Right, and Opposing enemies.\nGain 3 Entropy and refresh this party member's ability usage if they have less than 10 Entropy.";
            interest1.AbilitySprite = ResourceLoader.LoadSprite("ability_interest.png");
            interest1.Cost = [Pigments.Red, Pigments.Blue];
            interest1.Effects = new EffectInfo[4];
            interest1.Effects[0] = Effects.GenerateEffect(damage, 4, Slots.Front);
            interest1.Effects[1] = Effects.GenerateEffect(entropy, 2, Slots.FrontLeftRight);
            interest1.Effects[2] = Effects.GenerateEffect(entropy, 3, Slots.Self);
            interest1.Effects[3] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), 1, Slots.Self, ScriptableObject.CreateInstance<LessThan10EntropyCondition>());
            interest1.AddIntentsToTarget(Slots.Front, ["Damage_3_6"]);
            interest1.AddIntentsToTarget(Slots.FrontLeftRight, [Entropy.Intent]);
            interest1.AddIntentsToTarget(Slots.Self, [Entropy.Intent, IntentType_GameIDs.Misc_Additional.ToString()]);
            interest1.AnimationTarget = Slots.Front;
            interest1.Visuals = CustomVisuals.GetVisuals("Salt/Cube");

            Ability interest2 = new Ability(interest1.ability, "Cat_Interest_2_A", interest1.Cost);
            interest2.Name = "Dozen Interest";
            interest2.Description = "Deal 6 damage to the Opposing enemy and inflict 2 Entropy on the Left, Right, and Opposing enemies.\nGain 3 Entropy and refresh this party member's ability usage if they have less than 10 Entropy.";
            interest2.Effects[0].entryVariable = 6;

            Ability interest3 = new Ability(interest2.ability, "Cat_Interest_3_A", interest1.Cost);
            interest3.Name = "Score Interest";
            interest3.Description = "Deal 7 damage to the Opposing enemy and inflict 3 Entropy on the Left, Right, and Opposing enemies.\nGain 3 Entropy and refresh this party member's ability usage if they have less than 10 Entropy.";
            interest3.Effects[0].entryVariable = 7;
            interest3.Effects[1].entryVariable = 3;
            interest3.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability interest4 = new Ability(interest3.ability, "Cat_Interest_4_A", interest1.Cost);
            interest4.Name = "Gross Interest";
            interest4.Description = "Deal 7 damage to the Opposing enemy and inflict 4 Entropy on the Left, Right, and Opposing enemies.\nGain 3 Entropy and refresh this party member's ability usage if they have less than 10 Entropy.";
            interest4.Effects[1].entryVariable = 4;

            RemoveStatusEffectEffect remove = ScriptableObject.CreateInstance<RemoveStatusEffectEffect>();
            remove._status = Entropy.Object;
            DamageEffect byExit = ScriptableObject.CreateInstance<DamageEffect>();
            byExit._usePreviousExitValue = true;

            Ability graph1 = new Ability("Linear Graph", "Cat_Graph_1_A");
            graph1.Description = "Remove all Entropy from all enemies.\nDeal damage to the Opposing enemy equal to the amount of Entropy removed.";
            graph1.AbilitySprite = ResourceLoader.LoadSprite("ability_graph.png");
            graph1.Cost = [Pigments.Blue, Pigments.Yellow, Pigments.Red];
            graph1.Effects = new EffectInfo[3];
            graph1.Effects[0] = Effects.GenerateEffect(remove, 1, Targeting.Unit_AllOpponents);
            graph1.Effects[1] = Effects.GenerateEffect(byExit, 1, Slots.Front);
            graph1.Effects[2] = Effects.GenerateEffect(BasicEffects.Empty, 0, Slots.Self);
            graph1.AddIntentsToTarget(Targeting.Unit_AllOpponents, [Entropy.Rem]);
            graph1.AddIntentsToTarget(Slots.Front, ["Damage_7_10"]);
            graph1.AnimationTarget = Slots.Front;
            graph1.Visuals = CustomVisuals.GetVisuals("Salt/Drill");

            Ability graph2 = new Ability(graph1.ability, "Cat_Graph_2_A", graph1.Cost);
            graph2.Name = "Polynomial Graph";
            graph2.Description = "Remove all Entropy from all enemies.\nDeal damage to the Opposing enemy equal to the amount of Entropy removed.\nHeal this party member 2 health.";
            graph2.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), 2, Slots.Self);
            graph2.AddIntentsToTarget(Slots.Self, ["Heal_1_4"]);
            graph2.EffectIntents[1].intents[0] = "Damage_11_15";

            Ability graph3 = new Ability(graph2.ability, "Cat_Graph_3_A", graph1.Cost);
            graph3.Name = "Exponential Graph";
            graph3.Description = "Remove all Entropy from all enemies.\nDeal damage to the Opposing enemy equal to the amount of Entropy removed.\nHeal this party member 4 health.";
            graph3.Effects[2].entryVariable = 4;
            graph3.EffectIntents[1].intents[0] = "Damage_16_20";

            Ability graph4 = new Ability(graph3.ability, "Cat_Graph_4_A", graph1.Cost);
            graph4.Name = "Factorial Graph";
            graph4.Description = "Remove all Entropy from all enemies.\nDeal damage to the Opposing enemy equal to the amount of Entropy removed.\nHeal this party member 5 health.";
            graph4.Effects[2].entryVariable = 5;
            graph4.EffectIntents[2].intents[0] = "Heal_5_10";
            graph4.EffectIntents[1].intents[0] = "Damage_21";

            Ability line1 = new Ability("Line Shape", "Cat_Line_1_A");
            line1.Description = "Deal 5 damage to the Left and Right enemies and inflict 4 Entropy on them.\nGain 5 Entropy.";
            line1.AbilitySprite = ResourceLoader.LoadSprite("ability_line.png");
            line1.Cost = [Pigments.Blue, Pigments.Red, Pigments.Red];
            line1.Effects = new EffectInfo[3];
            line1.Effects[0] = Effects.GenerateEffect(damage, 5, Slots.LeftRight);
            line1.Effects[1] = Effects.GenerateEffect(entropy, 4, Slots.LeftRight);
            line1.Effects[2] = Effects.GenerateEffect(entropy, 5, Slots.Self);
            line1.AddIntentsToTarget(Slots.LeftRight, ["Damage_3_6", Entropy.Intent]);
            line1.AddIntentsToTarget(Slots.Self, [Entropy.Intent]);
            line1.AnimationTarget = Slots.LeftRight;
            line1.Visuals = Visuals.Strum;

            Ability line2 = new Ability(line1.ability, "Cat_Line_2_A", line1.Cost);
            line2.Name = "Line Weight";
            line2.Description = "Deal 7 damage to the Left and Right enemies and inflict 5 Entropy on them.\nGain 5 Entropy.";
            line2.Effects[0].entryVariable = 7;
            line2.Effects[1].entryVariable = 5;
            line2.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability line3 = new Ability(line2.ability, "Cat_Line_3_A", line1.Cost);
            line3.Name = "Line Gesture";
            line3.Description = "Deal 9 damage to the Left and Right enemies and inflict 6 Entropy on them.\nGain 5 Entropy.";
            line3.Effects[0].entryVariable = 9;
            line3.Effects[1].entryVariable = 6;

            Ability line4 = new Ability(line3.ability, "Cat_Line_4_A", line1.Cost);
            line4.Name = "Line Contour";
            line4.Description = "Deal 10 damage to the Left and Right enemies and inflict 8 Entropy on them.\nGain 5 Entropy.";
            line4.Effects[0].entryVariable = 10;
            line4.Effects[1].entryVariable = 8;

            Ability experiment = new Ability(line4.ability, "experiment_A", [Pigments.Purple]);
            experiment.Name = "EXPERIMENT A";
            experiment.AbilitySprite = ResourceLoader.LoadSprite("ability_face.png");
            CopyAndSpawnCustomCharacterAnywhereEffect c;

            catten.AddLevelData(20, [interest1, line1, graph1]);
            catten.AddLevelData(22, [interest2, line2, graph2]);
            catten.AddLevelData(25, [interest3, line3, graph3]);
            catten.AddLevelData(27, [interest4, line4, graph4]);
            catten.AddCharacter(true);
        }

        public static void Items()
        {
            PerformEffect_Item hazard = new PerformEffect_Item("Aprils_SafetyHazard_SW", []);
            hazard.Name = "Safety Hazard";
            hazard.Flavour = "\"Something might happen...\"";
            hazard.Description = "Deal 25% more damage to targets in Slip.";
            hazard.Icon = ResourceLoader.LoadSprite("item_safetyhazard.png");
            hazard.EquippedModifiers = [];
            hazard.TriggerOn = TriggerCalls.OnWillApplyDamage;
            hazard.DoesPopUpInfo = false;
            hazard.Conditions = [ScriptableObject.CreateInstance<SafetyHazardCondition>()];
            hazard.DoesActionOnTriggerAttached = false;
            hazard.ConsumeOnTrigger = TriggerCalls.Count;
            hazard.ConsumeOnUse = false;
            hazard.ConsumeConditions = [];
            hazard.ShopPrice = 1;
            hazard.IsShopItem = true;
            hazard.StartsLocked = true;
            hazard.OnUnlockUsesTHE = true;
            hazard.UsesSpecialUnlockText = false;
            hazard.SpecialUnlockID = UILocID.None;
            hazard.item._ItemTypeIDs = [];
            hazard.item.AddItem("locked_safetyhazard.png", HeavenACH);

            PerformEffect_Item molar = new PerformEffect_Item("Aprils_Molars_TW", []);
            molar.Name = "Molars";
            molar.Flavour = "\"Cheek Teeth\"";
            molar.Description = "Double Status Effects on damaged targets.";
            molar.Icon = ResourceLoader.LoadSprite("item_molars.png");
            molar.EquippedModifiers = [];
            molar.TriggerOn = AdvancedDamageTrigger.Dealt;
            molar.DoesPopUpInfo = false;
            molar.Conditions = [ScriptableObject.CreateInstance<MolarsCondition>()];
            molar.DoesActionOnTriggerAttached = false;
            molar.ConsumeOnTrigger = TriggerCalls.Count;
            molar.ConsumeOnUse = false;
            molar.ConsumeConditions = [];
            molar.ShopPrice = 4;
            molar.IsShopItem = false;
            molar.StartsLocked = true;
            molar.OnUnlockUsesTHE = true;
            molar.UsesSpecialUnlockText = false;
            molar.SpecialUnlockID = UILocID.None;
            molar.item._ItemTypeIDs = [];
            molar.item.AddItem("locked_molars.png", OsmanACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Catten", "Safety Hazard", "Molars", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Catten_CH", "Aprils_SafetyHazard_SW", "Aprils_Molars_TW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }


        public static string HeavenACH => "Aprils_Catten_Heaven_ACH";
        public static string OsmanACH => "Aprils_Catten_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Catten_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Catten_Osman_Unlock";
    }
}
