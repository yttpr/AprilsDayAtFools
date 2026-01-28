using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class MaskedAddDelayedAttackEffect : EffectSO
    {
        EffectSO _run;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (_run == null)
            {
                //miriam
                if (LoadedAssetsHandler.LoadedEnemies.ContainsKey("Miriam_EN") && LoadedAssetsHandler.LoadedEnemies["Miriam_EN"] != null && !LoadedAssetsHandler.LoadedEnemies["Miriam_EN"].Equals(null))
                {
                    _run = LoadedAssetsHandler.GetEnemy("Miriam_EN").abilities[1].ability.effects[0].effect;
                }
                else
                {
                    _run = ScriptableObject.CreateInstance<AddDelayedAttackEffect>();
                }
            }

            return _run.PerformEffect(stats, caster, targets, areTargetSlots, entryVariable, out exitAmount);
        }
    }
}
