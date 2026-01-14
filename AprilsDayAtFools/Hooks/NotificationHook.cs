using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AprilsDayAtFools
{

    public static class NotificationHook
    {
        public static List<Action<string, object, object>> BeforeActions;
        public static List<Action<string, object, object>> AfterActions;
        public static void AddAction(Action<string, object, object> action, bool before = false)
        {
            if (BeforeActions == null) BeforeActions = new List<Action<string, object, object>>();
            if (AfterActions == null) AfterActions = new List<Action<string, object, object>>();
            if (before) BeforeActions.Add(action);
            else AfterActions.Add(action);
        }
        public static void PostNotification(Action<CombatManager, string, object, object> orig, CombatManager self, string notificationName, object sender, object args)
        {
            if (BeforeActions != null) foreach (Action<string, object, object> action in BeforeActions) action(notificationName, sender, args);
            orig(self, notificationName, sender, args);
            if (AfterActions != null) foreach (Action<string, object, object> action in AfterActions) action(notificationName, sender, args);
        }
        public static void Setup()
        {
            IDetour hook = new Hook(typeof(CombatManager).GetMethod(nameof(CombatManager.PostNotification), ~BindingFlags.Default), typeof(NotificationHook).GetMethod(nameof(PostNotification), ~BindingFlags.Default));
        }
    }
}
