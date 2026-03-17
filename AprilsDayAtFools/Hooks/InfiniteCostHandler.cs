using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using System.Reflection;

namespace AprilsDayAtFools
{
    public static class InfiniteCostHandler
    {
        public static string Credits => "Harmony Patches stolen from SpecialAPI. thank u specialAPI.";
        public static void Setup()
        {
            Harmony instance = new Harmony("ADAFmod_Spapi_infinitecostpatches");
            instance.Patch(typeof(CombatVisualizationController).GetMethod(nameof(CombatVisualizationController.TrySetUpCostInformation)),
                new HarmonyMethod(typeof(InfiniteCostHandler).GetMethod(nameof(UnlimitedCostSlots_AttackButton_Prefix))));
            instance.Patch(typeof(AttackCostLayout).GetMethod(nameof(AttackCostLayout.SetSlotActivity), ~BindingFlags.Default), postfix:
                new HarmonyMethod(typeof(InfiniteCostHandler).GetMethod(nameof(UnlimitedCostSlots_AttackButton_DeactivateNewSlots_Postfix))));
            instance.Patch(typeof(AttackSlotLayout).GetMethod(nameof(AttackSlotLayout.FillCostInfo), ~BindingFlags.Default),
                new HarmonyMethod(typeof(InfiniteCostHandler).GetMethod(nameof(UnlimitedCostSlots_AbilitySlot_Prefix))));
            instance.Patch(typeof(Info_AttackLayout).GetMethod(nameof(Info_AttackLayout.SetInformation), ~BindingFlags.Default),
                new HarmonyMethod(typeof(InfiniteCostHandler).GetMethod(nameof(UnlimitedCostSlots_InfoUI_Prefix))));
        }
        public static void UnlimitedCostSlots_AttackButton_Prefix(CombatVisualizationController __instance, ManaColorSO[] slotCost)
        {
            if (__instance._characterCost == null || __instance._characterCost._costSlots == null || slotCost == null)
                return;

            if (slotCost.Length <= __instance._costBarInfo.Length)
                return;

            var toAdd = slotCost.Length - __instance._costBarInfo.Length;
            var newCostSlots = __instance._characterCost._costSlots.ToList();

            for (var i = 0; i < toAdd; i++)
            {
                var stuff = Object.Instantiate(newCostSlots[0].gameObject, newCostSlots[0].transform.parent).GetComponent<ManaSlotLayout>();

                stuff.SetManaSlotIDs(ManaBarType.Cost, newCostSlots.Count);
                stuff.transform.SetAsFirstSibling(); // Attack button cost slots are in opposite order

                newCostSlots.Add(stuff);
            }

            __instance._costBarInfo = [.. __instance._costBarInfo, .. new CostSlotUIInfo[toAdd].Select(x => new CostSlotUIInfo())];
            __instance._characterCost._costSlots = [.. newCostSlots];
            __instance._characterCost.CurrentCost = [.. __instance._characterCost.CurrentCost, .. new ManaColorSO[toAdd].Populate(null)];
        }
        public static void UnlimitedCostSlots_AttackButton_DeactivateNewSlots_Postfix(AttackCostLayout __instance, int index, bool enabled)
        {
            if (index < 6)
                return;

            // Hiding attack button slots doesn't deactivate their gameObjects, deactivate them manually so that it doesn't cause weird scaling for abilities not affected by unlimited costs
            __instance._costSlots[index].gameObject.SetActive(enabled);
        }
        public static void UnlimitedCostSlots_AbilitySlot_Prefix(AttackSlotLayout __instance, ManaColorSO[] costs)
        {
            if (__instance._PigmentCosts == null || costs == null)
                return;

            if (__instance._PigmentCosts.Length >= costs.Length)
                return;

            var toAdd = costs.Length - __instance._PigmentCosts.Length;
            var newCostSlots = __instance._PigmentCosts.ToList();

            for (var i = 0; i < toAdd; i++)
            {
                var stuff = Object.Instantiate(newCostSlots[0].gameObject, newCostSlots[0].transform.parent).GetComponent<Image>();

                newCostSlots.Add(stuff);
            }

            __instance._PigmentCosts = [.. newCostSlots];
        }
        public static void UnlimitedCostSlots_InfoUI_Prefix(Info_AttackLayout __instance, ManaColorSO[] cost)
        {
            if (__instance._costHolders == null || cost == null)
                return;

            if (__instance._costHolders.Length >= cost.Length)
                return;

            var toAdd = cost.Length - __instance._costHolders.Length;

            var newCostSlots = __instance._costHolders.ToList();
            var newCostImages = __instance._costImages.ToList();

            for (var i = 0; i < toAdd; i++)
            {
                var stuff = Object.Instantiate(newCostSlots[0].gameObject, newCostSlots[0].transform.parent);

                stuff.transform.SetAsFirstSibling(); // Attack button cost slots are in opposite order

                // Add both the new cost object as well as the new cost image to the arrays
                newCostSlots.Add(stuff);
                newCostImages.Add(stuff.GetComponentInChildren<Image>());
            }

            __instance._costHolders = [.. newCostSlots];
            __instance._costImages = [.. newCostImages];
        }
    }
}
