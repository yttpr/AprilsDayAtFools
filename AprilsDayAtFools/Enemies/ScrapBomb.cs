using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class ScrapBomb
    {
        public static void Add()
        {
            PerformEffectPassiveAbility chunk = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            chunk._passiveName = "Chunk";
            chunk.passiveIcon = ResourceLoader.LoadSprite("ChunkPassive.png");
            chunk.m_PassiveID = "Chunk_PA";
            chunk._characterDescription = "On being direct damaged, deal the damage taken indirectly to the Left and Right allies.";
            chunk._enemyDescription = "On being direct damaged, deal the damage taken indirectly to the Left and Right enemies.";
            chunk.conditions = new EffectorConditionSO[] { ChunkCondition.Create() };
            chunk.doesPassiveTriggerInformationPanel = false;
            chunk._triggerOn = [TriggerCalls.OnDirectDamaged];
            chunk.effects = [];

            EffectInfo start = Effects.GenerateEffect(ScriptableObject.CreateInstance<WitheringEffect>(), 0, null, ScriptableObject.CreateInstance<OneOrLessEnemyCondition>());

            Ability clockwork = new Ability("LikeClockwork_A");
            clockwork.Name = "Like Clockwork";
            clockwork.Description = "Move Left. Take a Painful amount of indirect damage.";
            clockwork.Rarity = Rarity.Common;
            clockwork.AnimationTarget = Slots.Self;
            clockwork.Visuals = LoadedAssetsHandler.GetCharacterAbility("Wrath_1_A").visuals;
            clockwork.Effects = new EffectInfo[3];
            clockwork.Effects[0] = Effects.GenerateEffect(BasicEffects.GoLeft, 1, Slots.Self);
            clockwork.Effects[1] = Effects.GenerateEffect(BasicEffects.Indirect, 4, Slots.Self);
            clockwork.Effects[2] = start;
            clockwork.AddIntentsToTarget(Slots.Self, ["Swap_Left", "Damage_3_6"]);

            Ability mechanics = new Ability("Mechanical Process", "MechanicalProcess_A");
            mechanics.Description = "Move Right. Take a Painful amount of indirect damage.";
            mechanics.Rarity = Rarity.Common;
            mechanics.AnimationTarget = Slots.Self;
            mechanics.Visuals = clockwork.ability.visuals;
            mechanics.Effects = new EffectInfo[3];
            mechanics.Effects[0] = Effects.GenerateEffect(BasicEffects.GoRight, 1, Slots.Self);
            mechanics.Effects[1] = clockwork.Effects[1];
            mechanics.Effects[2] = start;
            mechanics.AddIntentsToTarget(Slots.Self, ["Swap_Right", "Damage_3_6"]);

            Enemy bomb = new Enemy("Scrap Bomb", "ScrapBomb_EN")
            {
                Health = 16,
                HealthColor = Pigments.Grey,
                CombatSprite = ResourceLoader.LoadSprite("BombIcon.png"),
                OverworldAliveSprite = ResourceLoader.LoadSprite("BombWorld.png", new Vector2(0.5f, 0.0f)),
                OverworldDeadSprite = ResourceLoader.LoadSprite("BombWorld.png"),
            };
            bomb.PrepareEnemyPrefab("Assets/Chunk/Chunk_Enemy.prefab", Joyce.Assets, Joyce.Assets.LoadAsset<GameObject>("Assets/Chunk/Chunk_Gibs.prefab").GetComponent<ParticleSystem>());
            bomb.AddPassives([chunk, Passives.Unstable, Passives.Withering]);
            bomb.AddEnemyAbilities([clockwork.GenerateEnemyAbility(), mechanics.GenerateEnemyAbility()]);
            bomb.DamageSound = LoadedAssetsHandler.GetEnemy("ChoirBoy_EN").damageSound;
            bomb.DeathSound = LoadedAssetsHandler.GetEnemy("ChoirBoy_EN").deathSound;

            bomb.CombatEnterEffects = [Effects.GenerateEffect(CasterRootActionEffect.Create([start]), 1, Slots.Self)];

            bomb.AddEnemy();
        }
    }
}
