using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{

    public static class AnimatedIntentsHandler
    {
        public static void Setup()
        {
            IntentColor.Setup();

            IDetour hook1 = new Hook(typeof(TimelineIntentListLayout).GetMethod(nameof(TimelineIntentListLayout.SetInformation), ~BindingFlags.Default), typeof(AnimatedIntentsHandler).GetMethod(nameof(SetInformation), ~BindingFlags.Default));
            IDetour hook2 = new Hook(typeof(TargetIntentListLayout).GetMethod(nameof(TargetIntentListLayout.AddInformation), ~BindingFlags.Default), typeof(AnimatedIntentsHandler).GetMethod(nameof(AddInformation), ~BindingFlags.Default));
        }

        public static void SetInformation(Action<TimelineIntentListLayout, Sprite[], Color[]> orig, TimelineIntentListLayout self, Sprite[] icons, Color[] colors)
        {
            try
            {
                if (colors == null || icons == null || colors.Length <= 0)
                {
                    orig(self, icons, colors);
                    return;
                }
                if (colors.Contains(IntentColor._color))
                {
                    List<Sprite> animateSprites = new List<Sprite>();
                    List<Color> animateColors = new List<Color>();
                    bool animateThese = false;
                    int upTo = colors.Length;
                    for (int checkColor = 0; checkColor < colors.Length; checkColor++)
                    {
                        if (animateThese)
                        {
                            animateSprites.Add(icons[checkColor]);
                            animateColors.Add(colors[checkColor]);
                        }
                        if (colors[checkColor] == IntentColor._color || colors[checkColor].Equals(IntentColor._color))
                        {
                            animateThese = true;
                            upTo = checkColor;
                        }
                    }
                    while (self._intents.Count <= upTo) self.GenerateNewIntent();
                    for (int index = 0; index < self._intents.Count; ++index)
                    {
                        if (index < upTo)
                        {
                            ADAF_IntentLayoutAnimator[] array = self._intents[index].gameObject.GetComponents<ADAF_IntentLayoutAnimator>();
                            foreach (ADAF_IntentLayoutAnimator ain in array)
                            {
                                ain.enabled = false;
                                ain.IsActive = false;
                            }
                            self._intents[index].SetInformation(icons[index], colors[index]);
                            self._intents[index].SetActivation(true);
                        }
                        else if (index > upTo)
                        {
                            self._intents[index].SetActivation(false);
                        }
                        else
                        {
                            ADAF_IntentLayoutAnimator[] array = self._intents[index].gameObject.GetComponents<ADAF_IntentLayoutAnimator>();
                            foreach (ADAF_IntentLayoutAnimator ain in array)
                            {
                                ain.enabled = false;
                                ain.IsActive = false;
                            }
                            self._intents[index].SetInformation(icons[icons.Length - 1], colors[colors.Length - 1]);
                            self._intents[index].SetActivation(true);
                            ADAF_IntentLayoutAnimator grah = self._intents[index].gameObject.AddComponent<ADAF_IntentLayoutAnimator>();
                            grah.animate = self._intents[index];
                            grah.icons = animateSprites.ToArray();
                            grah.colors = animateColors.ToArray();
                            grah.IsActive = true;
                            grah.limit = 0.1f;
                        }
                    }
                }
                else
                {
                    orig(self, icons, colors);
                    foreach (TimelineIntentLayout lay in self._intents)
                    {
                        ADAF_IntentLayoutAnimator[] array = lay.gameObject.GetComponents<ADAF_IntentLayoutAnimator>();
                        foreach (ADAF_IntentLayoutAnimator ain in array)
                        {
                            ain.enabled = false;
                            ain.IsActive = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning("FallImageryHandler");
                UnityEngine.Debug.LogWarning(ex.ToString());
            }
        }
        public static void AddInformation(Action<TargetIntentListLayout, Sprite[], Color[]> orig, TargetIntentListLayout self, Sprite[] icons, Color[] colors)
        {
            if (colors.Contains(IntentColor._color))
            {
                if (self._unusedIntents.Count <= 0)
                    self.GenerateUnusedIntent();
                TargetIntentLayout targetIntentLayout = self._unusedIntents.Dequeue();
                targetIntentLayout.MoveToLast();
                targetIntentLayout.SetInformation(icons[icons.Length - 1], colors[colors.Length - 1]);
                targetIntentLayout.SetActivation(true);
                self._intentsInUse.Add(targetIntentLayout);
                foreach (ADAF_IntentLayoutAnimator old in targetIntentLayout.gameObject.GetComponents<ADAF_IntentLayoutAnimator>())
                {
                    old.IsActive = false;
                }

                List<Sprite> animateSprites = new List<Sprite>();
                List<Color> animateColors = new List<Color>();
                for (int i = 0; i < colors.Length; i++)
                {
                    if (colors[i] == IntentColor._color || colors[i].Equals(IntentColor._color)) continue;
                    animateSprites.Add(icons[i]);
                    animateColors.Add(colors[i]);
                }

                ADAF_IntentLayoutAnimator grah = targetIntentLayout.gameObject.AddComponent<ADAF_IntentLayoutAnimator>();
                grah.mutilate = targetIntentLayout;
                grah.icons = animateSprites.ToArray();
                grah.colors = animateColors.ToArray();
                grah.IsActive = true;
                grah.limit = 0.1f;
                //Debug.Log("TARGET INTENT ");
                //new IntentLayoutAnimator(targetIntentLayout, icons, colors);
            }
            else
            {
                orig(self, icons, colors);
                foreach (TargetIntentLayout lay in self._intentsInUse)
                {
                    ADAF_IntentLayoutAnimator[] array = lay.gameObject.GetComponents<ADAF_IntentLayoutAnimator>();
                    foreach (ADAF_IntentLayoutAnimator ain in array)
                    {
                        ain.enabled = false;
                        ain.IsActive = false;
                    }
                }
            }
        }

        public static List<ADAF_IntentLayoutAnimator> fullSet = new List<ADAF_IntentLayoutAnimator>();
        public static void Clear()
        {
            foreach (ADAF_IntentLayoutAnimator animator in fullSet)
            {
                if (animator.thread != null) animator.thread.Abort();
                animator.IsActive = false;
            }
            fullSet.Clear();
        }

        public class ADAF_IntentLayoutAnimator : MonoBehaviour
        {
            public TimelineIntentLayout animate;
            public TargetIntentLayout mutilate;

            public Sprite[] icons;
            public Color[] colors;

            public System.Threading.Thread thread;
            /*
            public IntentLayoutAnimator(TimelineIntentLayout target, Sprite[] Icons, Color[] Colors)
            {
                if (DoDebugs.MiscInfo) Debug.Log("timeline");
                animate = target;
                icons = Icons;
                colors = Colors;
                Thread newThread = new Thread(Animate);
                newThread.Start();
                thread = newThread;
                fullSet.Add(this);
            }
            public IntentLayoutAnimator(TargetIntentLayout target, Sprite[] Icons, Color[] Colors)
            {
                if (DoDebugs.MiscInfo) Debug.Log("target");
                mutilate = target;
                icons = Icons;
                colors = Colors;
                Thread newThread = new Thread(Mutilate);
                newThread.Start();
                thread = newThread;
                fullSet.Add(this);
            }
            */
            int currentSprite = -1;
            public bool IsActive = true;
            public void Animate()
            {
                while (animate != null && !animate.Equals(null) && animate.isActiveAndEnabled)
                {
                    try
                    {
                        IsActive = true;
                        System.Threading.Thread.Sleep(100);
                        int cap = Math.Min(icons.Length, colors.Length);
                        int index = UnityEngine.Random.Range(0, cap);
                        while (colors[index] == new Color(28f, 78f, 128f) || index == currentSprite) index = UnityEngine.Random.Range(0, cap);
                        animate.SetInformation(icons[index], colors[index]);
                        currentSprite = index;
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
                fullSet.Remove(this);
                IsActive = false;
            }
            public void Mutilate()
            {
                while (mutilate != null && !mutilate.Equals(null) && mutilate.isActiveAndEnabled)
                {
                    try
                    {
                        IsActive = true;
                        System.Threading.Thread.Sleep(100);
                        int cap = Math.Min(icons.Length, colors.Length);
                        int index = UnityEngine.Random.Range(0, cap);
                        while (colors[index] == new Color(28f, 78f, 128f) || index == currentSprite) index = UnityEngine.Random.Range(0, cap);
                        mutilate.SetInformation(icons[index], colors[index]);
                        currentSprite = index;
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
                fullSet.Remove(this);
                IsActive = false;
            }
            public float limit = 20f;
            public float time = 0f;
            public void Update()
            {
                //Debug.Log("Statr");
                if (!IsActive) return;
                //Debug.Log("Actiev");
                if (!RunFirstCheck || RedColor == null || PurpleColor == null || EnemyDamage == null || EnemyDamage.Length <= 0 || CharacterDamage == null || CharacterDamage.Length <= 0)
                {
                    CheckIsTrain(-1);
                    //Debug.Log("original trai check " + this);
                }
                try
                {
                    if (!fullSet.Contains(this))
                    {
                        fullSet.Add(this);
                        //Debug.Log("fullset addd " + fullSet.Count);
                        //Debug.Log(animate);
                        //Debug.Log(mutilate);
                    }
                }
                catch
                {
                    UnityEngine.Debug.LogError("failed to add self to fullset");
                    UnityEngine.Debug.LogError(this);
                }
                //if (mutilate != null && !mutilate.Equals(null) && mutilate.isActiveAndEnabled) { }
                //else if (animate != null && !animate.Equals(null) && animate.isActiveAndEnabled) { }
                //else { this.enabled = false; IsActive = false; }
                time += Time.deltaTime;
                if (time >= limit)
                {
                    try
                    {
                        Sprite[] icons = this.icons;
                        Color[] colors = this.colors;
                        if (ForceTrainColors)
                        {
                            if (HitAllies)
                            {
                                icons = EnemyDamage;
                                colors = new Color[icons.Length];
                                for (int i = 0; i < colors.Length; i++) colors[i] = RedColor;
                                //Debug.Log("enemy color");
                            }
                            else
                            {
                                icons = CharacterDamage;
                                colors = new Color[icons.Length];
                                for (int i = 0; i < colors.Length; i++) colors[i] = PurpleColor;
                                //Debug.Log("chara color");
                            }
                        }
                        time = 0f;
                        if (animate != null && !animate.Equals(null) && animate.isActiveAndEnabled)
                        {
                            int cap = Math.Min(icons.Length, colors.Length);
                            int index = UnityEngine.Random.Range(0, cap);
                            if (cap > 2 || (cap > 1 && ForceTrainColors))
                                while (colors[index] == new Color(28f, 78f, 128f) || index == currentSprite) index = UnityEngine.Random.Range(0, cap);
                            animate.SetInformation(icons[index], colors[index]);
                            currentSprite = index;
                            //Debug.Log("timeline");
                        }
                        if (mutilate != null && !mutilate.Equals(null) && mutilate.isActiveAndEnabled)
                        {
                            int cap = Math.Min(this.icons.Length, this.colors.Length);
                            int index = UnityEngine.Random.Range(0, cap);
                            if (cap > 2 || (cap > 1 && ForceTrainColors))
                                while (this.colors[index] == new Color(28f, 78f, 128f) || index == currentSprite) index = UnityEngine.Random.Range(0, cap);
                            mutilate.SetInformation(this.icons[index], this.colors[index]);
                            currentSprite = index;
                            //Debug.Log("target");
                        }
                    }
                    catch
                    {
                        UnityEngine.Debug.LogError("intent icon sprite changer FUCKING BROKE");
                    }
                }
            }

            public bool ForceTrainColors;
            public bool HitAllies;
            public static Color RedColor;
            public static Color PurpleColor;
            public static Sprite[] EnemyDamage;
            public static Sprite[] CharacterDamage;
            public bool RunFirstCheck = false;
            public void CheckIsTrain(int numb)
            {
                return;
            }
        }
    }
    public static class IntentColor
    {
        public static string Intent => "ADAF_AnimatedIntent_Identifier";
        public static Color _color => new Color(28f, 74f, 128f);
        public static void Setup()
        {
            Intents.CreateAndAddCustom_Basic_IntentToPool(Intent, ResourceLoader.LoadSprite("Pale.png"), _color);
        }
    }
}
