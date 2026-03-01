using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class LifestealEffect : DamageEffect
    {
        public EffectSO Heal;
        public BaseCombatTargettingSO HealTargetting;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (Heal == null)
            {
                Heal = ScriptableObject.CreateInstance<HealEffect>();
            }
            if (HealTargetting == null)
            {
                HealTargetting = Slots.Self;
            }

            exitAmount = 0;
            if (base.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable, out exitAmount))
            {
                Heal.PerformEffect(stats, caster, HealTargetting.GetTargets(stats.combatSlots, caster.SlotID, caster.IsUnitCharacter), HealTargetting.AreTargetSlots, exitAmount, out int exi);
                exitAmount += exi;
            }
            return exitAmount > 0;
        }
    }
}
