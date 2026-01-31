using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Qualia
    {
        public static void Add()
        {
            BasePassiveAbilitySO inanimate = ScriptableObject.Instantiate(Passives.Inanimate);
            inanimate.name = "Inanimate_Levelling";
            inanimate._characterDescription = "This party member cannot be manually moved, healed, or Ruptured.";

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
            qualia.AddPassives([inanimate, Passives.Withering]);

            ImmediatePerformEffectPassive depiction = ScriptableObject.CreateInstance<ImmediatePerformEffectPassive>();
            depiction._passiveName = "Depiction";
            depiction.passiveIcon = ResourceLoader.LoadSprite("DepictionPassive.png");
            depiction.m_PassiveID = IDs.Depiction;
            depiction._enemyDescription = "This enemy is only temporary";
            depiction._characterDescription = "This party member is only temporary.";
            depiction.doesPassiveTriggerInformationPanel = true;
            depiction.conditions = [];
            depiction.effects = [BasicEffects.SetStoreValue(TemporaryReplaceBoxer.AllowedFlee), 1, Slots.Self),
                Effects.GenerateEffect(ScriptableObject.CreateInstance<ImmediateFleeEffect>(), 1, Slots.Self)];
            depiction._triggerOn = [TimelineEndHandler.Before];
            depiction.AddToPassiveDatabase();
            depiction.AddPassiveToGlossary("Depiction", "This unit is only temporary.");

            Intents.CreateAndAddCustom_Basic_IntentToPool(IDs.Depiction, depiction.passiveIcon, Color.white);

            ExtraPassiveAbility_Wearable_SMS add_picture = ScriptableObject.CreateInstance<ExtraPassiveAbility_Wearable_SMS>();
            add_picture._extraPassiveAbility = depiction;

            TemporaryReplacementEffect replace_all = ScriptableObject.CreateInstance<TemporaryReplacementEffect>();
            replace_all._extraModifiers = [add_picture];
            replace_all.OnlyUseAbilities = false;

            TemporaryReplacementEffect replace_unused = ScriptableObject.CreateInstance<TemporaryReplacementEffect>();
            replace_unused._extraModifiers = [add_picture];
            replace_unused.OnlyUseAbilities = true;

            TargettingCanUseAbilities unused = ScriptableObject.CreateInstance<TargettingCanUseAbilities>();
            unused.getAllies = true;
            unused.getAllUnitSlots = false;
            unused.ignoreCastSlot = true;

            Ability image1 = new Ability("Reimagine the Self", "Qualia_Image_1_A");
            image1.Description = "Temporarily replace this character with a random level 1 party member.\nThis character will return at the end of the round.";
            image1.Cost = [Pigments.Purple];
            image1.AbilitySprite = ResourceLoader.LoadSprite("ability_reimagine.png");
            image1.Effects = [Effects.GenerateEffect(replace_all, 0, Slots.Self)];
            image1.AddIntentsToTarget(Slots.Self, [IDs.Depiction]);
            image1.AnimationTarget = Slots.Self;
            image1.Visuals = Visuals.Painting;

            Ability image2 = new Ability(image1.ability, "Qualia_Image_2_A", image1.Cost);
            image2.Name = "Reimagine the Life";
            image2.Description = "Temporarily replace this character with a random level 2 party member.\nThis character will return at the end of the round.";
            image2.Effects[0].entryVariable = 1;

            Ability image3 = new Ability(image2.ability, "Qualia_Image_3_A", image1.Cost);
            image3.Name = "Reimagine the World";
            image3.Description = "Temporarily replace this character with a random level 3 party member.\nThis character will return at the end of the round.";
            image3.Effects[0].entryVariable = 2;

            Ability image4 = new Ability(image3.ability, "Qualia_Image_4_A", image1.Cost);
            image4.Name = "Reimagine the Universe";
            image4.Description = "Temporarily replace this character with a random level 4 party member.\nThis character will return at the end of the round.";
            image4.Effects[0].entryVariable = 3;

            Ability box1 = new Ability("Trapped in a Box", "Qualia_Box_1_A");
            box1.Description = "Temporarily replace the left ally with a random level 1 party member.\nThey will return at the end of the round.";
            box1.Cost = [Pigments.Blue];
            box1.AbilitySprite = ResourceLoader.LoadSprite("ability_box.png");
            box1.Effects = [Effects.GenerateEffect(replace_all, 0, Targeting.Slot_AllyLeft)];
            box1.AddIntentsToTarget(Targeting.Slot_AllyLeft, [IDs.Depiction]);
            box1.AnimationTarget = Targeting.Slot_AllyLeft;
            box1.Visuals = Visuals.Painting;

            Ability box2 = new Ability(box1.ability, "Qualia_Box_2_A", box1.Cost);
            box2.Name = "Identity in a Box";
            box2.Description = "Temporarily replace the left ally with a random level 2 party member.\nThey will return at the end of the round.";
            box2.Effects[0].entryVariable = 1;

            Ability box3 = new Ability(box2.ability, "Qualia_Box_3_A", box1.Cost);
            box3.Name = "Life in a Box";
            box3.Description = "Temporarily replace the left ally with a random level 3 party member.\nThey will return at the end of the round.";
            box3.Effects[0].entryVariable = 2;

            Ability box4 = new Ability(box3.ability, "Qualia_Box_4_A", box1.Cost);
            box4.Name = "Reality in a Box";
            box4.Description = "Temporarily replace the left ally with a random level 4 party member.\nThey will return at the end of the round.";
            box4.Effects[0].entryVariable = 3;

            Ability change1 = new Ability("Changeling Adoption", "Qualia_Change_1_A");
            change1.Description = "Temporarily replace the all other allies that still have ability usage with a random level 1 party members.\nThey will return at the end of the round.";
            change1.Cost = [Pigments.Yellow, Pigments.Blue, Pigments.Red];
            change1.AbilitySprite = ResourceLoader.LoadSprite("ability_changeling.png");
            change1.Effects = [Effects.GenerateEffect(replace_unused, 0, Targeting.Unit_OtherAllies)];
            change1.AddIntentsToTarget(Targeting.Unit_OtherAllies, [IDs.Depiction]);
            change1.AnimationTarget = unused;
            change1.Visuals = Visuals.Painting;

            Ability change2 = new Ability(change1.ability, "Qualia_Change_2_A", change1.Cost);
            change2.Name = "Changeling Enrollment";
            change2.Description = "Temporarily replace the all other allies that still have ability usage with a random level 2 party members.\nThey will return at the end of the round.";
            change2.Effects[0].entryVariable = 1;

            Ability change3 = new Ability(change2.ability, "Qualia_Change_3_A", change1.Cost);
            change3.Name = "Changeling Occupation";
            change3.Description = "Temporarily replace the all other allies that still have ability usage with a random level 3 party members.\nThey will return at the end of the round.";
            change3.Effects[0].entryVariable = 2;

            Ability change4 = new Ability(change3.ability, "Qualia_Change_4_A", change1.Cost);
            change4.Name = "Changeling Invasion";
            change4.Description = "Temporarily replace the all other allies that still have ability usage with a random level 4 party members.\nThey will return at the end of the round.";
            change4.Effects[0].entryVariable = 3;

            qualia.AddLevelData(20, [image1, change1, box1]);
            qualia.AddLevelData(21, [image2, change2, box2]);
            qualia.AddLevelData(22, [image3, change3, box3]);
            qualia.AddLevelData(23, [image4, change4, box4]);
            qualia.AddCharacter(true);
            SlidingHandler.AddCharacter(qualia.character);
        }


        public static string HeavenACH => "Aprils_Qualia_Heaven_ACH";
        public static string OsmanACH => "Aprils_Qualia_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Qualia_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Qualia_Osman_Unlock";
    }
}
