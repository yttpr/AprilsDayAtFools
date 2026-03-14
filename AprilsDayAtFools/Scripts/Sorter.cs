using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public static class Sorter
    {
        public static List<IUnit> SortByHealth(List<IUnit> units, bool increasing)
        {
            if (units.Count <= 1) return units;
            
            List<IUnit> left = SortByHealth(units.GetRange(0, (int)Math.Floor(units.Count / 2f)), increasing);
            List<IUnit> right = SortByHealth(units.GetRange((int)Math.Floor(units.Count / 2f), units.Count - (int)Math.Floor(units.Count / 2f)), increasing);

            List<IUnit> ret = [];
            while (left.Count > 0 || right.Count > 0)
            {
                if (right.Count <= 0)
                {
                    ret.Add(left[0]);
                    left.RemoveAt(0);
                }
                else if (left.Count <= 0)
                {
                    ret.Add(right[0]);
                    right.RemoveAt(0);
                }
                else if (increasing ? right[0].CurrentHealth < left[0].CurrentHealth : right[0].CurrentHealth > left[0].CurrentHealth)
                {
                    ret.Add(right[0]);
                    right.RemoveAt(0);
                }
                else
                {
                    ret.Add(left[0]);
                    left.RemoveAt(0);
                }
            }

            return ret;
        }
    }
}
