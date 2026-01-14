using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Dog
    {
        public static void Add()
        {
            DoubleEffectCondition doublecondiion = ScriptableObject.CreateInstance<DoubleEffectCondition>();
            doublecondiion.first = Effects.ChanceCondition(50);
            doublecondiion.second = ScriptableObject.CreateInstance<FrontSlotEmptyCondition>();
            doublecondiion.And = true;
            PerformEffectPassiveAbility passive = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            passive._passiveName = "Rabid";
            passive.passiveIcon = ResourceLoader.LoadSprite("Rabid.png");
            passive.m_PassiveID = "Rabid_PA";
            passive._enemyDescription = "At the start of each turn, move Left or Right 1-3 times or until Opposing a party member, then perform a random ability, then apply 1 Dodge and 1 Stunned to this enemy.";
            passive._characterDescription = "At the start of each turn, move Left or Right 1-3 times or until Opposing an enemy, then perform a random ability, then apply 1 Dodge and 1 Stunned to this party member.";
            passive.doesPassiveTriggerInformationPanel = true;
            passive.effects = [
                Effects.GenerateEffect(ScriptableObject.CreateInstance<SwapToSidesEffect>(), 1, Slots.Self, doublecondiion.second),
                Effects.GenerateEffect(ScriptableObject.CreateInstance<SwapToSidesEffect>(), 1, Slots.Self, doublecondiion),
                Effects.GenerateEffect(ScriptableObject.CreateInstance<SwapToSidesEffect>(), 1, Slots.Self, doublecondiion),
                Effects.GenerateEffect(ScriptableObject.CreateInstance<PerformRandomAbilityEffect>(), 1, Slots.Self),
                Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyDodgeEffect>(), 1, Slots.Self),
                Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyStunnedEffect>(), 1, Slots.Self)
            ];
            passive._triggerOn = [TriggerCalls.OnTurnStart];

            Character rabid = new Character("Rabid Dog", "RabidDog_CH");
            rabid.HealthColor = Pigments.Red;
            rabid.UsesBasicAbility = false;
            //slap
            rabid.UsesAllAbilities = true;
            rabid.MovesOnOverworld = true;
            //animator
            rabid.FrontSprite = ResourceLoader.LoadSprite("DogFront.png");
            rabid.BackSprite = ResourceLoader.LoadSprite("DogBack.png");
            rabid.OverworldSprite = ResourceLoader.LoadSprite("DogWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            rabid.DamageSound = LoadedAssetsHandler.GetEnemy("Bronzo_Bananas_Mean_EN").damageSound;
            rabid.DeathSound = LoadedAssetsHandler.GetEnemy("Bronzo_Bananas_Mean_EN").deathSound;
            rabid.DialogueSound = LoadedAssetsHandler.GetEnemy("Bronzo_Bananas_Mean_EN").damageSound;
            rabid.AddPassive(passive);

            Ability bite = new Ability("Rabid Bite", "RabidBite_A");
            bite.Description = "Deal 7 damage to the Opposing enemy and inflict 3 Acid on them.";
            bite.AbilitySprite = ResourceLoader.LoadSprite("ability_bite.png");
            bite.Cost = [Pigments.Red, Pigments.Red, Pigments.Red];
            bite.Effects = new EffectInfo[2];
            bite.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 7, Slots.Front);
            bite.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ApplyAcidEffect>(), 3, Slots.Front);
            bite.AddIntentsToTarget(Slots.Front, ["Damage_7_10", Acid.Intent]);
            bite.Visuals = LoadedAssetsHandler.GetEnemyAbility("Chomp_A").visuals;
            bite.AnimationTarget = Slots.Front;

            rabid.AddLevelData(12, [bite]);
            rabid.AddCharacter(false, true);
        }
    }
}
