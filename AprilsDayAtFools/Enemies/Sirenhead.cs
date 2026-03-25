using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Sirenhead
    {
        public static void Add()
        {
            DamageEffect damage = ScriptableObject.CreateInstance<DamageEffect>();

            Ability fool = new Ability("sirenheadattack");
            fool.Name = "Sirenhead Attack!";
            fool.Description = "Deal an Agonizing amount of damage to the Opposing enemy.";
            fool.Rarity = Rarity.Rare;
            fool.AnimationTarget = Slots.Front;
            fool.Visuals = Visuals.Clobber_Left;
            fool.Effects = [Effects.GenerateEffect(damage, 10, Slots.Front)];
            fool.AddIntentsToTarget(Slots.Front, ["Damage_7_10"]);

            Ability enemy = new Ability("sirenheadattacksenemies");
            enemy.Name = "Sirenhead Attacks Enemies";
            enemy.Description = "Deal an Agonizing amount of damage to the Left and Right enemies.";
            enemy.Rarity = Rarity.Common;
            enemy.AnimationTarget = Slots.Sides;
            enemy.Visuals = Visuals.Clobber_Right;
            enemy.Effects = [Effects.GenerateEffect(damage, 10, Slots.Sides)];
            enemy.AddIntentsToTarget(Slots.Sides, ["Damage_7_10"]);


            Enemy sirenhead = new Enemy("Adds Sirenhead To The Siren", "Sirenhead_EN")
            {
                Health = 12,
                HealthColor = Pigments.Red,
                CombatSprite = ResourceLoader.LoadSprite("SirenheadIcon.png"),
                OverworldAliveSprite = ResourceLoader.LoadSprite("SirenheadWorld.png", new Vector2(0.5f, 0.0f)),
                OverworldDeadSprite = ResourceLoader.LoadSprite("SirenheadWorld.png"),
            };
            sirenhead.PrepareEnemyPrefab("Assets/Sludge/Sirenhead_Enemy.prefab", Joyce.Assets, Joyce.Assets.LoadAsset<GameObject>("Assets/Sludge/Sirenhead_Gibs.prefab").GetComponent<ParticleSystem>());
            sirenhead.AddPassives([Passives.Formless, Passives.Skittish, Passives.Fleeting3, Passives.Transfusion]);
            sirenhead.AddEnemyAbilities([fool.GenerateEnemyAbility(), enemy.GenerateEnemyAbility()]);
            sirenhead.DamageSound = "event:/Lunacy/SOUNDS4/NamelessHit";
            sirenhead.DeathSound = "event:/Lunacy/SOUNDS4/NamelessDie";

            sirenhead.AddEnemy();
        }
    }
}
