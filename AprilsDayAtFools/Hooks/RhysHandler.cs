using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class RhysHandler
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
                        ResourceLoader.LoadSprite("RhysFront1.png"),
                        ResourceLoader.LoadSprite("RhysFront2.png")
                    };
                    List<Sprite> backs = new List<Sprite>();
                    Sprite yeah = ResourceLoader.LoadSprite("RhysBack.png");
                    for (int i = 0; i < _hide.Length; i++) backs.Add(yeah);
                    _backs = backs.ToArray();
                }
                return _hide;
            }
        }
        public static Sprite[] _backs;
        public static string Value => "Rhys_CH";
        public static void TryUpdateLookAnimation(Action<CharacterInFieldLayout> orig, CharacterInFieldLayout self)
        {
            foreach (CharacterCombat characterCombat in CombatManager.Instance._stats.CharactersOnField.Values)
            {
                if (characterCombat.Character.name == "Rhys_CH" && characterCombat.ID == self.CharacterID && characterCombat.SimpleGetStoredValue(Value) > 0)
                    self.CharacterSprite = Sprites.GetRandom();
            }
            orig(self);
        }
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(CharacterInFieldLayout).GetMethod(nameof(CharacterInFieldLayout.TryUpdateLookAnimation), ~BindingFlags.Default), typeof(RhysHandler).GetMethod(nameof(TryUpdateLookAnimation), ~BindingFlags.Default));
        }
    }
}
