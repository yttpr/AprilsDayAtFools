using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class Anatomy
    {
        public static void Add()
        {
            MimicryPassive mimicry = ScriptableObject.CreateInstance<MimicryPassive>();
            mimicry._passiveName = "Mimicry";
            mimicry.m_PassiveID = "Mimicry_PA";
            mimicry.passiveIcon = ResourceLoader.LoadSprite("MimicryPassive.png");
            mimicry._enemyDescription = "Whenever a unit with Mimicry uses an abililty, all other units with Mimicry will attempt to use that ability as well.";
            mimicry._characterDescription = "Whenever a unit with Mimicry uses an abililty, all other units with Mimicry will attempt to use that ability as well.";
            mimicry.doesPassiveTriggerInformationPanel = false;
            mimicry.conditions = [];
            mimicry._triggerOn = [TriggerCalls.OnAbilityWillBeUsed, MimicryPassive.CustomTrigger];
            mimicry.AddPassiveToGlossary("Mimicry", "Whenever a unit with Mimicry uses an abililty, all other units with Mimicry will attempt to use that ability as well.");
            mimicry.AddToPassiveDatabase();

            Character anatomy = new Character("Anatomy Model", "AnatomyModel_CH");
            anatomy.HealthColor = Pigments.Red;
            anatomy.UsesBasicAbility = false;
            //slap
            anatomy.UsesAllAbilities = true;
            anatomy.MovesOnOverworld = false;
            //animator
            anatomy.FrontSprite = ResourceLoader.LoadSprite("AnatomicalFront.png");
            anatomy.BackSprite = ResourceLoader.LoadSprite("AnatomicalBack.png");
            anatomy.OverworldSprite = ResourceLoader.LoadSprite("AnatomicalWorld.png", new Vector2(0.5f, 0f));
            //extra sprites
            anatomy.DamageSound = LoadedAssetsHandler.GetCharacter("Gospel_CH").damageSound;
            anatomy.DeathSound = LoadedAssetsHandler.GetCharacter("Gospel_CH").deathSound;
            anatomy.DialogueSound = LoadedAssetsHandler.GetCharacter("Gospel_CH").dxSound;

            anatomy.AddPassives([mimicry, Passives.Inanimate, Passives.Withering]);

            anatomy.AddLevelData(8, new Ability[0]);
            anatomy.AddCharacter(false, true);
        }
    }
}
