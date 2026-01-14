using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class SaturnHandler
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
                        ResourceLoader.LoadSprite("SaturnFront.png"),
                    };
                    List<Sprite> backs = new List<Sprite>();
                    Sprite yeah = ResourceLoader.LoadSprite("SaturnBack.png");
                    for (int i = 0; i < _hide.Length; i++) backs.Add(yeah);
                    _backs = backs.ToArray();
                }
                return _hide;
            }
        }
        public static Sprite[] _backs;
        public static void TryUpdateLookAnimation(Action<CharacterInFieldLayout> orig, CharacterInFieldLayout self)
        {
            foreach (CharacterCombat characterCombat in CombatManager.Instance._stats.CharactersOnField.Values)
            {
                if (characterCombat.Character.name == "Saturn_CH" && characterCombat.ID == self.CharacterID)
                {
                    self.CharacterSprite = Sprites[0];

                    CombatVisualizationController ui = CombatManager.Instance._stats.combatUI;
                    if (ui._charactersInCombat.TryGetValue(characterCombat.ID, out var value))
                    {
                        value.TrySetExtraSprites(IDs.SaturnDefault);
                    }
                }
            }
            orig(self);
        }
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(CharacterInFieldLayout).GetMethod(nameof(CharacterInFieldLayout.TryUpdateLookAnimation), ~BindingFlags.Default), typeof(SaturnHandler).GetMethod(nameof(TryUpdateLookAnimation), ~BindingFlags.Default));
        }
    }
}
