using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class SlidingHandler
    {
        public static string Type => "ADAF_SlidingOverworld_Type";
        public static List<int> IDs = [];

        public static List<Sprite> Sliders = [];
        public static void Setup()
        {
            IDs = [];
            Sliders = [];
            IDetour hook1 = new Hook(typeof(OverworldCharactersManager).GetMethod(nameof(OverworldCharactersManager.SetPartyCharacters), ~BindingFlags.Default), typeof(SlidingHandler).GetMethod(nameof(OverworldCharactersManager_SetPartyCharacters), ~BindingFlags.Default));
            IDetour hook2 = new Hook(typeof(OverworldCharactersManager).GetMethod(nameof(OverworldCharactersManager.MoveCharacters), ~BindingFlags.Default), typeof(SlidingHandler).GetMethod(nameof(OverworldCharactersManager_MoveCharacters), ~BindingFlags.Default));
            IDetour hook3 = new Hook(typeof(CharacterInOverworld).GetMethod(nameof(CharacterInOverworld.SetMoveAnimation), ~BindingFlags.Default), typeof(SlidingHandler).GetMethod(nameof(CharacterInOverworld_SetMoveAnimation), ~BindingFlags.Default));
        }

        public static void AddCharacter(CharacterSO chara)
        {
            Sliders.Add(chara.characterOWSprite);
        }

        public static void OverworldCharactersManager_SetPartyCharacters(Action<OverworldCharactersManager, IMinimalCharacterInfo[]> orig, OverworldCharactersManager self, IMinimalCharacterInfo[] characters)
        {
            IDs = [];
            orig(self, characters);
            foreach (IMinimalCharacterInfo minimalCharacterInfo in characters)
            {
                if (minimalCharacterInfo == null)
                {
                    continue;
                }
                if (minimalCharacterInfo.Character.unitTypes != null && minimalCharacterInfo.Character.unitTypes.Contains(Type))
                {
                    IDs.Add(minimalCharacterInfo.PartySlot);
                }
            }
        }
        public static void OverworldCharactersManager_MoveCharacters(Action<OverworldCharactersManager, bool> orig, OverworldCharactersManager self, bool moving)
        {
            orig(self, moving);
            foreach (int id in IDs)
            {
                if (id >= self._movableCharacters.Length) continue;
                self._movableCharacters[id].SetMoveAnimation(false);
            }
        }
        public static void CharacterInOverworld_SetMoveAnimation(Action<CharacterInOverworld, bool> orig, CharacterInOverworld self, bool active)
        {
            if (Sliders.Contains(self._renderer.sprite)) active = false;
            orig(self, active);
        }
    }
}
