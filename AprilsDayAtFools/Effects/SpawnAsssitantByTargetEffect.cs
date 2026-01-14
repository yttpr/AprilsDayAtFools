using System;
using System.Collections.Generic;
using System.Text;

namespace AprilsDayAtFools
{
    public class SpawnAsssitantByTargetEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            int rank = 0;
            if (caster is CharacterCombat chara) rank = chara.Rank;

            exitAmount = 0;
            
            foreach (TargetSlotInfo target in targets)
            {
                if (target.HasUnit)
                {
                    EnemySO enemy = LoadedAssetsHandler.GetEnemy(PatchSetup.GetAssistant(target.Unit.HealthColor, rank));

                    CombatManager.Instance.AddSubAction(new SpawnEnemyAction(enemy, target.SlotID, false, true, "Spawn_Basic"));

                    exitAmount++;
                }
            }

            return exitAmount > 0;
        }
    }
}
