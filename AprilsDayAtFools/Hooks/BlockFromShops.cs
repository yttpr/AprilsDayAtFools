using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AprilsDayAtFools
{
    public static class BlockFromShops
    {
        public static List<string> Fools;

        public static void Setup()
        {
            IDetour hook = new Hook(typeof(ZoneBGDataBaseSO).GetMethod(nameof(ZoneBGDataBaseSO.TryGetNewFoolsCharacter), ~BindingFlags.Default), typeof(BlockFromShops).GetMethod(nameof(ZoneBGDataBaseSO_TryGetNewFoolsCharacter), ~BindingFlags.Default));
            Fools = [];
        }
        public static void Add(CharacterSO chara)
        {
            if (Fools == null) Fools = [];
            Fools.Add(chara.name);
        }
        public static CharacterSO ZoneBGDataBaseSO_TryGetNewFoolsCharacter(Func<ZoneBGDataBaseSO, CharacterSO> orig, ZoneBGDataBaseSO self)
        {
            List<string> removing = [];
            foreach (string name in Fools)
            {
                if (self._uncheckedCharacters.Contains(name))
                {
                    self._uncheckedCharacters.Remove(name);
                    removing.Add(name);
                }
            }

            CharacterSO ret = orig(self);

            self._uncheckedCharacters.AddRange(removing);

            return ret;
        }
    }
}
