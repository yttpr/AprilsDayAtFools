using BrutalAPI;
using BrutalAPI.Items;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Alpha
    {
        public static void Add()
        {
            ApplyInvertedEffect inverted = ScriptableObject.CreateInstance<ApplyInvertedEffect>();

            PerformEffectPassiveAbility special = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            special._passiveName = "Special (3)";
            special.name = "Special_3_PA";
            special.passiveIcon = ResourceLoader.LoadSprite("SpecialPassive.png");
            special.m_PassiveID = "Special_PA";
            special._enemyDescription = "might not work";
            special._characterDescription = "At the start of combat, gain 3 Inverted.";
            special.doesPassiveTriggerInformationPanel = true;
            special.effects = Effects.GenerateEffect(inverted, 3, Slots.Self).SelfArray();
            special._triggerOn = new TriggerCalls[1] { TriggerCalls.OnFirstTurnStart };
            special.AddPassiveToGlossary("Special", "At the start of combat, gain a certain amount of Inverted.");
            special.AddToPassiveDatabase();

            Character alpha = new Character("A", "Alpha_CH");
            alpha.HealthColor = Pigments.Purple;
            alpha.AddUnitType("FemaleID");
            alpha.AddUnitType("FemaleLooking");
            alpha.UsesBasicAbility = true;
            //slap
            alpha.UsesAllAbilities = false;
            alpha.MovesOnOverworld = true;
            //animator
            alpha.FrontSprite = ResourceLoader.LoadSprite("AlphaFront.png");
            alpha.BackSprite = ResourceLoader.LoadSprite("AlphaBack.png");
            alpha.OverworldSprite = ResourceLoader.LoadSprite("AlphaWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            alpha.DamageSound = "event:/Lunacy/SOUNDS3/DarkHit";
            alpha.DeathSound = "event:/Lunacy/SOUNDS3/DarkDie";
            alpha.DialogueSound = "event:/Lunacy/SOUNDS3/DarkHit";
            alpha.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            alpha.AddFinalBossAchievementData("Heaven", HeavenACH);
            alpha.GenerateMenuCharacter(ResourceLoader.LoadSprite("AlphaMenu.png"), ResourceLoader.LoadSprite("AlphaLock.png"));
            alpha.MenuCharacterIsSecret = true;
            alpha.MenuCharacterIgnoreRandom = false;
            alpha.SetMenuCharacterAsFullSupport();
            alpha.AddPassive(special);

            Ability newline1 = new Ability("Newline Addition", "A_Newline_1_A");
            newline1.Description = "Force the Opposing enemy to prematurely perform their next action.\n40% chance to refresh this party member's ability usage.";
            newline1.AbilitySprite = ResourceLoader.LoadSprite("ability_newline.png");
            newline1.Cost = [Pigments.Yellow];
            newline1.Effects = new EffectInfo[2];
            newline1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<TargetForceFirstActionEffect>(), 1, Slots.Front);
            newline1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), 1, Slots.Self, Effects.ChanceCondition(40));
            newline1.AddIntentsToTarget(Slots.Front, ["Misc"]);
            newline1.AddIntentsToTarget(Slots.Self, ["Misc"]);

            Ability newline2 = new Ability(newline1.ability, "A_Newline_2_A", newline1.Cost);
            newline2.Name = "Newline Rotation";
            newline2.Description = "Force the Opposing enemy to prematurely perform their next action.\n50% chance to refresh this party member's ability usage.";
            newline2.Effects[1].condition = Effects.ChanceCondition(50);

            Ability newline3 = new Ability(newline2.ability, "A_Newline_3_A", newline1.Cost);
            newline3.Name = "Newline Translation";
            newline3.Description = "Force the Opposing enemy to prematurely perform their next action.\n60% chance to refresh this party member's ability usage.";
            newline3.Effects[1].condition = Effects.ChanceCondition(60);

            Ability newline4 = new Ability(newline3.ability, "A_Newline_4_A", newline1.Cost);
            newline4.Name = "Newline Transformation";
            newline4.Description = "Force the Opposing enemy to prematurely perform their next action.\n66% chance to refresh this party member's ability usage.";
            newline4.Effects[1].condition = Effects.ChanceCondition(66);

            Ability alphabet1 = new Ability("Array Alphabetical", "A_Alphabet_1_A");
            alphabet1.Description = "Heal the Left ally 4 health and inflict 1 Inverted on them.";
            alphabet1.AbilitySprite = ResourceLoader.LoadSprite("ability_alphabet.png");
            alphabet1.Cost = [Pigments.Yellow, Pigments.Blue, Pigments.Blue];
            alphabet1.Effects = new EffectInfo[3];
            alphabet1.Effects[0] = Effects.GenerateEffect(BasicEffects.Empty, 2, Targeting.Slot_AllyLeft);
            alphabet1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), 4, Targeting.Slot_AllyLeft);
            alphabet1.Effects[2] = Effects.GenerateEffect(inverted, 1, Targeting.Slot_AllyLeft);
            alphabet1.AddIntentsToTarget(Targeting.Slot_AllyLeft, ["Heal_1_4", Inverted.Intent]);
            alphabet1.Visuals = CustomVisuals.GetVisuals("Salt/Ads");
            alphabet1.AnimationTarget = Targeting.Slot_AllyLeft;

            Ability alphabet2 = new Ability(alphabet1.ability, "A_Alphabet_2_A", alphabet1.Cost);
            alphabet2.Name = "Assembly Alphabetical";
            alphabet2.Description = "Reduce all Negative Status Effects on the Left ally by 2 and heal them 5 health and inflict 1 Inverted on them.";
            alphabet2.Effects[0].effect = ScriptableObject.CreateInstance<ReduceAllNegativeStatusEffect>();
            alphabet2.Effects[1].entryVariable = 5;
            alphabet2.EffectIntents[0].intents = ["Misc", "Heal_5_10", Inverted.Intent];

            Ability alphabet3 = new Ability(alphabet2.ability, "A_Alphabet_3_A", [Pigments.Blue, Pigments.Blue]);
            alphabet3.Name = "Acquisition Alphabetical";
            alphabet3.Description = "Reduce all Negative Status Effects on the Left ally by 2 and heal them 7 health and inflict 1 Inverted on them.";
            alphabet3.Effects[1].entryVariable = 7;

            Ability alphabet4 = new Ability(alphabet3.ability, "A_Alphabet_4_A", alphabet3.Cost);
            alphabet4.Name = "Anatomy Alphabetical";
            alphabet4.Description = "Reduce all Negative Status Effects on the Left ally by 4 and heal them 8 health and inflict 1 Inverted on them.";
            alphabet4.Effects[0].entryVariable = 4;
            alphabet4.Effects[1].entryVariable = 8;

            IncreaseStatusEffectsEffect up_neg = ScriptableObject.CreateInstance<IncreaseStatusEffectsEffect>();
            up_neg.m_AffectStatusEffects = true;
            up_neg.m_AffectFieldEffects = false;
            up_neg._increasePositives = false;

            Ability reverse1 = new Ability("Reverse Disposition", "A_Reverse_1_A");
            reverse1.Description = "Inflict 1 Inverted on the Right ally.\nForce the First enemy on the timeline to prematurely perform their next action.";
            reverse1.AbilitySprite = ResourceLoader.LoadSprite("ability_reverse.png");
            reverse1.Cost = [Pigments.Yellow, Pigments.Blue];
            reverse1.Effects = new EffectInfo[3];
            reverse1.Effects[0] = Effects.GenerateEffect(inverted, 1, Targeting.Slot_AllyRight);
            reverse1.Effects[1] = Effects.GenerateEffect(BasicEffects.Empty, 2, ScriptableObject.CreateInstance<TargettingByEnemyFirstTimeline>());
            reverse1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ForceFirstTimelineActionEffect>(), 1);
            reverse1.AddIntentsToTarget(Targeting.Slot_AllyRight, [Inverted.Intent]);
            reverse1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Misc_Hidden"]);
            reverse1.AnimationTarget = Targeting.Slot_AllyRight;
            reverse1.Visuals = CustomVisuals.GetVisuals("Salt/Notif");

            Ability reverse2 = new Ability(reverse1.ability, "A_Reverse_2_A", reverse1.Cost);
            reverse2.Name = "Reverse Constitution";
            reverse2.Description = "Inflict 1 Inverted on the Right ally.\nForce the First enemy on the timeline to prematurely perform their next action and increase their Negative Status Effects by 2.";
            reverse2.Effects[1].effect = up_neg;

            Ability reverse3 = new Ability(reverse2.ability, "A_Reverse_3_A", [Pigments.Grey, Pigments.Blue]);
            reverse3.Name = "Reverse Composition";

            Ability reverse4 = new Ability(reverse3.ability, "A_Reverse_4_A", reverse3.Cost);
            reverse4.Name = "Reverse Conformation";
            reverse4.Description = "Inflict 1 Inverted on the Right ally.\nForce the First enemy on the timeline to prematurely perform their next action and increase their Negative Status Effects by 3.";
            reverse4.Effects[1].entryVariable = 3;

            alpha.AddLevelData(12, [newline1, alphabet1, reverse1]);
            alpha.AddLevelData(16, [newline2, alphabet2, reverse2]);
            alpha.AddLevelData(20, [newline3, alphabet3, reverse3]);
            alpha.AddLevelData(24, [newline4, alphabet4, reverse4]);
            alpha.AddCharacter(Class1.UnlockedByDefault);
        }
        public static void Items()
        {
            Basic_Item defacer = new Basic_Item("Aprils_Defacer_TW");
            defacer.Name = "Defacer";
            defacer.Flavour = "\"Show off your true self\"";
            defacer.Description = "This party member has Unstable, Transfusion, Infestation (1), Unpredictable, Constricting, Ordinary, Slippery, Skittish, Focus, Leaky (1), Eternal, Backlash, Flowers, Delicate, Solitude, Withering, Construct, Scary, Ambush (2), Inferno (1), and Untethered Essence as passives.";
            defacer.Icon = ResourceLoader.LoadSprite("item_defacer.png");
            defacer.EquippedModifiers = [
                Passives.Unstable.GenerateSMS(), Passives.Transfusion.GenerateSMS(), Passives.Infestation1.GenerateSMS(),
                Passives.GetCustomPassive("Unpredictable_PA").GenerateSMS(), Passives.Constricting.GenerateSMS(), 
                Passives.GetCustomPassive("Ordinary_PA").GenerateSMS(), Passives.Slippery.GenerateSMS(), Passives.Skittish.GenerateSMS(), 
                Passives.Focus.GenerateSMS(), Passives.Leaky1.GenerateSMS(), Passives.GetCustomPassive("Eternal_PA").GenerateSMS(),
                Passives.GetCustomPassive("Backlash_PA").GenerateSMS(), Passives.GetCustomPassive("Flowers_PA").GenerateSMS(),
                Passives.Delicate.GenerateSMS(), Passives.GetCustomPassive("Solitude_PA").GenerateSMS(), Passives.Withering.GenerateSMS(),
                LoadedAssetsHandler.GetCharacter("Xet_CH").passiveAbilities[0].GenerateSMS(),
                Passives.GetCustomPassive("Scary_PA").GenerateSMS(), Passives.GetCustomPassive("Ambush_2_PA").GenerateSMS(),
                Passives.Inferno.GenerateSMS(), Passives.EssenceUntethered.GenerateSMS()
                ];
            defacer.TriggerOn = TriggerCalls.Count;
            defacer.DoesPopUpInfo = false;
            defacer.Conditions = [];
            defacer.DoesActionOnTriggerAttached = false;
            defacer.ConsumeOnTrigger = TriggerCalls.Count;
            defacer.ConsumeOnUse = false;
            defacer.ConsumeConditions = [];
            defacer.ShopPrice = 8;
            defacer.IsShopItem = false;
            defacer.StartsLocked = true;
            defacer.OnUnlockUsesTHE = true;
            defacer.UsesSpecialUnlockText = false;
            defacer.SpecialUnlockID = UILocID.None;
            defacer.item._ItemTypeIDs = ["Face", "Knife"];
            defacer.Item.AddItem("locked_defacer.png", OsmanACH);

            PerformEffect_Item identitytheft = new PerformEffect_Item("Aprils_IdentityTheft_TW", [Effects.GenerateEffect(CasterRootActionEffect.Create(Effects.GenerateEffect(ScriptableObject.CreateInstance<TransformSameLevelCharacterEffect>(), 1, Slots.Self).SelfArray()))]);
            identitytheft.Name = "Identity Theft";
            identitytheft.Flavour = "\"It's easier to be someone else.\"";
            identitytheft.Description = "On moving, transform into a random same level party member.";
            identitytheft.Icon = ResourceLoader.LoadSprite("item_identitytheft.png");
            identitytheft.EquippedModifiers = [];
            identitytheft.TriggerOn = TriggerCalls.OnMoved;
            identitytheft.DoesPopUpInfo = true;
            identitytheft.Conditions = [];
            identitytheft.DoesActionOnTriggerAttached = false;
            identitytheft.ConsumeOnTrigger = TriggerCalls.Count;
            identitytheft.ConsumeOnUse = false;
            identitytheft.ConsumeConditions = [];
            identitytheft.ShopPrice = 5;
            identitytheft.IsShopItem = false;
            identitytheft.StartsLocked = true;
            identitytheft.OnUnlockUsesTHE = false;
            identitytheft.UsesSpecialUnlockText = false;
            identitytheft.SpecialUnlockID = UILocID.None;
            identitytheft.Item.AddItem("locked_identitytheft.png", HeavenACH);
        }
        public static void Unlocks()
        {
            Unlocking.GenerateAchievements("Alpha", "Identity Theft", "Defacer", HeavenACH, OsmanACH);
            Unlocking.SetUpUnlocks("Alpha_CH", "Aprils_IdentityTheft_TW", "Aprils_Defacer_TW", HeavenACH, OsmanACH, HeavenUnlock, OsmanUnlock);
        }

        public static string HeavenACH => "Aprils_Alpha_Heaven_ACH";
        public static string OsmanACH => "Aprils_Alpha_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Alpha_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Alpha_Osman_Unlock";
    }
}
