using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    //ApplyAcidByPreviousEffect
    //TargetPerformAllAbilitiesEffect
    //SubOrRootBySidesEffect.Create (apply acid)
    //HalveAcidEffect
    public static class Lich
    {
        public static void Add()
        {
            PerformEffectPassiveAbility undead = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            undead.name = "Undead_PA_FORTHELOVEOFGODTHISWONTWORKONANYONEOTHERTHANLICH";
            undead._passiveName = "Undead";
            undead.passiveIcon = ResourceLoader.LoadSprite("UndeadPassive.png");
            undead.m_PassiveID = IDs.Undead;
            undead._enemyDescription = "FUCK you.";
            undead._characterDescription = "On death, return to the world.";
            undead.doesPassiveTriggerInformationPanel = true;
            undead.conditions = [];
            undead.effects = [];
            undead._triggerOn = [TriggerCalls.OnDeath];

            ExtraCCSprites_BasicSO lichExtra = ScriptableObject.CreateInstance<ExtraCCSprites_BasicSO>();
            lichExtra._DefaultID = IDs.LichDefault;
            lichExtra._frontSprite = ResourceLoader.LoadSprite("LichWorms.png");
            lichExtra._SpecialID = IDs.Lich;
            lichExtra._backSprite = ResourceLoader.LoadSprite("LichBack.png");

            SetCasterExtraSpritesRandomUpToEntryEffect lichSprites = ScriptableObject.CreateInstance<SetCasterExtraSpritesRandomUpToEntryEffect>();
            lichSprites._spriteType = lichExtra._SpecialID;
            SetCasterExtraSpritesRandomUpToEntryEffect lichDefault = ScriptableObject.CreateInstance<SetCasterExtraSpritesRandomUpToEntryEffect>();
            lichDefault._spriteType = lichExtra._DefaultID;

            Character lich = new Character("Von König", "Lich_CH");
            lich.HealthColor = Pigments.Red;
            lich.AddUnitType("FemaleID");
            lich.AddUnitType("FemaleLooking");
            lich.AddUnitType("Sandwich_Spirit");
            lich.UsesBasicAbility = true;
            //slap
            lich.UsesAllAbilities = false;
            lich.MovesOnOverworld = true;
            //animator
            lich.FrontSprite = ResourceLoader.LoadSprite("LichFront.png");
            lich.BackSprite = ResourceLoader.LoadSprite("LichBack.png");
            lich.OverworldSprite = ResourceLoader.LoadSprite("LichWorld.png", new Vector2(0.5f, 0f));
            lich.ExtraSprites = lichExtra;
            lich.DamageSound = "event:/Lunacy/SOUNDS4/StalkerHit";
            lich.DeathSound = "event:/Lunacy/SOUNDS4/StalkerDie";
            lich.DialogueSound = "event:/Lunacy/SOUNDS4/StalkerHit";
            //lich.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            //lich.AddFinalBossAchievementData("Heaven", HeavenACH);
            lich.GenerateMenuCharacter(ResourceLoader.LoadSprite("LichMenu.png"), ResourceLoader.LoadSprite("LichLock.png"));
            lich.MenuCharacterIsSecret = false;
            lich.MenuCharacterIgnoreRandom = false;
            lich.SetMenuCharacterAsFullSupport();
            lich.AddPassive(undead);

            ApplyAcidByPreviousEffect previousAcid = ScriptableObject.CreateInstance<ApplyAcidByPreviousEffect>();

            BaseCombatTargettingSO right = Targeting.Slot_AllyRight;

            HealEffect heal = ScriptableObject.CreateInstance<HealEffect>();

            Ability fuzzy1 = new Ability("Fuzzy with Vomit", "Lich_Fuzzy_1_A");
            fuzzy1.Description = "Halve the duration of Acid on the Right ally, then convert all Negative Status Effects on them into Acid.\nHeal them 7 health.";
            fuzzy1.AbilitySprite = ResourceLoader.LoadSprite("ability_fuzzy.png");
            fuzzy1.Cost = [Pigments.Blue, Pigments.Blue];
            fuzzy1.Effects = new EffectInfo[4];
            fuzzy1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<HalveAcidEffect>(), 1, right);
            fuzzy1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RemoveAllStatusEffectsEffect>(), 1, right);
            fuzzy1.Effects[2] = Effects.GenerateEffect(previousAcid, 1, right);
            fuzzy1.Effects[3] = Effects.GenerateEffect(heal, 7, right);
            fuzzy1.AddIntentsToTarget(right, ["Misc", Acid.Intent, "Heal_5_10"]);
            fuzzy1.AnimationTarget = right;
            fuzzy1.Visuals = CustomVisuals.GetVisuals("Salt/Ribbon");

            Ability fuzzy2 = new Ability(fuzzy1.ability, "Lich_Fuzzy_2_A", fuzzy1.Cost);
            fuzzy2.Name = "Fuzzy with Mold";
            fuzzy2.Description = "Halve the duration of Acid on the Right ally, then convert all Negative Status Effects on them into Acid.\nHeal them 9 health.";
            fuzzy2.Effects[3].entryVariable = 9;

            Ability fuzzy3 = new Ability(fuzzy2.ability, "Lich_Fuzzy_3_A", fuzzy1.Cost);
            fuzzy3.Name = "Fuzzy with Parasites";
            fuzzy3.Description = "Halve the duration of Acid on the Right ally, then convert all Negative Status Effects on them into Acid.\nHeal them 11 health.";
            fuzzy3.Effects[3].entryVariable = 11;
            fuzzy3.EffectIntents[0].intents[2] = "Heal_11_20";

            Ability fuzzy4 = new Ability(fuzzy3.ability, "Lich_Fuzzy_4_A", fuzzy1.Cost);
            fuzzy4.Name = "Fuzzy with Larvae";
            fuzzy4.Description = "Halve the duration of Acid on the Right ally, then convert all Negative Status Effects on them into Acid.\nHeal them 13 health.";
            fuzzy4.Effects[3].entryVariable = 13;

            Ability baptism1 = new Ability("Baptism of Beetles", "Lich_Baptism_1_A");
            baptism1.Description = "Force the Right ally to perform all of their abilities.\nInflict 4 Acid on them.";
            baptism1.AbilitySprite = ResourceLoader.LoadSprite("ability_baptism.png");
            baptism1.Cost = [Pigments.Blue, Pigments.Blue, Pigments.Purple];
            baptism1.Effects = new EffectInfo[2];
            baptism1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<TargetPerformAllAbilitiesEffect>(), 1, right);
            baptism1.Effects[1] = Effects.GenerateEffect(SubOrRootBySidesEffect.Create([Effects.GenerateEffect(previousAcid, 1, Slots.Self)], false), 4, right);
            baptism1.AddIntentsToTarget(right, ["Misc_Hidden", Acid.Intent]);
            baptism1.AnimationTarget = right;
            baptism1.Visuals = Visuals.LifeLink;

            Ability baptism2 = new Ability(baptism1.ability, "Lich_Baptism_2_A", baptism1.Cost);
            baptism2.Name = "Baptism of Flies";
            baptism2.Description = "Force the Right ally to perform all of their abilities.\nInflict 3 Acid on them.";
            baptism2.Effects[1].entryVariable = 3;

            Ability baptism3 = new Ability(baptism2.ability, "Lich_Baptism_3_A", [Pigments.BlueYellow, Pigments.Blue, Pigments.Purple]);
            baptism3.Name = "Baptism of Maggots";

            Ability baptism4 = new Ability(baptism3.ability, "Lich_Baptism_4_A", [Pigments.Grey, Pigments.Blue, Pigments.Purple]);
            baptism4.Name = "Baptism of Worms";

            RemoveStatusEffectEffect rem_acid = ScriptableObject.CreateInstance<RemoveStatusEffectEffect>();
            rem_acid._status = Acid.Object;

            Ability inside1 = new Ability("Inside Me, Squishies", "Lich_Inside_1_A");
            inside1.Description = "Remove all Acid from the Right ally, then move all Acid from all party members to them.\nHeal them 3 health.";
            inside1.AbilitySprite = ResourceLoader.LoadSprite("ability_insideme.png");
            inside1.Cost = [Pigments.Blue];
            inside1.Effects = new EffectInfo[5];
            inside1.Effects[0] = Effects.GenerateEffect(rem_acid, 1, right);
            inside1.Effects[1] = Effects.GenerateEffect(rem_acid, 1, Targeting.Unit_AllAllies);
            inside1.Effects[2] = Effects.GenerateEffect(previousAcid, 1, right);
            inside1.Effects[3] = Effects.GenerateEffect(heal, 3, right);
            inside1.Effects[4] = Effects.GenerateEffect(lichSprites, 1, Slots.Self);
            inside1.AddIntentsToTarget(Targetting.Everything(true), [Acid.Rem]);
            inside1.AddIntentsToTarget(right, [Acid.Intent, "Heal_1_4"]);
            inside1.AnimationTarget = right;
            inside1.Visuals = Visuals.WrigglingWrath;

            Ability inside2 = new Ability(inside1.ability, "Lich_Inside_2_A", inside1.Cost);
            inside2.Name = "Inside Me, Wrigglies";
            inside2.Description = "Remove all Acid from the Right ally, then move all Acid from all party members to them.\nHeal them 4 health.";
            inside2.Effects[3].entryVariable = 4;

            Ability inside3 = new Ability(inside2.ability, "Lich_Inside_3_A", inside1.Cost);
            inside3.Name = "Inside Me, Crawlies";
            inside3.Description = "Remove all Acid from the Right ally, then move all Acid from all party members to them.\nHeal them 5 health.";
            inside3.Effects[3].entryVariable = 5;
            inside3.EffectIntents[1].intents[1] = "Heal_5_10";

            Ability inside4 = new Ability(inside3.ability, "Lich_Inside_4_A", inside1.Cost);
            inside4.Name = "Inside Me, Squirmies";
            inside4.Description = "Remove all Acid from the Right ally, then move all Acid from all party members to them.\nHeal them 6 health.";
            inside4.Effects[3].entryVariable = 6;



            lich.AddLevelData(6, [fuzzy1, baptism1, inside1]);
            lich.AddLevelData(8, [fuzzy2, baptism2, inside2]);
            lich.AddLevelData(10, [fuzzy3, baptism3, inside3]);
            lich.AddLevelData(11, [fuzzy4, baptism4, inside4]);
            lich.AddCharacter(true);
        }


        public static string HeavenACH => "Aprils_Lich_Heaven_ACH";
        public static string OsmanACH => "Aprils_Lich_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Lich_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Lich_Osman_Unlock";
    }
}
