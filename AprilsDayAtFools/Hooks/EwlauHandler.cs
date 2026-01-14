using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class EwlauHandler
    {
        public static void Setup()
        {
            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool(IDs.Ewlau, "Ewlau +{0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Positive));

            NotificationHook.AddAction(NotifCheck);
        }
        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnWillApplyDamage.ToString() && args is DamageDealtValueChangeException dealt)
            {
                int num = 0;
                if (sender is IUnit unit && unit.IsUnitCharacter)
                {
                    foreach (CharacterCombat chara in CombatManager.Instance._stats.CharactersOnField.Values)
                    {
                        num += chara.SimpleGetStoredValue(IDs.Ewlau);
                    }
                }
                else
                {
                    foreach (EnemyCombat enemy in CombatManager.Instance._stats.EnemiesOnField.Values)
                    {
                        num += enemy.SimpleGetStoredValue(IDs.Ewlau);
                    }
                }
                dealt.AddModifier(new AdditionValueModifier(true, num));
            }
        }
    }
}
