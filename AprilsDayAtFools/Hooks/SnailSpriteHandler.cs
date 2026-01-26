using BrutalAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class SnailSpriteHandler
    {
        public static string Trigger => "Snail_Sprites";
        public static string Maw => "Snail_Maw_A";
        public static string Hand => "Snail_Hand_A";

        public static Dictionary<ManaColorSO, Sprite[]> Sprites;

        public static void Setup()
        {
            Sprites = new Dictionary<ManaColorSO, Sprite[]>();
            //none, maw only, hand only, both, back
            Texture2D alien = ResourceLoader.LoadTexture("SnailSheet.png");
            ManaColorSO[] types = [Pigments.Green, Pigments.Red, Pigments.Blue, Pigments.Yellow, Pigments.Purple, Pigments.Grey];
            int row = 0;
            foreach (ManaColorSO mana in types)
            {
                List<Sprite> list = [];
                int column = 0;
                for (int i = 0; i < 5; i++)
                {
                    list.Add(Sprite.Create(alien, new Rect(column, row, 64, 64), new Vector2(0.5f, 0.5f), 32));
                    column += 64;
                }
                Sprites.Add(mana, list.ToArray());
                row += 64;
            }

            IDetour hook = new Hook(typeof(CharacterCombat).GetMethod(nameof(CharacterCombat.DefaultPassiveAbilityInitialization), ~BindingFlags.Default), typeof(BlockFromShops).GetMethod(nameof(CharacterCombat_Initialize), ~BindingFlags.Default));
            NotificationHook.AddAction(NotifCheck);
        }

        public static Sprite[] GetByColor(ManaColorSO mana)
        {
            if (Sprites.ContainsKey(mana)) return Sprites[mana];
            return Sprites[Pigments.Green];
        }
        public static void NotifCheck(string name, object sender, object args)
        {
            if (sender is CharacterCombat chara && chara.Character.name == IDs.Snail)
            {
                if (name == Trigger || name == TriggerCalls.OnHealthColorChanged.ToString())
                {
                    Sprite[] sprites = GetByColor(chara.HealthColor);
                    Sprite front = sprites[0];
                    Sprite back = sprites[4];

                    bool maw = chara.SimpleGetStoredValue(Maw) > 0;
                    bool hand = chara.SimpleGetStoredValue(Hand) > 0;

                    if (maw && hand) front = sprites[3];
                    else if (hand) front = sprites[2];
                    else if (maw) front = sprites[1];

                    CombatManager.Instance.AddUIAction(new CharaSetSpritesUIAction(chara.ID, front, back));
                }
            }
        }
        public static void CharacterCombat_Initialize(Action<CharacterCombat> orig, CharacterCombat self)
        {
            orig(self);
            if (self.Character.name == IDs.Snail)
            {
                Sprite[] sprites = GetByColor(self.HealthColor);
                Sprite front = sprites[0];
                Sprite back = sprites[4];

                bool maw = self.SimpleGetStoredValue(Maw) > 0;
                bool hand = self.SimpleGetStoredValue(Hand) > 0;

                if (maw && hand) front = sprites[3];
                else if (hand) front = sprites[2];
                else if (maw) front = sprites[1];

                CombatManager.Instance.AddUIAction(new CharaSetSpritesUIAction(self.ID, front, back));
            }
        }
    }

    public class CharaSetSpritesUIAction : CombatAction
    {
        public int _id;

        public Sprite _front;
        public Sprite _back;

        public CharaSetSpritesUIAction(int id, Sprite front, Sprite back)
        {
            _id = id;
            _front = front;
            _back = back;
        }

        public static void SetCharaSprites(CombatStats stats, int chara, Sprite front, Sprite back)
        {
            if (stats.combatUI._charactersInCombat.TryGetValue(chara, out var value))
            {
                if (value.Portrait == front) return;

                value.Portrait = front;
                value.BackPortrait = back;
                stats.combatUI._characterZone.SetCharacterSprites(value.FieldID, value.Portrait, value.BackPortrait);
                stats.combatUI.TryUpdateCharacterIDInformation(chara);
            }
        }
        public override IEnumerator Execute(CombatStats stats)
        {
            SetCharaSprites(stats, _id, _front, _back);
            yield break;
        }
    }
    public class SnailSpritesTriggerEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            CombatManager.Instance.PostNotification(SnailSpriteHandler.Trigger, caster, null);
            return true;
        }
    }
}
