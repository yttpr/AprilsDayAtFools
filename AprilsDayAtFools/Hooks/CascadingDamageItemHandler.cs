using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

//also handles giving killers a DidDamage trigger with a target.
namespace AprilsDayAtFools
{
    public static class CascadingDamageItemHandler
    {
        public static TriggerCalls Call => (TriggerCalls)94723010;
        public static List<AdvancedDamageTempInfo> InfoList;
        public static void Setup()
        {
            NotificationHook.AddAction(NotifCheck);
        }
        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnBeingDamaged.ToString())
            {
                if (InfoList == null) InfoList = new List<AdvancedDamageTempInfo>();

                if (args is DamageReceivedValueChangeException reference)
                {
                    for (int i = InfoList.Count - 1; i >= 0; i--)
                        if (InfoList[i].Target == sender) InfoList.RemoveAt(i);

                    InfoList.Add(new AdvancedDamageTempInfo(reference.possibleSourceUnit, reference.damagedUnit, reference.directDamage, reference.damageTypeID));
                }
            }

            if (name == TriggerCalls.OnDirectDamaged.ToString())
            {
                if (InfoList == null) InfoList = new List<AdvancedDamageTempInfo>();

                for (int i = 0; i < InfoList.Count; i++)
                {
                    if (InfoList[i].Target == sender && InfoList[i].Direct)
                    {
                        AdvancedDamageTempInfo info = InfoList[i];
                        InfoList.RemoveAt(i);
                        AdvancedDamageTrigger.PostTrigger(info, sender, args);

                        if (info.Attacker == null) return;

                        CascadeSpecialBooleanReference check = new CascadeSpecialBooleanReference(false, info);
                        CombatManager.Instance.PostNotification(Call.ToString(), info.Attacker, check);
                        if (!check.value) return;

                        if (args is IntegerReference reference)
                        {
                            RunCascade(sender as IUnit, reference.value);
                        }

                        return;
                    }
                }
                AdvancedDamageTrigger.PostTrigger(true, sender, args);
            }
            if (name == TriggerCalls.OnIndirectDamaged.ToString())
            {
                if (InfoList == null) InfoList = new List<AdvancedDamageTempInfo>();

                for (int i = 0; i < InfoList.Count; i++)
                {
                    if (InfoList[i].Target == sender && !InfoList[i].Direct)
                    {
                        AdvancedDamageTempInfo info = InfoList[i];
                        InfoList.RemoveAt(i);
                        AdvancedDamageTrigger.PostTrigger(info, sender, args);

                        if (info.Attacker == null) return;

                        CascadeSpecialBooleanReference check = new CascadeSpecialBooleanReference(false, info);
                        CombatManager.Instance.PostNotification(Call.ToString(), info.Attacker, check);
                        if (!check.value) return;

                        if (args is IntegerReference reference)
                        {
                            RunCascade(sender as IUnit, reference.value);
                        }

                        return;
                    }
                }
                AdvancedDamageTrigger.PostTrigger(false, sender, args);
            }
        }

        public static void RunCascade(IUnit origin, int start)
        {
            SlotsCombat slots = CombatManager.Instance._stats.combatSlots;

            int left = origin.SlotID - 1;
            int right = origin.SlotID + origin.Size;

            for (int current = (int)Math.Floor((float)start / 2); current > 0; current = (int)Math.Floor((float)current / 2))
            {
                if (left >= 0 && left < 5)
                {
                    if (origin.IsUnitCharacter)
                    {
                        if (slots.CharacterSlots[left].HasUnit)
                        {
                            slots.CharacterSlots[left].Unit.Damage(current, null, "Basic", slots.CharacterSlots[left].SlotID - slots.CharacterSlots[left].Unit.SlotID, false, false, true);
                            left--;
                        }
                        else left = -1;
                    }
                    else
                    {
                        if (slots.EnemySlots[left].HasUnit)
                        {
                            slots.EnemySlots[left].Unit.Damage(current, null, "Basic", slots.EnemySlots[left].SlotID - slots.EnemySlots[left].Unit.SlotID, false, false, true);
                            left--;
                        }
                        else left = -1;
                    }
                }
                if (right >= 0 && right < 5)
                {
                    if (origin.IsUnitCharacter)
                    {
                        if (slots.CharacterSlots[right].HasUnit)
                        {
                            slots.CharacterSlots[right].Unit.Damage(current, null, "Basic", slots.CharacterSlots[right].SlotID - slots.CharacterSlots[right].Unit.SlotID, false, false, true);
                            right++;
                        }
                        else right = -1;
                    }
                    else
                    {
                        if (slots.EnemySlots[right].HasUnit)
                        {
                            slots.EnemySlots[right].Unit.Damage(current, null, "Basic", slots.EnemySlots[right].SlotID - slots.EnemySlots[right].Unit.SlotID, false, false, true);
                            right++;
                        }
                        else right = -1;
                    }
                }

                if ((left < 0 || left >= 5) && (right < 0 || right >= 5)) break;
            }
        }
    }

    public struct AdvancedDamageTempInfo
    {
        public IUnit Attacker;
        public IUnit Target;
        public bool Direct;
        public string Type;
        public AdvancedDamageTempInfo(IUnit attacker, IUnit target, bool direct, string type = "")
        {
            Attacker = attacker;
            Target = target;
            Direct = direct;
            Type = type;
        }
    }
    public class CascadeSpecialBooleanReference : BooleanReference
    {
        public AdvancedDamageTempInfo Info;
        public CascadeSpecialBooleanReference(bool entryValue, AdvancedDamageTempInfo info) : base(entryValue)
        {
            Info = info;
        }
    }
}
