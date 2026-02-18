using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace AprilsDayAtFools
{
    public class CasterConstrictedToggleEffect : EffectSO
    {
        public static string Value => "ADAF_ConstrictedToggleEffect";
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (caster.ContainsPassiveAbility("Constricting"))
            {
                if (caster.SimpleGetStoredValue(Value) <= 0)
                {
                    stats.combatSlots.DettachSlotStatusRestrictor("Constricted_ID", caster.SlotID, !caster.IsUnitCharacter, caster.Size);
                    caster.SimpleSetStoredValue(Value, 1);
                    exitAmount++;
                    //Debug.Log("disable");
                }
                else if (caster.SimpleGetStoredValue(Value) > 0)
                {
                    stats.combatSlots.ApplyFieldEffect(caster.SlotID, !caster.IsUnitCharacter, StatusField.Constricted, 0, 1, caster.Size);
                    caster.SimpleSetStoredValue(Value, 0);
                    exitAmount++;
                    //Debug.Log("enable");
                }
            }
            return exitAmount > 0;
        }
    }
}
