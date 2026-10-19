using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class TransferDamageCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if (args is DamageReceivedValueChangeException damage)
            {
                foreach (TargetSlotInfo target in Slots.Front.GetTargets(CombatManager.Instance._stats.combatSlots, effector.SlotID, effector.IsUnitCharacter))
                {
                    if (target.HasUnit)
                    {
                        (effector as IUnit).ShowItem();
                        PigmentDamageByStoredValueEffect hit = ScriptableObject.CreateInstance<PigmentDamageByStoredValueEffect>();
                        hit._indirect = true;
                        hit.m_unitStoredDataID = "ADAF_DO_NOT_USE";
                        hit._defaultStoredValue = 0;
                        CombatManager.Instance.AddSubAction(new EffectAction([Effects.GenerateEffect(hit, damage.amount, Slots.Front)], effector as IUnit));
                        damage.AddModifier(new ImmZeroMod());
                        //damage.ShouldIgnoreUI = true;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
