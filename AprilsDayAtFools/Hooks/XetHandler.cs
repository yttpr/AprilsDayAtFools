using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools//change this to your namepace
{
    public static class XetHandler//rename this class to whatever you want
    {
        public static Sprite Small;
        public static Sprite Large;
        public static void Setup()//call this function in your main awake. like, a line after you call ThisIsMyCharacter.Add(); call MyCharacterHandler.Setup();
        {
            Small = ResourceLoader.LoadSprite("XetSmall.png");//set this to your 64x64 sprite
            Large = ResourceLoader.LoadSprite("XetFront.png");//set this to the in combat sprite
            IDetour hook1 = new Hook(typeof(MainMenuController).GetMethod(nameof(MainMenuController.Start), ~BindingFlags.Default), typeof(XetHandler).GetMethod(nameof(MainMenuController_Start), ~BindingFlags.Default));
            IDetour hook2 = new Hook(typeof(CombatManager).GetMethod(nameof(CombatManager.Awake), ~BindingFlags.Default), typeof(XetHandler).GetMethod(nameof(CombatManager_Awake), ~BindingFlags.Default));
            IDetour hook3 = new Hook(typeof(OverworldManagerBG).GetMethod(nameof(OverworldManagerBG.Awake), ~BindingFlags.Default), typeof(XetHandler).GetMethod(nameof(OverworldManagerBG_Awake), ~BindingFlags.Default));
        }

        public static void MainMenuController_Start(Action<MainMenuController> orig, MainMenuController self)
        {
            orig(self);
            LoadedAssetsHandler.GetCharacter("Xet_CH").characterSprite = Small;//change this to call for your character, for instance Bob_CH 
        }

        public static void CombatManager_Awake(Action<CombatManager> orig, CombatManager self)
        {
            LoadedAssetsHandler.GetCharacter("Xet_CH").characterSprite = Large;//change this to call for your character
            orig(self);
        }

        public static void OverworldManagerBG_Awake(Action<OverworldManagerBG> orig, OverworldManagerBG self)
        {
            LoadedAssetsHandler.GetCharacter("Xet_CH").characterSprite = Small;//change this to call for your character
            orig(self);
        }
    }
}
