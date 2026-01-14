using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ForceTurnCleanupAction : CombatAction
    {
        public IUnit Unit;
        public ForceTurnCleanupAction(IUnit unit)
        {
            Unit = unit;
        }
        public override IEnumerator Execute(CombatStats stats)
        {
            if (Unit == null) yield break;
            Unit.SimpleSetStoredValue(IDs.ForcedTurn, 0);
            yield return null;
        }
    }
}
