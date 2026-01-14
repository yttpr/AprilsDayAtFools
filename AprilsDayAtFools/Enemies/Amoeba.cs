using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace AprilsDayAtFools
{
    public static class Amoeba
    {
        public static void Add()
        {
            Intents.CreateAndAddCustom_Basic_IntentToPool("Activator", LoadedAssetsHandler.GetWearable("WheelOfFortune_TW").wearableImage, Color.white);

            Ability left = new Ability("Left Brain Cell", "LeftBrainCell_A");
            left.Description = "Move Left.\nForce the Right party member to perform a random ability.";
            left.Rarity = Rarity.Common;
            left.Effects = new EffectInfo[3];
            left.Effects[0] = Effects.GenerateEffect(BasicEffects.GoLeft, 1, Slots.Self);
            left.Effects[1] = Effects.GenerateEffect(BasicEffects.GetVisuals("Slap_A", true, Slots.Right));
            left.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<PerformRandomAbilityEffect>(), 1, Slots.Right);
            left.AddIntentsToTarget(Slots.Self, ["Swap_Left"]);
            left.AddIntentsToTarget(Slots.Right, ["Activator"]);

            Ability right = new Ability("Right Brain Cell", "RightBrainCell_A");
            right.Description = "Move Right.\nForce the Left party member to perform a random ability.";
            right.Rarity = Rarity.Common;
            right.Effects = new EffectInfo[3];
            right.Effects[0] = Effects.GenerateEffect(BasicEffects.GoRight, 1, Slots.Self);
            right.Effects[1] = Effects.GenerateEffect(BasicEffects.GetVisuals("Slap_A", true, Slots.Left));
            right.Effects[2] = Effects.GenerateEffect(left.Effects[2].effect, 1, Slots.Left);
            right.AddIntentsToTarget(Slots.Self, ["Swap_Right"]);
            right.AddIntentsToTarget(Slots.Left, ["Activator"]);

            Enemy activator = new Enemy("Neuron Activator", "NeuronActivator_EN")
            {
                Health = 1,
                HealthColor = Pigments.Purple,
                CombatSprite = ResourceLoader.LoadSprite("ActivatorIcon.png"),
                OverworldAliveSprite = ResourceLoader.LoadSprite("ActivatorIcon.png", new Vector2(0.5f, 0.0f)),
                OverworldDeadSprite = ResourceLoader.LoadSprite("ActivatorIcon.png", new Vector2(0.5f, 0.0f)),
                Priority = Priority.VeryFast
            };
            activator.PrepareEnemyPrefab("Assets/sludge/Sludge_Enemy.prefab", Joyce.Other, Joyce.Other.LoadAsset<GameObject>("Assets/sludge/Sludge_Gibs.prefab").GetComponent<ParticleSystem>());
            activator.AddPassives([Passives.Leaky1, Passives.Withering, Passives.Abomination1]);
            activator.AddEnemyAbilities([left.GenerateEnemyAbility(), right.GenerateEnemyAbility()]);
            activator.DamageSound = LoadedAssetsHandler.GetEnemy("ManicHips_EN").damageSound;
            activator.DeathSound = LoadedAssetsHandler.GetEnemy("ManicHips_EN").deathSound;

            CasterStoredValueChangeEffect instance2 = ScriptableObject.CreateInstance<CasterStoredValueChangeEffect>();
            instance2._increase = true;
            instance2.m_unitStoredDataID = UnitStoredValueNames_GameIDs.AbominationPA.ToString();
            activator.CombatEnterEffects = [
                Effects.GenerateEffect(ScriptableObject.CreateInstance<AddTurnCasterToTimelineEffect>(), 1, Slots.Self),
                Effects.GenerateEffect(instance2, 1)
            ];

            activator.AddEnemy();
        }
    }
}
