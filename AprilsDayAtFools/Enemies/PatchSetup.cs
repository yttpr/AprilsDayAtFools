using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class PatchSetup
    {
        public static void Setup()
        {
            Ability damage1 = new Ability("Excommunication", "Patch_Enemy_1_A");
            damage1.Description = "Deal 6 damage to the Left and Right enemies if they are not Assistants.\nGenerate 1 Pigment of this enemy's health color and instantly kill this enemy.";
            damage1.Rarity = Rarity.CreateAndAddCustomRarityToPool("Patch_High", 999);
            damage1.Cost = [Pigments.Grey, Pigments.Grey, Pigments.Grey];
            damage1.Effects = new EffectInfo[3];
            damage1.Effects[0] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageNotAssistantsEffect>(), 6, Slots.Sides);
            damage1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<GenerateCasterHealthManaEffect>(), 1);
            damage1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DirectDeathEffect>(), 1, Slots.Self);
            damage1.AddIntentsToTarget(Slots.Sides, ["Damage_3_6"]);
            damage1.AddIntentsToTarget(Slots.Self, ["Mana_Generate", "Damage_Death"]);
            damage1.Visuals = LoadedAssetsHandler.GetEnemyAbility("RapturousReverberation_A").visuals;
            damage1.AnimationTarget = Slots.Sides;
            damage1.GenerateEnemyAbility();
            Ability damage2 = new Ability(damage1.ability, "Patch_Enemy_2_A", damage1.Cost, damage1.Rarity);
            damage2.Description = "Deal 8 damage to the Left and Right enemies.\nGenerate 1 Pigment of this enemy's health color and instantly kill this enemy.";
            damage2.Effects[0].entryVariable = 8;
            damage2.EffectIntents[0].intents[0] = "Damage_7_10";
            Ability damage3 = new Ability(damage2.ability, "Patch_Enemy_3_A", damage1.Cost, damage1.Rarity);
            damage3.Description = "Deal 10 damage to the Left and Right enemies.\nGenerate 1 Pigment of this enemy's health color and instantly kill this enemy.";
            damage3.Effects[0].entryVariable = 10;
            Ability damage4 = new Ability(damage3.ability, "Patch_Enemy_4_A", damage1.Cost, damage1.Rarity);
            damage4.Description = "Deal 12 damage to the Left and Right enemies.\nGenerate 1 Pigment of this enemy's health color and instantly kill this enemy.";
            damage4.Effects[0].entryVariable = 12;
            damage4.EffectIntents[0].intents[0] = "Damage_11_15";

            InfestationRandomSetEffect increase = ScriptableObject.CreateInstance<InfestationRandomSetEffect>();
            increase._infestationPassive = Passives.Infestation1 as InfestationPassiveAbility;
            Ability infest = new Ability("Delay", "Patch_EnemyOther_A");
            infest.Description = "Increase this enemy's Infestation by 1.\nGenerate 1 Pigment of this enemy's health color.";
            infest.Rarity = Rarity.CreateAndAddCustomRarityToPool("Patch_Low", 1);
            infest.Cost = [Pigments.Grey];
            infest.Effects = new EffectInfo[2];
            infest.Effects[0] = Effects.GenerateEffect(increase, 1, Slots.Self);
            infest.Effects[1] = damage1.Effects[1];
            infest.AddIntentsToTarget(Slots.Self, ["PA_Infestation", "Mana_Generate"]);
            infest.Visuals = LoadedAssetsHandler.GetEnemyAbility("Wriggle_A").visuals;
            infest.AnimationTarget = Slots.Self;
            infest.GenerateEnemyAbility();

            AddEnemy("Patch_Assistant_1_Red_EN", Pigments.Red, "HeavensGateRed_BOSS", [damage1, infest]);
            AddEnemy("Patch_Assistant_2_Red_EN", Pigments.Red, "HeavensGateRed_BOSS", [damage2, infest]);
            AddEnemy("Patch_Assistant_3_Red_EN", Pigments.Red, "HeavensGateRed_BOSS", [damage3, infest]);
            AddEnemy("Patch_Assistant_4_Red_EN", Pigments.Red, "HeavensGateRed_BOSS", [damage4, infest]);

            AddEnemy("Patch_Assistant_1_Blue_EN", Pigments.Blue, "HeavensGateBlue_BOSS", [damage1, infest]);
            AddEnemy("Patch_Assistant_2_Blue_EN", Pigments.Blue, "HeavensGateBlue_BOSS", [damage2, infest]);
            AddEnemy("Patch_Assistant_3_Blue_EN", Pigments.Blue, "HeavensGateBlue_BOSS", [damage3, infest]);
            AddEnemy("Patch_Assistant_4_Blue_EN", Pigments.Blue, "HeavensGateBlue_BOSS", [damage4, infest]);

            AddEnemy("Patch_Assistant_1_Yellow_EN", Pigments.Yellow, "HeavensGateYellow_BOSS", [damage1, infest]);
            AddEnemy("Patch_Assistant_2_Yellow_EN", Pigments.Yellow, "HeavensGateYellow_BOSS", [damage2, infest]);
            AddEnemy("Patch_Assistant_3_Yellow_EN", Pigments.Yellow, "HeavensGateYellow_BOSS", [damage3, infest]);
            AddEnemy("Patch_Assistant_4_Yellow_EN", Pigments.Yellow, "HeavensGateYellow_BOSS", [damage4, infest]);

            AddEnemy("Patch_Assistant_1_Purple_EN", Pigments.Purple, "HeavensGatePurple_BOSS", [damage1, infest]);
            AddEnemy("Patch_Assistant_2_Purple_EN", Pigments.Purple, "HeavensGatePurple_BOSS", [damage2, infest]);
            AddEnemy("Patch_Assistant_3_Purple_EN", Pigments.Purple, "HeavensGatePurple_BOSS", [damage3, infest]);
            AddEnemy("Patch_Assistant_4_Purple_EN", Pigments.Purple, "HeavensGatePurple_BOSS", [damage4, infest]);
        }

        public static void AddEnemy(string id, ManaColorSO pigment, string baseEN, Ability[] abilities)
        {
            Enemy enemy = new Enemy("Assistant", id);
            enemy.Health = 30;
            enemy.HealthColor = pigment;
            enemy.Priority = Priority.VeryFast;

            EnemySO origin = LoadedAssetsHandler.GetEnemy(baseEN);
            enemy.enemy.enemyTemplate = origin.enemyTemplate;
            enemy.CombatSprite = origin.enemySprite;
            enemy.OverworldAliveSprite = origin.enemyOverworldSprite;
            enemy.OverworldDeadSprite = origin.enemyOWCorpseSprite;
            enemy.DamageSound = origin.damageSound;
            enemy.DeathSound = origin.deathSound;

            enemy.AddPassives([Passives.Infestation1, Passives.Slippery, Passives.Withering]);
            List<EnemyAbilityInfo> list = new List<EnemyAbilityInfo>();
            for (int i = 0; i < abilities.Length; i++)
            {
                list.Add(abilities[i].GenerateEnemyAbility());
            }
            enemy.AddEnemyAbilities(list.ToArray());

            //enemy.CombatEnterEffects = Effects.GenerateEffect(ScriptableObject.CreateInstance<AddTurnCasterToTimelineEffect>(), 1).SelfArray();

            enemy.AddEnemy();
        }

        public static string GetAssistant(ManaColorSO pigment, int rank)
        {
            string ret = "Patch_Assistant_" + (rank + 1).ToString();

            ManaColorSO[] primaries = [Pigments.Red, Pigments.Blue, Pigments.Yellow, Pigments.Purple];

            List<ManaColorSO> choose = new List<ManaColorSO>();
            for (int i = 0; i < primaries.Length; i++)
            {
                if (pigment.SharesPigmentColor(primaries[i])) choose.Add(primaries[i]);
            }

            ManaColorSO pick = Pigments.Red;
            if (choose.Count > 0) pick = choose.GetRandom();
            else pick = primaries.GetRandom();

            if (pick == Pigments.Red) ret += "_Red_EN";
            else if (pick == Pigments.Blue) ret += "_Blue_EN";
            else if (pick == Pigments.Yellow) ret += "_Yellow_EN";
            else ret += "_Purple_EN";

            return ret;
        }
    }
}
