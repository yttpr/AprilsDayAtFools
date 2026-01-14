using System.Collections.Generic;
using UnityEngine;

namespace AprilsDayAtFools
{
    public class RandomizeAmountPigmentEffect : EffectSO
    {
        public bool _randomBetweenPrev;
        public ManaColorSO[] Options;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            List<int> list = new List<int>();
            List<ManaColorSO> list2 = new List<ManaColorSO>();
            ManaBarSlot[] manaBarSlots = stats.MainManaBar.ManaBarSlots;
            List<ManaBarSlot> copy = [.. manaBarSlots];
            
            for (int i = 0; i < (_randomBetweenPrev ? Random.Range(PreviousExitValue, entryVariable + 1) : entryVariable); i++)
            {
                if (copy.Count <= 0) break;
                ManaBarSlot manaBarSlot = copy.GetRandom();
                copy.Remove(manaBarSlot);
                if (!manaBarSlot.IsEmpty)
                {
                    List<ManaColorSO> copy2 = [.. Options];
                    if (copy2.Contains(manaBarSlot.ManaColor)) copy2.Remove(manaBarSlot.ManaColor);
                    if (copy2.Count <= 0)
                    {
                        i--;
                        continue;
                    }
                    ManaColorSO set = copy2.GetRandom();
                    manaBarSlot.SetMana(set);
                    list.Add(manaBarSlot.SlotIndex);
                    list2.Add(set);
                }
                else i--;
            }

            if (list.Count > 0)
            {
                CombatManager.Instance.AddUIAction(new ModifyManaSlotsUIAction(stats.MainManaBar.ID, list.ToArray(), list2.ToArray()));
            }

            exitAmount = list.Count;
            return exitAmount > 0;
        }
    }
}
