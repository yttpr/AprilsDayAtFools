using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AprilsDayAtFools
{

    public class RandomizeTargetHealthColorsEffect : EffectSO
    {
        public List<ManaColorSO> options;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (options == null)
            {
                options = new List<ManaColorSO>() { Pigments.Red, Pigments.Blue, Pigments.Yellow, Pigments.Purple };
            }
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    List<ManaColorSO> list = new List<ManaColorSO>(options);
                    if (list.Contains(target.Unit.HealthColor)) list.Remove(target.Unit.HealthColor);

                    if (list.Count <= 0) continue;

                    if (target.Unit.ChangeHealthColor(list.GetRandom())) exitAmount++;
                }
            }
            return exitAmount > 0;
        }
        public static RandomizeTargetHealthColorsEffect Create(bool grey = false)
        {
            RandomizeTargetHealthColorsEffect ret = ScriptableObject.CreateInstance<RandomizeTargetHealthColorsEffect>();
            ret.options = new List<ManaColorSO>() { Pigments.Red, Pigments.Blue, Pigments.Yellow, Pigments.Purple };
            if (grey) ret.options.Add(Pigments.Grey);
            return ret;
        }
    }
}
