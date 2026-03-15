using BrutalAPI;
using DG.Tweening;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class NeedlesHandler
    {
        public static Color color;
        public static string[] names;
        public static void Setup()
        {
            color = Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Positive);
            names = ["A Hundred Needles", "A Thousand Needles", "A Million Needles", "A Trillion Needles"];

            IDetour hook = new Hook(typeof(TooltipLayout).GetMethod(nameof(TooltipLayout.DelayShow), ~BindingFlags.Default), typeof(NeedlesHandler).GetMethod(nameof(TooltipLayout_DelayShow), ~BindingFlags.Default));
        }

        public static void TooltipLayout_DelayShow(Action<TooltipLayout, string, string, string> orig, TooltipLayout self, string content, string header, string extraContent)
        {
            if (names.Contains(header))
            {
                extraContent = "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + "Current Turn: " + (CombatManager.Instance._stats.TurnsPassed + 1).ToString() + "</color>";
            }

            orig(self, content, header, extraContent);
        }
    }
}
