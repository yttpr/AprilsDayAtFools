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
            nothing.Description = "Instantly Die.";
            nothing.Rarity = Rarity.Common;
            nothing.AnimationTarget = Slots.Self;
            nothing.Visuals = null;
            nothing.Effects = [Effects.GenerateEffect(BasicEffects.Die(true), 1, Slots.Self)];
            nothing.AddIntentsToTarget(Slots.Self, ["Damage_Death"]);

            PerformEffectPassiveAbility ghostly = ScriptableObject.CreateInstance<PerformEffectPassiveAbility>();
            ghostly._passiveName = "Ghost";
            ghostly.name = "ADAF_Ghost_PA";
            ghostly.passiveIcon = ResourceLoader.LoadSprite("GhostPassive.png");
            ghostly.m_PassiveID = "ADAF_Ghost_PA";
            ghostly._enemyDescription = "On receiving any damage, even if it was prevented, instantly die.";
            ghostly._characterDescription = "On receiving any damage, even if it was prevented, instantly die.";
            ghostly.doesPassiveTriggerInformationPanel = true;
            ghostly.effects = Effects.GenerateEffect(BasicEffects.Die(true), 1, Slots.Self).SelfArray();
            ghostly._triggerOn = new TriggerCalls[1] { TriggerCalls.OnBeingDamaged };
            ghostly.conditions = [];
            ghostly.AddPassiveToGlossary("Ghost", "On receiving any damage, even if it was prevented, instantly die.");
            ghostly.AddToPassiveDatabase();

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
            immortal.AddPassives([Passives.Immortal, ghostly, Passives.Withering]);
            immortal.AddEnemyAbilities([nothing.GenerateEnemyAbility()]);
            immortal.DamageSound = LoadedAssetsHandler.GetEnemy("JumbleGuts_Hollowing_EN").damageSound;
            immortal.DeathSound = LoadedAssetsHandler.GetEnemy("JumbleGuts_Hollowing_EN").deathSound;

            immortal.AddEnemy();
        }
    }
}
