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
            Character alpha = new Character("A", "Alpha_CH");
            alpha.HealthColor = Pigments.Purple;
            alpha.AddUnitType("FemaleID");
            alpha.UsesBasicAbility = false;
            //slap
            alpha.UsesAllAbilities = true;
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
            alpha.MenuCharacterIgnoreRandom = true;
            alpha.SetMenuCharacterAsFullDPS();
            alpha.AddPassive(Passives.Delicate);

            Ability a1 = new Ability("Ability A 1", "A_A_1_A");
            a1.Description = "Perform one of the Opposing enemy's actions.";
            a1.AbilitySprite = ResourceLoader.LoadSprite("ability_a.png");
            a1.Cost = [Pigments.Yellow];
            a1.Effects = new EffectInfo[2];
            a1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<CasterPerformRandomTargetAbilityEffect>(), 1, Slots.Front);
            a1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RefreshAbilityUseEffect>(), 1, Slots.Self, Effects.ChanceCondition(0));
            a1.AddIntentsToTarget(Slots.Front, ["Misc_Hidden"]);

            Ability a2 = new Ability(a1.ability, "A_A_2_A", a1.Cost);
            a2.Name = "Ability A 2";
            a2.Description = "Perform one of the Opposing enemy's actions.\n10% chance to refresh this party member's ability usage.";
            a2.Effects[1].condition = Effects.ChanceCondition(10);
            a2.AddIntentsToTarget(Slots.Self, ["Misc"]);

            Ability a3 = new Ability(a2.ability, "A_A_3_A", a1.Cost);
            a3.Name = "Ability A 3";
            a3.Description = "Perform one of the Opposing enemy's actions.\n25% chance to refresh this party member's ability usage.";
            a3.Effects[1].condition = Effects.ChanceCondition(25);

            Ability a4 = new Ability(a3.ability, "A_A_4_A", a1.Cost);
            a4.Name = "Ability A 4";
            a4.Description = "Perform one of the Opposing enemy's actions.\n35% chance to refresh this party member's ability usage.";
            a4.Effects[1].condition = Effects.ChanceCondition(35);

            Ability b1 = new Ability("Ability B 1", "A_B_1_A");
            b1.Description = "Deal 6 damage to the Opposing enemy and reroll one of their actions.";
            b1.AbilitySprite = ResourceLoader.LoadSprite("ability_b.png");
            b1.Cost = [Pigments.Yellow, Pigments.Yellow];
            b1.Effects = new EffectInfo[2];
            b1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 6, Slots.Front);
            b1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ReRollTargetTimelineAbilityEffect>(), 1, Slots.Front);
            b1.AddIntentsToTarget(Slots.Front, ["Damage_3_6", "Misc"]);
            b1.Visuals = LoadedAssetsHandler.GetCharacterAbility("Parry_1_A").visuals;
            b1.AnimationTarget = Slots.Front;

            Ability b2 = new Ability(b1.ability, "A_B_2_A", b1.Cost);
            b2.Name = "Ability B 2";
            b2.Description = "Deal 8 damage to the Opposing enemy and reroll one of their actions.";
            b2.Effects[0].entryVariable = 8;
            b2.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability b3 = new Ability(b2.ability, "A_B_3_A", b1.Cost);
            b3.Name = "Ability B 3";
            b3.Description = "Deal 10 damage to the Opposing enemy and reroll one of their actions.";
            b3.Effects[0].entryVariable = 10;

            Ability b4 = new Ability(b3.ability, "A_B_4_A", b1.Cost);
            b4.Name = "Ability B 4";
            b4.Description = "Deal 12 damage to the Opposing enemy and reroll one of their actions.";
            b4.Effects[0].entryVariable = 12;
            b4.EffectIntents[0].intents[0] = "Damage_11_15";

            EffectInfo gainHaste = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyHasteEffect>(), 1, Slots.Self);
            Ability c1 = new Ability("Ability C 1", "A_C_1_A");
            c1.Description = "Heal this party member 4 health.\nGain 1 Haste, 20% chance to gain another.";
            c1.AbilitySprite = ResourceLoader.LoadSprite("ability_c.png");
            c1.Cost = [Pigments.Purple, Pigments.Purple];
            c1.Effects = new EffectInfo[3];
            c1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), 4, Slots.Self);
            c1.Effects[1] = Effects.GenerateEffect(CasterRootActionEffect.Create(gainHaste.SelfArray()));
            c1.Effects[2] = Effects.GenerateEffect(c1.Effects[1].effect, 0, null, Effects.ChanceCondition(20));
            c1.AddIntentsToTarget(Slots.Self, ["Heal_1_4", Haste.Intent]);
            c1.Visuals = CustomVisuals.GetVisuals("Salt/Think");
            c1.AnimationTarget = Slots.Self;

            Ability c2 = new Ability(c1.ability, "A_C_2_A", c1.Cost);
            c2.Name = "Ability C 2";
            c2.Description = "Heal this party member 6 health.\nGain 1 Haste, 40% chance to gain another.";
            c2.Effects[0].entryVariable = 6;
            c2.Effects[2].condition = Effects.ChanceCondition(40);
            c2.EffectIntents[0].intents[0] = "Heal_5_10";

            Ability c3 = new Ability(c2.ability, "A_C_3_A", c1.Cost);
            c3.Name = "Ability C 3";
            c3.Description = "Heal this party member 8 health.\nGain 1 Haste, 55% chance to gain another.";
            c3.Effects[0].entryVariable = 8;
            c3.Effects[2].condition = Effects.ChanceCondition(55);

            Ability c4 = new Ability(c3.ability, "A_C_4_A", c1.Cost);
            c4.Name = "Ability C 4";
            c4.Description = "Heal this party member 10 health.\nGain 1 Haste, 70% chance to gain another.";
            c4.Effects[0].entryVariable = 10;
            c4.Effects[2].condition = Effects.ChanceCondition(70);

            alpha.AddLevelData(12, [a1, b1, c1]);
            alpha.AddLevelData(16, [a2, b2, c2]);
            alpha.AddLevelData(20, [a3, b3, c3]);
            alpha.AddLevelData(24, [a4, b4, c4]);
            alpha.AddCharacter(false, true);
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
            identitytheft.Description = "On taking any damage, transform into a random same level party member.";
            identitytheft.Icon = ResourceLoader.LoadSprite("item_identitytheft.png");
            identitytheft.EquippedModifiers = [];
            identitytheft.TriggerOn = TriggerCalls.OnDamaged;
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
