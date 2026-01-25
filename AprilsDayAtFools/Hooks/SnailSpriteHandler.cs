using BrutalAPI;
using System;
using System.Collections;
using System.Collections.Generic;
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



            NotificationHook.AddAction(NotifCheck);
        }

        public static Sprite[] GetByColor(ManaColorSO mana)
        {
            if (Sprites.ContainsKey(mana)) return Sprites[mana];
            return Sprites[Pigments.Green];
        }
        public static void NotifCheck(string name, object sender, object args)
        {
            if (sender is CharacterCombat chara)
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
