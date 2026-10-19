using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ComplexHealEffect  : HealEffect
    {
        public ManaColorSO _pigment;
        public EffectSO _status;
        public int _status_amt = 2;

        public AttackVisualsSO _animation;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            List<TargetSlotInfo> newtargets = [];
            List<ManaColorSO> usedmana = [.. PigmentUsedCollector.lastUsed];
            usedmana.Reverse();

            for (int i = 0; targets != null && i < usedmana.Count && i < targets.Length; i++)
            {
                if (usedmana[i].SharesPigmentColor(_pigment))
                    newtargets.Add(targets[i]);
            }

            if (_animation != null)
                CombatManager.Instance.AddUIAction(new PlayAbilityAnimBySlotsAction(_animation, newtargets.ToArray(), areTargetSlots, caster));
            if (_status != null)
                _status.PerformEffect(stats, caster, newtargets.ToArray(), areTargetSlots, _status_amt, out exitAmount);
            return base.PerformEffect(stats, caster, newtargets.ToArray(), areTargetSlots, entryVariable, out exitAmount);
        }
    }
}
