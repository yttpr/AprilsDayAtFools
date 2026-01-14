using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{

    public static class RotcoreHandler
    {
        public static Sprite[] Sprites;
        public static Sprite[] _backs;
        public static void TryUpdateLookAnimation(Action<CharacterInFieldLayout> orig, CharacterInFieldLayout self)
        {
            foreach (CharacterCombat characterCombat in CombatManager.Instance._stats.CharactersOnField.Values)
            {
                if (characterCombat.Character.name == "Rotcore_CH" && characterCombat.ID == self.CharacterID)
                {
                    self.CharacterSprite = Sprites.GetRandom();
                    self.CharacterBackSprite = _backs.GetRandom();
                }      
            }
            orig(self);
        }
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(CharacterInFieldLayout).GetMethod(nameof(CharacterInFieldLayout.TryUpdateLookAnimation), ~BindingFlags.Default), typeof(RotcoreHandler).GetMethod(nameof(TryUpdateLookAnimation), ~BindingFlags.Default));
            Sprites = [
                ResourceLoader.LoadSprite("RotcoreFront1.png"),
                ResourceLoader.LoadSprite("RotcoreFront2.png"),
                ResourceLoader.LoadSprite("RotcoreFront3.png"),
                ];
            _backs = [
                ResourceLoader.LoadSprite("RotcoreBack0.png"),
                ResourceLoader.LoadSprite("RotcoreBack0.png"),
                ResourceLoader.LoadSprite("RotcoreBack0.png"),
                ResourceLoader.LoadSprite("RotcoreBack1.png"),
                ];
        }
    }
}
