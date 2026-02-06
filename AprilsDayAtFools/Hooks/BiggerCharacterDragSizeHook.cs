using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace AprilsDayAtFools
{
    public static class BiggerCharacterDragSizeHook
    {
        public static string[] Names;
        public static void Setup()
        {
            Names = ["MedAnimController", "XetAnimController2", "QualiaAnimator"];
            IDetour hook = new Hook(typeof(CombatPointerLayout).GetMethod(nameof(CombatPointerLayout.Dragging), ~BindingFlags.Default), typeof(BiggerCharacterDragSizeHook).GetMethod(nameof(CombatPointerLayout_Dragging), ~BindingFlags.Default));
        }

        public static IEnumerator CombatPointerLayout_Dragging(Func<CombatPointerLayout, Image, IEnumerator> orig, CombatPointerLayout self, Image dragItem)
        {
            if (dragItem.transform.parent.parent.GetComponent<Animator>() != null && Names.Contains(dragItem.transform.parent.parent.GetComponent<Animator>().runtimeAnimatorController.name))
            {
                //Debug.Log(dragItem.transform.parent.parent.GetComponent<Animator>().name);
                //Debug.Log(dragItem.gameObject.GetComponent<RectTransform>().rect.size);
                //Debug.Log("IM TRYING");
                return self.Custom_Dragging(dragItem, 1.5f);
            }
            else
                return orig(self, dragItem);
        }

        public static IEnumerator Custom_Dragging(this CombatPointerLayout self, Image dragItem, float mult)
        {
            self.IsDragging = true;
            self._dragIsActive = true;
            Vector2 oldMousePosition = self._rawMousePosition;
            GameObject currentDraggedObject = dragItem.gameObject;
            RectTransform component = dragItem.GetComponent<RectTransform>();
            self._pointerTransform.sizeDelta = component.rect.size * mult;
            self._pointerTransform.position = component.position;
            self._pointerImage.enabled = true;
            while (self._dragIsActive && currentDraggedObject.activeInHierarchy)
            {
                self._pointerImage.sprite = dragItem.sprite;
                self._pointerTransform.anchoredPosition += (self._rawMousePosition - oldMousePosition) / self._canvas.scaleFactor;
                oldMousePosition = self._rawMousePosition;
                yield return null;
            }
            self._pointerImage.enabled = false;
            self._dragIsActive = false;
            self.IsDragging = false;
        }
    }
}
