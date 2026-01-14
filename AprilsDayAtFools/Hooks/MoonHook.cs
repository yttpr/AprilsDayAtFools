using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class MoonHandler
    {
        public static void Setup()
        {
            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool(IDs.Isolation, "Isolation -{0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Negative));
            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool(IDs.Wallowing, "Wallowing -{0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Negative));
            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool(IDs.Lighter, "Damage will not Decrease this Turn", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Positive));

            NotificationHook.AddAction(NotifCheck);
        }
        public static void TickDown(IUnit unit)
        {
            if (unit is CharacterCombat chara)
            {
                if (unit.SimpleGetStoredValue(IDs.Lighter) > 0)
                {
                    return;
                }
                foreach (CombatAbility ability in chara.CombatAbilities)
                {
                    //CHANGE THIS TO BE ALL OF THE POTENTIAL LEVEL NAMES
                    if (ability.ability.name == "Moon_Isolation_1_A" ||
                        ability.ability.name == "Moon_Isolation_2_A" ||
                        ability.ability.name == "Moon_Isolation_3_A" ||
                        ability.ability.name == "Moon_Isolation_4_A")
                    {
                        unit.SimpleSetStoredValue(IDs.Isolation, unit.SimpleGetStoredValue(IDs.Isolation) + 2);
                        if (unit.SimpleGetStoredValue(IDs.Isolation) >= ability.ability.effects[0].entryVariable) unit.SimpleSetStoredValue(IDs.Isolation, 0);
                    }
                    if (ability.ability.name == "Moon_Wallow_1_A" ||
                        ability.ability.name == "Moon_Wallow_2_A" ||
                        ability.ability.name == "Moon_Wallow_3_A" ||
                        ability.ability.name == "Moon_Wallow_4_A")
                    {
                        unit.SimpleSetStoredValue(IDs.Wallowing, unit.SimpleGetStoredValue(IDs.Wallowing) + 1);
                        if (unit.SimpleGetStoredValue(IDs.Wallowing) >= ability.ability.effects[1].entryVariable) unit.SimpleSetStoredValue(IDs.Wallowing, 0);
                    }
                }
            }
        }
        public static void Reset(IUnit unit) => unit.SimpleSetStoredValue(IDs.Lighter, 0);
        public static void NotifCheck(string name, object sender, object args)
        {
            if (sender is IUnit unit)
            {
                if (name == TriggerCalls.OnTurnFinished.ToString() || name == TriggerCalls.OnBeingDamaged.ToString()) TickDown(unit);
                if (name == TriggerCalls.OnTurnStart.ToString()) Reset(unit);
            }
        }
    }
}
