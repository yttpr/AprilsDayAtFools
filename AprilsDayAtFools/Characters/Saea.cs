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
            PerformEffectPassiveAbility permanent = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            permanent._passiveName = "Permanent";
            permanent.passiveIcon = PermanentPassiveHandler.Icon;
            permanent.m_PassiveID = IDs.Permanent;
            permanent.name = IDs.Permanent;
            permanent._enemyDescription = "This enemy's maximum health cannot be changed.";
            permanent._characterDescription = "This party member's maximum health cannot be changed.";
            permanent.doesPassiveTriggerInformationPanel = true;
            permanent.effects = [];
            permanent._triggerOn = [];
            permanent.AddPassiveToGlossary("Permanent", "This unit's maximum health cannot be changed.");
            permanent.AddToPassiveDatabase();

            Character saea = new Character("Saea", "Saea_CH");
            saea.HealthColor = Pigments.Blue;
            saea.AddUnitType("FemaleID");
            saea.AddUnitType("FemaleLooking");
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
            saea.SetMenuCharacterAsFullSupport();
            saea.AddPassive(permanent);

            DamageEffect damage = ScriptableObject.CreateInstance<DamageEffect>();
            HealEffect lifesteal = ScriptableObject.CreateInstance<HealEffect>();
            lifesteal.usePreviousExitValue = true;
            ApplyKarmaEffect karma = ScriptableObject.CreateInstance<ApplyKarmaEffect>();

            Ability claim1 = new Ability("Action of Recovery", "Saea_Claim_1_A");
            claim1.Description = "Deal 4 damage to the Opposing enemy and heal this party member for the amount of damage dealt.";
            claim1.AbilitySprite = ResourceLoader.LoadSprite("ability_claim.png");
            claim1.Cost = [Pigments.Grey, Pigments.Grey, Pigments.Red];
            claim1.Effects = new EffectInfo[2];
            claim1.Effects[0] = Effects.GenerateEffect(damage, 4, Slots.Front);
            claim1.Effects[1] = Effects.GenerateEffect(lifesteal, 1, Slots.Self, BasicEffects.DidThat(true));
            claim1.AddIntentsToTarget(Slots.Front, ["Damage_3_6"]);
            claim1.AddIntentsToTarget(Slots.Self, ["Heal_1_4"]);
            claim1.Visuals = Visuals.Womb;
            claim1.AnimationTarget = Slots.Front;

            Ability claim2 = new Ability(claim1.ability, "Saea_Claim_2_A", claim1.Cost);
            claim2.Name = "Action of Reclamation";
            claim2.Description = "Deal 6 damage to the Opposing enemy and heal this party member for the amount of damage dealt.";
            claim2.Effects[0].entryVariable = 6;
            claim2.EffectIntents[1].intents[0] = "Heal_5_10";

            Ability claim3 = new Ability(claim2.ability, "Saea_Claim_3_A", claim1.Cost);
            claim3.Name = "Action of Repossession";
            claim3.Description = "Deal 7 damage to the Opposing enemy and heal this party member for the amount of damage dealt.";
            claim3.Effects[0].entryVariable = 7;
            claim3.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability claim4 = new Ability(claim3.ability, "Saea_Claim_4_A", claim1.Cost);
            claim4.Name = "Action of Reappropriation";
            claim4.Description = "Deal 8 damage to the Opposing enemy and heal this party member for the amount of damage dealt.";
            claim4.Effects[0].entryVariable = 8;

            AddExtraAbilityIfNotHaveEffect act1 = AddExtraAbilityIfNotHaveEffect.Create(claim1.GenerateCharacterAbility(true));
            AddExtraAbilityIfNotHaveEffect act2 = AddExtraAbilityIfNotHaveEffect.Create(claim2.GenerateCharacterAbility(true));
            AddExtraAbilityIfNotHaveEffect act3 = AddExtraAbilityIfNotHaveEffect.Create(claim3.GenerateCharacterAbility(true));
            AddExtraAbilityIfNotHaveEffect act4 = AddExtraAbilityIfNotHaveEffect.Create(claim4.GenerateCharacterAbility(true));

            Ability onset1 = new Ability("Onset of Shadows", "Saea_Onset_1_A");
            onset1.Description = "Heal this and the Right allies 6 health then inflict 4 Karma on them, increasing their maximum health if necessary.\nKarma inflicted will not exceed 11.";
            onset1.AbilitySprite = ResourceLoader.LoadSprite("ability_onset.png");
            onset1.Cost = [Pigments.Blue, Pigments.Blue, Pigments.Blue];
            onset1.Effects = new EffectInfo[3];
            onset1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<MaxHealthlessHealEffect>(), 6, Targeting.Slot_SelfAndRight);
            onset1.Effects[1] = Effects.GenerateEffect(BasicEffects.Empty, 11);
            onset1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyKarmaCappedToExitEffect>(), 4, Targeting.Slot_SelfAndRight);
            onset1.AddIntentsToTarget(Targeting.Slot_SelfAndRight, ["Heal_5_10", IntentType_GameIDs.Other_MaxHealth.ToString(), Karma.Intent]);
            onset1.Visuals = Visuals.UglyOnTheInside;
            onset1.AnimationTarget = Targeting.Slot_SelfAndRight;

            Ability onset2 = new Ability(onset1.ability, "Saea_Onset_2_A", onset1.Cost);
            onset2.Name = "Onset of Darkness";
            onset2.Description = "Heal this, the Right, and Far Right allies 7 health then inflict 5 Karma on them, increasing their maximum health if necessary.\nKarma inflicted will not exceed 13.";
            onset2.Effects[0].entryVariable = 7;
            onset2.Effects[1].entryVariable = 13;
            onset2.Effects[2].entryVariable = 5;
            onset2.AnimationTarget = Slots.SlotTarget([0, 1, 2], true);
            onset2.Effects[0].targets = onset2.ability.animationTarget;
            onset2.Effects[2].targets = onset2.ability.animationTarget;
            onset2.EffectIntents[0].targets = onset2.ability.animationTarget;

            Ability onset3 = new Ability(onset2.ability, "Saea_Onset_3_A", onset1.Cost);
            onset3.Name = "Onset of Death";
            onset3.Description = "Heal this, the Right, the Far Right, and the Far Far Right allies 8 health then inflict 6 Karma on them, increasing their maximum health if necessary.\nKarma inflicted will not exceed 14.";
            onset3.Effects[0].entryVariable = 8;
            onset3.Effects[1].entryVariable = 14;
            onset3.Effects[2].entryVariable = 6;
            onset3.AnimationTarget = Slots.SlotTarget([0, 1, 2, 3], true);
            onset3.Effects[0].targets = onset3.ability.animationTarget;
            onset3.Effects[2].targets = onset3.ability.animationTarget;
            onset3.EffectIntents[0].targets = onset3.ability.animationTarget;

            Ability onset4 = new Ability(onset3.ability, "Saea_Onset_4_A", onset1.Cost);
            onset4.Name = "Onset of Hell";
            onset4.Description = "Heal this and All allies to the Rights 8 health then inflict 6 Karma on them, increasing their maximum health if necessary.\nKarma inflicted will not exceed 15.";
            onset4.AnimationTarget = Slots.SlotTarget([0, 1, 2, 3, 4], true);
            onset4.Effects[1].entryVariable = 15;
            onset4.Effects[0].targets = onset4.ability.animationTarget;
            onset4.Effects[2].targets = onset4.ability.animationTarget;
            onset4.EffectIntents[0].targets = onset4.ability.animationTarget;

            ReduceKarmaEffect reduce = ScriptableObject.CreateInstance<ReduceKarmaEffect>();
            reduce._randomBetweenPrevious = true;

            Ability ori1 = new Ability("Placated Origin", "Saea_Ori_1_A");
            ori1.Description = "Attempt to resurrect an ally in the Left position at 3 health, inflicting 5 Karma on them if succesful.\nIf no ally was resurrected, reduce Karma on All party members by 2.";
            ori1.AbilitySprite = ResourceLoader.LoadSprite("ability_origin.png");
            ori1.Cost = [Pigments.Purple];
            ori1.Effects = new EffectInfo[4];
            ori1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ResurrectEffect>(), 3, Targeting.Slot_AllyLeft);
            ori1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyKarmaEffect>(), 5, Targeting.Slot_AllyLeft, BasicEffects.DidThat(true));
            ori1.Effects[2] = Effects.GenerateEffect(BasicEffects.Empty, 2);
            ori1.Effects[3] = Effects.GenerateEffect(reduce, 2, Targeting.Unit_AllAllies, BasicEffects.DidThat(false, 3));
            ori1.AddIntentsToTarget(Targeting.Slot_AllyLeft, ["Other_Resurrect", Karma.Intent]);
            ori1.Visuals = CustomVisuals.GetVisuals("Salt/Insta/Shatter");
            ori1.AnimationTarget = Targeting.Slot_AllyLeft;

            Ability ori2 = new Ability(ori1.ability, "Saea_Ori_2_A", ori1.Cost);
            ori2.Name = "Cordial Origin";
            ori2.Description = "Attempt to resurrect an ally in the Left position at 5 health, inflicting 7 Karma on them if succesful.\nIf no ally was resurrected, reduce Karma on All party members by 2-3.";
            ori2.Effects[0].entryVariable = 5;
            ori2.Effects[1].entryVariable = 5;
            ori2.Effects[3].entryVariable = 3;

            Ability ori3 = new Ability(ori2.ability, "Saea_Ori_3_A", ori1.Cost);
            ori3.Name = "Amiable Origin";
            ori3.Description = "Attempt to resurrect an ally in the Left position at 8 health, inflicting 8 Karma on them if succesful.\nIf no ally was resurrected, reduce Karma on All party members by 2-3.";
            ori3.Effects[0].entryVariable = 8;
            ori3.Effects[1].entryVariable = 8;

            Ability ori4 = new Ability(ori3.ability, "Saea_Ori_4_A", ori1.Cost);
            ori4.Name = "Hospitable Origin";
            ori4.Description = "Attempt to resurrect an ally in the Left position at 10 health, inflicting 10 Karma on them if succesful.\nIf no ally was resurrected, reduce Karma on All party members by 3.";
            ori4.Effects[0].entryVariable = 10;
            ori4.Effects[1].entryVariable = 10;
            ori4.Effects[2].entryVariable = 3;

            EffectInfo animself = Effects.GenerateEffect(BasicEffects.GetVisuals("Salt/Keyhole", false, Slots.Self));
            EffectInfo animleft = Effects.GenerateEffect(BasicEffects.GetVisuals("Salt/Keyhole", false, Targeting.Slot_AllyLeft));
            ExistsLeftAllyCondition left = ScriptableObject.CreateInstance<ExistsLeftAllyCondition>();
            PreviousEffectCondition didnt = BasicEffects.DidThat(false);
            Intents.CreateAndAddCustom_Basic_IntentToPool("Claim_A", claim1.ability.abilitySprite, Color.white);
            ApplyShieldSlotEffect shield = ScriptableObject.CreateInstance<ApplyShieldSlotEffect>();
            BaseCombatTargettingSO sh_left = Targeting.Slot_SelfAndLeft;
            DoubleTargetting sh_self = ScriptableObject.CreateInstance<DoubleTargetting>();
            sh_self.firstTargetting = Slots.Self;
            sh_self.secondTargetting = Slots.Self;

            Ability visions1 = new Ability("Vile Visions", "Saea_Visions_1_A");
            visions1.Description = "Give the Left ally \"Action of Recovery\" as an extra ability.\nIf they already have \"Action of Recovery,\" apply 4 Shield to them and this party member.\nIf there is no Left ally, this ability targets this party member.";
            visions1.AbilitySprite = ResourceLoader.LoadSprite("ability_visions.png");
            visions1.Cost = [Pigments.Red, Pigments.Blue];
            visions1.Effects = new EffectInfo[2];
            visions1.Effects[0] = Effects.GenerateEffect(ImmediateActionEffect.Create([
                animleft,
                Effects.GenerateEffect(act1, 1, Targeting.Slot_AllyLeft),
                Effects.GenerateEffect(shield, 4, sh_left, BasicEffects.DidThat(false)),
                ]), 1, Slots.Self, left);
            visions1.Effects[1] = Effects.GenerateEffect(ImmediateActionEffect.Create([
                animself,
                Effects.GenerateEffect(act1, 1, Slots.Self),
                Effects.GenerateEffect(shield, 4, sh_self, BasicEffects.DidThat(false)),
                ]), 1, Slots.Self, didnt);
            visions1.AddIntentsToTarget(Targeting.Slot_AllyLeft, ["Claim_A", "Field_Shield"]);
            visions1.AddIntentsToTarget(Slots.Self, ["Field_Shield", "Misc_Hidden"]);

            Ability visions2 = new Ability(visions1.ability, "Saea_Visions_2_A", visions1.Cost);
            visions2.Name = "Torturous Visions";
            visions2.Description = "Give the Left ally \"Action of Reclamation\" as an extra ability.\nIf they already have \"Action of Reclamation,\" apply 5 Shield to them and this party member.\nIf there is no Left ally, this ability targets this party member.";
            visions2.Effects[0] = Effects.GenerateEffect(ImmediateActionEffect.Create([
                animleft,
                Effects.GenerateEffect(act2, 1, Targeting.Slot_AllyLeft),
                Effects.GenerateEffect(shield, 5, sh_left, BasicEffects.DidThat(false)),
                ]), 1, Slots.Self, left);
            visions2.Effects[1] = Effects.GenerateEffect(ImmediateActionEffect.Create([
                animself,
                Effects.GenerateEffect(act2, 1, Slots.Self),
                Effects.GenerateEffect(shield, 5, sh_self, BasicEffects.DidThat(false)),
                ]), 1, Slots.Self, didnt);

            Ability visions3 = new Ability(visions2.ability, "Saea_Visions_3_A", visions1.Cost);
            visions3.Name = "Apocalyptic Visions";
            visions3.Description = "Give the Left ally \"Action of Repossession\" as an extra ability.\nIf they already have \"Action of Repossession,\" apply 6 Shield to them and this party member.\nIf there is no Left ally, this ability targets this party member.";
            visions3.Effects[0] = Effects.GenerateEffect(ImmediateActionEffect.Create([
                animleft,
                Effects.GenerateEffect(act3, 1, Targeting.Slot_AllyLeft),
                Effects.GenerateEffect(shield, 6, sh_left, BasicEffects.DidThat(false)),
                ]), 1, Slots.Self, left);
            visions3.Effects[1] = Effects.GenerateEffect(ImmediateActionEffect.Create([
                animself,
                Effects.GenerateEffect(act3, 1, Slots.Self),
                Effects.GenerateEffect(shield, 6, sh_self, BasicEffects.DidThat(false)),
                ]), 1, Slots.Self, didnt);

            Ability visions4 = new Ability(visions3.ability, "Saea_Visions_4_A", visions1.Cost);
            visions4.Name = "Cataclysmic Visions";
            visions4.Description = "Give the Left ally \"Action of Reappropriation\" as an extra ability.\nIf they already have \"Action of Reappropriation,\" apply 8 Shield to them and this party member.\nIf there is no Left ally, this ability targets this party member.";
            visions4.Effects[0] = Effects.GenerateEffect(ImmediateActionEffect.Create([
                animleft,
                Effects.GenerateEffect(act4, 1, Targeting.Slot_AllyLeft),
                Effects.GenerateEffect(shield, 8, sh_left, BasicEffects.DidThat(false)),
                ]), 1, Slots.Self, left);
            visions4.Effects[1] = Effects.GenerateEffect(ImmediateActionEffect.Create([
                animself,
                Effects.GenerateEffect(act4, 1, Slots.Self),
                Effects.GenerateEffect(shield, 8, sh_self, BasicEffects.DidThat(false)),
                ]), 1, Slots.Self, didnt);

            saea.AddLevelData(12, [visions1, ori1, onset1]);
            saea.AddLevelData(14, [visions2, ori2, onset2]);
            saea.AddLevelData(15, [visions3, ori3, onset3]);
            saea.AddLevelData(16, [visions4, ori4, onset4]);
            saea.AddCharacter(false);
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
