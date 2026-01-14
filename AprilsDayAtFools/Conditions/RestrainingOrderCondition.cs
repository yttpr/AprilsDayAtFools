using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class RestrainingOrderCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is DamageDealtValueChangeException reference)
            {
                int num = 30;
                if (reference.damagedUnit != null)
                {
                    SlotsCombat slots = CombatManager.Instance._stats.combatSlots;
                    TargetSlotInfo[] left = Targeting.Slot_AllyLeft.GetTargets(slots, reference.damagedUnit.SlotID, reference.damagedUnit.IsUnitCharacter);
                    TargetSlotInfo[] right = Targeting.Slot_AllyRight.GetTargets(slots, reference.damagedUnit.SlotID, reference.damagedUnit.IsUnitCharacter);

                    if (left.Length > 0)
                    {
                        foreach (TargetSlotInfo target in left)
                        {
                            if (target.HasUnit)
                            {
                                num -= 15;
                                break;
                            }
                        }
                    }

                    if (right.Length > 0)
                    {
                        foreach (TargetSlotInfo target in right)
                        {
                            if (target.HasUnit)
                            {
                                num -= 15;
                                break;
                            }
                        }
                    }

                    if (num > 0)
                    {
                        if (effector is IUnit unit) unit.ShowItem();
                        reference.AddModifier(new PercentageValueModifier(true, num, true));
                    }
                }
            }
            return true;
        }
    }
}
