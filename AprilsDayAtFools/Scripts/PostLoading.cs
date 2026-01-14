using MonoMod.RuntimeDetour;
using System;
using UnityEngine;
using System.Reflection;


namespace AprilsDayAtFools
{
    public static class PostLoading
    {
        public static void Setup()
        {
            IDetour postsaveHook = new Hook(typeof(MainMenuController).GetMethod(nameof(MainMenuController.LoadLanguage), ~BindingFlags.Default), typeof(PostLoading).GetMethod(nameof(ProcessGameStart), ~BindingFlags.Default));
            IDetour prestartHook = new Hook(typeof(MainMenuController).GetMethod(nameof(MainMenuController.Start), ~BindingFlags.Default), typeof(PostLoading).GetMethod(nameof(ProcessPreStart), ~BindingFlags.Default));
            Called = false;
        }

        static bool Called;
        public static void ProcessGameStart(Action<MainMenuController> orig, MainMenuController self)
        {
            if (Called)
            {
                orig(self);
                return;
            }
            Called = true;

            Class1.PCall(ItemPostLoader.Load);

            orig(self);
        }
        public static void ProcessPreStart(Action<MainMenuController> orig, MainMenuController self)
        {
            if (Called)
            {
                orig(self);
                return;
            }

            Class1.PCall(MarchItems.Add);
            Class1.PCall(MarchAchievements.Add);

            orig(self);
        }
    }
}
