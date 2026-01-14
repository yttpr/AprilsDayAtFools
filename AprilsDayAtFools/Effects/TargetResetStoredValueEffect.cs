using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class TargetResetStoredValueEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    if (target.Unit is CharacterCombat chara)
                    {
                        foreach (UnitStoreDataHolder data in chara.StoredValues.Values)
                        {
                            exitAmount += data.m_MainData;
                            data.m_MainData = 0;
                        }
                    }
                    else if (target.Unit is EnemyCombat enemy)
                    {
                        foreach (UnitStoreDataHolder data in enemy.StoredValues.Values)
                        {
                            exitAmount += data.m_MainData;
                            data.m_MainData = 0;
                        }
                    }
                }
            }
            return exitAmount > 0;
        }
    }
}
