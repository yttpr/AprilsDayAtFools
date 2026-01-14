using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class CasterFleetingIfAliveEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (!caster.IsAlive) return false;
            caster.UnitWillFlee();
            CombatManager.Instance.AddSubAction(new FleetingUnitAction(caster.ID, caster.IsUnitCharacter));
            return true;
        }
    }
}
