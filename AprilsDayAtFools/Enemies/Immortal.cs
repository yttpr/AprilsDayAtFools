using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Immortal
    {
        public static void Add()
        {
            Ability nothing = new Ability("ADAF_Spectator_A");
            nothing.Name = "Spectator";
            nothing.Description = "Simply watch.";
            nothing.Rarity = Rarity.Common;
            nothing.AnimationTarget = Slots.Self;
            nothing.Visuals = null;
            nothing.Effects = [];

            Enemy immortal = new Enemy("Immortal Figures", "ImmortalFigures_EN")
            {
                Health = 1,
                HealthColor = Pigments.Grey,
                CombatSprite = ResourceLoader.LoadSprite("ImmortalIcon.png"),
                OverworldAliveSprite = ResourceLoader.LoadSprite("ImmortalWorld.png", new Vector2(0.5f, 0.0f)),
                OverworldDeadSprite = ResourceLoader.LoadSprite("ImmortalWorld.png"),
                Priority = Priority.VerySlow
            };
            immortal.PrepareEnemyPrefab("Assets/Immortal/Immortal_Enemy.prefab", Joyce.Assets, Joyce.Assets.LoadAsset<GameObject>("Assets/Immortal/Immortal_Gibs.prefab").GetComponent<ParticleSystem>());
            immortal.AddPassives([Passives.Immortal, Passives.Withering]);
            immortal.AddEnemyAbilities([nothing.GenerateEnemyAbility()]);
            immortal.DamageSound = LoadedAssetsHandler.GetEnemy("JumbleGuts_Hollowing_EN").damageSound;
            immortal.DeathSound = LoadedAssetsHandler.GetEnemy("JumbleGuts_Hollowing_EN").deathSound;

            immortal.AddEnemy();
        }
    }
}
