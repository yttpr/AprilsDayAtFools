using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CasterConstrictedToggleEffect : EffectSO
    {
        public static string Value => "ADAF_ConstrictedToggleEffect";
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (caster.ContainsPassiveAbility(PassiveType_GameIDs.Constricting.ToString()))
            {
                caster.SimpleSetStoredValue(Value, 1);
                if (caster.TryRemovePassiveAbility(PassiveType_GameIDs.Constricting.ToString()))
                    exitAmount++;
            }
            else if (caster.SimpleGetStoredValue(Value) > 0)
            {
                caster.AddPassiveAbility(Passives.Constricting);
                caster.SimpleSetStoredValue(Value, 0);
                exitAmount++;
            }
            return exitAmount > 0;
        }
    }
}
