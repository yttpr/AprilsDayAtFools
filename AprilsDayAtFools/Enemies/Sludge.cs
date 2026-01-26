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

            Ability glrbl = new Ability("Glrbl_A");
            glrbl.Name = "Glrbl";
            glrbl.Description = "Turn Blue.";
            glrbl.Rarity = Rarity.Common;
            glrbl.AnimationTarget = Slots.Self;
            glrbl.Visuals = LoadedAssetsHandler.GetCharacterAbility("Oil_1_A").visuals;
            glrbl.Effects = new EffectInfo[2];
            RandomizeTargetHealthColorsEffect turnBlue = ScriptableObject.CreateInstance<RandomizeTargetHealthColorsEffect>();
            turnBlue.options = [Pigments.Blue];
            glrbl.Effects[0] = Effects.GenerateEffect(turnBlue, 1, Slots.Self);
            glrbl.Effects[2] = start;
            glrbl.AddIntentsToTarget(Slots.Self, ["Mana_Modify"]);

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
            sludge.AddEnemyAbilities([glrbl.GenerateEnemyAbility()]);
            sludge.DamageSound = LoadedAssetsHandler.GetEnemy("TaintedYolk_EN").damageSound;
            sludge.DeathSound = LoadedAssetsHandler.GetEnemy("TaintedYolk_EN").deathSound;

            sludge.CombatEnterEffects = [Effects.GenerateEffect(CasterRootActionEffect.Create([start]), 1, Slots.Self)];

            sludge.AddEnemy();
        }
    }
}
