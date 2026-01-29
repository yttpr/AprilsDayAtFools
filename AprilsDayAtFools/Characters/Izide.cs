using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Izide
    {
        public static void Add()
        {
            ExtraCCSprites_BasicSO izideExtra = ScriptableObject.CreateInstance<ExtraCCSprites_BasicSO>();
            izideExtra._DefaultID = IDs.IzideDefault;
            izideExtra._frontSprite = ResourceLoader.LoadSprite("IzideSpeak.png");
            izideExtra._SpecialID = IDs.Izide;
            izideExtra._backSprite = ResourceLoader.LoadSprite("IzideBack.png");

            SetCasterExtraSpritesRandomUpToEntryEffect izideSprites = ScriptableObject.CreateInstance<SetCasterExtraSpritesRandomUpToEntryEffect>();
            izideSprites._spriteType = izideExtra._SpecialID;
            SetCasterExtraSpritesRandomUpToEntryEffect izideDefault = ScriptableObject.CreateInstance<SetCasterExtraSpritesRandomUpToEntryEffect>();
            izideDefault._spriteType = izideExtra._DefaultID;

            Character izide = new Character("Izide", "Izide_CH");
            izide.HealthColor = Pigments.Purple;
            izide.AddUnitType("FemaleID");
            izide.AddUnitType("FemaleLooking");
            izide.AddUnitType("Sandwich_Gambling");
            izide.UsesBasicAbility = true;
            //slap
            izide.UsesAllAbilities = false;
            izide.MovesOnOverworld = true;
            //custom animator
            izide.FrontSprite = ResourceLoader.LoadSprite("IzideFront.png");
            izide.BackSprite = ResourceLoader.LoadSprite("IzideBack.png");
            izide.OverworldSprite = ResourceLoader.LoadSprite("IzideWorld.png", new Vector2(0.5f, 0f));
            izide.ExtraSprites = izideExtra;
            izide.DamageSound = "event:/Lunacy/SOUNDS4/EnigmaHurt";
            izide.DeathSound = "event:/Lunacy/SOUNDS4/EnigmaRoar";
            izide.DialogueSound = "event:/Lunacy/SOUNDS4/EnigmaDie";
            //snail.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            //snail.AddFinalBossAchievementData("Heaven", HeavenACH);
            izide.GenerateMenuCharacter(ResourceLoader.LoadSprite("IzideMenu.png"), ResourceLoader.LoadSprite("IzideLock.png"));
            izide.MenuCharacterIsSecret = false;
            izide.MenuCharacterIgnoreRandom = false;
            izide.SetMenuCharacterAsFullDPS();
            izide.AddPassive(Passives.Fleeting6);

            Intents.CreateAndAddCustom_Damage_IntentToPool("ADAF_Damage_Delay", ResourceLoader.LoadSprite("DelayedAttackIcon.png"), (Intents.GetInGame_IntentInfo(IntentType_GameIDs.Damage_11_15) as IntentInfoDamage).GetColor(true),
                ResourceLoader.LoadSprite("DelayedAttackIcon.png"), (Intents.GetInGame_IntentInfo(IntentType_GameIDs.Damage_11_15) as IntentInfoDamage).GetColor(false));

            MulticolorStoreData_ModIntSO.CreateAndAdd(IDs.Ruin, "Of Ruin {0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Negative), "Of Ruin +{0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Positive), false);
            MulticolorStoreData_ModIntSO.CreateAndAdd(IDs.Behind, "From Behind {0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Negative), "From Behind +{0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Positive), false);

            CasterChangeMultiStoredValueEffect increase = ScriptableObject.CreateInstance<CasterChangeMultiStoredValueEffect>();
            increase._increase = true;
            increase._usePreviousExitValue = true;
            increase._minimumValue = -999;
            increase.ValueNames = [IDs.Ruin, IDs.Behind];
            CasterStoredValueChangeEffect ruin_stat = ScriptableObject.CreateInstance<CasterStoredValueChangeEffect>();
            ruin_stat._increase = false;
            ruin_stat._minimumValue = -999;
            ruin_stat.m_unitStoredDataID = IDs.Ruin;
            CasterStoredValueChangeEffect behind_stat = ScriptableObject.CreateInstance<CasterStoredValueChangeEffect>();
            behind_stat._increase = false;
            behind_stat._minimumValue = -999;
            behind_stat.m_unitStoredDataID = IDs.Behind;
            CasterStoredValueChangeEffect fleeting = ScriptableObject.CreateInstance<CasterStoredValueChangeEffect>();
            fleeting._increase = true;
            fleeting.m_unitStoredDataID = UnitStoredValueNames_GameIDs.FleetingPA.ToString();

            MaskedDelayedAttackStoredValueEffect ruin_dmg = ScriptableObject.CreateInstance<MaskedDelayedAttackStoredValueEffect>();
            ruin_dmg.ValueName = IDs.Ruin;
            MaskedDelayedAttackStoredValueEffect behind_dmg = ScriptableObject.CreateInstance<MaskedDelayedAttackStoredValueEffect>();
            behind_dmg.ValueName = IDs.Behind;

            TargettingByAlreadyAttacked behind_target = TargettingByAlreadyAttacked.Create(Targetting.Everything(false));

            Ability med1 = new Ability("Meditations on Senescence", "Izide_Med_1_A");
            med1.Description = "Deal 2 damage to the Opposing enemy and increase the damage of \"Of Ruin\" and \"From Behind\" by the amount of damage dealt.\nFlee 1 turn sooner.";
            med1.AbilitySprite = ResourceLoader.LoadSprite("ability_meditations.png");
            med1.Cost = [Pigments.Purple, Pigments.Red];
            med1.Effects = new EffectInfo[3];
            med1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 2, Slots.Front);
            med1.Effects[1] = Effects.GenerateEffect(increase, 1, Slots.Self, BasicEffects.DidThat(true));
            med1.Effects[2] = Effects.GenerateEffect(izideDefault, 1, Slots.Self);
            med1.AddIntentsToTarget(Slots.Front, ["Damage_1_2"]);
            med1.AddIntentsToTarget(Slots.Self, ["Misc"]);
            med1.AnimationTarget = Slots.Front;
            med1.Visuals = Visuals.RingABell;

            Ability med2 = new Ability(med1.ability, "Izide_Med_2_A", med1.Cost);
            med2.Name = "Meditations on Passage";
            med2.Description = "Deal 3 damage to the Opposing enemy and increase the damage of \"Of Ruin\" and \"From Behind\" by the amount of damage dealt.\nFlee 1 turn sooner.";
            med2.Effects[0].entryVariable = 3;
            med2.EffectIntents[0].intents[0] = "Damage_3_6";

            Ability med3 = new Ability(med2.ability, "Izide_Med_3_A", med1.Cost);
            med3.Name = "Meditations on Afterlife";
            med3.Description = "Deal 4 damage to the Opposing enemy and increase the damage of \"Of Ruin\" and \"From Behind\" by the amount of damage dealt.\nFlee 1 turn sooner.";
            med3.Effects[0].entryVariable = 4;

            Ability med4 = new Ability(med3.ability, "Izide_Med_4_A", med1.Cost);
            med4.Name = "Meditations on Eternity";
            med4.Description = "Deal 5 damage to the Opposing enemy and increase the damage of \"Of Ruin\" and \"From Behind\" by the amount of damage dealt.\nFlee 1 turn sooner.";
            med4.Effects[0].entryVariable = 5;

            Ability ruin1 = new Ability("Follower of Ruin", "Izide_Ruin_1_A");
            ruin1.Description = "At the start of the next turn, deal 12 damage to the current Opposing enemy position.\nDecrease this ability's damage by 3.";
            ruin1.AbilitySprite = ResourceLoader.LoadSprite("ability_ruin.png");
            ruin1.Cost = [Pigments.Red, Pigments.Red, Pigments.Red];
            ruin1.Effects = new EffectInfo[3];
            ruin1.Effects[0] = Effects.GenerateEffect(ruin_dmg, 12, Slots.Front);
            ruin1.Effects[1] = Effects.GenerateEffect(ruin_stat, 3, Slots.Self);
            ruin1.Effects[2] = Effects.GenerateEffect(izideDefault, 1, Slots.Self);
            ruin1.AddIntentsToTarget(Slots.Front, ["ADAF_Damage_Delay", "Damage_11_15"]);
            ruin1.AddIntentsToTarget(Slots.Self, ["Misc"]);
            ruin1.UnitStoreData = UnitStoreData.GetCustom_UnitStoreData(IDs.Ruin);
            ruin1.AnimationTarget = Slots.Front;
            ruin1.Visuals = Visuals.Bell;

            Ability ruin2 = new Ability(ruin1.ability, "Izide_Ruin_2_A", ruin1.Cost);
            ruin2.Name = "Emissary of Ruin";
            ruin2.Description = "At the start of the next turn, deal 16 damage to the current Opposing enemy position.\nDecrease this ability's damage by 4.";
            ruin2.Effects[0].entryVariable = 16;
            ruin2.Effects[1].entryVariable = 4;
            ruin2.EffectIntents[0].intents[1] = "Damage_16_20";

            Ability ruin3 = new Ability(ruin2.ability, "Izide_Ruin_3_A", ruin1.Cost);
            ruin3.Name = "Servant of Ruin";
            ruin3.Description = "At the start of the next turn, deal 20 damage to the current Opposing enemy position.\nDecrease this ability's damage by 5.";
            ruin3.Effects[0].entryVariable = 20;
            ruin3.Effects[1].entryVariable = 5;

            Ability ruin4 = new Ability(ruin3.ability, "Izide_Ruin_4_A", ruin1.Cost);
            ruin4.Name = "Handmaid of Ruin";
            ruin4.Description = "At the start of the next turn, deal 24 damage to the current Opposing enemy position.\nDecrease this ability's damage by 6.";
            ruin4.Effects[0].entryVariable = 24;
            ruin4.Effects[1].entryVariable = 6;
            ruin4.EffectIntents[0].intents[1] = "Damage_21";

            Ability behind1 = new Ability("From Behind Time", "Izide_Behind_1_A");
            behind1.Description = "At the start of the next turn, deal 8 damage to the current positions of all enemies this party member has damaged before.\nDecrease this ability's damage by 3.";
            behind1.AbilitySprite = ResourceLoader.LoadSprite("ability_behind.png");
            behind1.Cost = [Pigments.Yellow, Pigments.Red, Pigments.Red];
            behind1.Effects = new EffectInfo[4];
            behind1.Effects[0] = Effects.GenerateEffect(behind_dmg, 8, behind_target);
            behind1.Effects[1] = Effects.GenerateEffect(behind_stat, 3, Slots.Self);
            behind1.Effects[2] = Effects.GenerateEffect(izideDefault, 1, Slots.Self);
            behind1.Effects[3] = Effects.GenerateEffect(izideSprites, 1, Slots.Self, BasicEffects.DidThat(false, 3));
            behind1.AddIntentsToTarget(Targeting.Unit_AllOpponents, ["Misc_Hidden"]);
            behind1.AddIntentsToTarget(behind_target, ["ADAF_Damage_Delay", "Damage_7_10"]);
            behind1.AddIntentsToTarget(Slots.Self, ["Misc"]);
            behind1.UnitStoreData = UnitStoreData.GetCustom_UnitStoreData(IDs.Behind);
            behind1.AnimationTarget = TargettingByAlreadyAttacked.Create(Targeting.Unit_AllOpponents);
            behind1.Visuals = CustomVisuals.GetVisuals("Salt/Rose");

            Ability behind2 = new Ability(behind1.ability, "Izide_Behind_2_A", behind1.Cost);
            behind2.Name = "From Behind Past";
            behind2.Description = "At the start of the next turn, deal 10 damage to the current positions of all enemies this party member has damaged before.\nDecrease this ability's damage by 4.";
            behind2.Effects[0].entryVariable = 10;
            behind2.Effects[1].entryVariable = 4;

            Ability behind3 = new Ability(behind2.ability, "Izide_Behind_3_A", behind1.Cost);
            behind3.Name = "From Behind History";
            behind3.Description = "At the start of the next turn, deal 12 damage to the current positions of all enemies this party member has damaged before.\nDecrease this ability's damage by 5.";
            behind3.Effects[0].entryVariable = 12;
            behind3.Effects[1].entryVariable = 5;
            behind3.EffectIntents[1].intents[1] = "Damage_11_15";

            Ability behind4 = new Ability(behind3.ability, "Izide_Behind_4_A", behind1.Cost);
            behind4.Name = "From Behind Creation";
            behind4.Description = "At the start of the next turn, deal 14 damage to the current positions of all enemies this party member has damaged before.\nDecrease this ability's damage by 6.";
            behind4.Effects[0].entryVariable = 14;
            behind4.Effects[1].entryVariable = 6;
            
            izide.AddLevelData(12, [med1, ruin1, behind1]);
            izide.AddLevelData(16, [med2, ruin2, behind2]);
            izide.AddLevelData(20, [med3, ruin3, behind3]);
            izide.AddLevelData(22, [med4, ruin4, behind4]);
            izide.AddCharacter(true);
        }
        public static void AddDialogueEmote()
        {
            SpeakerBundle mad = new SpeakerBundle();
            mad.bundleTextColor = new Color32(147, 55, 121, 255);
            mad.dialogueSound = "event:/Lunacy/SOUNDS4/EnigmaDie";
            mad.portrait = ResourceLoader.LoadSprite("IzideSpeak.png");

            SpeakerEmote emotion = new SpeakerEmote();
            emotion.emotion = "Speak";
            emotion.bundle = mad;

            LoadedAssetsHandler.GetSpeakerData("Izide_SpeakerData")._emotionBundles = [emotion];
        }




        public static string HeavenACH => "Aprils_Izide_Heaven_ACH";
        public static string OsmanACH => "Aprils_Izide_Osman_ACH";

        public static string HeavenUnlock => "Aprils_Izide_Heaven_Unlock";
        public static string OsmanUnlock => "Aprils_Izide_Osman_Unlock";
    }
}
