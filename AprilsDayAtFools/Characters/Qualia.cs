using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Qualia
    {
        public static void Test()
        {
            ImmediatePerformEffectPassive depiction = ScriptableObject.CreateInstance<ImmediatePerformEffectPassive>();
            depiction._passiveName = "Depiction";
            depiction.passiveIcon = ResourceLoader.LoadSprite("DepictionPassive.png");
            depiction.m_PassiveID = IDs.Depiction;
            depiction._enemyDescription = "This enemy is only temporary";
            depiction._characterDescription = "This party member is only temporary.";
            depiction.doesPassiveTriggerInformationPanel = true;
            depiction.conditions = [];
            depiction.effects = [Effects.GenerateEffect(ScriptableObject.CreateInstance<FleeTargetEffect>(), 1, Slots.Self)];
            depiction._triggerOn = [TimelineEndHandler.Before];
            depiction.AddToPassiveDatabase();
            depiction.AddPassiveToGlossary("Depiction", "This unit is only temporary.");

            ExtraPassiveAbility_Wearable_SMS add_picture = ScriptableObject.CreateInstance<ExtraPassiveAbility_Wearable_SMS>();
            add_picture._extraPassiveAbility = depiction;

            TemporaryReplacementEffect replace_all = ScriptableObject.CreateInstance<TemporaryReplacementEffect>();
            replace_all._extraModifiers = [add_picture];

            TemporaryReplacementEffect replace_unused = ScriptableObject.CreateInstance<TemporaryReplacementEffect>();
            replace_unused._extraModifiers = [add_picture];

            TargettingCanUseAbilities unused = ScriptableObject.CreateInstance<TargettingCanUseAbilities>();
            unused.getAllies = true;
            unused.getAllUnitSlots = false;
            unused.ignoreCastSlot = true;
        }
        public static void Add()
        {
            Character qualia = new Character("Qualia", "Qualia_CH");
            qualia.HealthColor = Pigments.Purple;
            qualia.AddUnitType("FemaleID");
            qualia.AddUnitType("FemaleLooking");
            qualia.AddUnitType(SlidingHandler.Type);
            qualia.UsesBasicAbility = true;
            //slap replacer
            qualia.UsesAllAbilities = false;
            qualia.MovesOnOverworld = true;
            qualia.Animator = Joyce.Assets.LoadAsset<RuntimeAnimatorController>("Assets/QualiaAnim/QualiaAnimator.overrideController");
            qualia.FrontSprite = ResourceLoader.LoadSprite("QualiaFront.png");
            qualia.BackSprite = ResourceLoader.LoadSprite("QualiaBack.png");
            qualia.OverworldSprite = ResourceLoader.LoadSprite("QualiaWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            qualia.DamageSound = "event:/Lunacy/SOUNDS4/YangHit";
            qualia.DeathSound = "event:/Lunacy/SOUNDS4/YangDie";
            qualia.DialogueSound = "event:/Lunacy/SOUNDS4/YangHit";
            //qualia.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            //qualia.AddFinalBossAchievementData("Heaven", HeavenACH);
            qualia.GenerateMenuCharacter(ResourceLoader.LoadSprite("QualiaMenu.png"), ResourceLoader.LoadSprite("QualiaLock.png"));
            qualia.MenuCharacterIsSecret = false;
            qualia.MenuCharacterIgnoreRandom = false;
            qualia.SetMenuCharacterAsFullSupport();

            ImmediatePerformEffectPassive depiction = ScriptableObject.CreateInstance<ImmediatePerformEffectPassive>();
            depiction._passiveName = "Depiction";
            depiction.passiveIcon = ResourceLoader.LoadSprite("DepictionPassive.png");
            depiction.m_PassiveID = IDs.Depiction;
            depiction._enemyDescription = "This enemy is only temporary";
            depiction._characterDescription = "This party member is only temporary.";
            depiction.doesPassiveTriggerInformationPanel = true;
            depiction.conditions = [];
            depiction.effects = [Effects.GenerateEffect(ScriptableObject.CreateInstance<FleeTargetEffect>(), 1, Slots.Self)];
            depiction._triggerOn = [TimelineEndHandler.Before];
            depiction.AddToPassiveDatabase();
            depiction.AddPassiveToGlossary("Depiction", "This unit is only temporary.");

            ExtraPassiveAbility_Wearable_SMS add_picture = ScriptableObject.CreateInstance<ExtraPassiveAbility_Wearable_SMS>();
            add_picture._extraPassiveAbility = depiction;

            TemporaryReplacementEffect replace_all = ScriptableObject.CreateInstance<TemporaryReplacementEffect>();
            replace_all._extraModifiers = [add_picture];

            TemporaryReplacementEffect replace_unused = ScriptableObject.CreateInstance<TemporaryReplacementEffect>();
            replace_unused._extraModifiers = [add_picture];

            TargettingCanUseAbilities unused = ScriptableObject.CreateInstance<TargettingCanUseAbilities>();
            unused.getAllies = true;
            unused.getAllUnitSlots = false;
            unused.ignoreCastSlot = true;

            SpawnEnemyAnywhereEffect summonScrap = ScriptableObject.CreateInstance<SpawnEnemyAnywhereEffect>();
            summonScrap._spawnTypeID = "Spawn_Basic";
            summonScrap.enemy = LoadedAssetsHandler.GetEnemy("ScrapBomb_EN");
            Ability design1 = new Ability("Scrap Design", "Qualia_Design_1_A");
            design1.Description = "Deal 4 damage to a random enemy.\nGain 4 Shield and spawn a Scrap Bomb.";
            design1.Cost = [Pigments.Red, Pigments.Blue];
            design1.AbilitySprite = ResourceLoader.LoadSprite("ability_creak");
            design1.Effects = new EffectInfo[3];
            design1.Effects[0] = Effects.GenerateEffect(replace1.Effects[0].effect, 4, ScriptableObject.CreateInstance<TargettingRandomUnit>());
            design1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyShieldSlotEffect>(), 4, Slots.Self);
            design1.Effects[2] = Effects.GenerateEffect(summonScrap, 1);
            design1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Damage_3_6"]);
            design1.AddIntentsToTarget(Slots.Self, ["Field_Shield"]);
            design1.Visuals = CustomVisuals.GetVisuals("Salt/Cube");
            design1.AnimationTarget = Slots.Self;

            Ability design2 = new Ability(design1.ability, "Qualia_Design_2_A", design1.Cost);
            design2.Name = "Creak Design";
            design2.Description = "Deal 6 damage to a random enemy.\nGain 5 Shield and spawn a Scrap Bomb.";
            design2.Effects[0].entryVariable = 6;
            design2.Effects[1].entryVariable = 5;

            Ability design3 = new Ability(design2.ability, "Qualia_Design_3_A", design1.Cost);
            design3.Name = "Tinker Design";
            design3.Description = "Deal 7 damage to a random enemy.\nGain 6 Shield and spawn a Scrap Bomb.";
            design3.Effects[0].entryVariable = 7;
            design3.Effects[1].entryVariable = 6;
            design3.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability design4 = new Ability(design3.ability, "Qualia_Design_4_A", design1.Cost);
            design4.Name = "Homebrew Design";
            design4.Description = "Deal 8 damage to a random enemy.\nGain 7 Shield and spawn a Scrap Bomb.";
            design4.Effects[0].entryVariable = 8;
            design4.Effects[1].entryVariable = 7;


            qualia.AddLevelData(10, [design1, replace1, rearrange1]);
            qualia.AddLevelData(14, [design2, replace2, rearrange2]);
            qualia.AddLevelData(17, [design3, replace3, rearrange3]);
            qualia.AddLevelData(19, [design4, replace4, rearrange4]);
            qualia.AddCharacter(true);
        }


        public static string HeavenACH => "Aprils_Qualia_Heaven_ACH";
        public static string OsmanACH => "Aprils_Qualia_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Qualia_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Qualia_Osman_Unlock";
    }
}
