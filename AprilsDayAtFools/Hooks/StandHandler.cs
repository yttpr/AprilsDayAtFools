using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class StandHandler
    {
        public static void Setup()
        {
            NotificationHook.AddAction(NotifCheck);
            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool(IDs.Stand, "Party Member Cannot Move This Turn", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Positive));
        }
        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.CanSwap.ToString() || name == TriggerCalls.CanBeSwapped.ToString())
            {
                if (sender is IUnit unit && args is BooleanWithTriggerReference reference)
                {
                    if (unit.SimpleGetStoredValue(IDs.Stand) > 0) reference.value = false;
                }
            }
            if (name == TriggerCalls.OnTurnStart.ToString())
            {
                if (sender is IUnit unit && unit.SimpleGetStoredValue(IDs.Stand) > 0)
                    unit.SimpleSetStoredValue(IDs.Stand, 0);
                else return;
                if (sender is CharacterCombat chara)
                    CombatManager.Instance.AddUIAction(new CharacterUpdateVolatilesUIAction(chara.ID, chara.CanSwapNoTrigger, chara.CanUseAbilitiesNoTrigger, shouldPopUp: true));
            }
        }
    }
}
