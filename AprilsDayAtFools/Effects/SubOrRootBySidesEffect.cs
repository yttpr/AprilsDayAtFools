using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class SubOrRootBySidesEffect : EffectSO
    {
        public EffectInfo[] effects;
        public bool sub_is_fools;
        public bool give_exit;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    exitAmount++;
                    if (target.Unit.IsUnitCharacter == sub_is_fools)
                    {
                        CombatManager.Instance.AddSubAction(new EffectAction(effects, target.Unit, give_exit ? entryVariable : 0));
                    }
                    else
                    {
                        CombatManager.Instance.AddRootAction(new EffectAction(effects, target.Unit, give_exit ? entryVariable : 0));
                    }
                }
            }
            return exitAmount > 0;
        }


        public static SubOrRootBySidesEffect Create(EffectInfo[] effects, bool sub_is_fools, bool give_exit = false)
        {
            SubOrRootBySidesEffect ret = ScriptableObject.CreateInstance<SubOrRootBySidesEffect>();
            ret.effects = effects;
            ret.sub_is_fools = sub_is_fools;
            ret.give_exit = give_exit;
            return ret;
        }
    }
}
