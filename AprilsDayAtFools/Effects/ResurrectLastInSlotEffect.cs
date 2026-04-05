using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class ResurrectLastInSlotEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            List<CharacterCombat> list = stats.GetPossibleResurrectionCharacters();

            foreach (TargetSlotInfo target in targets)
            {
                if (!target.HasUnit)
                {
                    if (list.Count <= 0)
                    {
                        return exitAmount > 0;
                    }
                    
                    CharacterCombat character = list[list.Count - 1];
                    list.RemoveAt(list.Count - 1);

                    if (stats.ResurrectDeadCharacter(character, target.SlotID, entryVariable))
                    {
                        exitAmount++;
                    }
                }
            }

            return exitAmount > 0;
        }
    }
}
