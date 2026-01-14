using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AprilsDayAtFools
{
    public static class WarmupHandler
    {
        public static void Setup()
        {
            UnitStoreData.CreateAndAdd_IntTooltip_UnitStoreDataToPool(IDs.Warmup, "Qarmuip +{0}", Misc.GetInGame_UITextColor(Misc.UITextColorIDs.Positive));

            NotificationHook.AddAction(NotifCheck);
        }
        public static void NotifCheck(string name, object sender, object args)
        {
            if (name == TriggerCalls.OnDidApplyDamage.ToString())
            {
                if (sender is IUnit unit && unit.IsUnitCharacter)
                {
                    foreach (CharacterCombat chara in CombatManager.Instance._stats.CharactersOnField.Values)
                    {
                        foreach (CombatAbility ability in chara.CombatAbilities)
                        {
                            if (ability.ability.name == "Wtmiyr_Warmup_1_A" || 
                                ability.ability.name == "Wtmiyr_Warmup_2_A" ||
                                ability.ability.name == "Wtmiyr_Warmup_3_A" ||
                                ability.ability.name == "Wtmiyr_Warmup_4_A")
                            {
                                chara.SimpleSetStoredValue(IDs.Warmup, chara.SimpleGetStoredValue(IDs.Warmup) + 1);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
