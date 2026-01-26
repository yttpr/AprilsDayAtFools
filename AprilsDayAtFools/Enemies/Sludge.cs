using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Sludge
    {
        public static void Add()
        {
            EffectInfo start = Effects.GenerateEffect(ScriptableObject.CreateInstance<WitheringEffect>(), 0, null, ScriptableObject.CreateInstance<OneOrLessEnemyCondition>());

            Ability clockwork = new Ability("Glrbl_A");
            clockwork.Name = "Glrbl";
            clockwork.Description = "Turn Blue.";
            clockwork.Rarity = Rarity.Common;
            clockwork.AnimationTarget = Slots.Self;
            clockwork.Visuals = LoadedAssetsHandler.GetCharacterAbility("Oil_1_A").visuals;
            clockwork.Effects = new EffectInfo[2];
            RandomizeTargetHealthColorsEffect turnBlue = ScriptableObject.CreateInstance<RandomizeTargetHealthColorsEffect>();
            turnBlue.options = [Pigments.Blue];
            clockwork.Effects[0] = Effects.GenerateEffect(turnBlue, 1, Slots.Self);
            clockwork.Effects[2] = start;
            clockwork.AddIntentsToTarget(Slots.Self, ["Mana_Modify"]);


            Enemy sludge = new Enemy("The Sludge", "TheSludge_EN")
            {
                Health = 6,
                HealthColor = Pigments.Red,
                CombatSprite = ResourceLoader.LoadSprite("SludgeIcon.png"),
                OverworldAliveSprite = ResourceLoader.LoadSprite("SludgeWorld.png", new Vector2(0.5f, 0.0f)),
                OverworldDeadSprite = ResourceLoader.LoadSprite("SludgeWorld.png"),
                UnitTypes = ["Alien"],
            };
            sludge.PrepareEnemyPrefab("Assets/Sludge/Sludge_Enemy.prefab", Joyce.Assets, Joyce.Assets.LoadAsset<GameObject>("Assets/Sludge/Sludge_Gibs.prefab").GetComponent<ParticleSystem>());
            sludge.AddPassives([Passives.Withering]);
            sludge.AddEnemyAbilities([clockwork.GenerateEnemyAbility()]);
            sludge.DamageSound = LoadedAssetsHandler.GetEnemy("TaintedYolk_EN").damageSound;
            sludge.DeathSound = LoadedAssetsHandler.GetEnemy("TaintedYolk_EN").deathSound;

            sludge.CombatEnterEffects = [Effects.GenerateEffect(CasterRootActionEffect.Create([start]), 1, Slots.Self)];

            sludge.AddEnemy();
        }
    }
}
