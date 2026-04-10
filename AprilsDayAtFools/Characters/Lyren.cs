using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Lyren
    {
        public static void Add()
        {
            Character lyren = new Character("Lyren", "Lyren_CH");
            lyren.HealthColor = Pigments.Blue;
            lyren.AddUnitType("FemaleID");
            lyren.AddUnitType("Sandwich_Fish");
            lyren.AddUnitType("FemaleLooking");
            lyren.UsesBasicAbility = true;
            //slap
            lyren.UsesAllAbilities = false;
            lyren.MovesOnOverworld = true;
            //animator
            lyren.FrontSprite = ResourceLoader.LoadSprite("LyrenFront.png");
            lyren.BackSprite = ResourceLoader.LoadSprite("LyrenBack.png");
            lyren.OverworldSprite = ResourceLoader.LoadSprite("LyrenWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            lyren.DamageSound = LoadedAssetsHandler.GetEnemy("Woodwind_EN").damageSound;
            lyren.DeathSound = LoadedAssetsHandler.GetEnemy("Woodwind_EN").deathSound;
            lyren.DialogueSound = LoadedAssetsHandler.GetEnemy("Woodwind_EN").damageSound;
            //lyren.AddFinalBossAchievementData("OsmanSinnoks", OsmanACH);
            //lyren.AddFinalBossAchievementData("Heaven", HeavenACH);
            //lyren.GenerateMenuCharacter(ResourceLoader.LoadSprite("LyrenMenu.png"), ResourceLoader.LoadSprite("LyrenLock.png"));
            //lyren.MenuCharacterIsSecret = false;
            //lyren.MenuCharacterIgnoreRandom = false;
            //lyren.SetMenuCharacterAsFullDPS();
            lyren.AddPassive(Passives.Constricting);
            
            ApplyRupturedEffect rupture = ScriptableObject.CreateInstance<ApplyRupturedEffect>();
            DamageEffect indirect = BasicEffects.Indirect;
            CasterConstrictedToggleEffect constrict = ScriptableObject.CreateInstance<CasterConstrictedToggleEffect>();
            EffectInfo off = Effects.GenerateEffect(constrict, 1, Slots.Self);
            EffectInfo on = off;

            Ability breakfast1 = new Ability("Rain Breakfast", "Lyren_Breakfast_1_A");
            breakfast1.Description = "Deal 3 indirect damage to the Opposing enemy.\nReduce all Negative Field Effects on this position by 1 and restore this party member's swap usage.";
            breakfast1.AbilitySprite = ResourceLoader.LoadSprite("ability_breakfast.png");
            breakfast1.Cost = [Pigments.Blue, Pigments.Yellow];
            breakfast1.Effects = new EffectInfo[4];
            breakfast1.Effects[0] = Effects.GenerateEffect(indirect, 3, Slots.Front);
            breakfast1.Effects[1] = Effects.GenerateEffect(rupture, 0, Slots.Front);
            breakfast1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<ReduceAllNegativeFieldEffect>(), 1, Slots.Self);
            breakfast1.Effects[3] = Effects.GenerateEffect(ScriptableObject.CreateInstance<RestoreSwapUseEffect>(), 1, Slots.Self);
            breakfast1.AddIntentsToTarget(Slots.Front, ["Damage_3_6"]);
            breakfast1.AddIntentsToTarget(Slots.Self, ["Misc"]);
            breakfast1.Visuals = CustomVisuals.GetVisuals("Salt/Whisper");
            breakfast1.AnimationTarget = Slots.Front;

            Ability breakfast2 = new Ability(breakfast1.ability, "Lyren_Breakfast_2_A", breakfast1.Cost);
            breakfast2.Name = "Watery Breakfast";
            breakfast2.Description = "Deal 4 indirect damage to the Opposing enemy.\nReduce all Negative Field Effects on this position by 2 and restore this party member's swap usage.";
            breakfast2.Effects[0].entryVariable = 4;
            breakfast2.Effects[2].entryVariable = 2;

            Ability breakfast3 = new Ability(breakfast2.ability, "Lyren_Breakfast_3_A", breakfast1.Cost);
            breakfast3.Name = "Sea Breakfast";
            breakfast3.Description = "Deal 6 indirect damage to the Opposing enemy and infict 2 Ruptured on them.\nReduce all Negative Field Effects on this position by 2 and restore this party member's swap usage.";
            breakfast3.Effects[0].entryVariable = 6;
            breakfast3.Effects[1].entryVariable = 2;
            breakfast3.AddIntentsToTarget(Slots.Front, ["Status_Ruptured"]);

            Ability breakfast4 = new Ability(breakfast3.ability, "Lyren_Breakfast_4_A", breakfast1.Cost);
            breakfast4.Name = "Abyssal Breakfast";
            breakfast4.Description = "Deal 7 indirect damage to the Opposing enemy and infict 2 Ruptured on them.\nReduce all Negative Field Effects on this position by 3 and restore this party member's swap usage.";
            breakfast4.Effects[0].entryVariable = 7;
            breakfast4.Effects[2].entryVariable = 3;
            breakfast4.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability novelty1 = new Ability("Filter Novelty", "Lyren_Novelty_1_A");
            novelty1.Description = "Deal 6 damage to the Opposing enemy and move them Left or Right, then produce 2 Blue Pigment.\nThis ability ignores this party member's Constricting.";
            novelty1.AbilitySprite = ResourceLoader.LoadSprite("ability_novelty.png");
            novelty1.Cost = [Pigments.Red, Pigments.Blue];
            novelty1.Effects = new EffectInfo[5];
            novelty1.Effects[0] = off;
            novelty1.Effects[1] = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(), 6, Slots.Front);
            novelty1.Effects[2] = Effects.GenerateEffect(ScriptableObject.CreateInstance<SwapToSidesEffect>(), 1, Slots.Front);
            novelty1.Effects[3] = Effects.GenerateEffect(BasicEffects.GenPigment(Pigments.Blue), 2, Slots.Self);
            novelty1.Effects[4] = on;
            novelty1.AddIntentsToTarget(Slots.Front, ["Damage_3_6", "Swap_Sides"]);
            novelty1.AddIntentsToTarget(Slots.Self, ["PA_Constricting", "Mana_Generate"]);
            novelty1.Visuals = CustomVisuals.GetVisuals("Salt/Class");
            novelty1.AnimationTarget = Slots.Front;

            Ability novelty2 = new Ability(novelty1.ability, "Lyren_Novelty_2_A", novelty1.Cost);
            novelty2.Name = "Static Novelty";
            novelty2.Description = "Deal 9 damage to the Opposing enemy and move them Left or Right, then produce 2 Blue Pigment.\nThis ability ignores this party member's Constricting.";
            novelty2.Effects[1].entryVariable = 9;
            novelty2.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability novelty3 = new Ability(novelty2.ability, "Lyren_Novelty_3_A", novelty1.Cost);
            novelty3.Name = "Noise Novelty";
            novelty3.Description = "Deal 11 damage to the Opposing enemy and move them Left or Right, then produce 2 Blue Pigment.\nThis ability ignores this party member's Constricting.";
            novelty3.Effects[1].entryVariable = 11;
            novelty3.EffectIntents[0].intents[0] = "Damage_11_15";

            Ability novelty4 = new Ability(novelty3.ability, "Lyren_Novelty_4_A", novelty1.Cost);
            novelty4.Name = "Screech Novelty";
            novelty4.Description = "Deal 13 damage to the Opposing enemy and move them Left or Right, then produce 2 Blue Pigment.\nThis ability ignores this party member's Constricting.";
            novelty4.Effects[1].entryVariable = 13;

            Ability sing1 = new Ability("Sing Trust", "Lyren_Sing_1_A");
            sing1.Description = "Deal 5 indirect damage and inflict 1 Ruptured to the Left and Right enemies, then move them towards this party member.\nThis ability ignores this party member's Constricting.";
            sing1.AbilitySprite = ResourceLoader.LoadSprite("ability_sing.png");
            sing1.Cost = [Pigments.Blue, Pigments.Blue, Pigments.Yellow];
            sing1.Effects = new EffectInfo[6];
            sing1.Effects[0] = off;
            sing1.Effects[1] = Effects.GenerateEffect(indirect, 5, Slots.LeftRight);
            sing1.Effects[2] = Effects.GenerateEffect(rupture, 1, Slots.LeftRight);
            sing1.Effects[3] = Effects.GenerateEffect(BasicEffects.GoRight, 1, Slots.Left);
            sing1.Effects[4] = Effects.GenerateEffect(BasicEffects.GoLeft, 1, Slots.Right);
            sing1.Effects[5] = on;
            sing1.AddIntentsToTarget(Slots.LeftRight, ["Damage_3_6", "Status_Ruptured"]);
            sing1.AddIntentsToTarget(Slots.Left, ["Swap_Right"]);
            sing1.AddIntentsToTarget(Slots.Right, ["Swap_Left"]);
            sing1.AddIntentsToTarget(Slots.Self, ["PA_Constricting"]);
            sing1.Visuals = CustomVisuals.GetVisuals("Salt/Coda");
            sing1.AnimationTarget = Slots.LeftRight;

            Ability sing2 = new Ability(sing1.ability, "Lyren_Sing_2_A", sing1.Cost);
            sing2.Name = "Sing Faith";
            sing2.Description = "Deal 7 indirect damage and inflict 1 Ruptured to the Left and Right enemies, then move them towards this party member.\nThis ability ignores this party member's Constricting.";
            sing2.Effects[1].entryVariable = 7;
            sing2.EffectIntents[0].intents[0] = "Damage_7_10";

            Ability sing3 = new Ability(sing2.ability, "Lyren_Sing_3_A", [Pigments.Blue, Pigments.Blue]);
            sing3.Name = "Sing Glory";
            sing3.Description = "Deal 8 indirect damage and inflict 3 Ruptured to the Left and Right enemies, then move them towards this party member.\nThis ability ignores this party member's Constricting.";
            sing3.Effects[1].entryVariable = 8;
            sing3.Effects[2].entryVariable = 3;

            Ability sing4 = new Ability(sing3.ability, "Lyren_Sing_4_A", sing3.Cost);
            sing4.Name = "Sing Eternity";
            sing4.Description = "Deal 10 indirect damage and inflict 3 Ruptured to the Left and Right enemies, then move them towards this party member.\nThis ability ignores this party member's Constricting.";
            sing4.Effects[1].entryVariable = 10;

            lyren.AddLevelData(14, [breakfast1, novelty1, sing1]);
            lyren.AddLevelData(18, [breakfast2, novelty2, sing2]);
            lyren.AddLevelData(20, [breakfast3, novelty3, sing3]);
            lyren.AddLevelData(25, [breakfast4, novelty4, sing4]);
            lyren.AddCharacter(false, true);
        }
    }
}
