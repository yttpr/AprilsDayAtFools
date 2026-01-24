using BrutalAPI;
using BrutalAPI.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Sunflower
    {
        public static void Add()
        {
            ApplyWaterSlotEffect water = ScriptableObject.CreateInstance<ApplyWaterSlotEffect>();

            PerformEffectPassiveAbility trench = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            trench.name = "Trench_2_PA";
            trench._passiveName = "Trench (2)";
            trench.m_PassiveID = "Trench_PA";
            trench.passiveIcon = ResourceLoader.LoadSprite("TrenchPassive.png");
            trench._characterDescription = "On moving, gain 2 Deep Water.";
            trench._enemyDescription = trench._characterDescription;
            trench._triggerOn = [TriggerCalls.OnMoved];
            trench.conditions = [];
            trench.doesPassiveTriggerInformationPanel = true;
            trench.effects = [Effects.GenerateEffect(water, 2, Slots.Self)];
            trench.AddToPassiveDatabase();
            trench.AddPassiveToGlossary("Trench", "On moving, gain a certain amount of Deep Water.");

            Character sunflower = new Character("Sunflower", "Sunflower_CH");
            sunflower.HealthColor = Pigments.Blue;
            sunflower.AddUnitType("FemaleID");
            sunflower.AddUnitType("FemaleLooking");
            sunflower.UsesBasicAbility = true;
            //slap
            sunflower.UsesAllAbilities = false;
            sunflower.MovesOnOverworld = true;
            sunflower.Animator = Joyce.Assets.LoadAsset<RuntimeAnimatorController>("Assets/SunflowerAnim/SunflowerAnimation.overrideController");
            sunflower.FrontSprite = ResourceLoader.LoadSprite("SunflowerFront.png");
            sunflower.BackSprite = ResourceLoader.LoadSprite("SunflowerBack.png");
            sunflower.OverworldSprite = ResourceLoader.LoadSprite("SunflowerWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            sunflower.DamageSound = "event:/Lunacy/SOUNDS3/ApparatusHit";
            sunflower.DeathSound = "event:/Lunacy/SOUNDS3/ApparatusDie";
            sunflower.DialogueSound = "";
            sunflower.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            sunflower.AddFinalBossAchievementData("Heaven", HeavenACH);
            sunflower.GenerateMenuCharacter(ResourceLoader.LoadSprite("SunflowerMenu.png"), ResourceLoader.LoadSprite("SunflowerLock.png"));
            sunflower.MenuCharacterIsSecret = false;
            sunflower.MenuCharacterIgnoreRandom = false;
            sunflower.SetMenuCharacterAsFullSupport();
            sunflower.AddPassives([trench]);

            TargettingByFieldEffect inWater = TargettingByFieldEffect.Create(true, Water.FieldID, false);
            DoubleTargetting waterLeft = ScriptableObject.CreateInstance<DoubleTargetting>();
            waterLeft.firstTargetting = inWater;
            waterLeft.secondTargetting = Targeting.Slot_AllyLeft;

            ApplyShieldSlotEffect shield = ScriptableObject.CreateInstance<ApplyShieldSlotEffect>();

            Ability buried1 = new Ability("Buried Under", "Flower_Buried_1_A");
            buried1.Description = "Inflict 2 Deep Water on the Left allied position.\nHeal all allies in Deep Water 3 health, apply 3 Shield to them if no healing was dealt.";
            buried1.AbilitySprite = ResourceLoader.LoadSprite("ability_buried.png");
            buried1.Cost = [Pigments.Blue, Pigments.Yellow];
            buried1.Effects = new EffectInfo[2];
            buried1.Effects[0] = Effects.GenerateEffect(water, 2, Targeting.Slot_AllyLeft);
            buried1.Effects[1] = Effects.GenerateEffect(ShieldIfNotHealEffect.Create(3, shield), 3, inWater);
            buried1.AddIntentsToTarget(Targeting.Slot_AllyLeft, [Water.Intent]);
            buried1.AddIntentsToTarget(Targetting.Everything(true), ["Misc_Hidden"]);
            buried1.AddIntentsToTarget(Targeting.Unit_AllAllies, ["Heal_1_4"]);
            buried1.AnimationTarget = waterLeft;
            buried1.Visuals = CustomVisuals.GetVisuals("Salt/Claws");

            Ability buried2 = new Ability(buried1.ability, "Flower_Buried_2_A", [Pigments.Blue, Pigments.Grey]);
            buried2.Name = "Buried Hands";
            buried2.Description = "Inflict 2 Deep Water on the Left allied position.\nHeal all allies in Deep Water 4 health, apply 4 Shield to them if no healing was dealt.";
            buried2.Effects[1].entryVariable = 4;
            buried2.Effects[1].effect = ShieldIfNotHealEffect.Create(4, shield);
            buried2.EffectIntents[2].intents[0] = "Heal_5_10";

            Ability buried3 = new Ability(buried2.ability, "Flower_Buried_3_A", buried2.Cost);
            buried3.Name = "Buried Away";
            buried3.Description = "Inflict 2 Deep Water on the Left allied position.\nHeal all allies in Deep Water 5 health, apply 5 Shield to them if no healing was dealt.";
            buried3.Effects[1].entryVariable = 5;
            buried3.Effects[1].effect = ShieldIfNotHealEffect.Create(5, shield);

            Ability buried4 = new Ability(buried3.ability, "Flower_Buried_4_A", buried2.Cost);
            buried4.Name = "Buried Memories";
            buried4.Description = "Inflict 2 Deep Water on the Left allied position.\nHeal all allies in Deep Water 6 health, apply 6 Shield to them if no healing was dealt.";
            buried4.Effects[1].entryVariable = 6;
            buried4.Effects[1].effect = ShieldIfNotHealEffect.Create(6, shield);

            Ability void1 = new Ability("Nightmares of Loneliness", "Flower_Void_1_A");
            void1.Description = "Heal the Right ally by their current health plus the amount of Drowning they have.\nInflict 4 Deep Water on the Right allied position.";
            void1.AbilitySprite = ResourceLoader.LoadSprite("ability_void.png");
            void1.Cost = [Pigments.Blue, Pigments.Red];
            void1.Effects = new EffectInfo[2];
            void1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<HealByHealthPlusDrowningEffect>(), 0, Targeting.Slot_AllyRight);
            void1.Effects[1] = Effects.GenerateEffect(water, 4, Targeting.Slot_AllyRight);
            void1.AddIntentsToTarget(Targeting.Slot_AllyRight, ["Heal_11_20", Water.Intent]);
            void1.AnimationTarget = Targeting.Slot_AllyRight;
            void1.Visuals = CustomVisuals.GetVisuals("Salt/Monster");

            Ability void2 = new Ability(void1.ability, "Flower_Void_2_A", [Pigments.Blue, Pigments.BlueRed]);
            void2.Name = "Nightmares of Solitude";
            void2.Description = "Heal the Right ally by their current health plus the amount of Drowning they have.\nInflict 5 Deep Water on the Right allied position.";
            void2.Effects[1].entryVariable = 5;

            Ability void3 = new Ability(void2.ability, "Flower_Void_3_A", [Pigments.BlueYellow, Pigments.BlueRed]);
            void3.Name = "Nightmares of Emptiness";
            void3.Description = "Heal the Right ally by their current health plus the amount of Drowning they have.\nInflict 6 Deep Water on the Right allied position.";
            void3.Effects[1].entryVariable = 6;

            Ability void4 = new Ability(void3.ability, "Flower_Void_4_A", [Pigments.Grey, Pigments.BlueRed]);
            void4.Name = "Nightmares of Void";
            void4.Description = "Heal the Right ally by their current health plus the amount of Drowning they have.\nInflict 7 Deep Water on the Right allied position.";
            void4.Effects[1].entryVariable = 7;

            CascadeHealPercentEffect hazeEffect = ScriptableObject.CreateInstance<CascadeHealPercentEffect>();
            hazeEffect._cascadeDecay = 0.5f;
            hazeEffect._consistentCascade = true;
            hazeEffect._directHeal = true;
            hazeEffect._usePreviousExitValue = true;
            RemoveStatusEffectEffect remove = ScriptableObject.CreateInstance<RemoveStatusEffectEffect>();
            remove._status = Drowning.Object;

            Ability haze1 = new Ability("Afternoon Haze", "Flower_Haze_1_A");
            haze1.Description = "Remove all Drowning from all party members.\nHeal this party member for the amount of Drowning removed, healing spreads Left and Right.";
            haze1.AbilitySprite = ResourceLoader.LoadSprite("ability_haze.png");
            haze1.Cost = [Pigments.Blue, Pigments.Blue, Pigments.Blue, Pigments.Blue];
            haze1.Effects = new EffectInfo[2];
            haze1.Effects[0] = Effects.GenerateEffect(remove, 1, Targeting.Unit_AllAllies);
            haze1.Effects[1] = Effects.GenerateEffect(hazeEffect, 0, Slots.SlotTarget([0, -1, -2, -3, -4, 1, 2, 3, 4], true));
            haze1.AddIntentsToTarget(Targeting.Unit_AllAllies, [Drowning.Rem]);
            haze1.AddIntentsToTarget(Slots.Self, ["Heal_21"]);
            haze1.AddIntentsToTarget(Targeting.Slot_AllyLeft, ["Swap_Left"]);
            haze1.AddIntentsToTarget(Targeting.Slot_AllyRight, ["Swap_Right"]);
            haze1.AnimationTarget = Slots.Self;
            haze1.Visuals = LoadedAssetsHandler.GetEnemyAbility("Boil_A").visuals;

            Ability haze2 = new Ability(haze1.ability, "Flower_Haze_2_A", haze1.Cost);
            haze2.Name = "Sunset Haze";
            haze2.Description = "Remove all Drowning from all party members.\nHeal this party member for the amount of Drowning removed + 2, healing spreads Left and Right.";
            haze2.Effects[1].entryVariable = 2;

            Ability haze3 = new Ability(haze2.ability, "Flower_Haze_3_A", haze1.Cost);
            haze3.Name = "Twilight Haze";
            haze3.Description = "Remove all Drowning from all party members.\nHeal this party member for the amount of Drowning removed + 3, healing spreads Left and Right.";
            haze3.Effects[1].entryVariable = 3;

            Ability haze4 = new Ability(haze3.ability, "Flower_Haze_4_A", haze1.Cost);
            haze4.Name = "Moonless Haze";
            haze4.Description = "Remove all Drowning from all party members.\nHeal this party member for the amount of Drowning removed + 5, healing spreads Left and Right.";
            haze4.Effects[1].entryVariable = 5;


            sunflower.AddLevelData(8, [buried1, void1, haze1]);
            sunflower.AddLevelData(11, [buried2, void2, haze2]);
            sunflower.AddLevelData(15, [buried3, void3, haze3]);
            sunflower.AddLevelData(17, [buried4, void4, haze4]);
            sunflower.AddCharacter(true);
        }



        public static void Items()
        {
            PerformEffect_Item sadness = new PerformEffect_Item("Aprils_DepictionOfSadness_TW", []);
            sadness.Name = "Depiction of Sorrow";
            sadness.Flavour = "\"Do you feel no pity?\"";
            sadness.Description = "Block all Ruptured, Oil-Slicked, Left, Entropy, Frail, Scars, Cursed, Muted, and Acid and convert it into Drowning.";
            sadness.Icon = ResourceLoader.LoadSprite("item_depictionofsadness.png");
            sadness.EquippedModifiers = [];
            sadness.TriggerOn = TriggerCalls.CanApplyStatusEffect;
            sadness.DoesPopUpInfo = false;
            sadness.Conditions = [ScriptableObject.CreateInstance<SadnessCondition>()];
            sadness.DoesActionOnTriggerAttached = false;
            sadness.ConsumeOnTrigger = TriggerCalls.Count;
            sadness.ConsumeOnUse = false;
            sadness.ConsumeConditions = [];
            sadness.ShopPrice = 4;
            sadness.IsShopItem = false;
            sadness.StartsLocked = true;
            sadness.OnUnlockUsesTHE = true;
            sadness.UsesSpecialUnlockText = false;
            sadness.SpecialUnlockID = UILocID.None;
            sadness.item._ItemTypeIDs = [];
            sadness.item.AddItem("locked_depictionofsadness.png", HeavenACH);

            PerformEffect_Item rope = new PerformEffect_Item("Aprils_InfiniteRope_SW", []);
            rope.Name = "Infinite Rope";
            rope.Flavour = "\"We'll reach somewhere eventually.\"";
            rope.Description = "This party member is immune to Drowning, Left, and Divine Protection.";
            rope.Icon = ResourceLoader.LoadSprite("item_infiniterope.png");
            rope.EquippedModifiers = [];
            rope.TriggerOn = TriggerCalls.CanApplyStatusEffect;
            rope.DoesPopUpInfo = false;
            rope.Conditions = [ScriptableObject.CreateInstance<InfiniteRopeCondition>()];
            rope.DoesActionOnTriggerAttached = false;
            rope.ConsumeOnTrigger = TriggerCalls.Count;
            rope.ConsumeOnUse = false;
            rope.ConsumeConditions = [];
            rope.ShopPrice = 3;
            rope.IsShopItem = true;
            rope.StartsLocked = true;
            rope.OnUnlockUsesTHE = true;
            rope.UsesSpecialUnlockText = false;
            rope.SpecialUnlockID = UILocID.None;
            rope.item._ItemTypeIDs = [];
            rope.item.AddItem("locked_infiniterope.png", OsmanACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Sunflower", "Depiction of Sorrow", "Infinite Rope", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Sunflower_CH", "Aprils_DepictionOfSadness_TW", "Aprils_InfiniteRope_SW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Sunflower_Heaven_ACH";
        public static string OsmanACH => "Aprils_Sunflower_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Sunflower_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Sunflower_Osman_Unlock";
    }
}
