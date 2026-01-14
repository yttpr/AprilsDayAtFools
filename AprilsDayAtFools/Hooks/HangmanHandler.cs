using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class HangmanHandler
    {
        public static Sprite[] _hide;
        public static Sprite[] Sprites
        {
            get
            {
                if (_hide == null)
                {
                    _hide = new Sprite[]
                    {
                        ResourceLoader.LoadSprite("HangmanFront0.png"),
                        ResourceLoader.LoadSprite("HangmanFront1.png"),
                        ResourceLoader.LoadSprite("HangmanFront2.png")
                    };
                    List<Sprite> backs = new List<Sprite>();
                    Sprite yeah = ResourceLoader.LoadSprite("HangmanBack.png");
                    for (int i = 0; i < _hide.Length; i++) backs.Add(yeah);
                    _backs = backs.ToArray();
                }
                return _hide;
            }
        }
        public static Sprite[] _backs;
        public static string Value => "Hangman_CH";
        public static void TryUpdateLookAnimation(Action<CharacterInFieldLayout> orig, CharacterInFieldLayout self)
        {
            foreach (CharacterCombat characterCombat in CombatManager.Instance._stats.CharactersOnField.Values)
            {
                if (characterCombat.Character.name == "Hangman_CH" && characterCombat.ID == self.CharacterID)
                    self.CharacterSprite = Sprites.GetRandom();
            }
            orig(self);
        }
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(CharacterInFieldLayout).GetMethod(nameof(CharacterInFieldLayout.TryUpdateLookAnimation), ~BindingFlags.Default), typeof(HangmanHandler).GetMethod(nameof(TryUpdateLookAnimation), ~BindingFlags.Default));
        }
    }
}
