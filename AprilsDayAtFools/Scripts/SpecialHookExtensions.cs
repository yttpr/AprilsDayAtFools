using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using UnityEngine;
using MonoMod.RuntimeDetour;


namespace AprilsDayAtFools
{
    public static class SpecialEffectExtensions
    {
        static MethodReference WillApplyDamage;
        public static bool ContainsWillApplyDamage(this EffectSO effect)
        {
            if (effect == null || effect.Equals(null)) return false;

            try
            {
                string pathTo = effect.GetType().Assembly.Location;
                AssemblyDefinition that = AssemblyDefinition.ReadAssembly(pathTo);

                MethodReference startingpoint = that.MainModule.ImportReference(effect.GetType().GetMethod(nameof(effect.PerformEffect)));

                if (WillApplyDamage == null)
                {
                    WillApplyDamage = AssemblyDefinition.ReadAssembly(typeof(IUnit).Assembly.Location)
                        .MainModule.ImportReference(typeof(IUnit).GetMethod(nameof(IUnit.WillApplyDamage)));
                }

                return startingpoint
                    //.GetOriginalMethod() // jump to original (for generics e.g.)
                    .Resolve()           // get the definition from the IL image
                    .Body.Instructions.Cast<Instruction>()
                    .Any(i => i.OpCode == OpCodes.Callvirt && i.Operand == (WillApplyDamage));
            }
            catch
            {
                Debug.LogError("FAILED TO MONO.CECIL CHECK IF WILLAPPLYDAMAGE FOR EFFECT: ");
                Debug.LogError(effect);
                Debug.LogError("OF POSSIBLE TYPE: ");
                if (effect != null) Debug.LogError(effect.GetType());
                else Debug.Log("null??");
                return false;
            }
        }


        public static bool Matches(this object first, object second)
        {
            if (first == second) return true;
            if (first.Equals(second)) return true;

            if (first == null || first.Equals(null) || second == null || second.Equals(null)) return false;

            if (first.GetType() != second.GetType()) return false;

            try
            {
                foreach (FieldInfo field in first.GetType().GetFields())
                {
                    if (field.GetValue(first) == field.GetValue(second) || field.GetValue(first).Equals(field.GetValue(second))) continue;

                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        public static void Setup()
        {
            IDetour hook1 = new Hook(typeof(EffectAction).GetMethod(nameof(EffectAction.Execute), ~BindingFlags.Default), typeof(SpecialEffectExtensions).GetMethod(nameof(EffectAction_Execute), ~BindingFlags.Default));
            IDetour hook2 = new Hook(typeof(EffectInfo).GetMethod(nameof(EffectInfo.StartEffect), ~BindingFlags.Default), typeof(SpecialEffectExtensions).GetMethod(nameof(EffectInfo_StartEffect), ~BindingFlags.Default));

            Action_Pre = [];
            Action_Post = [];

            Effect_Pre = [];
            Effect_Post = [];
        }

        //EffectAction hook
        public static int ActionsTracked;
        public static EffectAction Tracking;
        public static IEnumerator EffectAction_Execute(Func<EffectAction, CombatStats, IEnumerator> orig, EffectAction self, CombatStats stats)
        {
            //do some preliminary checks
            ActionsTracked = 0;
            Tracking = self;

            //preactions
            foreach (Action<EffectAction, CombatStats> running in Action_Pre)
            {
                running(self, stats);
            }

            //run all effects
            IEnumerator ret = orig(self, stats);

            //postactions
            foreach (Action<EffectAction, CombatStats> running in Action_Post)
            {
                running(self, stats);
            }

            //do some resets
            ActionsTracked = 0;
            Tracking = null;

            return ret;
        }

        public static int EffectInfo_StartEffect(Func<EffectInfo, CombatStats, IUnit, TargetSlotInfo[], bool, int, int> orig, EffectInfo self, CombatStats stats, IUnit caster, TargetSlotInfo[] possibleTargets, bool areTargetSlots, int previousExitValue)
        {
            //do some preliminary checking

            //preactions
            foreach (Action<EffectInfo, CombatStats, IUnit, TargetSlotInfo[], bool, int> running in Effect_Pre)
            {
                running(self, stats, caster, possibleTargets, areTargetSlots, previousExitValue);
            }

            //run effect
            int ret = orig(self, stats, caster, possibleTargets, areTargetSlots, previousExitValue);

            //postaction
            foreach (Action<EffectInfo, CombatStats, IUnit, TargetSlotInfo[], bool, int> running in Effect_Post)
            {
                running(self, stats, caster, possibleTargets, areTargetSlots, previousExitValue);
            }

            //do some post tracking
            if (self.InCurrentAction()) ActionsTracked++;

            return ret;
        }

        //add action shit
        public static List<Action<EffectAction, CombatStats>> Action_Pre;
        public static List<Action<EffectAction, CombatStats>> Action_Post;

        public static List<Action<EffectInfo, CombatStats, IUnit, TargetSlotInfo[], bool, int>> Effect_Pre;
        public static List<Action<EffectInfo, CombatStats, IUnit, TargetSlotInfo[], bool, int>> Effect_Post;

        public static void AddAction(Action<EffectAction, CombatStats> action, bool pre = false)
        {
            if (pre) Action_Pre.Add(action); else Action_Post.Add(action);
        }
        public static void AddAction(Action<EffectInfo, CombatStats, IUnit, TargetSlotInfo[], bool, int> action, bool pre = false)
        {
            if (pre) Effect_Pre.Add(action); else Effect_Post.Add(action);
        }

        public static bool InCurrentAction(this EffectInfo self)
        {
            if (Tracking != null && !Tracking.Equals(null) && Tracking._effects != null && !Tracking._effects.Equals(null))
            {
                return Tracking._effects.Length > ActionsTracked && Tracking._effects[ActionsTracked] == self;
            }

            return false;
        }
    }
}
