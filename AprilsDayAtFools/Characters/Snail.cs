using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Snail
    {
        public static void Add()
        {
            Character snail = new Character("Snail", "Snail_CH");
            snail.HealthColor = Pigments.Grey;
            snail.AddUnitType("FemaleID");
            snail.AddUnitType("FemaleLooking");
            snail.AddUnitType("Sandwich_SuperOrganism");
            snail.UsesBasicAbility = true;
            //slap
            snail.UsesAllAbilities = false;
            snail.MovesOnOverworld = false;
            //custom animator
            snail.FrontSprite = SnailSpriteHandler.Sprites[Pigments.Green][0];
            snail.BackSprite = SnailSpriteHandler.Sprites[Pigments.Green][4] ;
            snail.OverworldSprite = ResourceLoader.LoadSprite("SnailWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            snail.DamageSound = "event:/Lunacy/SOUNDS4/YNLHit";
            snail.DeathSound = "event:/Lunacy/SOUNDS4/YNLDie";
            snail.DialogueSound = "event:/Lunacy/SOUNDS4/YNLRoar";
            //snail.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            //snail.AddFinalBossAchievementData("Heaven", HeavenACH);
            snail.GenerateMenuCharacter(ResourceLoader.LoadSprite("SnailMenu.png"), ResourceLoader.LoadSprite("SnailLock.png"));
            snail.MenuCharacterIsSecret = false;
            snail.MenuCharacterIgnoreRandom = false;
            snail.SetMenuCharacterAsFullDPS();

            SnailSpritesTriggerEffect trigger = ScriptableObject.CreateInstance<SnailSpritesTriggerEffect>();

            RandomizeTargetHealthColorsEffect randomize = RandomizeTargetHealthColorsEffect.Create(true);
            RandomizeTargetHealthColorsEffect nogray = RandomizeTargetHealthColorsEffect.Create(false);

            TargettingBySharingPigmentUsedColor used = ScriptableObject.CreateInstance<TargettingBySharingPigmentUsedColor>();
            StoreTargetting store = StoreTargetting.Create(used);
            GetStoredTargetting get = GetStoredTargetting.Create(store);

            Ability maw1 = new Ability("Feed the Maw", "Snail_TheMaw_1_A");
            maw1.Description = "Randomize the health colors of all enemies sharing health colors with the Pigment used for this ability, then inflict 2 Acid on them.\nRandomize the health color of this party member.";
            maw1.AbilitySprite = ResourceLoader.LoadSprite("ability_themaw.png");
            maw1.Cost = [Pigments.Grey, Pigments.Grey];
            maw1.Effects = new EffectInfo[6];
            maw1.Effects[0] = Effects.GenerateEffect(BasicEffects.SetStoreValue(SnailSpriteHandler.Maw), 1, Slots.Self);
            maw1.Effects[1] = Effects.GenerateEffect(trigger, 1, Slots.Self);
            maw1.Effects[2] = Effects.GenerateEffect(BasicEffects.GetVisuals("Siphon_A", false, store), 1, Slots.Self);
            maw1.Effects[3] = Effects.GenerateEffect(nogray, 1, store);
            maw1.Effects[4] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyAcidEffect>(), 2, get);
            maw1.Effects[5] = Effects.GenerateEffect(randomize, 1, Slots.Self);
            maw1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Misc_Hidden", "Mana_Modify", Acid.Intent]);
            maw1.AddIntentsToTarget(Slots.Self, ["Mana_Modify"]);

            Ability maw2 = new Ability(maw1.ability, "Snail_TheMaw_2_A", maw1.Cost);
            maw2.Name = "Nourish the Maw";
            maw2.Description = "Randomize the health colors of all enemies sharing health colors with the Pigment used for this ability, then inflict 3 Acid on them.\nRandomize the health color of this party member.";
            maw2.Effects[4].entryVariable = 3;

            Ability maw3 = new Ability(maw2.ability, "Snail_TheMaw_3_A", maw1.Cost);
            maw3.Name = "Fuel the Maw";
            maw3.Description = "Randomize the health colors of all enemies sharing health colors with the Pigment used for this ability, then inflict 5 Acid on them.\nRandomize the health color of this party member.";
            maw3.Effects[4].entryVariable = 5;

            Ability maw4 = new Ability(maw3.ability, "Snail_TheMaw_4_A", maw1.Cost);
            maw4.Name = "Bloat the Maw";
            maw4.Description = "Randomize the health colors of all enemies sharing health colors with the Pigment used for this ability, then inflict 9 Acid on them.\nRandomize the health color of this party member.";
            maw4.Effects[4].entryVariable = 9;

            AnimationVisualsEffect prov = ScriptableObject.CreateInstance<AnimationVisualsEffect>();
            prov._animationTarget = Slots.Front;
            prov._visuals = Visuals.Providence;

            DamageEffect damage = ScriptableObject.CreateInstance<DamageEffect>();

            SpawnEnemyAnywhereEffect sludge = ScriptableObject.CreateInstance<SpawnEnemyAnywhereEffect>();
            sludge._spawnTypeID = "Spawn_Basic";
            sludge.enemy = LoadedAssetsHandler.GetEnemy("TheSludge_EN");

            EffectConditionSO condition = ScriptableObject.CreateInstance<FrontTargetUniqueHealthColorCondition>();

            Ability hand1 = new Ability("Fear the Hand", "Snail_TheHand_1_A");
            hand1.Description = "If the Opposing enemy is the only enemy of its health color, inflict 2 Frail on them.\nDeal 5 damage to the Opposing enemy.\nAttempt to spawn The Sludge.";
            hand1.AbilitySprite = ResourceLoader.LoadSprite("ability_thehand.png");
            hand1.Cost = [Pigments.Blue];
            hand1.Effects = new EffectInfo[7];
            hand1.Effects[0] = Effects.GenerateEffect(BasicEffects.SetStoreValue(SnailSpriteHandler.Hand), 1, Slots.Self);
            hand1.Effects[1] = Effects.GenerateEffect(trigger, 1, Slots.Self);
            hand1.Effects[2] = Effects.GenerateEffect(prov, 1, Slots.Front, condition);
            hand1.Effects[3] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyFrailEffect>(), 2, Slots.Front, condition);
            hand1.Effects[4] = Effects.GenerateEffect(BasicEffects.GetVisuals("Slap_A", true, Slots.Front), 1, Slots.Front);
            hand1.Effects[5] = Effects.GenerateEffect(damage, 5, Slots.Front);
            hand1.Effects[6] = Effects.GenerateEffect(sludge, 1, Slots.Self);
            hand1.AddIntentsToTarget(Slots.Front, ["Misc_Hidden", "Status_Frail", "Damage_3_6"]);
            hand1.AddIntentsToTarget(Slots.Self, ["Other_Spawn"]);

            Ability hand2 = new Ability(hand1.ability, "Snail_TheHand_2_A", hand1.Cost);
            hand2.Name = "Dread the Hand";
            hand2.Description = "If the Opposing enemy is the only enemy of its health color, inflict 2 Frail on them.\nDeal 6 damage to the Opposing enemy.\nAttempt to spawn The Sludge.";
            hand2.Effects[5].entryVariable = 6;

            Ability hand3 = new Ability(hand2.ability, "Snail_TheHand_3_A");
            hand3.Name = "Awe the Hand";
            hand3.Description = "If the Opposing enemy is the only enemy of its health color, inflict 2 Frail on them.\nDeal 8 damage to the Opposing enemy.\nAttempt to spawn The Sludge.";
            hand3.Effects[5].entryVariable = 8;
            hand3.EffectIntents[0].intents[2] = "Damage_7_10";

            Ability hand4 = new Ability(hand3.ability, "Snail_TheHand_4_A");
            hand4.Name = "Revere the Hand";
            hand4.Description = "If the Opposing enemy is the only enemy of its health color, inflict 2 Frail on them.\nDeal 9 damage to the Opposing enemy.\nAttempt to spawn The Sludge.";
            hand4.Effects[5].entryVariable = 9;

            Ability camo1 = new Ability("Chameleon Snake", "Snail_Camo_1_A");
            camo1.Description = "Change this party member's health color to match the Opposing enemy's.\nDeal 6 damage to all enemies sharing this party member's health color.\nRandomize the Opposing enemy's health color.";
            camo1.AbilitySprite = ResourceLoader.LoadSprite("ability_chameleon.png");
            camo1.Cost = [Pigments.Blue, Pigments.Blue, Pigments.Red];
            camo1.Effects = new EffectInfo[3];
            camo1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<CasterCopyTargetHealthColorEffect>(), 1, Slots.Front);
            camo1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageTargetsSharingCasterHealthColorEffect>(), 6, Targeting.Unit_AllOpponents);
            camo1.Effects[2] = Effects.GenerateEffect(nogray, 1, Slots.Front);
            camo1.AddIntentsToTarget(Slots.Self, ["Mana_Modify"]);
            camo1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Damage_3_6"]);
            camo1.AddIntentsToTarget(Slots.Front, ["Mana_Modify"]);
            camo1.AnimationTarget = Slots.Front;
            camo1.Visuals = CustomVisuals.GetVisuals("Salt/Spotlight");

            Ability camo2 = new Ability(camo1.ability, "Snail_Camo_2_A", camo1.Cost);
            camo2.Name = "Chameleon Dance";
            camo2.Description = "Change this party member's health color to match the Opposing enemy's.\nDeal 7 damage to all enemies sharing this party member's health color.\nRandomize the Opposing enemy's health color.";
            camo2.Effects[1].entryVariable = 7;
            camo2.EffectIntents[1].intents[0] = "Damage_7_10";

            Ability camo3 = new Ability(camo2.ability, "Snail_Camo_3_A", camo1.Cost);
            camo3.Name = "Chameleon Skin";
            camo3.Description = "Change this party member's health color to match the Opposing enemy's.\nDeal 9 damage to all enemies sharing this party member's health color.\nRandomize the Opposing enemy's health color.";
            camo3.Effects[1].entryVariable = 9;

            Ability camo4 = new Ability(camo3.ability, "Snail_Camo_4_A", camo1.Cost);
            camo4.Name = "Chameleon Citizen";
            camo4.Description = "Change this party member's health color to match the Opposing enemy's.\nDeal 10 damage to all enemies sharing this party member's health color.\nRandomize the Opposing enemy's health color.";
            camo4.Effects[1].entryVariable = 10;

            snail.AddLevelData(10, [maw1, hand1, camo1]);
            snail.AddLevelData(15, [maw2, hand2, camo2]);
            snail.AddLevelData(16, [maw3, hand3, camo3]);
            snail.AddLevelData(19, [maw4, hand4, camo4]);
            snail.AddCharacter(true);
        }
        public static void AddDialogueEmote()
        {
            SpeakerBundle mad = new SpeakerBundle();
            mad.bundleTextColor = new Color32(91, 171, 65, 255);
            mad.dialogueSound = "event:/Lunacy/SOUNDS4/YNLRoar";
            mad.portrait = SnailSpriteHandler.Sprites[Pigments.Green][3];

            SpeakerEmote emotion = new SpeakerEmote();
            emotion.emotion = "Mouth";
            emotion.bundle = mad;

            LoadedAssetsHandler.GetSpeakerData("Snail_SpeakerData")._emotionBundles = [emotion];
        }


        public static string HeavenACH => "Aprils_Snail_Heaven_ACH";
        public static string OsmanACH => "Aprils_Snail_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Snail_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Snail_Osman_Unlock";
    }
}
