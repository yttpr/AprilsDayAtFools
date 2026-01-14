using BrutalAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Yarn;

namespace AprilsDayAtFools
{
    public class RandomizeHealthColorAction : CombatAction
    {
        public IUnit Unit;
        public RandomizeHealthColorAction(IUnit _unit)
        {
            Unit = _unit;
        }
        public override IEnumerator Execute(CombatStats stats)
        {
            List<ManaColorSO> randomize = [Pigments.Red, Pigments.Yellow, Pigments.Blue, Pigments.Purple, Pigments.Grey];
            if (randomize.Contains(Unit.HealthColor)) randomize.Remove(Unit.HealthColor);
            Unit.ChangeHealthColor(randomize.GetRandom());
            yield return null;
        }
    }
}
